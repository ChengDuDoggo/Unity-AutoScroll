using UnityEngine;
using UnityEngine.UI;
//定义滚动方向
public enum ScrollDir
{
    BottomToTop = 1,
    TopToBottom = 2,
    LeftToRight = 3,
    RightToLeft = 4,
}
public class AutoScroll : MonoBehaviour
{
    public ScrollDir AutoScrollDir = ScrollDir.BottomToTop;
    public float Step = 0.1f;//滚动步长
    private ScrollRect scrollRect;
    private RectTransform scrollTran;
    private HorizontalOrVerticalLayoutGroup layoutGroup;
    private GridLayoutGroup gridGroup;
    private float space = 0.0f;//滚动间隔
    //子节点高和宽
    private float itemWidth = 0.0f;
    private float itemHeight = 0.0f;
    //初始化
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollTran = scrollRect.GetComponent<RectTransform>();
        layoutGroup = scrollRect.content.GetComponent<HorizontalOrVerticalLayoutGroup>();
        gridGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
        //设置滚动间隔
        if (layoutGroup != null)
        {
            space = layoutGroup.spacing;
        }
        else if (gridGroup != null)
        {
            switch (AutoScrollDir)
            {
                case ScrollDir.BottomToTop://由底至顶向上滚动
                case ScrollDir.TopToBottom://由顶至底向下滚动
                    space = gridGroup.spacing.y;
                    break;
                case ScrollDir.LeftToRight://由左至右向右滚动
                case ScrollDir.RightToLeft://由右向左向左滚动
                    space = gridGroup.spacing.x;
                    break;
                default:
                    space = 0.0f;
                    break;
            }
        }
        //设置子节点的高度和宽度
        if (layoutGroup != null && scrollRect.content.childCount > 0)
        {
            itemWidth = scrollRect.content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
            itemHeight = scrollRect.content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        }
        else if (gridGroup != null)
        {
            itemWidth = gridGroup.cellSize.x;
            itemHeight = gridGroup.cellSize.y;
        }
    }
    private void FixedUpdate()
    {
        DoAutoScroll();
    }
    /// <summary>
    /// 自动滑动
    /// </summary>
    private void DoAutoScroll()
    {
        switch (AutoScrollDir)
        {
            //由底至顶向上滚动
            case ScrollDir.BottomToTop:
                {
                    if (scrollRect.content.sizeDelta.y > scrollTran.sizeDelta.y + itemHeight)
                    {
                        scrollRect.content.anchoredPosition3D += new Vector3(0, Step, 0);
                        if (scrollRect.content.anchoredPosition3D.y >= (scrollRect.content.sizeDelta.y - scrollTran.sizeDelta.y))
                        {
                            if (gridGroup != null && gridGroup.constraintCount > 1)
                            {
                                for (int i = 0; i < gridGroup.constraintCount; i++)
                                    scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                                scrollRect.content.anchoredPosition3D -= new Vector3(0, (itemHeight + space), 0);
                            }
                            else
                            {
                                scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                                scrollRect.content.anchoredPosition3D -= new Vector3(0, (itemHeight + space), 0);
                            }
                        }
                    }
                }
                break;
            //由顶至底向下滚动
            case ScrollDir.TopToBottom:
                {
                    if (scrollRect.content.sizeDelta.y > scrollTran.sizeDelta.y + itemHeight)
                    {
                        scrollRect.content.anchoredPosition3D -= new Vector3(0, Step, 0);
                        if (scrollRect.content.anchoredPosition3D.y <= (scrollRect.content.sizeDelta.y - scrollTran.sizeDelta.y))
                        {
                            if (gridGroup != null && gridGroup.constraintCount > 1)
                            {
                                for (int i = 0; i < gridGroup.constraintCount; i++)
                                    scrollRect.content.GetChild(scrollRect.content.childCount - 1).transform.SetAsFirstSibling();
                                scrollRect.content.anchoredPosition3D += new Vector3(0, (itemHeight + space), 0);
                            }
                            else
                            {
                                scrollRect.content.GetChild(scrollRect.content.childCount - 1).transform.SetAsFirstSibling();
                                scrollRect.content.anchoredPosition3D += new Vector3(0, (itemHeight + space), 0);
                            }
                        }
                    }
                }
                break;
            //由左至右向右滚动
            case ScrollDir.LeftToRight:
                {
                    if (scrollRect.content.sizeDelta.x > scrollTran.sizeDelta.x + itemWidth)
                    {
                        scrollRect.content.anchoredPosition3D += new Vector3(Step, 0, 0);
                        if (scrollRect.content.anchoredPosition3D.x >= -(scrollRect.content.sizeDelta.x - scrollTran.sizeDelta.x))
                        {
                            if (gridGroup != null && gridGroup.constraintCount > 1)
                            {
                                for (int i = 0; i < gridGroup.constraintCount; i++)
                                    scrollRect.content.GetChild(scrollRect.content.childCount - 1).transform.SetAsFirstSibling();
                                scrollRect.content.anchoredPosition3D -= new Vector3((itemWidth + space), 0, 0);
                            }
                            else
                            {
                                scrollRect.content.GetChild(scrollRect.content.childCount - 1).transform.SetAsFirstSibling();
                                scrollRect.content.anchoredPosition3D -= new Vector3((itemWidth + space), 0, 0);
                            }
                        }
                    }
                }
                break;
            //由右至左向左滚动
            case ScrollDir.RightToLeft:
                {
                    if (scrollRect.content.sizeDelta.x > scrollTran.sizeDelta.x + itemWidth)
                    {
                        scrollRect.content.anchoredPosition3D -= new Vector3(Step, 0, 0);
                        if (scrollRect.content.anchoredPosition3D.x <= -(scrollRect.content.sizeDelta.x - scrollTran.sizeDelta.x))
                        {
                            if (gridGroup != null && gridGroup.constraintCount > 1)
                            {
                                for (int i = 0; i < gridGroup.constraintCount; i++)
                                    scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                                scrollRect.content.anchoredPosition3D += new Vector3((itemWidth + space), 0, 0);
                            }
                            else
                            {
                                scrollRect.content.GetChild(0).transform.SetAsLastSibling();
                                scrollRect.content.anchoredPosition3D += new Vector3((itemWidth + space), 0, 0);
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
}