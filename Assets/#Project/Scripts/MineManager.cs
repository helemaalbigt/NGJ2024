using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public static MineManager Instance { get; private set; }

    public Mine _minePrefab;

    private List<Mine> _mines;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(Transform aPosition)
    {
        aPosition.position = new Vector3(aPosition.position.x, 0, aPosition.position.z);
        Mine mine = Instantiate(_minePrefab, aPosition);
        _mines.Add(mine);
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
