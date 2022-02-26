using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

enum ITEMID
{
    ID_GOLD = 1,
    ID_SHOULDER_ARMOR = 2,
    ID_BOOTS = 3,
    ID_HPPORTION = 4,
    ID_MPPORTION = 5,
}

public class Item
{
    public int ID;
    public string ItemName;
    public int SellPrice;

    public Item(int id = 0, string itemname = "", int price = 0)
    {
        ID = id;        
        ItemName = itemname;
        SellPrice = price;
    }
}

public class ItemManage : MonoBehaviour
{
    public static ItemManage instance;
    private TextAsset ItemDataText;
    private PlayerInfo playerinfo;
    private GameObject Tab1;
    private GameObject Tab2;
    private List<Dictionary<string, object>> ItemTable;
    private List<Item> ItemDataList = new List<Item>();
    internal int ItemMax;
    
    private void Awake()
    {
        instance = this;
        SaveItemInfo();
        SaveJsonFile();
        Tab1 = GameObject.Find("UI Root").transform.Find("MENUButton")
            .Find("MenuChild").Find("InvenButton").Find("Inventory")
            .Find("Tab1").Find("Tab1Scroll").Find("Scroll View1").Find("Tab1Grid").gameObject;
        Tab2 = GameObject.Find("UI Root").transform.Find("MENUButton")
            .Find("MenuChild").Find("InvenButton").Find("Inventory")
            .Find("Tab2").Find("Tab2Scroll").Find("Scroll View2").Find("Tab2Grid").gameObject;
        ItemMax = ItemTable.Count;
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    private void SaveItemInfo()
    {
        ItemTable = CSVReader.Read("Data/ItemTable");
        for(int i =0; i < ItemTable.Count; i++)
        {
            ItemDataList.Add(new Item((int)ItemTable[i]["ID"], 
                (string)ItemTable[i]["ItemName"],
                (int)ItemTable[i]["SellPrice"]));
        }
    }
    
    private void SaveJsonFile()
    {
        JsonData ItemJson = JsonMapper.ToJson(ItemDataList);

        File.WriteAllText(Application.dataPath + "/Resources/Data/ItemInfoData.json", ItemJson.ToString());
    }

    internal void DropItem(Transform EnemyTf,int index)
    {
        GameObject Objt = Instantiate(Resources.Load("Prefab/Object/Item"), EnemyTf.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Transform itemTf = Objt.GetComponent<Transform>();
        GameObject ObjtLabel = Instantiate(Resources.Load("Prefab/UI/ItemName")) as GameObject;
        GetTarget gt = ObjtLabel.GetComponent<GetTarget>();
        gt.SetTargetTransform(itemTf);
        if (ItemDataList[index].ID == (int)ITEMID.ID_GOLD)//골드인 경우
        {
            Objt.tag = "Gold";
            int Gold = Random.Range(1, 1001);
            gt.SetLabel(Gold.ToString() + "G");
        }
        else//골드가 아닌경우
        {
            Objt.tag = "Item";
            gt.SetLabel(ItemDataList[index].ItemName);
        }
    }

    internal Item GetItem(string itemname)
    {
        foreach(Item tmp in ItemDataList)
        {
            if(tmp.ItemName == itemname)
            {
                return tmp;
            }
        }
        return null;
    }

    /// <summary>
    /// 플레이어의 인벤토리에 추가
    /// </summary>
    /// <param name="ID"></param>
    internal void AddItemInInventory(ITEMID ID)
    {        
        switch (ID)
        {
            case ITEMID.ID_BOOTS:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSprite"), Tab1.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    tmp.tag = "Inven";
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();                    
                    SpriteTmp.spriteName = "Orc Armor - Boots";
                    tmp.GetComponent<ItemInfo>().Index = playerinfo.GetItemObjectList().Count;
                    playerinfo.IndexList.Add(playerinfo.GetItemObjectList().Count);
                    playerinfo.AddAtItemObjectList(tmp);
                }                
                break;

            case ITEMID.ID_SHOULDER_ARMOR:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSprite"), Tab1.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    tmp.tag = "Inven";
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();                    
                    SpriteTmp.spriteName = "Orc Armor - Shoulders";
                    tmp.GetComponent<ItemInfo>().Index = playerinfo.GetItemObjectList().Count;
                    playerinfo.IndexList.Add(playerinfo.GetItemObjectList().Count);
                    playerinfo.AddAtItemObjectList(tmp);
                }                
                break;

            case ITEMID.ID_HPPORTION:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSpritePot"), Tab2.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    tmp.tag = "Inven";
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "RedPotionOn";
                    tmp.GetComponent<ItemInfo>().Index = playerinfo.GetItemObjectList().Count;
                    playerinfo.IndexList.Add(playerinfo.GetItemObjectList().Count);
                    playerinfo.AddAtItemObjectList(tmp);
                }
                break;

            case ITEMID.ID_MPPORTION:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSpritePot"), Tab2.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    tmp.tag = "Inven";
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "BluePotionOn";
                    tmp.GetComponent<ItemInfo>().Index = playerinfo.GetItemObjectList().Count;
                    playerinfo.IndexList.Add(playerinfo.GetItemObjectList().Count);
                    playerinfo.AddAtItemObjectList(tmp);
                }
                break;
        }
    }

    /// <summary>
    /// 대상의 인벤토리에 추가
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="Target"></param>
    internal GameObject AddItemInInventoryAtTarget(ITEMID ID, GameObject Target)
    {
        switch (ID)
        {
            case ITEMID.ID_BOOTS:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSprite"), Target.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "Orc Armor - Boots";
                    return tmp;
                }

            case ITEMID.ID_SHOULDER_ARMOR:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSprite"), Target.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "Orc Armor - Shoulders";
                    return tmp;
                }

            case ITEMID.ID_HPPORTION:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSpritePot"), Target.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "RedPotionOn";
                    return tmp;
                }

            case ITEMID.ID_MPPORTION:
                {
                    GameObject tmp = Instantiate(Resources.Load("Prefab/UI/ItemINInvenSpritePot"), Target.transform) as GameObject;
                    tmp.name = ItemDataList[(int)ID - 1].ItemName;
                    UISprite SpriteTmp = tmp.GetComponent<UISprite>();
                    SpriteTmp.spriteName = "BluePotionOn";
                    return tmp;
                }
        }
        return null;
    }

    internal List<Item> GetItemDataList()
    {
        return ItemDataList;
    }
    
}
