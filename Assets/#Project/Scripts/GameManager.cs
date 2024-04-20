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

    public MonoState gameOverState;
    public MonoState playerChangeState;
    public int playerCount;

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

    // Start is called before the first frame update
    void Start()
    {
        //StartGame(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        _currentRoundCount = 0;
        _currentPlayerId = 0;
    }

    public void StartRun()
    {
        _currentRunTriggeredMines.Clear();
        _currentRunAvailableMines = 3;
        _currentRunStartTime = Time.time;
        _currentRunNextCheckpointIndex = 0;
        MineManager.Instance.ShowVisuals(false);
        _checkPointManager.ResetAllCheckpoints();
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
    }

    public Run GetCurrentRun()
    {
        return _playerRuns[_currentPlayerId].Last();
    }

    public MonoState GetEndRunState()
    {
        _currentPlayerId++;
        if (_currentPlayerId >= playerCount)
        {
            _currentPlayerId = 0;
            _currentRoundCount++;
        }

        if (_currentRoundCount >= _numberOfRounds - 1)
            return gameOverState;
        else
            return playerChangeState;
    }

    public void OnGameOver()
    {

    }
}
