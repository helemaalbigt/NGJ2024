using Rowhouse;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CheckPointManager _checkPointManager;

    public int _numberOfRounds = 3;
    public int _numberOfMinesPerRun = 3;

    public int playerCount;

    public enum RunState
    {
        Idle,
        Started,
        Ended
    }

    [HideInInspector] public RunState runState = RunState.Idle;

    public struct Run
    {
        public float time;
        public List<Mine> minesTriggered;
    }

    private Dictionary<int, List<Run>> _playerRuns = new Dictionary<int, List<Run>>();
    private Dictionary<int, float> _leaderBoard = new Dictionary<int, float>();

    private int _currentRoundCount;
    private int _currentPlayerId;

    // Run
    private float _currentRunStartTime;
    private int _currentRunAvailableMines;
    private int _currentRunNextCheckpointIndex;
    private List<Mine> _currentRunTriggeredMines;
    private List<Mine> _currentRunPlacedMines;

    private void Awake()
    {
        playerCount = 2;
        Instance = this;
        _currentRunTriggeredMines = new List<Mine>();
        _currentRunPlacedMines = new List<Mine>();
    }

    public void StartGame()
    {
        runState = RunState.Idle;
        _currentRoundCount = 0;
        _currentPlayerId = 0;

        _leaderBoard.Clear();
        for (int i = 0; i < playerCount; i++)
        {
            _leaderBoard.Add(i, 0.0f);
        }
    }

    public void StartRun()
    {
        foreach(Mine mine in _currentRunPlacedMines)
        {
            mine.SetState(Mine.State.Active);
        }
        _currentRunPlacedMines.Clear();
        _currentRunTriggeredMines.Clear();
        _currentRunAvailableMines = _numberOfMinesPerRun;
        _currentRunStartTime = Time.time;

        MineManager.Instance.ShowVisuals(false);

        _currentRunNextCheckpointIndex = 0;
        _checkPointManager.ResetAllCheckpoints();
        _checkPointManager.SetActiveCheckPoint(_currentRunNextCheckpointIndex);

        runState = RunState.Started;
    }

    public void OnMineTriggered(Mine aMine)
    {
        _currentRunTriggeredMines.Add(aMine);
        aMine.SetState(Mine.State.Triggered);
    }

    public Mine PlaceMine(Vector3 aPosition)
    {
        if (_currentRunAvailableMines == 0)
            return null;

        _currentRunAvailableMines--;
        Mine mine = MineManager.Instance.Spawn(aPosition, _currentPlayerId, _currentRoundCount);
        _currentRunPlacedMines.Add(mine);
        return mine;
    }

    public void OnCheckPointEntered(CheckPoint aCheckPoint)
    {
        if (aCheckPoint.index != _currentRunNextCheckpointIndex)
            return;

        aCheckPoint.isChecked = true;
        _currentRunNextCheckpointIndex++;
        _checkPointManager.SetActiveCheckPoint(_currentRunNextCheckpointIndex);
    }

    public void OnStartMoundEnter()
    {
        if (_currentRunNextCheckpointIndex != _checkPointManager.CheckPointCount)
            return;

        EndRun();
    }

    public void EndRun()
    {
        Run run;
        run.minesTriggered = new List<Mine>(_currentRunTriggeredMines);
        run.time = Time.time - _currentRunStartTime;
        if (!_playerRuns.ContainsKey(_currentPlayerId)) {
            _playerRuns[_currentPlayerId] = new List<Run>();
        } 
        _playerRuns[_currentPlayerId].Add(run);

        HapticsManager.Instance.StopAll();
        runState = RunState.Ended;
    }

    public int GetAvailableMines()
    {
        return _currentRunAvailableMines;
    }

    public int GetCurrentRound() {
        return _currentRoundCount;
    }

    public Run GetCurrentRun()
    {
        return _playerRuns[_currentPlayerId].Last();
    }

    public int GetCurrentPlayerId() {
        return _currentPlayerId;
    }

    public List<KeyValuePair<int, float>> GetOrderedLeaderBoard()
    {
        var orderedLeaderBoard = from entry in _leaderBoard orderby entry.Value ascending select entry;
        return orderedLeaderBoard.ToList();
    }
    
    public bool IsGameOver()
    {
        int nextPlayerId = _currentPlayerId + 1;
        int roundCount = _currentRoundCount;
        if (nextPlayerId >= playerCount)
        {
            roundCount++;
        }

        return roundCount >= _numberOfRounds;
    }

    public void SetNextRunState()
    {
        _leaderBoard[_currentPlayerId] += GetCurrentRun().time;
        foreach (Mine mine in GetCurrentRun().minesTriggered)
        {
            ProcessMineScore(mine._playerId, _currentPlayerId);
        }

        _currentPlayerId++;
        if (_currentPlayerId >= playerCount)
        {
            _currentPlayerId = 0;
            _currentRoundCount++;
        }
    }

    public void OnGameOver()
    {

    }

    public void ProcessMineScore(int instigatorId, int victimId)
    {
        if (instigatorId == victimId)
        {
            _leaderBoard[victimId] = Mathf.Max(0.0f, _leaderBoard[victimId] + ScoreModifiers.SelfHitPenality);
            return;
        }

        _leaderBoard[victimId] = Mathf.Max(0.0f, _leaderBoard[victimId] + ScoreModifiers.HitPenality);
        _leaderBoard[instigatorId] = Mathf.Max(0.0f, _leaderBoard[instigatorId] - ScoreModifiers.HitBonus);
    }
}
