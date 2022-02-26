using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLabel : MonoBehaviour
{
    public  string  PortalName;
    public Transform TargetTransform;
    private UILabel label;
    private Camera  uiCamera;    

    private void Awake()
    {
        uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Vector3 Vposition;
        Vposition = Camera.main.WorldToViewportPoint(TargetTransform.position
            + new Vector3(0, 2, 0));
        transform.position = uiCamera.ViewportToWorldPoint(Vposition);
    }
}
