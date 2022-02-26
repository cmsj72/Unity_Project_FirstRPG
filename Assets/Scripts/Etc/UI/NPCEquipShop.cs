using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEquipShop : MonoBehaviour
{
    private List<Item> NPCSellList;
    private GameObject EquipShop;

    private void Awake()
    {
        NPCSellList = new List<Item>();
        NPCSellList.Add(ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_SHOULDER_ARMOR - 1]);
        NPCSellList.Add(ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_BOOTS - 1]);
        EquipShop = GameObject.Find("UI Root").transform.Find("EquipShop")
            .Find("EquipShop Scroll View").Find("EquipShopGrid").gameObject;
        foreach(Item element in NPCSellList)
        {
            ItemManage.instance.AddItemInInventoryAtTarget((ITEMID)element.ID, EquipShop);
        }

        Transform trShopGrid = EquipShop.transform;
        for(int count =0; count < trShopGrid.childCount; count++)
        {
            trShopGrid.GetChild(count).gameObject.tag = "Shop";
        }
    }
}
