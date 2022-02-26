using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public int MenuID;

    /// <summary>
    /// 1 : 새 게임(플레이어 이름입력하는 화면)
    /// 2 : 이어하기(타이틀->세이브목록)
    /// 3 : 세이브하기(게임->세이브목록)
    /// 4 : 캠프
    /// 5 : 타이틀화면으로
    /// </summary>
    public void SelectMenu()
    {
        switch(MenuID)
        {
            case 1:
                SceneManager.LoadScene("SetPlayerName");
                SaveLoadManager.instance.playselect = SaveLoadManager.PLAYSELECT.NEW;
                break;

            case 2:
                SceneManager.LoadScene("SaveList");
                SaveLoadManager.instance.playselect = SaveLoadManager.PLAYSELECT.CON;
                break;

            case 3:
                SceneManager.LoadScene("SaveList");
                SaveLoadManager.instance.playselect = SaveLoadManager.PLAYSELECT.SAV;
                break;

            case 4:
                SceneManager.LoadScene("Camp");
                break;

            case 5:
                SceneManager.LoadScene("Title");
                break;
        }
    }
}
