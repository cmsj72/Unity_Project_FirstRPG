using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInven : MonoBehaviour
{
    private Transform trInvenButton;
    private GameObject Inventory;
    private PlayerInfo playerinfo;

    // Start is called before the first frame update
    void Start()
    {
        trInvenButton = GetComponent<Transform>();
        Inventory = trInvenButton.Find("Inventory").gameObject;
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    public void OpenInventory()
    {
        if(Inventory.activeSelf == false)
        {
            playerinfo.SetPSTATE(2);
            Inventory.SetActive(true);
        }
        else if(Inventory.activeSelf == true)
        {
            playerinfo.SetPSTATE(0);
            Inventory.SetActive(false);
        }
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Camp"
            && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Dungeon")
        {
            MinimapCon.instance.OpenMiniMap();
        }        
    }
}
