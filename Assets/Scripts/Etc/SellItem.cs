using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    private Item SelectedItem;
    private GameObject SelectedObject;
    private UILabel Totalcal;
    private PlayerInfo playerinfo;

    private void Awake()
    {
        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        SelectedItem = new Item();
        Totalcal = transform.Find("TotalCalBackGround").Find("TotalCal").GetComponent<UILabel>();
    }

    private void Update()
    {
        Totalcal.text = "+" + (SelectedItem.SellPrice.ToString()) + "G";
    }

    public void SoldItem()
    {
        playerinfo.IncreaseGold(SelectedItem.SellPrice);
        playerinfo.DeleteItemInInven(SelectedObject);
        OpenSellWindow();
    }

    internal void SelectItem(string ItemName)
    {
        SelectedItem = ItemManage.instance.GetItem(ItemName);
    }

    internal void SelectObject(GameObject gobject)
    {
        SelectedObject = gobject;
    }

    public void OpenSellWindow()
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
