using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlayerSkill : MonoBehaviour
{
    private PlayerMove playermove;
    public int SkillID;

    void Start()
    {
        playermove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public void OnClick()
    {
        switch(SkillID)
        {
            case 1:
                playermove.UseAtk1();
                break;
            case 4:
                playermove.UseAtk4();
                break;
            case 5:
                playermove.UseBuff();
                break;
        }
    }
}
