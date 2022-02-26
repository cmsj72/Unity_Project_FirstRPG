using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManage : MonoBehaviour
{
    public enum STAGEGOAL
    {
        FIRST_STAGEGOAL = 2,
        SECOND_STAGEGOAL = 6,
    }

    public enum CURRENTSTAGE
    {
        FIRST_STAGE = 1,
        SECOND_STAGE = 2,
        BOSS_STAGE = 3,
    }

    static public PlayManage instance;

    private GameObject Goal;
    private GameObject SpawnPoint;
    private List<GameObject> EnemyList;
    private List<GameObject> BossList;
    private List<GameObject> WallList;
    private PlayerInfo playerinfo;
    private bool PrintMissionFlag;
    private int KilledEnemy;
    private int SpawnEnemyCount;
    private CURRENTSTAGE CurStage;
    private string CurSceneName;
    private int RescuedNPC;

    private void Awake()
    {
        instance = this;

        playerinfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
        Goal = GameObject.Find("GoalPoint");
        Goal.SetActive(false);

        EnemyList = new List<GameObject>();
        var Tmp1 = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject obj in Tmp1)
        {
            EnemyList.Add(obj);
        }

        

        BossList = new List<GameObject>();
        var Tmp2 = GameObject.FindGameObjectsWithTag("Boss");
        foreach(GameObject obj in Tmp2)
        {
            BossList.Add(obj);
        }
        
        if(SceneManager.GetActiveScene().name == "Field")
        {
            WallInit();
        }
        if(SceneManager.GetActiveScene().name == "Arena")
        {
            SpawnEnemyCount = 0;
            SpawnPoint = GameObject.Find("SpawnPoint");
            SpawnPoint.SetActive(false);
            StartCoroutine(SpawnEnemy());
        }
        if(SceneManager.GetActiveScene().name == "Dungeon")
        {
            RescuedNPC = 0;
        }
    }

    private void WallInit()
    {
        WallList = new List<GameObject>();
        for(int count = 1; count <= 3; count++)
        {
            if(GameObject.Find("StageWall" + count.ToString()))
            {
                WallList.Add(GameObject.Find("StageWall" + count.ToString()));
            }
        }
    }

    private void Start()
    {
        PrintMissionFlag = true;
        CurSceneName = SceneManager.GetActiveScene().name;
        KilledEnemy = 0;
        if (EnemyList.Count == 0)
        {
            CurStage = CURRENTSTAGE.BOSS_STAGE;
        }
        else
        {
            CurStage = CURRENTSTAGE.FIRST_STAGE;
        }

    }

    void Update()
    {
        if (playerinfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
        {
            switch(CurStage)
            {
                case CURRENTSTAGE.FIRST_STAGE:
                case CURRENTSTAGE.SECOND_STAGE:
                    if(EnemyList.Count > 0)
                    {
                        for (int count = 0; count < EnemyList.Count; count++)
                        {
                            if (EnemyList[count] == null)
                            {
                                KilledEnemy++;
                                EnemyList.RemoveAt(count);
                                CheckStage();
                            }
                        }
                    }                    
                    break;

                case CURRENTSTAGE.BOSS_STAGE:
                    if(BossList.Count > 0)
                    {
                        for(int count = 0; count < BossList.Count; count++)
                        {
                            if(BossList[count] == null)
                            {
                                BossList.RemoveAt(count);
                            }
                        }
                    }
                    else
                    {
                        foreach(GameObject obj in EnemyList)
                        {
                            Destroy(obj);
                        }

                        GoalActive();
                        if (CurSceneName == "Field")
                        {
                            Destroy(WallList[2]);
                        }
                    }
                    break;
            }
        }
        else if(playerinfo.GetPSTATE() == PlayerInfo.PSTATE.DIE)
        {
            StartCoroutine(LoseBackToCamp());
        }        
    }

    internal void CheckStage()
    {
        if(CurSceneName != "Dungeon")
        {
            switch (CurStage)
            {
                case CURRENTSTAGE.BOSS_STAGE:
                    break;

                case CURRENTSTAGE.SECOND_STAGE:
                    if (KilledEnemy == (int)STAGEGOAL.SECOND_STAGEGOAL)
                    {
                        KilledEnemy = 0;
                        if (CurSceneName == "Field")
                        {
                            Destroy(WallList[1]);
                        }
                        CurStage = CURRENTSTAGE.BOSS_STAGE;
                    }
                    break;

                case CURRENTSTAGE.FIRST_STAGE:
                    if (KilledEnemy == (int)STAGEGOAL.FIRST_STAGEGOAL)
                    {
                        KilledEnemy = 0;
                        if (CurSceneName == "Field")
                        {
                            Destroy(WallList[0]);
                        }
                        CurStage = CURRENTSTAGE.SECOND_STAGE;
                    }
                    break;
            }
        }
        else
        {
            if(RescuedNPC == 3)
            {
                CurStage = CURRENTSTAGE.BOSS_STAGE;
                Destroy(GameObject.Find("StageWall1"));
            }
        }
        
    }

    private void GoalActive()
    {
        if(Goal != null)
        {
            if(SceneManager.GetActiveScene().name == "Arena")
            {// 적 소환 코루틴 정지
                StopAllCoroutines();
                SpawnPoint.SetActive(false);
            }
            Goal.SetActive(true);
        }        
    }

    internal void StartEliminateMission()
    {
        if(PrintMissionFlag)
        {
            StartCoroutine(ShowEliminateMission());
        }        
    }

    internal void StartRescueMission()
    {
        if(PrintMissionFlag)
        {
            StartCoroutine(ShowRescueMission());
        }
    }

    IEnumerator ShowEliminateMission()
    {
        yield return new WaitForSeconds(0.5f);
        HUDManager.instance.PrintEliminateMission();
        PrintMissionFlag = false;
        yield return null;
    }

    IEnumerator ShowRescueMission()
    {
        yield return new WaitForSeconds(0.5f);
        HUDManager.instance.PrintRescueMission();
        PrintMissionFlag = false;
        yield return null;
    }

    IEnumerator LoseBackToCamp()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Camp");
        yield return null;
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            if(playerinfo.GetPSTATE() == PlayerInfo.PSTATE.ALIVE)
            {
                yield return new WaitForSeconds(5.0f);
                SpawnEnemyCount++;
                SpawnPoint.SetActive(true);
                GameObject tmp = Instantiate(Resources.Load("Prefab/Object/Enemy"),
                    new Vector3(50, 0, 50), Quaternion.Euler(0, 0, 0)) as GameObject;
                EnemyInstan.instance.AddNewEnemyHUD(tmp, SpawnEnemyCount);
                EnemyList.Add(tmp);
                yield return new WaitForSeconds(4.0f);
                SpawnPoint.SetActive(false);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    internal CURRENTSTAGE GetCurStage()
    {
        return CurStage;
    }

    internal int GetRescuedNPC()
    {
        return RescuedNPC;
    }

    internal void IncreaseRescuedNPC()
    {
        RescuedNPC++;
    }

    public void SetNOTSAV_CON()
    {
        SaveLoadManager.instance.playselect = SaveLoadManager.PLAYSELECT.NOTSAV_CON;
    }
}
