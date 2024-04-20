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

    private void Awake()
    {
        playerCount = 2;
        Instance = this;
    }

    public struct Run
    {
        public float time;
        public List<Mine> minesTriggered;
    }

    private Dictionary<int, List<Run>> _playerRuns = new Dictionary<int, List<Run>>();

    private int _currentRoundCount;
    private int _currentPlayerId;

    // Run
    private float _currentRunStartTime;
    private int _currentRunAvailableMines;
    private int _currentRunNextCheckpointIndex;
    private List<Mine> _currentRunTriggeredMines;

    public void StartGame()
    {
        runState = RunState.Idle;
        _currentRoundCount = 0;
        _currentPlayerId = 0;
    }

    public void StartRun()
    {
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
        return MineManager.Instance.Spawn(aPosition, _currentPlayerId, _currentRoundCount);
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
        _playerRuns[_currentPlayerId].Add(run);

        HapticsManager.Instance.StopAll();
        runState = RunState.Ended;
    }

    public int GetAvailableMines()
    {
        return _currentRunAvailableMines;
    }

    public Run GetCurrentRun()
    {
        return _playerRuns[_currentPlayerId].Last();
    }

    public int GetCurrentPlayerId() {
        return _currentPlayerId;
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
}
