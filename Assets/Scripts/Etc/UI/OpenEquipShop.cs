using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenEquipShop : MonoBehaviour
{
    private GameObject EquipShop;
    private PlayerInfo playerinfo;

    private void Awake()
    {
        EquipShop = GameObject.Find("UI Root").transform.Find("EquipShop").gameObject;
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        EquipShop.SetActive(false);
    }

    public void OpenEqupShop()
    {
        if (EquipShop.activeSelf == false)
        {
            playerinfo.SetPSTATE(2);
            EquipShop.SetActive(true);
        }
        else if(EquipShop.activeSelf == true)
        {
            playerinfo.SetPSTATE(0);
            EquipShop.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OpenEqupShop();
        }
    }
}
