using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GCarouselController : MonoBehaviour
{
    public RectTransform m_canvas;
    public RectTransform m_carouselPanel;
    public RectTransform m_cell;
    public RectTransform m_carouselCenterPoint;

    public List<RectTransform> m_cellList = new List<RectTransform>();

    public void SetupCell(string name, Texture2D texture)
    {
        GameObject cell = Instantiate(m_cell.gameObject);
        cell.transform.SetParent(m_carouselPanel.Find("Viewport").Find("Content"));
        cell.name = name;
        cell.GetComponent<RawImage>().texture = texture;

        m_cellList.Add(cell.GetComponent<RectTransform>());

        RectTransform rt = cell.GetComponent<RectTransform>();
        rt.GetComponent<FixScrollRect>().MainScroll = rt.parent.parent.parent.GetComponent<ScrollRect>();
    }


    void Awake()
    {
        /*
        for (int i = 0; i < 5; i++)
        {
            SetupCell("Cell_" + i,new Texture2D(50,50));
        }*/
    }

    void Start()
    {

        //focusOnItem(2);
    }



    public void onCellClicked(PointerEventData data)
    {
        string clickedCellNam = data.pointerClick.name;

        GameObject cell = GameObject.Find(clickedCellNam).gameObject;

        //cell.transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;

        float distance = Vector3.Distance(cell.transform.position, m_carouselCenterPoint.transform.position);

        //print("distance: " + distance);

        if(cell.transform.position.x < m_carouselCenterPoint.transform.position.x)
        {
            float destPos = cell.transform.parent.transform.position.x + distance;
            cell.transform.parent.transform.DOMoveX(destPos, 0.2f);
        }
        if (cell.transform.position.x > m_carouselCenterPoint.transform.position.x)
        {
            float destPos = cell.transform.parent.transform.position.x - distance;
            cell.transform.parent.transform.DOMoveX(destPos, 0.2f);
        }

        GameObject.Find("_manager").GetComponent<UIController>().onBrandOrModelButtonClicked(data);
    }


    public void focusOnItem(int itemIndex, float duration)
    {

        float itemPositionX = m_cellList[itemIndex].transform.position.x;




        float distance = Mathf.Abs(m_carouselCenterPoint.transform.position.x - itemPositionX);// Vector3.Distance(m_cellList[itemIndex].transform.position, m_carouselCenterPoint.transform.position);

        if (itemPositionX < m_carouselCenterPoint.transform.position.x)
        {
            float destPos = m_cellList[itemIndex].transform.parent.transform.position.x + distance;
            m_cellList[itemIndex].transform.parent.transform.DOMoveX(destPos, duration);
        }
        if (itemPositionX > m_carouselCenterPoint.transform.position.x)
        {
            float destPos = m_cellList[itemIndex].transform.parent.transform.position.x - distance;
            m_cellList[itemIndex].transform.parent.transform.DOMoveX(destPos, duration);
        }
    }

    public void onDrag()
    {


    }

    public void onEndDrag(PointerEventData eventData)
    {
        float minDistance = float.MaxValue;
        RectTransform closestItem = new RectTransform();
        for (int i = 0; i < m_cellList.Count; i++)
        {
            if(Vector3.Distance(m_cellList[i].transform.position,m_carouselCenterPoint.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(m_cellList[i].transform.position, m_carouselCenterPoint.transform.position);
                closestItem = m_cellList[i];
            }
        }
        print("on end drag " + minDistance + " closestItem: " + closestItem.name);
        if(closestItem.transform.position.x < m_carouselCenterPoint.transform.position.x)
        {
            float destPos = closestItem.transform.parent.transform.position.x + minDistance;
            closestItem.parent.transform.DOMoveX(destPos, 0.3f);
        }
        if (closestItem.transform.position.x > m_carouselCenterPoint.transform.position.x)
        {
            float destPos = closestItem.transform.parent.transform.position.x - minDistance;
            closestItem.parent.transform.DOMoveX(destPos, 0.3f);
        }
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            focusOnItem(2,0.2f);
            //SetupCell("Cell_", new Texture2D(50, 50));
        }

    }
}
