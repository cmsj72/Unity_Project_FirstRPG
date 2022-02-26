using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SaveLoadManager : MonoBehaviour
{
    static class Constants
    {
        internal const int ListMax = 3;
    }

    public enum PLAYSELECT
    {
        NEW,
        CON,
        SAV,
        NOTSAV_CON,
    }

    public class PlayerData
    {
        public string Name;
        public string Class;
        public int Level;
        public int Gold;
        public double Exp;
        public PlayerData(string name = "", string cls = "", int lv = 0, int gd = 0, double exp = 0)
        {
            Name = name;
            Class = cls;
            Level = lv;
            Gold = gd;
            Exp = exp;
        }
    }

    public class SaveDataSlot
    {
        public int number;
        public bool SaveFlag;
        public string ItemDataPath;
        public PlayerData data;
        public SaveDataSlot(int num, bool flag = false, string datapath = "")
        {
            number = num;
            SaveFlag = flag;
            ItemDataPath = datapath;
            data = new PlayerData();
        }
    }



    static public   SaveLoadManager     instance;
    private         List<SaveDataSlot>  PlayerDataList = new List<SaveDataSlot>();
    private         List<Item>          SaveItemList = new List<Item>();
    private         List<Item>          LoadedItemList = new List<Item>();
    private         TextAsset           SaveDataText;
    private         JsonData            SaveDataToJson;
    private         PlayerData          LoadedPlayerData = new PlayerData();
    private         PlayerData          PlayerDataForSave = new PlayerData();
    public          PLAYSELECT          playselect;
    internal        int                 LatestSlotNumer;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        if(!CheckSaveFile())
        {
            InitSaveFile();
        }
    }

    public void InitSaveFile()
    {
        for(int index =1; index <= Constants.ListMax; index++)
        {
            PlayerDataList.Add(new SaveDataSlot(index));
        }

        JsonData SaveJson = JsonMapper.ToJson(PlayerDataList);
        File.WriteAllText(Application.dataPath + "/Resources/Data/SaveFile.json", SaveJson.ToString());

        for(int count = 1; count <= Constants.ListMax; count++)
        {
            SaveJson = JsonMapper.ToJson(SaveItemList);
            File.WriteAllText(Application.dataPath + "/Resources/Data/SaveItemFile" + count.ToString() + ".json", SaveJson.ToString());
        }
    }

    public void SavePlayerData(int SlotNumber)
    {
        LatestSlotNumer = SlotNumber;
        PlayerDataList[SlotNumber].data = PlayerDataForSave;
        PlayerDataList[SlotNumber].SaveFlag = true;
        PlayerDataList[SlotNumber].ItemDataPath = "Data/SaveItemFile" + PlayerDataList[SlotNumber].number.ToString();
        JsonData SaveJson = JsonMapper.ToJson(PlayerDataList);
        if(File.Exists(Application.dataPath + "/Resources/Data/SaveFile.json"))
        {
            File.Delete(Application.dataPath + "/Resources/Data/SaveFile.json");
        }        
        File.WriteAllText(Application.dataPath + "/Resources/Data/SaveFile.json", SaveJson.ToString());

        SaveJson = JsonMapper.ToJson(SaveItemList);
        if(File.Exists(Application.dataPath + " /Resources/Data/SaveItemFile" + PlayerDataList[SlotNumber].number.ToString() + ".json"))
        {
            File.Delete(Application.dataPath + " /Resources/Data/SaveItemFile" + PlayerDataList[SlotNumber].number.ToString() + ".json");
        }        
        File.WriteAllText(Application.dataPath + " /Resources/Data/SaveItemFile" + PlayerDataList[SlotNumber].number.ToString() + ".json", SaveJson.ToString());
    }

    public bool CheckSaveFile()
    {
        TextAsset Data = Resources.Load("Data/SaveFile") as TextAsset;
        if(Data == null)
        {
            return false;
        }
        else
        {
            TextAsset ItemData;
            for(int count = 1; count <= Constants.ListMax; count++)
            {
                ItemData = Resources.Load("Data/SaveItemFile" + count.ToString()) as TextAsset;
                if(ItemData == null)
                {
                    return false;
                }
            }
            SaveDataText = Data;
            SaveDataToJson = JsonMapper.ToObject(SaveDataText.text);
            for(int index = 0; index < SaveDataToJson.Count; index++)
            {
                bool flag = bool.Parse(SaveDataToJson[index]["SaveFlag"].ToString());
                if(flag == false)
                {
                    PlayerDataList.Add(new SaveDataSlot(index + 1));
                }
                else
                {
                    SaveDataSlot SlotTemp = new SaveDataSlot(index + 1, true, SaveDataToJson[index]["ItemDataPath"].ToString());
                    PlayerData DataTemp = new PlayerData
                        (
                        SaveDataToJson[index]["data"]["Name"].ToString(),
                        SaveDataToJson[index]["data"]["Class"].ToString(),
                        int.Parse(SaveDataToJson[index]["data"]["Level"].ToString()),
                        int.Parse(SaveDataToJson[index]["data"]["Gold"].ToString()),
                        double.Parse(SaveDataToJson[index]["data"]["Exp"].ToString())
                        );
                    SlotTemp.data = DataTemp;
                    PlayerDataList.Add(SlotTemp);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 플레이어 데이터가 있으면 true 없으면 false
    /// </summary>
    /// <param name="SlotNumber"></param>
    /// <returns></returns>
    internal bool CheckSlot(int SlotNumber)
    {
        if(PlayerDataList[SlotNumber].SaveFlag == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    internal SaveDataSlot GetSlot(int SlotNumber)
    {
        return PlayerDataList[SlotNumber];
    }

    internal void SetPlayerData(int Number)
    {
        LatestSlotNumer = Number;
        LoadedPlayerData = PlayerDataList[Number].data;
        SetPlayerItemList(Number);
    }

    internal void SetPlayerItemList(int Number)
    {
        TextAsset data = Resources.Load(PlayerDataList[Number].ItemDataPath) as TextAsset;
        JsonData jsonData = JsonMapper.ToObject(data.text);
        for(int count = 0; count < jsonData.Count; count++)
        {
            LoadedItemList.Add(new Item(int.Parse(jsonData[count]["ID"].ToString()), 
                jsonData[count]["ItemName"].ToString(),
                int.Parse(jsonData[count]["SellPrice"].ToJson())));
        }
    }

    internal void SetPlayerDataForSave(PlayerData data, List<Item> itemlist)
    {
        PlayerDataForSave = data;
        SaveItemList.Clear();
        foreach(Item element in itemlist)
        {
            SaveItemList.Add(element);
        }
    }

    internal PlayerData GetLoadedPlayerData()
    {
        return LoadedPlayerData;
    }

    internal PlayerData GetPlayerDataForSave()
    {
        return PlayerDataForSave;
    }

    internal List<Item> GetLoadedItemList()
    {
        return LoadedItemList;
    }
    
    internal List<Item> GetSaveItemList()
    {
        return SaveItemList;
    }

    internal void OverWrite()
    {
        LoadedPlayerData = PlayerDataForSave;
        LoadedItemList.Clear();
        LoadedItemList = SaveItemList;
    }
}