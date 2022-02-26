using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    private PlayerInfo playerinfo;
    private GameObject HUDTextPrefab;
    private GameObject HUDRoot;
    private GameObject player;
    private HUDText levelup;
    private HUDText mission;
    private HUDText clear;
    private HUDText item;

    ////Atk01
    private GameObject NormalAtkCoolObj;
    private UI2DSprite NormalAtkCool;

    //Show
    private GameObject BuffCoolObj;
    private UI2DSprite BuffCool;

    private GameObject BuffOnObj;
    private UI2DSprite BuffOn;

    //Atk04
    private GameObject PowerAtkCoolObj;
    private UI2DSprite PowerAtkCool;

    private void Awake()
    {
        instance = this;
        if(GameObject.Find("Player"))
        {
            player = GameObject.Find("Player");
        }        
        playerinfo = player.GetComponent<PlayerInfo>();
        StartCoroutine(NormalAtkCoolDown());
        StartCoroutine(BuffCoolDown());
        StartCoroutine(BuffDurationDown());
        StartCoroutine(PowerAtkCoolDown());
    }

    // Start is called before the first frame update    
    void Start()
    {        
        HUDTextPrefab = Resources.Load("Prefab/UI/HUDText") as GameObject;
        HUDRoot = GameObject.Find("HUD");

        GameObject temp = NGUITools.AddChild(HUDRoot, HUDTextPrefab);
        temp.name = "LevelUp";
        levelup = temp.GetComponentInChildren<HUDText>();
        temp.AddComponent<UIFollowTarget>().target = player.transform;

        temp = NGUITools.AddChild(HUDRoot, HUDTextPrefab);
        temp.name = "ETC";
        mission = temp.GetComponentInChildren<HUDText>();
        temp.AddComponent<UIFollowTarget>().target = player.transform;

        clear = temp.GetComponentInChildren<HUDText>();
        temp.AddComponent<UIFollowTarget>().target = player.transform;

        temp = NGUITools.AddChild(HUDRoot, HUDTextPrefab);
        temp.name = "GainItem";
        item = temp.GetComponentInChildren<HUDText>();
        temp.AddComponent<UIFollowTarget>().target = player.transform;

        InitSkillObject();
        
    }

    private void InitSkillObject()
    {
        NormalAtkCoolObj = GameObject.Find("NormalAtkCool");
        NormalAtkCool = NormalAtkCoolObj.GetComponent<UI2DSprite>();
        NormalAtkCoolObj.SetActive(false);


        BuffCoolObj = GameObject.Find("BuffCool");
        BuffCool = GameObject.Find("BuffCool").GetComponent<UI2DSprite>();
        BuffCoolObj.SetActive(false);

        BuffOnObj = GameObject.Find("BuffOn");
        BuffOn = BuffOnObj.GetComponent<UI2DSprite>();
        BuffOnObj.SetActive(false);

        PowerAtkCoolObj = GameObject.Find("PowerAtkCool");
        PowerAtkCool = GameObject.Find("PowerAtkCool").GetComponent<UI2DSprite>();
        PowerAtkCoolObj.SetActive(false);
    }

    internal void PrintLevelUp()
    {
        levelup.Add("LEVELUP", Color.red, 1.0f);
    }

    internal void PrintEliminateMission()
    {
        mission.Add("Eliminate all enemies", Color.white, 2.0f);
    }

    internal void PrintRescueMission()
    {
        mission.Add("Rescue Orc and Eleminate All enemies", Color.white, 2.0f);
    }

    internal void PrintRescuedNPC()
    {
        mission.Add("Rescued! " + PlayManage.instance.GetRescuedNPC() + " / 3", Color.red, 1.0f);
    }

    internal void PrintClear()
    {
        clear.Add("CLEAR", Color.yellow, 3.0f);
    }

    internal void PrintGetItem(string ItemName)
    {
        item.Add(ItemName, Color.blue, 1.0f);
    }


    IEnumerator NormalAtkCoolDown()
    {
        while(true)
        {
            if(playerinfo.Atk01CoolON == true)
            {
                if(NormalAtkCoolObj.activeSelf == false)
                {
                    NormalAtkCoolObj.SetActive(true);
                }
                playerinfo.Atk01CoolingDown(Time.deltaTime);
                NormalAtkCool.fillAmount = 1 - playerinfo.Atk01CoolRate();
                if(NormalAtkCool.fillAmount == 1)
                {
                    NormalAtkCoolObj.SetActive(false);
                }
            }
            yield return null;
        }
    }

    IEnumerator BuffCoolDown()
    {
        while(true)
        {
            if(playerinfo.BuffCoolON == true)
            {                
                if(BuffCoolObj.activeSelf == false)
                {
                    BuffCoolObj.SetActive(true);
                }
                playerinfo.BuffCoolingDown(Time.deltaTime);
                BuffCool.fillAmount = 1 - playerinfo.BuffCoolRate();
                if(BuffCool.fillAmount == 1)
                {
                    BuffCoolObj.SetActive(false);
                }
            }
            yield return null;
        }
    }

    IEnumerator BuffDurationDown()
    {
        while (true)
        {
            if (playerinfo.BuffFlag == true)
            {
                if (BuffOnObj.activeSelf == false)
                {
                    BuffOnObj.SetActive(true);
                }
                playerinfo.BuffDurationDown(Time.deltaTime);
                BuffOn.fillAmount = 1 - playerinfo.BuffDurationRate();
                if (BuffOn.fillAmount == 1)
                {
                    BuffOnObj.SetActive(false);
                }
            }
            yield return null;
        }
    }

    IEnumerator PowerAtkCoolDown()
    {
        while(true)
        {
            if(playerinfo.Atk04CoolON == true)
            {
                if(PowerAtkCoolObj.activeSelf == false)
                {
                    PowerAtkCoolObj.SetActive(true);
                }
                playerinfo.Atk04CoolingDown(Time.deltaTime);
                PowerAtkCool.fillAmount = 1 - playerinfo.Atk04CoolRate();
                if(PowerAtkCool.fillAmount == 1)
                {
                    PowerAtkCoolObj.SetActive(false);
                }
            }
            yield return null;
        }
    }
}