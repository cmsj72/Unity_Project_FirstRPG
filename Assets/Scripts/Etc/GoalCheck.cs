using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    private PlayerMove playermove;
    private AskSave askSave;

    private void Awake()
    {
        playermove = GameObject.Find("Player").GetComponent<PlayerMove>();
        askSave = GameObject.Find("AskSave").GetComponent<AskSave>();
    }

    private void OnTriggerEnter(Collider other)
    {
        HUDManager.instance.PrintClear();
        playermove.GoalArrived();
        askSave.ShowSaveWindow();
        Destroy(this.gameObject);
    }
}
