using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill1 : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInfo>().DecreaseCurHP(1);
        }
    }
}
