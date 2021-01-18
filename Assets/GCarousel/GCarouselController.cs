﻿using System.Collections;
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
    float m_distanceBetweenCells = 38.0f;

    public List<RectTransform> m_cellList = new List<RectTransform>();

    public enum CellArrangementMethod { eLeftToRight, eRightToLeft, eFromCenterToRight, eFromCenterToLeft };
    CellArrangementMethod e_cellArrangementMethod;

    bool m_swapEnabled = false;

    public void setCellArrangementMethod(CellArrangementMethod method)
    {
        e_cellArrangementMethod = method;
    }

    int m_focusedItemIndex = 0;

    public void SetupCell(string name, Texture2D texture)
    {
        GameObject cell = Instantiate(m_cell.gameObject);
        cell.transform.SetParent(m_carouselPanel.Find("Viewport").Find("Content"));
        cell.name = name;
        cell.GetComponent<RawImage>().texture = texture;
        m_cellList.Add(cell.GetComponent<RectTransform>());


        RectTransform rt = cell.GetComponent<RectTransform>();
        rt.GetComponent<FixScrollRect>().MainScroll = rt.parent.parent.parent.GetComponent<ScrollRect>();

        if (this.gameObject.transform.parent.name.Contains("Models"))
        {
            rt.sizeDelta = new Vector2(294, 294);
        }

        reArrangeCells();
    }

    void reArrangeCells()
    {
        if (e_cellArrangementMethod == CellArrangementMethod.eFromCenterToRight)
        {
            for (int i = 0; i < m_cellList.Count; i++)
            {
                m_cellList[i].transform.localPosition = new Vector3(i * (m_carouselCenterPoint.transform.localPosition.x + m_cell.sizeDelta.x + m_distanceBetweenCells), 0, 0);
            }
        }

        if (e_cellArrangementMethod == CellArrangementMethod.eFromCenterToLeft)
        {
            for (int i = 0; i < m_cellList.Count; i++)
            {
                m_cellList[i].transform.localPosition = new Vector3(-i * (m_carouselCenterPoint.transform.localPosition.x + m_cell.sizeDelta.x + m_distanceBetweenCells), 0, 0);
            }
        }
        if (e_cellArrangementMethod == CellArrangementMethod.eLeftToRight)
        {
            for (int i = 0; i < m_cellList.Count; i++)
            {
                m_cellList[i].transform.localPosition = new Vector3(m_distanceBetweenCells + (-m_carouselPanel.sizeDelta.x / 2) + ((m_cell.sizeDelta.x / 2) + (m_distanceBetweenCells * 2)) + i * (m_cell.sizeDelta.x + m_distanceBetweenCells), 0, 0);
            }
        }
        if (e_cellArrangementMethod == CellArrangementMethod.eRightToLeft)
        {
            for (int i = 0; i < m_cellList.Count; i++)
            {
                m_cellList[i].transform.localPosition = new Vector3((m_carouselPanel.sizeDelta.x / 2) - (m_distanceBetweenCells + ((m_cell.sizeDelta.x / 2) + (m_distanceBetweenCells * 2))) - i * (m_cell.sizeDelta.x + m_distanceBetweenCells), 0, 0);
            }
        }
    }

    void Awake()
    {

    }
    void Start()
    {
        if (this.gameObject.transform.parent.name.Contains("Models"))
        {
            this.gameObject.transform.parent.GetComponent<ScrollRect>().inertia = true;
        }
    }

    IEnumerator focusItemCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("SIZE DELTA " + m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x);
        if (m_focusedItemIndex <= m_cellList.Count - 1)
        {
            focusOnItem(m_focusedItemIndex,0.0f);
        }
        else
        {
            Debug.Log("Can't focus on index " + m_focusedItemIndex + "  " + "no such element in cell list");
        }
    }
    public void focusOnCellIndex(int cellIndex)
    {
        m_focusedItemIndex = cellIndex;
        StartCoroutine("focusItemCoroutine");
    }

    public void onCellClicked(PointerEventData data)
    {

        if (this.gameObject.transform.parent.name.Contains("Models"))
        {
            GameObject.Find("_manager").GetComponent<UIController>().onBrandOrModelButtonClicked(data);
            return;
        }

        string clickedCellNam = data.pointerClick.name;

        GameObject cell = GameObject.Find(clickedCellNam).gameObject;

        float distance = Vector3.Distance(cell.transform.position, m_carouselCenterPoint.transform.position);

        GameObject selectedBrandBorder = GameObject.Find("SelectedBrandBorder");


        //selectedBrandBorder.transform.position = cell.transform.position;
        float globalCenterX = m_carouselCenterPoint.transform.position.x;

        if (this.gameObject.transform.parent.name.Contains("Brands"))
        {
            if(data.pointerClick.transform.position.x > m_carouselCenterPoint.transform.position.x + m_cell.sizeDelta.x / 2)
            {
                selectedBrandBorder.transform.position = m_carouselCenterPoint.transform.position;
            }
            if (data.pointerClick.transform.position.x < m_carouselCenterPoint.transform.position.x - m_cell.sizeDelta.x / 2)
            {
                selectedBrandBorder.transform.position = m_carouselCenterPoint.transform.position;
            }

            if (data.pointerClick.transform.position.x > m_carouselCenterPoint.transform.position.x - m_cell.sizeDelta.x/2 && data.pointerClick.transform.position.x < m_carouselCenterPoint.transform.position.x + m_cell.sizeDelta.x / 2)
            {
                selectedBrandBorder.transform.position = data.pointerClick.transform.position;
            }

        }



        if (cell.transform.position.x < m_carouselCenterPoint.transform.position.x)
        {
            if (m_cellList[2].position.x > globalCenterX - m_distanceBetweenCells && m_cellList[2].transform.position.x < globalCenterX + m_distanceBetweenCells)
            {
                Debug.Log("End from left");
                if (this.gameObject.transform.parent.name.Contains("Brands"))
                {
                    selectedBrandBorder.transform.position = cell.transform.position;
                }
                return;

            }
            float destPos = cell.transform.parent.transform.position.x + distance;
            cell.transform.parent.transform.DOMoveX(destPos, 0.2f);
        }
        if (cell.transform.position.x > m_carouselCenterPoint.transform.position.x)
        {
            if (m_cellList[m_cellList.Count - 3].position.x > globalCenterX - m_distanceBetweenCells && m_cellList[m_cellList.Count - 3].transform.position.x < globalCenterX + m_distanceBetweenCells)
            {
                Debug.Log("End from right");
                if (this.gameObject.transform.parent.name.Contains("Brands"))
                {
                    selectedBrandBorder.transform.position = cell.transform.position;
                }
                return;
            }
            float destPos = cell.transform.parent.transform.position.x - distance;
            cell.transform.parent.transform.DOMoveX(destPos, 0.2f);
        }

        GameObject.Find("_manager").GetComponent<UIController>().onBrandOrModelButtonClicked(data);
    }

    void focusOnItem(int itemIndex, float duration)
    {
        float itemPositionX = m_cellList[itemIndex].transform.position.x;

        float distance = Vector3.Distance(m_cellList[itemIndex].transform.position, m_carouselCenterPoint.transform.position);

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

    public void onScroll(PointerEventData data)
    {
        //Debug.Log("onScroll Calling");
    }

    public void onDrag()
    {

        if (this.gameObject.transform.parent.name.Contains("Models"))
        {
            Debug.Log("onDrag Calling");

        }

        return;
        if (m_swapEnabled)
        {
            float rightPnt = m_carouselCenterPoint.transform.position.x + m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;
            float leftPnt = m_carouselCenterPoint.transform.position.x - m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;

            if (m_cellList[m_cellList.Count - 1].GetComponent<RectTransform>().position.x >= rightPnt)
            {
                float newPosX = m_cellList[0].transform.localPosition.x - (m_cell.sizeDelta.x + m_distanceBetweenCells);
                m_cellList[m_cellList.Count - 1].transform.localPosition = new Vector3(newPosX, 0, 0);
                m_cellList[m_cellList.Count - 1].SetSiblingIndex(0);

                updateCellList();

            }

            if (m_cellList[0].GetComponent<RectTransform>().position.x <= leftPnt)
            {
                float newPosX = m_cellList[m_cellList.Count - 1].transform.localPosition.x + (m_cell.sizeDelta.x + m_distanceBetweenCells);
                m_cellList[0].transform.localPosition = new Vector3(newPosX, 0, 0);
                m_cellList[0].SetSiblingIndex(m_cellList.Count - 1);

                updateCellList();

            }
        }
        return;
        //Debug.Log("onDrag Call: " + m_cellList[4].transform.position);
        if (m_cellList[4].transform.position.x >= m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x)
        {
            Debug.Log("Change Place");
        }
        for (int i = 0; i < m_cellList.Count; i++)
        {
            //if(m_cellList[i].)
        }

    }

    public void onEndDrag(PointerEventData data)
    {
        //return;
        if(this.gameObject.transform.parent.name.Contains("Models"))
        {
            return;
        }
        float minDistance = float.MaxValue;
        RectTransform closestItem = new RectTransform();
        for (int i = 0; i < m_cellList.Count; i++)
        {
            if (Vector3.Distance(m_cellList[i].transform.position, m_carouselCenterPoint.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(m_cellList[i].transform.position, m_carouselCenterPoint.transform.position);
                closestItem = m_cellList[i];
            }
        }
        //print("on end drag " + minDistance + " closestItem: " + closestItem.name);
        if (closestItem.transform.position.x < m_carouselCenterPoint.transform.position.x)
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

    void updateCellList()
    {
        RectTransform rt = m_carouselPanel.Find("Viewport").Find("Content").GetComponent<RectTransform>();



        for (int i = 0; i < rt.childCount; i++)
        {
            //m_cellList.Add(rt.GetChild(i).GetComponent<RectTransform>());


            m_cellList[i] = rt.GetChild(i).GetComponent<RectTransform>();
            m_cellList[i].name = rt.GetChild(i).name;
            rt.GetChild(i).GetComponent<RawImage>().texture = rt.GetChild(i).GetComponent<RawImage>().texture;
        }

    }

    void Update()
    {
        return;
        if(this.gameObject.transform.parent.name.Contains("Models"))
        {
            float rightPnt = m_carouselCenterPoint.transform.position.x + m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;
            float leftPnt = m_carouselCenterPoint.transform.position.x - m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;

            if (m_cellList[0].GetComponent<RectTransform>().position.x >= leftPnt + m_cell.sizeDelta.x + m_distanceBetweenCells/2)
            {

                print("Stoppppp Left");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.gameObject.transform.parent.GetComponent<ScrollRect>().inertia = true;
                Debug.Log("Pressed left click.");
            }
            /*
            if (m_cellList[m_cellList.Count-1].GetComponent<RectTransform>().position.x >= rightPnt - (m_cellList[m_cellList.Count - 1].GetComponent<RectTransform>().sizeDelta.x / 2 + m_distanceBetweenCells / 2))
            {
                print("Stoppppp Right");
            }*/
        }



        if (m_swapEnabled)
        {
            float rightPnt = m_carouselCenterPoint.transform.position.x + m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;
            float leftPnt = m_carouselCenterPoint.transform.position.x - m_carouselPanel.GetComponent<RectTransform>().sizeDelta.x / 2;

            if (m_cellList[m_cellList.Count - 1].GetComponent<RectTransform>().position.x >= rightPnt)
            {
                float newPosX = m_cellList[0].transform.localPosition.x - (m_cell.sizeDelta.x + m_distanceBetweenCells);
                m_cellList[m_cellList.Count - 1].transform.localPosition = new Vector3(newPosX, 0, 0);
                m_cellList[m_cellList.Count - 1].SetSiblingIndex(0);

                updateCellList();

            }

            if (m_cellList[0].GetComponent<RectTransform>().position.x <= leftPnt)
            {
                float newPosX = m_cellList[m_cellList.Count - 1].transform.localPosition.x + (m_cell.sizeDelta.x + m_distanceBetweenCells);
                m_cellList[0].transform.localPosition = new Vector3(newPosX, 0, 0);
                m_cellList[0].SetSiblingIndex(m_cellList.Count - 1);

                updateCellList();

            }

        }
    }
}