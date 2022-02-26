using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PLAYERMOVE
{
    //캐릭터의 현재 행동 상태
    enum ACTIONSTATE
    {
        ATTACK,
        HURT,
        ELSE,
    }      

    //쿨타임을 관리하기 위한 스킬의 사용 상태
    enum USEDSKILL
    {
        NOSKILL,
        ATK01,
        ATK04,
        BUFF,
    }
}

public class PlayerMove : MonoBehaviour
{
    private UIJoystick JoyStick;
    private CharacterController controller;
    private Transform tr;
    private Animator anim;
    private PlayerInfo playerinfo;
    private Menu menu;
    private GameObject SwordObject;
    private GameObject BuffEffect;
    private GameObject BuffHandEffect;
    private PLAYERMOVE.USEDSKILL PlayerSkillState;
    internal PLAYERMOVE.ACTIONSTATE PlayerActionState;

    private Vector2 StickPosition;
    private Vector3 MoveDir;
    private float moveSpeed = 5.0f;

    private void Awake()
    {
        JoyStick = GameObject.Find("JoyStickBackGround").GetComponent<UIJoystick>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerinfo = GetComponent<PlayerInfo>();
        SwordObject = transform.Find("104710_mnqs_npc").Find("Bip001").Find("Bip001 Prop1").gameObject;
        menu = GameObject.Find("MENUButton").GetComponent<Menu>();
        PlayerSkillState = PLAYERMOVE.USEDSKILL.NOSKILL;
        PlayerActionState = PLAYERMOVE.ACTIONSTATE.ELSE;
        BuffEffectInit();
        MoveDir = tr.forward;
        controller.slopeLimit = 1f;
        controller.stepOffset = 1.0f;
    }

    void Update ()
    {
        //플레이어의 상태가 살아있는 상태이면
        if(playerinfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
        {
            OperateInput();
        }        
    }

    //플레이어의 조작을 구현한 함수
    void OperateInput()
    {
        #region -JoyStick-
#if UNITY_ANDROID
        StickPosition = JoyStick.position;
        Vector3 Tmp = StickPosition;
        Vector3 Dir = new Vector3(Tmp.x, Tmp.z, Tmp.y);
        Vector3 Total = tr.position + Dir;
        tr.LookAt(Total);
        if (PlayerActionState == PLAYERMOVE.ACTIONSTATE.ELSE
             && playerinfo.GetPSTATE() != PlayerInfo.PSTATE.CLEAR)
        {
            if (StickPosition != Vector2.zero)
            {

                controller.Move(Dir * 0.1f);
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }
        }
#endif
        #endregion

        #region -Key Board-
        //캐릭터의 상태가 공격을 하는 중이거나 피격당하는 상태일때
        if (PlayerActionState == PLAYERMOVE.ACTIONSTATE.ELSE)
        {
            //controller.Move(Vector3.down);
            //입력 방향키에 따라 현재 캐릭터의 위치에서 누른 방향키의 방향으로 바라보고
            //움직이게 함
            //걷는 애니메이션이 꺼져있는 상태이면 재생
            if (Input.GetKey(KeyCode.UpArrow))
            {
                tr.LookAt(tr.position + Vector3.forward);
                MoveDir = Vector3.forward;
                controller.Move(moveSpeed * MoveDir * Time.deltaTime);
                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("walking", true);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                tr.LookAt(tr.position + Vector3.back);
                MoveDir = Vector3.back;
                controller.Move(moveSpeed * MoveDir * Time.deltaTime);
                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("walking", true);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                tr.LookAt(tr.position + Vector3.left);
                MoveDir = Vector3.left;
                controller.Move(moveSpeed * MoveDir * Time.deltaTime);
                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("walking", true);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                tr.LookAt(tr.position + Vector3.right);
                MoveDir = Vector3.right;
                controller.Move(moveSpeed * MoveDir * Time.deltaTime);
                if (anim.GetBool("walking") == false)
                {
                    anim.SetBool("walking", true);
                }
            }

        }

        //a,s,d 를 이용해 스킬 사용
        if (PlayerActionState == PLAYERMOVE.ACTIONSTATE.ELSE)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UseAtk1();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                UseAtk4();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                UseBuff();
            }
        }

