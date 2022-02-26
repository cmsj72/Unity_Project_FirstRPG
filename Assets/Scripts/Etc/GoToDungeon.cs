using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDungeon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInfo playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        if (SaveLoadManager.instance.playselect != SaveLoadManager.PLAYSELECT.NEW)
        {
            SaveLoadManager.instance.SetPlayerDataForSave(new SaveLoadManager.PlayerData(playerinfo.GetPlayerName(),
            playerinfo.GetPlayerClass(),
            playerinfo.GetPlayerLevel(),
            playerinfo.GetPlayerGold(),
            playerinfo.GetPlayerCurEXP()), playerinfo.GetPlayerItemList());
            SaveLoadManager.instance.SavePlayerData(SaveLoadManager.instance.LatestSlotNumer);
            SaveLoadManager.instance.OverWrite();
        }
        SceneManager.LoadScene("Dungeon");
    }
}
