using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeActive : MonoBehaviour
{
    private Transform player;
    private GetTarget gt;
    private UILabel label;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        gt = GetComponent<GetTarget>();
        label = GetComponent<UILabel>();
    }

    void Update()
    {
        InRangePlayer();
    }

    private void InRangePlayer()
    {
        if(gt.GetTargetTransform() != null)
        {
            if (Vector3.Distance(gt.GetTargetTransform().position, player.position) <= 3.0f)
            {
                label.enabled = true;
            }
            else
            {
                label.enabled = false;
            }
        }        
    }
}
