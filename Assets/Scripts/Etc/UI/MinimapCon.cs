using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCon : MonoBehaviour
{
    public static MinimapCon instance;
    private GameObject MiniMap;

    private void Awake()
    {
        instance = this;
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Dungeon")
        {
            MiniMap = GameObject.Find("Canvas").transform.Find("Minimap").gameObject;
        }        
    }

    public void OpenMiniMap()
    {
        if (MiniMap.activeSelf == false)
        {
            MiniMap.SetActive(true);
        }
        else if (MiniMap.activeSelf == true)
        {
            MiniMap.SetActive(false);
        }
    }
}
