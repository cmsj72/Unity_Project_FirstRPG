using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private Transform trMenu;
    private List<GameObject> childList;
    private bool  bShowMenu = false;

    void Start()
    {
        childList = new List<GameObject>();
        trMenu = GetComponent<Transform>();
        GameObject Tmp;
        for(int i =0; i < trMenu.Find("MenuChild").childCount; i++)
        {
            Tmp = trMenu.Find("MenuChild").GetChild(i).gameObject;
            Tmp.SetActive(false);
            childList.Add(Tmp);
        }
    }

    public void MenuControl()
    {
        if(bShowMenu)
        {
            bShowMenu = false;
        }
        else
        {
            bShowMenu = true;
        }
        for(int index =0; index < childList.Count; index++)
        {
            childList[index].SetActive(bShowMenu);
            if(bShowMenu)
            {
                if(childList[index].transform.childCount > 1)
                {
                    childList[index].transform.GetChild(1).gameObject.SetActive(false);
                }                
            }
        }
    }
}
