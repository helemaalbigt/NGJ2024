using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {
    public CheckPoint checkPointPrefab;
    public GameObject startMound;

    private List<CheckPoint> checkPoints = new List<CheckPoint>();

    private const float MinMargin = 2f;

    public bool TryAddCheckPoint(Vector3 pos) {
        pos.y = 0;
        if (IsValidPlacementPos(pos)) {
            var checkPoint = Instantiate(checkPointPrefab, transform);
            checkPoint.transform.localPosition = pos;
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
    
    void Update()
    {
        
    }
}
