using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInfo : MonoBehaviour
{   
    public enum PSTATE
    {
        ALIVE,
        DIE,
        MENU,
        CLEAR,
    }

    private string              PlayerName;
    private string              PlayerClass;
    private float               PlayerMaxHP;
    private float               PlayerCurHP;
    private float               PlayerMaxMP;
    private float               PlayerCurMP;
    private int                 PlayerGold;
    private int                 PlayerLevel;
    private float               PlayerMaxEXP;
    private float               PlayerCurEXP;

    private PSTATE              PlayerState;
    private PlayerMove          playerMove;
    private UISlider            uiHPBar, uiMPBar, uiEXPBar;
    private Animator            anim;
    private BoxCollider         boxcoll;
    private List<Item>          PlayerItemList = new List<Item>();
    private List<GameObject>    ItemObjectList = new List<GameObject>();
    public List<int> IndexList = new List<int>();
    private List<Dictionary<string, object>> LvTable;
    private List<Dictionary<string, object>> ClassTable;
    private ScrollControl Tab1ScrllCon;
    private ScrollControl Tab2ScrllCon;


    private float   Atk01Cost = 5.0f;
    private float   Atk01Damage = 20.0f;
    private float   Atk01CoolTime = 0.5f;
    private float   Atk01CurCool = 0.0f;
    public  bool    Atk01CoolON { get; set; }

    private float   Atk04Cost = 10.0f;
    private float   Atk04Damage = 20.0f;
    private float   Atk04CoolTime = 3.0f;
    private float   Atk04CurCool = 0.0f;
    public  bool    Atk04CoolON { get; set; }

    private float   BuffCost = 30.0f;
    private float   BuffPow = 1.0f;
    private float   BuffCoolTime = 40.0f;
    private float   BuffCurCool = 0.0f;
    private float   BuffDurationTime = 20.0f;
    private float   BuffCurDuration = 0.0f;
    public  bool    BuffFlag { get; set; }
    public  bool    BuffCoolON { get; set; }

    private void Awake()
    {
        //UnityEditor.AssetDatabase.Refresh();
        LvTable = CSVReader.Read("Data/PlayerLvTable");
        ClassTable = CSVReader.Read("Data/ClassTable");        

        if(SaveLoadManager.instance.playselect == SaveLoadManager.PLAYSELECT.NEW)
        {
            NewGame();
        }
        else if(SaveLoadManager.instance.playselect == SaveLoadManager.PLAYSELECT.CON)
        {
            ContinueGame(SaveLoadManager.instance.GetLoadedPlayerData());
        }
        else if(SaveLoadManager.instance.playselect == SaveLoadManager.PLAYSELECT.NOTSAV_CON
            || SaveLoadManager.instance.playselect == SaveLoadManager.PLAYSELECT.SAV)
        {
            ContinueGame(SaveLoadManager.instance.GetPlayerDataForSave());
        }

        uiHPBar = GameObject.Find("HPBar").GetComponent<UISlider>();
        uiMPBar = GameObject.Find("MPBar").GetComponent<UISlider>();
        uiEXPBar = GameObject.Find("EXPBar").GetComponent<UISlider>();
        playerMove = GetComponent<PlayerMove>();
        anim = GetComponent<Animator>();
        boxcoll = GetComponent<BoxCollider>();

        Tab1ScrllCon = GameObject.Find("UI Root").transform.Find("MENUButton")
           .Find("MenuChild").Find("InvenButton").Find("Inventory")
           .Find("Tab1").Find("Tab1Scroll").Find("Scroll View1").GetComponent<ScrollControl>();
        Tab2ScrllCon = GameObject.Find("UI Root").transform.Find("MENUButton")
            .Find("MenuChild").Find("InvenButton").Find("Inventory")
            .Find("Tab2").Find("Tab2Scroll").Find("Scroll View2").GetComponent<ScrollControl>();
    }

    private void Start()
    {
        InitInven();
    }

    void Update()
    {
        uiHPBar.value = PlayerCurHP / PlayerMaxHP;
        uiMPBar.value = PlayerCurMP / PlayerMaxMP;
        uiEXPBar.value = PlayerCurEXP / PlayerMaxEXP;
    }

    private void LateUpdate()
    {
        CheckCurHP();
    }

    private void OnTriggerEnter(Collider other)//아이템 콜리더와 충돌했을경우
    {
        //골드인경우 습득한 골드의 양 만큼 획득 문자 출력, 드랍된 오브젝트 제거
        if (other.tag == "Gold")
        {
            string tmp = other.gameObject.GetComponent<ItemInfo>().GetItemName();
            tmp = tmp.Substring(0, tmp.Length - 1);
            int gold = int.Parse(tmp);
            IncreaseGold(gold);
            Destroy(other.gameObject);
            HUDManager.instance.PrintGetItem(tmp + "G");
        }
        //아이템인 경우 플레이어의 아이템 소유 리스트에 추가
        else if (other.tag == "Item")
        {
            ItemInfo othersinfo = other.gameObject.GetComponent<ItemInfo>();
            PlayerItemList.Add(ItemManage.instance.GetItem(othersinfo.GetItemName()));
            foreach(Item element in ItemManage.instance.GetItemDataList())
            {
                if(othersinfo.GetItemName() == element.ItemName)
                {
                    ItemManage.instance.AddItemInInventory((ITEMID)element.ID);
                    HUDManager.instance.PrintGetItem(element.ItemName);
                }
            }            
            Destroy(other.gameObject);
        }
    }

    #region -Skill-       

    //Atk01데미지 
    internal float GetAtk01Damage()
    {
        if (BuffFlag)
        {
            return Atk01Damage + (Atk01Damage * BuffPow);
        }
        else
        {
            return Atk01Damage;
        }
    }

    internal float Atk01CoolRate()
    {
        return Atk01CurCool / Atk01CoolTime;
    }

    internal void Atk01CoolingDown(float time)
    {
        if(Atk01CurCool + time >= Atk01CoolTime)
        {
            Atk01CurCool = 0.0f;
            Atk01CoolON = false;
        }
        else
        {
            Atk01CurCool += time;
        }        
    }

    //Atk04데미지
    internal float GetAtk04Damage()
    {
        if (BuffFlag)
        {
            return Atk04Damage + (Atk04Damage * BuffPow);
        }
        else
        {
            return Atk04Damage;
        }
    }

    internal float Atk04CoolRate()
    {
        return Atk04CurCool / Atk04CoolTime;
    }

    internal void Atk04CoolingDown(float time)
    {
        if(Atk04CurCool + time >= Atk04CoolTime)
        {
            Atk04CurCool = 0.0f;
            Atk04CoolON = false;
        }
        else
        {
            Atk04CurCool += time;
        }
    }

    //버프 사용
    internal void BuffOn()
    {
        BuffFlag = true;
    }

    internal float BuffCoolRate()
    {
        return BuffCurCool / BuffCoolTime;
    }

    internal void BuffCoolingDown(float time)
    {
        if(BuffCurCool + time >= BuffCoolTime)
        {
            BuffCurCool = 0.0f;
            BuffCoolON = false;
        }
        else
        {
            BuffCurCool += time;
        }
    }

    internal float BuffDurationRate()
    {
        return BuffCurDuration / BuffDurationTime;
    }

    internal void BuffDurationDown(float time)
    {
        if(BuffCurDuration +time >= BuffDurationTime)
        {
            playerMove.BuffEffectOff();
            BuffCurDuration = 0.0f;
            BuffFlag = false;
            anim.SetFloat("attackspeed", 1.0f);
        }
        else
        {
            BuffCurDuration += time;
        }
    }

    #endregion

    #region -HP-
    private void CheckCurHP()
    {
        if (PlayerCurHP <= 0.0f)
        {
            PlayerState = PSTATE.DIE;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("stun"))
            {
                anim.SetTrigger("stun");
                anim.SetBool("stunflag", true);
            }
        }
    }

    internal float GetPlayerCurHP()
    {
        return PlayerCurHP;
    }

    internal float GetPlayerMaxHP()
    {
        return PlayerMaxHP;
    }

    internal void IncreaseCurHP(float HP)
    {
        PlayerCurHP += HP;
        if(PlayerCurHP >= PlayerMaxHP)
        {
            PlayerCurHP = PlayerMaxHP;
        }
    }

    internal void DecreaseCurHP(float Damage)
    {
        if (PlayerCurHP > 0)
        {
            PlayerCurHP -= Damage;
        }
    }
    #endregion

    #region -MP-
    internal float GetPlayerCurMP()
    {
        return PlayerCurMP;
    }

    internal float GetPlayerMaxMP()
    {
        return PlayerMaxMP;
    }

    internal void IncreaseCurMP(float MP)
    {
        PlayerCurMP += MP;
        if(PlayerCurMP >= PlayerMaxMP)
        {
            PlayerCurMP = PlayerMaxMP;
        }
    }

    internal void DecreaseCurMP(float usedMP)
    {
        PlayerCurMP -= usedMP;
    }

    internal void CostAtk01()//Atk01은 사용때마다 5회복된다
    {
        if(Atk01Cost + PlayerCurMP >= PlayerMaxMP)
        {
            IncreaseCurMP(PlayerMaxMP - PlayerCurMP);
        }
        else
        {
            IncreaseCurMP(Atk01Cost);
        }        
    }

    internal bool CostAtk04()
    {
        if(PlayerCurMP >= Atk04Cost)
        {
            PlayerCurMP -= Atk04Cost;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    internal bool CostBuff()
    {
        if(PlayerCurMP >= BuffCost)
        {
            PlayerCurMP -= BuffCost;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region -Gold-
    internal void IncreaseGold(int gold)
    {
        PlayerGold += gold;
    }

    internal void DecreaseGold(int gold)
    {
        if(PlayerGold >= gold)
        {
            PlayerGold -= gold;
        }        
    }

    internal int GetPlayerGold()
    {
        return PlayerGold;
    }
    #endregion

    #region -EXP-

    internal void IncreaseExp(float Exp)
    {
        if(PlayerCurEXP + Exp >= PlayerMaxEXP)
        {
            PlayerCurEXP = Exp - (PlayerMaxEXP - PlayerCurEXP);
            PlayerLevel += 1;
            HUDManager.instance.PrintLevelUp();
            StatUpdate();
        }
        else
        {
            PlayerCurEXP += Exp;
        }        
    }

    internal float GetPlayerCurEXP()
    {
        return PlayerCurEXP;
    }

    internal float GetPlayerMaxEXP()
    {
        return PlayerMaxEXP;
    }

    #endregion

    #region -ETC_

    internal string GetPlayerName()
    {
        return PlayerName;
    }

    internal string GetPlayerClass()
    {
        return PlayerClass;
    }

    internal int GetPlayerLevel()
    {
        return PlayerLevel;
    }    

    internal void GetHurted()
    {
        playerMove.SetACTStateHURT();
        anim.SetTrigger("hurt");
        if (PlayerCurHP <= 0.0f)
        {
            anim.SetBool("stunflag", true);
        }
    }

    internal PSTATE GetPSTATE()
    {
        return PlayerState;
    }

    internal List<Item> GetPlayerItemList()
    {
        return PlayerItemList;
    }

    internal List<GameObject> GetItemObjectList()
    {
        return ItemObjectList;
    }

    internal void AddAtItemObjectList(GameObject obj)
    {
        ItemObjectList.Add(obj);
    }

    internal void AddAtPlayerItemList(Item item)
    {
        PlayerItemList.Add(item);
    }

    internal void DeleteItemInInven(GameObject gObject)
    {
        int DelIndex = gObject.GetComponent<ItemInfo>().Index;
        PlayerItemList.RemoveAt(DelIndex);
        ItemObjectList.RemoveAt(DelIndex);        
        Destroy(gObject);

        for(int index = 0; index < PlayerItemList.Count; index++)
        {
            ItemObjectList[index].GetComponent<ItemInfo>().ChangeIndex(index);
        }

        Tab1ScrllCon.RePositionInven();
        Tab2ScrllCon.RePositionInven();
    }

    /// <summary>
    /// 0 : ALIVE, 1 : DIE, 2 : MENU, 3 : CLEAR
    /// </summary>
    internal void SetPSTATE(int state)
    {
        switch(state)
        {
            case 0:
                PlayerState = PSTATE.ALIVE;
                break;

            case 1:
                PlayerState = PSTATE.DIE;
                break;

            case 2:
                PlayerState = PSTATE.MENU;
                break;

            case 3:
                PlayerState = PSTATE.CLEAR;
                break;
        }        
    }

    private void StatUpdate()
    {
        PlayerMaxEXP = (int)LvTable[PlayerLevel - 1]["MAXEXP"];
        PlayerMaxHP = PlayerCurHP = (int)LvTable[PlayerLevel - 1]["MAXHP"];
        PlayerMaxMP = PlayerCurMP = (int)LvTable[PlayerLevel - 1]["MAXMP"];
    }    

    private void NewGame()
    {
        PlayerLevel = (int)LvTable[0]["LEVEL"];
        PlayerMaxEXP = (int)LvTable[0]["MAXEXP"];
        PlayerCurEXP = 0;
        PlayerMaxHP = PlayerCurHP = (int)LvTable[0]["MAXHP"];
        PlayerMaxMP = PlayerCurMP = (int)LvTable[0]["MAXMP"];
        PlayerState = PSTATE.ALIVE;
        PlayerName = TempInfoManage.instance.PlayerNameTemp;
        PlayerClass = (string)ClassTable[0]["Class"];
    }

    private void ContinueGame(SaveLoadManager.PlayerData data)
    {
        PlayerLevel = data.Level;
        PlayerMaxEXP = (int)LvTable[PlayerLevel - 1]["MAXEXP"];
        PlayerCurEXP = (float)data.Exp;
        PlayerMaxHP = PlayerCurHP = (int)LvTable[PlayerLevel - 1]["MAXHP"];
        PlayerMaxMP = PlayerCurMP = (int)LvTable[PlayerLevel - 1]["MAXMP"];
        PlayerState = PSTATE.ALIVE;
        PlayerName = data.Name;
        PlayerClass = data.Class;
        PlayerGold = data.Gold;
        switch(SaveLoadManager.instance.playselect)
        {
            case SaveLoadManager.PLAYSELECT.CON:
                foreach (Item element in SaveLoadManager.instance.GetLoadedItemList())
                {
                    PlayerItemList.Add(element);
                }
                break;

            case SaveLoadManager.PLAYSELECT.NOTSAV_CON:
            case SaveLoadManager.PLAYSELECT.SAV:
                foreach(Item element in SaveLoadManager.instance.GetSaveItemList())
                {
                    PlayerItemList.Add(element);
                }
                break;
        }        
    }

    private void InitInven()
    {
        if(PlayerItemList.Count != 0)
        {
            foreach(Item element in PlayerItemList)
            {
                ItemManage.instance.AddItemInInventory((ITEMID)element.ID);
            }
        }
    }   

    #endregion
}
