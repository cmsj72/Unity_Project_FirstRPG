using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeItemImage : MonoBehaviour
{
    private UISprite InnerFrame;
    private UISprite itemSprite;
    private UIDragScrollView scView;
    private InvenGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = transform.parent.gameObject.GetComponent<InvenGrid>();
        scView = GetComponent<UIDragScrollView>();
        scView.scrollView = transform.parent.parent.gameObject.GetComponent<UIScrollView>();
        InnerFrame = GameObject.Find("InnerFrame").GetComponent<UISprite>();
        itemSprite = GetComponent<UISprite>();

        itemSprite.width = InnerFrame.width / grid.ColumnMax;
        itemSprite.height = itemSprite.width;
    }
}