        //esc를 누르면 메뉴를 on,off 가능
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.MenuControl();
        }

        //방향키를 눌러 움직이는 중이 아니면 걷는 애니메이션 재생 중지
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)
           && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("walking", false);
        }

        #endregion

    }

    internal void GoalArrived()
    {
        anim.SetTrigger("victory");
        playerinfo.SetPSTATE(3);
    }

    #region -Skill-
    //기본공격
    internal void UseAtk1()
    {
        if(playerinfo.Atk01CoolON == false)
        {
            playerinfo.Atk01CoolON = true;
            PlayerSkillState = PLAYERMOVE.USEDSKILL.ATK01;
            PlayerActionState = PLAYERMOVE.ACTIONSTATE.ATTACK;
            anim.SetTrigger("atk1");
        }        
    }

    //스킬공격의 쿨타임이 아니고 마나(비용)보다 마나가 많을때 사용 가능
    internal void UseAtk4()
    {
        if(playerinfo.Atk04CoolON == false)
        {
            if (playerinfo.CostAtk04())
            {
                playerinfo.Atk04CoolON = true;
                PlayerSkillState = PLAYERMOVE.USEDSKILL.ATK04;
                PlayerActionState = PLAYERMOVE.ACTIONSTATE.ATTACK;
                anim.SetTrigger("atk4");
            }
        }        
    }

    //플레이어의 버프스킬이 쿨타임이 아니고 마나(비용)보다 마나가 많을때 사용가능
    internal void UseBuff()//애니메이터에는 show로 되어있음
    {
        if(playerinfo.BuffCoolON == false)
        {
            if (playerinfo.CostBuff())
            {
                playerinfo.BuffCoolON = true;
                playerinfo.BuffOn();
                BuffHandEffect.SetActive(true);
                BuffEffect.SetActive(true);
                PlayerSkillState = PLAYERMOVE.USEDSKILL.BUFF;
                PlayerActionState = PLAYERMOVE.ACTIONSTATE.ATTACK;
                anim.SetTrigger("show");
                anim.SetFloat("attackspeed", 2.0f);
            }
        }        
    }

    //버프 스킬 이펙트를 위한 이펙트를 생성
    //캐릭터 transform을 부모로 지정하고 off 상태로 변경
    private void BuffEffectInit()
    {
        BuffHandEffect = Instantiate(Resources.Load("Prefab/Effect/Effect7_Hand"),
            SwordObject.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        BuffHandEffect.transform.SetParent(SwordObject.transform);
        BuffEffect = Instantiate(Resources.Load("Prefab/Effect/Effect21"),
            tr.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        BuffEffect.transform.SetParent(tr);
        BuffEffectOff();
    }

    internal void BuffEffectOff()
    {
        BuffHandEffect.SetActive(false);
        BuffEffect.SetActive(false);
    }
    #endregion

    #region -SkillState-
    //캐릭터의 스킬 사용 상태
    internal void SetStateNoSkill()
    {
        PlayerSkillState = PLAYERMOVE.USEDSKILL.NOSKILL;
    }

    internal PLAYERMOVE.USEDSKILL GetSkillState()
    {
        return PlayerSkillState;
    }

    #endregion

    #region -ActionState-
    //캐릭터의 행동 상태 변경
    internal void SetACTStateELSE()
    {
        PlayerActionState = PLAYERMOVE.ACTIONSTATE.ELSE;
    }

    internal void SetACTStateHURT()
    {
        PlayerActionState = PLAYERMOVE.ACTIONSTATE.HURT;
    }

    #endregion
    
}

