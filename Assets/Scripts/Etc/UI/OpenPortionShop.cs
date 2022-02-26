using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPortionShop : MonoBehaviour
{
    private GameObject PortionShop;
    private PlayerInfo playerinfo;

    private void Awake()
    {
        PortionShop = GameObject.Find("UI Root").transform.Find("PortionShop").gameObject;
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        PortionShop.SetActive(false);
    }

    public void OpenPorShop()
    {
        if (PortionShop.activeSelf == false)
        {
            playerinfo.SetPSTATE(2);
            PortionShop.SetActive(true);
        }
        else if(PortionShop.activeSelf == true)
        {
            playerinfo.SetPSTATE(0);
            PortionShop.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OpenPorShop();
        }        
    }
}
