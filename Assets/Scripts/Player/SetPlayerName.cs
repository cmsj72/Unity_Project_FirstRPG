using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPlayerName : MonoBehaviour
{
    private string PlayerName;
    private GameObject Warn;

    private void Awake()
    {
        Warn = GameObject.Find("Warn");
        Warn.SetActive(false);
    }

    public void SetName()
    {
        PlayerName = GameObject.Find("InputName").GetComponent<UIInput>().value;
        if(PlayerName != "")
        {
            TempInfoManage.instance.PlayerNameTemp = PlayerName;
            SceneManager.LoadScene("Camp");
        }
        else
        {
            StartCoroutine(Warning());
        }
    }

    IEnumerator Warning()
    {
        Warn.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Warn.SetActive(false);
        yield return null;
    }
}
