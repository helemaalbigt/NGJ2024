using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public List<GameObject> _meshes = new List<GameObject>();
    public GameObject _collider;
    public GameObject _destroyedVisuals;

    public GameObject _innerDebug;
    public GameObject _outerDebug;

    public enum State
    {
        Active,
        Triggered
    }

    [HideInInspector] public State _state;
    int _playerId;

    private void Awake()
    {
        SetState(State.Active);
    }

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

    public void ShowDebug(float anInner, float anOuter)
    {
        _innerDebug.transform.localScale = new Vector3(anInner, _innerDebug.transform.localScale.y, anInner);
        _outerDebug.transform.localScale = new Vector3(anOuter, _outerDebug.transform.localScale.y, anOuter);

        _innerDebug.SetActive(true);
        _outerDebug.SetActive(true);
    }

    public void HideDebug()
    {
        _innerDebug.SetActive(false);
        _outerDebug.SetActive(false);
    }

    public void SetState(State aState)
    {
        switch (aState)
        {
            case State.Active:
                _collider.SetActive(true);
                _destroyedVisuals.SetActive(false);
                break;
            case State.Triggered:
                _collider.SetActive(false);
                _destroyedVisuals.SetActive(true);
                HideDebug();
                Show(false);
                break;
        }

        _state = aState;
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
