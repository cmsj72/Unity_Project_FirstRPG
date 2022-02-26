using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPotionShop : MonoBehaviour
{
    private List<Item> NPCSellList;
    private GameObject PotionShop;

    private void Awake()
    {
        NPCSellList = new List<Item>();
        NPCSellList.Add(ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_HPPORTION - 1]);
        NPCSellList.Add(ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_MPPORTION - 1]);
        PotionShop = GameObject.Find("UI Root").transform.Find("PortionShop")
            .Find("PotionShop Scroll View").Find("PotionShopGrid").gameObject;
        foreach (Item element in NPCSellList)
        {
            ItemManage.instance.AddItemInInventoryAtTarget((ITEMID)element.ID, PotionShop);
        }

        Transform trShopGrid = PotionShop.transform;
        for (int count = 0; count < trShopGrid.childCount; count++)
        {
            trShopGrid.GetChild(count).gameObject.tag = "Shop";
        }
    }
}
