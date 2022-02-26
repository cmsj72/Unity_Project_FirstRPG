using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSelect : MonoBehaviour
{
    static public DataSelect instance;
    private int SelectedSlotNum = 0;

    private void Awake()
    {
        instance = this;
    }

    public void SelectSlot(int number)
    {
        SelectedSlotNum = number;
    }

    public void Confirm()
    {
        if (SelectedSlotNum != 0)
        {
            switch (SaveLoadManager.instance.playselect)
            {
                case SaveLoadManager.PLAYSELECT.CON:
                    if(SaveLoadManager.instance.GetSlot(SelectedSlotNum-1).SaveFlag == true)
                    {
                        SaveLoadManager.instance.SetPlayerData(SelectedSlotNum - 1);
                        SceneManager.LoadScene("Camp");
                    }                    
                    break;

                case SaveLoadManager.PLAYSELECT.SAV:
                    SaveLoadManager.instance.SavePlayerData(SelectedSlotNum - 1);
                    SceneManager.LoadScene("Camp");
                    break;
            }
        }
    }
}