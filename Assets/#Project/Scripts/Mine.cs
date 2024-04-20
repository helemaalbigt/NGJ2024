using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public List<GameObject> _meshes = new List<GameObject>();

    [HideInInspector] int _playerId;

    public void Show(bool aValue)
    {
        foreach (GameObject obj in _meshes)
        {
            obj.SetActive(aValue);
        }
    }

    public void SetPlayerId(int aPlayerId)
    {
        _playerId = aPlayerId;
        // Set mesh color
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
