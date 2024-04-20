using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineAvailableUI : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.GetAvailableMines().ToString() + "/" + GameManager.Instance._numberOfMinesPerRun.ToString();
    }
}
