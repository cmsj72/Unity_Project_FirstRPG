using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    private string          itemName;
    private string          CurSceneName;
    private PurchaseItem    purChaseitem;
    private SellItem        sellitem;
    private PotionUse       potionUse;


    public  int Index;

    private void Awake()
    {
        CurSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        switch(CurSceneName)
        {
            case "Camp":
                purChaseitem = GameObject.Find("UI Root").transform.Find("PurchaseWindowPanel")
                    .Find("PurchaseWindow").GetComponent<PurchaseItem>();
                sellitem = GameObject.Find("UI Root").transform.Find("SellWindowPanel")
                    .Find("SellWindow").GetComponent<SellItem>();
                break;

            case "Field":
            case "Arena":
                potionUse = GameObject.Find("UI Root").transform.Find("PotionUseWindowPanel")
                    .Find("PotionUseWindow").GetComponent<PotionUse>();
                break;
        }
    }

    internal void SetItemName(string name)
    {
        itemName = name;
    }

    internal string GetItemName()
    {
        return itemName;
    }

    internal void GetNameForPurchaseSell()
    {
        switch(tag)
        {
            case "Shop":
                purChaseitem.SelectItem(gameObject.name);
                break;

            case "Inven":
                sellitem.SelectItem(gameObject.name);
                break;
        }
    }

    internal void PurchaseAndSell()
    {
        if(gameObject.tag == "Shop")
        {
            purChaseitem.OpenPurchaseWindow();
        }
        else if(gameObject.tag == "Inven")
        {
            sellitem.OpenSellWindow();
            sellitem.SelectObject(gameObject);
        }
    }

    internal void PotionUse()
    {
        if (gameObject.tag == "Inven")
        {
            if(potionUse == null)
            {
                potionUse = GameObject.Find("UI Root").transform.Find("PotionUseWindowPanel")
                    .Find("PotionUseWindow").GetComponent<PotionUse>();
            }
            if(potionUse.IsSelectObject(gameObject))
            {
                potionUse.OpenPotionUseWindow();
                potionUse.SelectObject(gameObject);
                potionUse.SelectItem(gameObject.name);
            }
        }
    }

    internal void ChangeIndex(int index)
    {
        Index = index;
    }

    private void OnClick()
    {
        switch(CurSceneName)
        {
            case "Camp":
                PurchaseAndSell();
                GetNameForPurchaseSell();
                break;

            case "Field":
            case "Arena":
            case "Dungeon":
                PotionUse();
                break;
        }
    }
}
