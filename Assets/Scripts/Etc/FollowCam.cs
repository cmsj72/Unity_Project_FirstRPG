using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    private float moveDamping = 15.0f;
    private float distance       = 5.0f;
    private float height         = 5.0f;
    private float targetOffset   = 2.0f;
    private Vector3 ExcamPos;
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }    

    void LateUpdate()
    {
        Vector3 camPos = target.position - (Vector3.forward * distance) 
                        + (Vector3.up * height);
        ExcamPos = camPos;

        if (Physics.Raycast(target.position + Vector3.up * 2, camPos - target.position, out RaycastHit hit, distance))
        {
            if(hit.collider.tag != "Enemy")
            {
                camPos = hit.point;
            }            
        }
        else
        {
            camPos = ExcamPos;
        }
        
        tr.position = Vector3.Slerp(tr.position, camPos, moveDamping);

        tr.LookAt(target.position + target.up);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}
