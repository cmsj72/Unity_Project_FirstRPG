using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AskSave : MonoBehaviour
{
    private Transform trExitButton;
    private PlayerInfo playerinfo;
    private GameObject SaveNotice;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Camp")
        {
            trExitButton = GetComponent<Transform>();
            SaveNotice = trExitButton.Find("SaveNotice").gameObject;
        }
        else
        {
            SaveNotice = GameObject.Find("UI Root").transform.Find("SaveNotice").gameObject;
        }
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    public void ShowSaveWindow()
    {
        if(SceneManager.GetActiveScene().name == "Field" 
            || SceneManager.GetActiveScene().name == "Arena"
            || SceneManager.GetActiveScene().name == "Dungeon")
        {
            StartCoroutine(ShowSaveInPlay());
        }
        else
        {
            playerinfo.SetPSTATE(2);
            SaveNotice.SetActive(true);
        }
        SaveLoadManager.instance.SetPlayerDataForSave(new SaveLoadManager.PlayerData(playerinfo.GetPlayerName(),
            playerinfo.GetPlayerClass(),
            playerinfo.GetPlayerLevel(),
            playerinfo.GetPlayerGold(),
            playerinfo.GetPlayerCurEXP()), playerinfo.GetPlayerItemList());
    }

    IEnumerator ShowSaveInPlay()
    {
        yield return new WaitForSeconds(5.0f);
        OpenSaveWindow();
        yield return null;
    }

    public void OpenSaveWindow()
    {
        if(SaveNotice.activeSelf == false)
        {
            playerinfo.SetPSTATE(2);
            SaveNotice.SetActive(true);
        }
        else if(SaveNotice.activeSelf == true)
        {
            playerinfo.SetPSTATE(0);
            SaveNotice.SetActive(false);
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Camp"
            && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Dungeon")
        {
            MinimapCon.instance.OpenMiniMap();
        }
    }
}
