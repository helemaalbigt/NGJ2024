using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CheckPointManager _checkPointManager;

    public int _numberOfRuns = 3;

    private int _playerCount;

    private void Awake()
    {
        _playerCount = 1;
        Instance = this;
    }

    private struct Run
    {
        public float time;
        public int minesTriggered;
    }

    private Dictionary<int, List<Run>> _playerRuns = new Dictionary<int, List<Run>>();

    private int _currentRunCount;
    private int _currentPlayerId;
    private float _currentRunStartTime;
    private int _currentRunMinesTriggeredCount;
    private int _currentRunAvailableMines;
    private int _currentRunMissingCheckPointCount;

    // Start is called before the first frame update
    void Start()
    {
        //StartGame(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame(int aPlayerCount)
    {
        _playerCount = aPlayerCount;
        _currentRunCount = 0;

        // We should wait for player confirmation or something
        StartRun(0);
    }

    public void StartRun(int aPlayerId)
    {
        _currentPlayerId = aPlayerId;
        _currentRunMinesTriggeredCount = 0;
        _currentRunAvailableMines = 3;
        _currentRunStartTime = Time.time;
        _currentRunMissingCheckPointCount = _checkPointManager.CheckPointCount;
        MineManager.Instance.ShowVisuals(false);
        _checkPointManager.ResetAllCheckpoints();
    }

    public void OnMineTriggered(Mine aMine)
    {
        _currentRunMinesTriggeredCount++;
        aMine.SetState(Mine.State.Triggered);
    }

    public Mine PlaceMine(Vector3 aPosition)
    {
        if (_currentRunAvailableMines == 0)
            return null;

        _currentRunAvailableMines--;
        return MineManager.Instance.Spawn(aPosition, _currentPlayerId, _currentRunCount);
    }

    public void OnCheckPointEntered(CheckPoint aCheckPoint)
    {
        aCheckPoint.isChecked = true;
        _currentRunMissingCheckPointCount--;
        if (_currentRunMissingCheckPointCount == 0)
            EndRun();
    }

    public void EndRun()
    {
        Run run;
        run.minesTriggered = _currentRunMinesTriggeredCount;
        run.time = Time.time - _currentRunStartTime;
        _playerRuns[_currentPlayerId].Add(run);

        _currentPlayerId++;
        if (_currentPlayerId >= _playerCount)
        {
            _currentPlayerId = 0;
            _currentRunCount++;
        }

        if (_currentRunCount == _numberOfRuns - 1)
            GameOver();
        else
            StartRun(_currentPlayerId);
    }

    private void GameOver()
    {

    }
}
