using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineAvailableUI : MonoBehaviour
{
    public float mineModelRotSpeed = 25.0f;
    public GameObject mineModel;
    public Text text;

    // Update is called once per frame
    void Update()
    {
        mineModel.transform.RotateAround(mineModel.transform.position, mineModel.transform.up, Time.deltaTime * mineModelRotSpeed);
        text.text = GameManager.Instance.GetAvailableMines().ToString() + "/" + GameManager.Instance._numberOfMinesPerRun.ToString();
    }
}
