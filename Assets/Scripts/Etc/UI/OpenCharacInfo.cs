using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCharacInfo : MonoBehaviour
{
    private Transform trCharacInfoButton;
    private PlayerInfo playerinfo;
    private GameObject Info;
    private GameObject MenuChild;
    // Start is called before the first frame update
    void Start()
    {
        trCharacInfoButton = GetComponent<Transform>();
        Info = trCharacInfoButton.Find("Info").gameObject;
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    public void OpenCharacInfoWindow()
    {
        if(Info.activeSelf == false)
        {
            playerinfo.SetPSTATE(2);
            Info.SetActive(true);
        }
        else if(Info.activeSelf == true)
        {
            playerinfo.SetPSTATE(0);
            Info.SetActive(false);
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Camp"
            && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Dungeon")
        {
            MinimapCon.instance.OpenMiniMap();
        }
    }
}
