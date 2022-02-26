using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private Transform tr;
    private PlayerInfo playerinfo;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    public void Atk01Check()
    {
        var col = Physics.OverlapSphere(tr.position + tr.transform.forward, 1.5f);
        if(tr.tag == "Player")
        {
            for(int index =0; index < col.Length; index++)
            {
                if(col[index].tag == "Enemy")
                {
                    col[index].transform.GetComponent<EnemyInfo>().DecreaseHP(playerinfo.GetAtk01Damage());
                    col[index].transform.GetComponent<FSMTest>().GetHurted();
                }
                else if(col[index].tag == "Boss")
                {
                    col[index].transform.GetComponent<BossInfo>().DecreaseHP(playerinfo.GetAtk01Damage());
                }
            }
        }
        else if(tr.tag == "Enemy")
        {
            for(int index =0; index < col.Length; index++)
            {
                if(col[index].tag == "Player")
                {
                    playerinfo.DecreaseCurHP(10);
                    playerinfo.GetHurted();
                }
            }
        }
        else if(tr.tag == "Boss")
        {
            for (int index = 0; index < col.Length; index++)
            {
                if (col[index].tag == "Player")
                {
                    playerinfo.DecreaseCurHP(40);
                    playerinfo.GetHurted();
                }
            }
        }
    }

    public void Atk04Check(int combo)
    {
        var col = Physics.OverlapSphere(tr.position + tr.transform.forward, 2.5f);
        if(tr.tag == "Player")
        {
            for (int index = 0; index < col.Length; index++)
            {
                if (col[index].tag == "Enemy")
                {
                    col[index].transform.GetComponent<EnemyInfo>().DecreaseHP(playerinfo.GetAtk04Damage());
                    col[index].transform.GetComponent<FSMTest>().GetHurted();
                }
                else if (col[index].tag == "Boss")
                {
                    col[index].transform.GetComponent<BossInfo>().DecreaseHP(playerinfo.GetAtk04Damage());
                }
            }
        }        
    }
}
