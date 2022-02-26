using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataShow : MonoBehaviour
{
    public int SlotNumber;
    private UILabel label;

    private void Awake()
    {
        //UnityEditor.AssetDatabase.Refresh();
        label = transform.Find("SlotLabel").GetComponent<UILabel>();
    }

    private void Update()
    {
        if (SaveLoadManager.instance.CheckSlot(SlotNumber - 1))
        {
            label.text = "Name : " + SaveLoadManager.instance.GetSlot(SlotNumber - 1).data.Name + "\n" + 
                            " Level : " + SaveLoadManager.instance.GetSlot(SlotNumber - 1).data.Level.ToString();
        }
        else
        {
            label.text = "Empty Slot";
        }
    }

    public void SelectedSlot()
    {
        DataSelect.instance.SelectSlot(SlotNumber);
    }
}
