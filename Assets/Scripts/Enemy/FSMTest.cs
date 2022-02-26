using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMTest : MonoBehaviour
{
    public enum ENEMY_STATE
    {
        IDLE,
        ATTACK,
        TRACE,
        DIE,
    }

    private Transform tr;
    private Animator anim;
    private GameObject Player;
    private PlayerInfo playerInfo;
    private NavMeshAgent nav;
    private float attackDist;
    private float traceDist;
    private bool IsAtkCoroutine;
    private bool MotionFlag;
    internal ENEMY_STATE Enemystate;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        playerInfo = Player.GetComponent<PlayerInfo>();
        Enemystate = ENEMY_STATE.IDLE;
        attackDist = 3.0f;
        traceDist = 10.0f;
        IsAtkCoroutine = false;
        MotionFlag = true;

        StartCoroutine(CheckState());
    }   

    void Update()
    {
        if(Enemystate != ENEMY_STATE.IDLE && playerInfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
        {
            tr.LookAt(Player.transform);
        }
        if(playerInfo.GetPSTATE() == PlayerInfo.PSTATE.DIE || 
            playerInfo.GetPSTATE() == PlayerInfo.PSTATE.CLEAR ||
            Enemystate == ENEMY_STATE.DIE)
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
        while(true)
        {
            if(playerInfo.GetPSTATE() == PlayerInfo.PSTATE.MENU)
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
                Enemystate = ENEMY_STATE.ATTACK;
            }
            else
            {
                if (Dist <= traceDist)
                {
                    Enemystate = ENEMY_STATE.TRACE;
                }
                else
                {
                    Enemystate = ENEMY_STATE.IDLE;
                }
            }

            switch(Enemystate)
            {
                case ENEMY_STATE.IDLE:
                    StopAttacking();
                    nav.isStopped = true;
                    anim.SetInteger("EnemyState", (int)ENEMY_STATE.IDLE);
                    break;

                case ENEMY_STATE.TRACE:
                    StopAttacking();
                    nav.SetDestination(Player.transform.position);
                    nav.isStopped = false;
                    anim.SetInteger("EnemyState", (int)ENEMY_STATE.TRACE);
                    break;

                case ENEMY_STATE.ATTACK:
                    anim.SetInteger("EnemyState", (int)ENEMY_STATE.ATTACK);
                    if (IsAtkCoroutine == false && MotionFlag == true)
                    {
                        StartCoroutine(Attacking());
                        IsAtkCoroutine = true;
                    }
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }        
    }

    IEnumerator Attacking()
    {
        while (Enemystate == ENEMY_STATE.ATTACK)
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
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    internal void GetHurted()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("atk01"))
        {
            anim.SetTrigger("hurtTrigger");
        }        
    }
}