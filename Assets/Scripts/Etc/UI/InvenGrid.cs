using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenGrid : MonoBehaviour
{
    private UIGrid uigrid;
    private UIPanel scviewPanel;
    internal int ColumnMax = 8;
    internal float celHeight;
    internal float celWidth;

    // Start is called before the first frame update
    void Start()
    {
        scviewPanel = GetComponentInParent<UIPanel>();
        uigrid = GetComponent<UIGrid>();
        uigrid.maxPerLine = ColumnMax;
        celWidth = scviewPanel.width / ColumnMax;
        celHeight = celWidth;
        uigrid.cellHeight = celHeight;
        uigrid.cellWidth = celWidth;        
    }
}
