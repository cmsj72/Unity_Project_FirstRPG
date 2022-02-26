using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstan : MonoBehaviour
{
    public static EnemyInstan instance;
    private PlayerInfo playerInfo;

    private void Awake()
    {
        instance = this;

        playerInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        var TmpEnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject hud = GameObject.Find("HUD");
        for (int count = 0; count < TmpEnemyList.Length; count++)
        {
            GameObject tmp = Instantiate(Resources.Load("Prefab/UI/EnemyHPBar")) as GameObject;
            EnemyInfo EItmp = TmpEnemyList[count].GetComponent<EnemyInfo>();
            EItmp.HPBar = tmp.GetComponent<UISlider>();
            tmp.name = "EnemyHPBar" + count.ToString();
            tmp.AddComponent<GetTarget>();
            GetTarget gt = tmp.GetComponent<GetTarget>();
            gt.SetTargetTransform(TmpEnemyList[count].transform);
        }

        var TmpBossList = GameObject.FindGameObjectsWithTag("Boss");
        for(int count = 0; count < TmpBossList.Length; count++)
        {
            GameObject tmp = Instantiate(Resources.Load("Prefab/UI/BossBar")) as GameObject;
            BossInfo BItmp = TmpBossList[count].GetComponent<BossInfo>();
            BItmp.HPBar = tmp.transform.Find("BossHPBar").GetComponent<UISlider>();
            BItmp.MPBar = tmp.transform.Find("BossMPBar").GetComponent<UISlider>();
            tmp.name = "BossBar" + count.ToString();
            tmp.AddComponent<GetTarget>();
            GetTarget gt = tmp.GetComponent<GetTarget>();
            gt.SetTargetTransform(TmpBossList[count].transform);
        }

    }

    internal void AddNewEnemyHUD(GameObject target,int count)
    {
        GameObject tmp = Instantiate(Resources.Load("Prefab/UI/EnemyHPBar")) as GameObject;
        EnemyInfo EItmp = target.GetComponent<EnemyInfo>();
        EItmp.HPBar = tmp.GetComponent<UISlider>();
        tmp.name = "EnemyHPBar" + count.ToString();
        tmp.AddComponent<GetTarget>();
        GetTarget gt = tmp.GetComponent<GetTarget>();
        gt.SetTargetTransform(target.transform);
    }
}
