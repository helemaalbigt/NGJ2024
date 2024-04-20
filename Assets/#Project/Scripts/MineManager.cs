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
    }

    public Mine Spawn(Vector3 aPosition)
    {
        aPosition = new Vector3(aPosition.x, 0, aPosition.z);
        Mine mine = Instantiate(_minePrefab, _parent);
        mine.transform.position = aPosition;
        _mines.Add(mine);

        return mine;
    }

    public bool GetClosestMinePosition(Vector3 aPosition, out Vector3 aClosestMinePosition)
    {
        aClosestMinePosition = Vector3.zero;

        if (_mines.Count == 0)
            return false;

        float distance2 = float.MaxValue;
        foreach (Mine mine in _mines)
        {
            Vector3 offset = aPosition - mine.transform.position;
            float sqrDist = offset.sqrMagnitude;
            if (sqrDist < distance2)
            {
                distance2 = sqrDist;
                aClosestMinePosition = mine.transform.position;
            }
        }

        return true;
    }

    public void UnspawnAll()
    {
        foreach (var mine in _mines)
        {
            Destroy(mine);
        }

        _mines.Clear();
    }
}
