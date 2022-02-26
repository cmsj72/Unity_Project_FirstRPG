using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{    
    private     float       EnemyCurHP;
    private     float       EnemyMaxHP;
    private     float       EXP;
    private     FSMTest     fsmTest;
    private     PlayerInfo  playerinfo;

    internal    UISlider    HPBar;
    internal    FSMTest.ENEMY_STATE state;

    private void Awake()
    {
        EnemyCurHP = 100;
        EnemyMaxHP = EnemyCurHP;
        EXP = 50;
        fsmTest = GetComponent<FSMTest>();
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    void Update()
    {
        if(state != FSMTest.ENEMY_STATE.DIE)
        {
            state = fsmTest.Enemystate;
            if (HPBar)
            {
                HPBar.value = EnemyCurHP / EnemyMaxHP;
            }
            CheckHP();
        }
    }

    private void OnDestroy()
    {        
        if(EnemyCurHP <= 0)
        {
            playerinfo.IncreaseExp(EXP);
            GameObject gObject = Instantiate(Resources.Load("Prefab/Effect/Effect10_Collision"),
                    transform.position + new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            int rnum = Random.Range(0, ItemManage.instance.ItemMax);
            ItemManage.instance.DropItem(transform, rnum);
        }
    }

    internal void DecreaseHP(float Damage)
    {
        EnemyCurHP -= Damage;
    }

    private void CheckHP()
    {
        if(EnemyCurHP <= 0)
        {
            fsmTest.Enemystate = FSMTest.ENEMY_STATE.DIE;
            Destroy(this.gameObject);
        }
    }
}
