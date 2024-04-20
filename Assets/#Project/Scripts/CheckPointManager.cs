using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {
    public CheckPoint checkPointPrefab;
    public GameObject startMound;


    public int CheckPointCount => checkPoints.Count;

    [Space(20)]
    [SerializeField]
    private List<CheckPoint> checkPoints = new List<CheckPoint>();

    private const float MinMargin = 2f;

    public CheckPoint CreateCheckPoint() {
        return Instantiate(checkPointPrefab, transform);
    }
    
    public bool TryAddCheckPoint(CheckPoint checkPoint) {
        if (IsValidPlacementPos(checkPoint.transform.position)) {
            checkPoint.index = checkPoints.Count;
            checkPoints.Add(checkPoint);
            return true;
        } else {
            return false;
        }
    }

    public bool IsValidPlacementPos(Vector3 pos) {
        pos.y = 0;
        var valid = true;

        foreach (var checkPoint in checkPoints) {
            if (Vector3.Distance(pos, checkPoint.transform.position) < MinMargin)
                valid = false;
        }
        
        if (Vector3.Distance(pos, startMound.transform.position) < MinMargin)
            valid = false;

        return valid;
    }

    public void ClearAllCheckpoints() {
        foreach (var cp in checkPoints) {
            Destroy(cp.gameObject);
        }
        checkPoints.Clear();
    }

    public void ResetAllCheckpoints()
    {
        foreach (var checkPoint in checkPoints)
        {
            checkPoint.isChecked = false;
            checkPoint.isActive = false;
        }
    }

    public void SetActiveCheckPoint(int anIndex)
    {
        foreach (var checkPoint in checkPoints)
        {
            checkPoint.isActive = checkPoint.index == anIndex;
        }
    }
}
