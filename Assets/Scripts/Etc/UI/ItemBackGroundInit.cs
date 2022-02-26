using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBackGroundInit : MonoBehaviour
{
    private UISprite sprite;
    private UISprite parentSprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<UISprite>();
        sprite.SetAnchor(transform.parent);
        parentSprite = transform.parent.gameObject.GetComponent<UISprite>();
        sprite.depth = parentSprite.depth - 1;
    }
}
