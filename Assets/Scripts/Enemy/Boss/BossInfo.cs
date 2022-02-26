using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfo : MonoBehaviour
{
    static class Constants
    {
        internal const float ManaGenTime = 2.0f;
    }
    private float BossCurHP;
    private float BossMaxHP;
    private float BossMaxMP;
    private float BossCurMP;
    private float EXP;
    private float ManaRegenTime;
    private BossFSM Bossfsm;
    private PlayerInfo playerinfo;

    internal UISlider HPBar;
    internal UISlider MPBar;
    internal BossFSM.BOSS_STATE state;

    private void Awake()
    {
        BossCurHP = 400;
        BossMaxHP = BossCurHP;
        BossMaxMP = 100;
        BossCurMP = 0;
        ManaRegenTime = 0.0f;
        EXP = 500;
        Bossfsm = GetComponent<BossFSM>();
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    void Update()
    {
        if(state != BossFSM.BOSS_STATE.DIE)
        {
            if(PlayManage.instance.GetCurStage() == PlayManage.CURRENTSTAGE.BOSS_STAGE
                && playerinfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
            {
                ManaRegenTime += Time.deltaTime;
                if (ManaRegenTime >= Constants.ManaGenTime)
                {
                    if (BossCurMP < BossMaxMP)
                    {
                        BossCurMP += 10;
                    }
                    ManaRegenTime = 0.0f;
                }
            }           

            state = Bossfsm.Bossstate;
            if(HPBar)
            {
                HPBar.value = BossCurHP / BossMaxHP;
            }
            if(MPBar)
            {
                MPBar.value = BossCurMP / BossMaxMP;
            }
            CheckHP();
        }
    }

    private void OnDestroy()
    {
        if(BossCurHP <= 0)
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
        BossCurHP -= Damage;
    }

    internal void DecreaseMP(float Used)
    {
        BossCurMP -= Used;
    }

    internal bool IsCurMPMax()
    {
        if(BossCurMP >= BossMaxMP)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckHP()
    {
        if(BossCurHP <= 0)
        {
            Bossfsm.Bossstate = BossFSM.BOSS_STATE.DIE;
            Destroy(this.gameObject);
        }
    }
}
