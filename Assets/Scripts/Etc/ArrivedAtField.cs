using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivedAtField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string scenename = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch(scenename)
        {
            case "Field":
            case "Arena":
                PlayManage.instance.StartEliminateMission();
                break;

            case "Dungeon":
                PlayManage.instance.StartRescueMission();
                break;
        }
        
    }
}
