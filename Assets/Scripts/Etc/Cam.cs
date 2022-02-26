using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;
    public float height = 3.0f;
    public float targetOffset = 1.0f;
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 camPos = target.position + (target.up * height);


        tr.LookAt(target.position + (target.up * targetOffset));
    }
}
