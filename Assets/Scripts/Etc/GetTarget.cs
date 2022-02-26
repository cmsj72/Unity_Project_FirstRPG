using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTarget : MonoBehaviour
{
    public Transform TargetTransform;
    private UILabel label;
    private Camera uiCamera;

    private void Awake()
    {
        uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }
    
    void Update()
    {
        if(TargetTransform)
        {
            SetPositionHUD();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void SetPositionHUD()
    {
        Vector3 Vposition;
        switch (tag)
        {
            case "HPBar":
                {
                    Vposition = Camera.main.WorldToViewportPoint(TargetTransform.position
            + new Vector3(0, 2, 0));
                    transform.position = uiCamera.ViewportToWorldPoint(Vposition);
                }                
                break;

            case "MPBar":
                {
                    Vposition = Camera.main.WorldToViewportPoint(TargetTransform.position
            + new Vector3(0, 2, 0));
                    transform.position = uiCamera.ViewportToWorldPoint(Vposition);
                }
                break;

            case "ItemNameLabel":
                {
                    Vposition = Camera.main.WorldToViewportPoint(TargetTransform.position
           + new Vector3(0, 0.3f, 0));
                    transform.position = uiCamera.ViewportToWorldPoint(Vposition);
                }                
                break;

            case "BossBar":
                {
                    Vposition = Camera.main.WorldToViewportPoint(TargetTransform.position
            + new Vector3(0, 3, 0));
                    transform.position = uiCamera.ViewportToWorldPoint(Vposition);
                }
                break;
        }
    }

    internal void SetTargetTransform(Transform trsform)
    {
        TargetTransform = trsform;
    }

    internal Transform GetTargetTransform()
    {
        return TargetTransform;
    }

    internal void SetLabel(string name)
    {
        label = this.GetComponent<UILabel>();
        label.text = name;
        TargetTransform.gameObject.GetComponent<ItemInfo>().SetItemName(name);
    }

    
}
