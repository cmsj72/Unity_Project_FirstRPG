using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInfoManage : MonoBehaviour
{
    static public TempInfoManage instance;
    internal string PlayerNameTemp;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

}
