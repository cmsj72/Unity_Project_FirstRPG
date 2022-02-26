using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    private PlayerInfo  playerinfo;
    private UILabel uilabel;

    private void Awake()
    {
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        uilabel = GetComponent<UILabel>();
    }

    void Update()
    {
        uilabel.text = playerinfo.GetPlayerGold().ToString() + " G";        
    }
}
