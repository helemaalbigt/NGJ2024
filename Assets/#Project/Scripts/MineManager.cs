using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public static MineManager Instance { get; private set; }

    public Mine _minePrefab;

    private List<Mine> _mines;
    private Transform _parent;
    
    private void Awake()
    {
        Instance = this;
        _parent = new GameObject("[MineParent]").transform;
        _mines = new List<Mine>();
    }

    public Mine Spawn(Vector3 aPosition, int aPlayerId, int aRunCount)
    {
        aPosition = new Vector3(aPosition.x, 0, aPosition.z);
        Mine mine = Instantiate(_minePrefab, _parent);
        mine.transform.position = aPosition;
        mine.SetPlayerIdAndRun(aPlayerId, aRunCount);
        _mines.Add(mine);

        return mine;
    }

    public Mine GetClosestMine(Vector3 aPosition)
    {
        if (_mines.Count == 0)
            return null;

        float distance2 = float.MaxValue;
        Mine closestMine = null;
        foreach (Mine mine in _mines)
        {
            if (mine._state != Mine.State.Active)
                continue;

            Vector3 offset = aPosition - mine.transform.position;
            float sqrDist = offset.sqrMagnitude;
            if (sqrDist < distance2)
            {
                distance2 = sqrDist;
                closestMine = mine;
            }
        }

        return closestMine;
    }

    public void UnspawnAll()
    {
        foreach (var mine in _mines)
        {
            Destroy(mine);
        }

        _mines.Clear();
    }

    public void ShowVisuals(bool aValue)
    {
        foreach(Mine mine in _mines)
        {
            mine.Show(aValue);
        }
    }
}
