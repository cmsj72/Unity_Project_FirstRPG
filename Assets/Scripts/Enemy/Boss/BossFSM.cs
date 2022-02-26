using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFSM : MonoBehaviour
{
    public enum BOSS_STATE
    {
        IDLE,
        ATTACK,
        TRACE,
        SKILL,
        DIE,
    }

    private Transform tr;
    private Animator anim;
    private BossInfo bossinfo;
    private GameObject Player;
    private PlayerInfo playerInfo;
    private NavMeshAgent nav;
    private float attackDist;
    private float traceDist;
    private bool IsAtkCoroutine;
    private bool MotionFlag;
    internal BOSS_STATE Bossstate;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        bossinfo = GetComponent<BossInfo>();
        Player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        playerInfo = Player.GetComponent<PlayerInfo>();
        Bossstate = BOSS_STATE.IDLE;
        attackDist = 4.0f;
        traceDist = 11.0f;
        IsAtkCoroutine = false;
        MotionFlag = true;

        StartCoroutine(CheckState());
    }

    // Update is called once per frame
    void Update()
    {     
        if (Bossstate != BOSS_STATE.IDLE && playerInfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
        {
            tr.LookAt(Player.transform);
        }
        if (playerInfo.GetPSTATE() == PlayerInfo.PSTATE.DIE ||
            playerInfo.GetPSTATE() == PlayerInfo.PSTATE.CLEAR ||
            Bossstate == BOSS_STATE.DIE)
        {
            StopAllCoroutines();
        }
    }

    private void StopAttacking()
    {
        IsAtkCoroutine = false;
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (playerInfo.GetPSTATE() == PlayerInfo.PSTATE.MENU)
            {
                nav.isStopped = true;
            }
            else
            {
                nav.isStopped = false;
            }
            float Dist = Vector3.Distance(Player.transform.position, tr.position);

            if (Dist <= attackDist)
            {
                if (bossinfo.IsCurMPMax() == true)
                {
                    Bossstate = BOSS_STATE.SKILL;
                }
                else
                {
                    Bossstate = BOSS_STATE.ATTACK;
                }
            }
            else
            {
                if (Dist <= traceDist)
                {
                    Bossstate = BOSS_STATE.TRACE;
                }
                else
                {
                    Bossstate = BOSS_STATE.IDLE;
                }
            }

            switch (Bossstate)
            {
                case BOSS_STATE.IDLE:
                    StopAttacking();
                    nav.isStopped = true;
                    anim.SetInteger("BossState", (int)BOSS_STATE.IDLE);
                    break;

                case BOSS_STATE.TRACE:
                    StopAttacking();
                    nav.SetDestination(Player.transform.position);
                    nav.isStopped = false;
                    anim.SetInteger("BossState", (int)BOSS_STATE.TRACE);
                    break;

                case BOSS_STATE.ATTACK:
                    anim.SetInteger("BossState", (int)BOSS_STATE.ATTACK);
                    if (IsAtkCoroutine == false && MotionFlag == true)
                    {
                        StartCoroutine(Attacking());
                        IsAtkCoroutine = true;
                    }
                    break;

                case BOSS_STATE.SKILL:
                    StopAttacking();
                    nav.isStopped = true;
                    StartCoroutine(UseSkill());
                    break;
            }
            yield return new WaitForSeconds(2.0f);
        }        
    }

    IEnumerator Attacking()
    {
        while(Bossstate == BOSS_STATE.ATTACK)
        {
            if(playerInfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
            {
                nav.isStopped = true;
                MotionFlag = false;
                anim.SetTrigger("atk01Trigger");
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                yield return new WaitForSeconds(2.0f);
                MotionFlag = true;
            }            
        }
    }

    IEnumerator UseSkill()
    {
        MotionFlag = false;
        anim.SetTrigger("atk03Trigger");
        yield return new WaitForSeconds(1.0f);
        GameObject tmp = Instantiate(Resources.Load("Prefab/Effect/BossSkill"),
            tr.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        bossinfo.DecreaseMP(100.0f);
        MotionFlag = true;
        yield return null;
    }

}
