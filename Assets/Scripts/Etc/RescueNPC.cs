using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueNPC : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayManage.instance.IncreaseRescuedNPC();
            PlayManage.instance.CheckStage();
            HUDManager.instance.PrintRescuedNPC();
            Destroy(transform.parent.gameObject);
        }
    }
}
