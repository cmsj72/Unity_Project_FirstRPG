using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionUse : MonoBehaviour
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
        switch ((ITEMID)SelectedItem.ID)
        {
            case ITEMID.ID_HPPORTION:
                Totalcal.text = "+200HP";
                break;

            case ITEMID.ID_MPPORTION:
                Totalcal.text = "+50MP";
                break;
        }
    }

    public void UseItem()
    {
        switch((ITEMID)SelectedItem.ID)
        {
            case ITEMID.ID_HPPORTION:
                playerinfo.IncreaseCurHP(200.0f);
                break;

            case ITEMID.ID_MPPORTION:
                playerinfo.IncreaseCurMP(50.0f);
                break;
        }
        playerinfo.DeleteItemInInven(SelectedObject);
        OpenPotionUseWindow();
    }

    internal void SelectItem(string ItemName)
    {
        SelectedItem = ItemManage.instance.GetItem(ItemName);
    }

    internal bool IsSelectObject(GameObject gobject)
    {
        if(gobject.name == ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_HPPORTION - 1].ItemName
            || gobject.name == ItemManage.instance.GetItemDataList()[(int)ITEMID.ID_MPPORTION - 1].ItemName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void SelectObject(GameObject gobject)
    {
        SelectedObject = gobject;
    }

    public void OpenPotionUseWindow()
    {
        if (gameObject.activeSelf == false)
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
