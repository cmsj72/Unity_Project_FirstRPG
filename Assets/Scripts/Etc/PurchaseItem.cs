using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : MonoBehaviour
{
    private UIInput uiInput;
    private int InputAmount;
    private UILabel Totalcal;
    private Item SelectedItem;
    private PlayerInfo playerinfo;

    private void Awake()
    {
        uiInput = transform.Find("amount").GetComponent<UIInput>();
        Totalcal = transform.Find("TotalCalBackGround").Find("TotalCal").GetComponent<UILabel>();
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        SelectedItem = new Item();
        InputAmount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Totalcal.text = "0G";
    }

    // Update is called once per frame
    void Update()
    {
        int.TryParse(uiInput.value, out InputAmount);
        if (InputAmount != 0)
        {
            Totalcal.text = (InputAmount * SelectedItem.SellPrice * -1).ToString() + "G";
        }
    }

    public void PurchasedItem()
    {
        if(InputAmount > 0 && playerinfo.GetPlayerGold() >= (SelectedItem.SellPrice * InputAmount))
        {
            for (int count = 0; count < InputAmount; count++)
            {
                ItemManage.instance.AddItemInInventory((ITEMID)SelectedItem.ID);
                playerinfo.AddAtPlayerItemList(ItemManage.instance.GetItem(SelectedItem.ItemName));
            }
            playerinfo.DecreaseGold(SelectedItem.SellPrice * InputAmount);
            OpenPurchaseWindow();
        }   
    }

    internal void SelectItem(string ItemName)
    {
        SelectedItem = ItemManage.instance.GetItem(ItemName);
    }

    public void OpenPurchaseWindow()
    {
        if(gameObject.activeSelf == false)
        {
            transform.parent.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
        else if(gameObject == true)
        {
            transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }        
    }
}
