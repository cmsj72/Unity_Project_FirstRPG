using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollControl : MonoBehaviour
{
    private UIGrid grid;
    private UIScrollView scView;
    private UIPanel panel;
    private int ExCount;
    private int CurCount;

    private void Awake()
    {
        scView = GetComponent<UIScrollView>();
        grid = GetComponentInChildren<UIGrid>();
        panel = GetComponent<UIPanel>();
        ExCount = grid.transform.childCount;        
    }

    private void Start()
    {
        grid.Reposition();
        scView.ResetPosition();
        panel.Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        CurCount = grid.transform.childCount;
        if(ExCount < CurCount)
        {
            
            grid.Reposition();
            scView.ResetPosition();
            panel.Refresh();

            ExCount = CurCount;
        }
    }

    internal void RePositionInven()
    {
        grid.Reposition();
        scView.ResetPosition();
        panel.Refresh();
    }
}
