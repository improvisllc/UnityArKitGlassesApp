using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

[System.Serializable]
public class CarouselEvent : UnityEvent<int, int> { }

public class CarouselController : MonoBehaviour
{
  public CarouselEvent _onCellClickedEvent;

  public CarouselConstants.iCarouselType _carouselType = CarouselConstants.iCarouselType.iCarouselTypeLinear;

  public ScrollRect _scroll;
  public RectTransform _scrollRT;
  // to hold the cells
  public RectTransform _panel;
  // Center to compare the distance for each objects     
  public RectTransform _center;
  // Cell created by auto create
  public GameObject _cell;

  // Cell should snap to center after scrolling is finished
  public bool _shouldFocusCenter = true;
  // Cell should snap to center when being seleted
  public bool _shouldCenterSelect = true;
  // If the controller should auto create default cells
  public bool _shouldAutoCreateCells = true;
  // If the carousel should loop 
  public bool _shouldLoop = true;
  // Vertical or horizontal
  public bool _isHorizontal = true;

  public int _totalCells = 5;
  public float _cellGap = 25f;
  public float _focusSpeed = 5f;
  public float _scaleSpeed = 5f;
  public float _rotateSpeed = 5f;
  public float _moveSpeed = 10f;
  // When the scroll velocity is below this threshold, start focusing 
  public float _focusCenterVelocityThreshold = 50f;
  public Vector3 _scaleRatio = new Vector3(1, 1, 1);
  public Vector3 _coverflowAngles = new Vector3(0, 80, 0);

  // Hold all cells
  public List<GameObject> m_cellContainer = new List<GameObject>();

  // Will be true, while we drag the panel
  bool m_dragging = false;
  bool m_isCircularMovment = false;
  int m_selectIndex = -1;
  int m_centerCellIndex = 0;
  int m_arcLength;
  int m_offsetIndex;
  int m_tempCenterIndex = -1;
  // Boundary is dynamically calculated based on total cells
  Vector3 m_boundary;
  Vector3 m_dragStartPos;
  // Used for cell style calculation 
  Vector3 m_newPos;
  Vector3 m_newScale;
  Vector3 m_newRot;
  Vector3 m_offset;
  Vector3 m_final;

  public void StartDrag()
  {
    //Debug.Log("StartDrag");
    // Reset select index so it won't focus on last select index
    m_selectIndex = -1;
    m_dragging = true;
    m_dragStartPos = _panel.position;
  }

  public void EndDrag()
  {
    //Debug.Log("EndDrag");
    m_dragging = false;
  }

  public void AddCell(string name, Texture2D tex)
  {
    // Add the current center cell
    if (_cell == null)
      return;

    GameObject go = Instantiate(_cell, _panel);

    AddCellData(go, m_cellContainer.Count);


        go.GetComponent<RawImage>().texture = tex;
        go.name = name;
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = rt.GetComponent<EventTrigger>();
        EventTrigger.Entry clicked = new EventTrigger.Entry();
        clicked.eventID = EventTriggerType.PointerClick;
        clicked.callback.AddListener((data) => { GameObject.Find("_manager").GetComponent<UIController>().onBrandOrModelButtonClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(clicked);
        rt.GetComponent<FixScrollRect>().MainScroll = rt.parent.parent.parent.GetComponent<ScrollRect>();


    // Last cell, we just add
    // Otherwise, we insert
    if (m_centerCellIndex >= m_cellContainer.Count - 1)
    {
      m_cellContainer.Add(go);
    }
    else
    {
      m_cellContainer.Insert(m_centerCellIndex + 1, go);
    }
    // Recaluclate the arc length 
    m_arcLength = 360 / m_cellContainer.Count;
    _scroll.velocity = Vector3.zero;
  }

  public void RemoveCenterCell()
  {
    RemoveCell(m_centerCellIndex);
  }

  public void RemoveCell(int index)
  {

    if (m_cellContainer.Count <= 0)
      return;
    if (index < 0 || index >= m_cellContainer.Count)
      return;
    // Remove the current center cell
    GameObject go = m_cellContainer[index];
    RemoveCellData(go);
    m_cellContainer.Remove(go);
    Destroy(go);
    _scroll.velocity = Vector3.zero;
    if (m_cellContainer.Count > 0)
      m_arcLength = 360 / m_cellContainer.Count;
    else
      m_arcLength = 360;
  }

  void OnEnable()
  {
    // Get the start position here so the center offset will always be zero
    m_dragStartPos = _panel.position;
    // Default center cell is zero
    m_centerCellIndex = 0;
    /// Load all Images from /Resources/ShowImages path
    LoadCells();
    // Setup cells with high speed so they appear to be instant movement
    SetupCells(true);
  }

  void OnDisable()
  {
    UnloadCells();
  }

  void LoadCells()
  {
    if (_shouldAutoCreateCells)
    {
      for (int i = 0; i < _totalCells; ++i)
      {
        //AddCell();
      }
    }
    else
    {
      // Get all the child cells from panel
      for (int i = 0; i < _panel.childCount; ++i)
      {
        GameObject go = _panel.GetChild(i).gameObject;
        m_cellContainer.Add(go);
        AddCellData(go, i);
      }
    }

  }

  void UnloadCells()
  {
    while (m_cellContainer.Count != 0)
    {
      RemoveCell(0);
    }

    m_cellContainer.Clear();
  }

  void Update()
  {
    // Udpate scroll settings in real time 
    _scroll.horizontal = _isHorizontal;
    _scroll.vertical = !_isHorizontal;

    SetupCells(false);
    CheckBoundary();
    FindCenterCellIndex();
    CheckAutoFocus();
    CheckCenterSelect();
  }

  void OnCellBtnCallback(object sender, CellEventArgs e)
  {
    int index = e.Index;
    for (int i = 0; i < m_cellContainer.Count; ++i)
    {
      if (m_cellContainer[i] == (sender as CellController).gameObject)
      {
        //Debug.Log("select index: " + i);
        index = i;
      }
    }

    if (_onCellClickedEvent != null)
      _onCellClickedEvent.Invoke(index, e.Index);

    // Check if we should center the selected cell
    if (_shouldCenterSelect)
    {
      m_selectIndex = index;
      _scroll.velocity = Vector3.zero;
    }
  }

  void AddCellData(GameObject go, int index)
  {
    CellController cc = go.GetComponent<CellController>();
    if (cc == null)
      return;
    cc._index = index;
    cc.name = "" + cc._index;
    cc.UpdateDesc("btn " + cc._index);
    cc.Clicked += OnCellBtnCallback;

  }

  void RemoveCellData(GameObject go)
  {
    CellController cc = go.GetComponent<CellController>();
    if (cc == null)
      return;
    cc.Clicked -= OnCellBtnCallback;
  }

  void SetupCells(bool instantUpdate)
  {

    for (int i = 0; i < m_cellContainer.Count; i++)
    {
      RectTransform rt = m_cellContainer[i].GetComponent<RectTransform>();
      if (rt == null)
        continue;

      m_newPos = Vector3.zero;
      m_newScale = new Vector3(1, 1, 1);
      m_newRot = Vector3.zero;
      m_offset = _panel.position - m_dragStartPos;
      m_offsetIndex = i - m_centerCellIndex;
      m_isCircularMovment = false;

      switch (_carouselType)
      {
        case CarouselConstants.iCarouselType.iCarouselTypeLinear:
          {
            m_newPos.x = (rt.rect.width + _cellGap) * m_offsetIndex;
            //Debug.Log(m_offset);
          }
          break;
        case CarouselConstants.iCarouselType.iCarouselTypeScaledLinear:
          {
            m_newPos.x = (rt.rect.width + _cellGap) * m_offsetIndex;
            float dis = Vector3.Distance(rt.position, _center.position);
            // Make sure it is not too small nor not too big
            float scaleRatio = Mathf.Clamp(1 - Mathf.Abs(dis / (_scrollRT.rect.width / 2)), 0.1f, 100);
            m_newScale = new Vector3(scaleRatio * _scaleRatio.x, scaleRatio * _scaleRatio.y, scaleRatio * _scaleRatio.z);
          }
          break;
        case CarouselConstants.iCarouselType.iCarouselTypeCoverFlow:
          {
            m_newPos.x = (rt.rect.width + _cellGap) * m_offsetIndex;
            if (m_offsetIndex < 0)
            {
              m_newRot = -_coverflowAngles;
            }
            else if (m_offsetIndex > 0)
            {
              m_newRot = _coverflowAngles;
            }
          }
          break;
        case CarouselConstants.iCarouselType.iCarouselTypeScaledCoverFlow:
          {
            m_newPos.x = (rt.rect.width + _cellGap) * m_offsetIndex;
            float dis = Vector3.Distance(rt.position, _center.position);
            // Make sure it is not too small nor not too big
            float scaleRatio = Mathf.Clamp(1 - Mathf.Abs(dis / (_scrollRT.rect.width / 2)), 0.1f, 100);
            m_newScale = new Vector3(scaleRatio * _scaleRatio.x, scaleRatio * _scaleRatio.y, scaleRatio * _scaleRatio.z);
            if (m_offsetIndex < 0)
            {
              m_newRot = -_coverflowAngles;
            }
            else if (m_offsetIndex > 0)
            {
              m_newRot = _coverflowAngles;
            }
          }
          break;
      }

      // Only allow one direction at a time

      if (!_isHorizontal)
      {

        m_newPos.y = m_newPos.x;
        m_newPos.x = 0;
        m_newRot.x = m_newRot.y;
        m_newRot.y = 0;

      }


      //Debug.Log(i + " newPOS: " + newPos);
      m_final = _center.position + m_newPos + m_offset;

      if (instantUpdate || m_isCircularMovment)
      {
        rt.position = m_final;
        //rt.DOMove(m_final, 0.5f);
        rt.localScale = m_newScale;
        rt.localRotation = Quaternion.Euler(m_newRot);
        _panel.ForceUpdateRectTransforms();
      }
      else
      {

                    rt.position = Vector3.Lerp(rt.position, m_final, Time.deltaTime * _moveSpeed);


        
        //rt.DOMove(m_final, 0.5f);

        rt.localScale = Vector3.Lerp(rt.localScale, m_newScale, Time.deltaTime * _scaleSpeed);
        rt.localRotation = Quaternion.Lerp(rt.localRotation, Quaternion.Euler(m_newRot), Time.deltaTime * _rotateSpeed);
      }
    }
  }


    void CheckAutoFocus()
  {
    if (!_shouldFocusCenter)
      return;
    if (m_selectIndex != -1)
      return;
    if (m_dragging)
      return;
    if (Mathf.Abs(_scroll.velocity.x) > _focusCenterVelocityThreshold)
      return;
    if (m_cellContainer.Count == 0)
      return;

    _scroll.velocity = Vector3.zero;
    m_dragStartPos = _panel.position;
  }

  void CheckCenterSelect()
  {
    if (!_shouldCenterSelect)
      return;
    if (m_dragging)
      return;
    if (m_selectIndex == -1)
      return;
    if (m_cellContainer.Count == 0)
      return;

    // Stop velocity 
    _scroll.velocity = Vector3.zero;
    m_centerCellIndex = m_selectIndex;
    m_dragStartPos = _panel.position;
  }

  void FindCenterCellIndex()
  {
    if (m_cellContainer.Count == 0)
      return;

    m_tempCenterIndex = -1;
    for (int i = 0; i < m_cellContainer.Count; i++)
    {
      // If there is non selected, we just select the first one 
      if (m_tempCenterIndex == -1)
      {
        m_tempCenterIndex = i;
      }
      else
      {
        // Find the nearest one as the center one
        GameObject go = m_cellContainer[i];
        GameObject oldGo = m_cellContainer[m_tempCenterIndex];


        // Find the center index by shortest distance
        float oldDis = Vector3.Distance(oldGo.transform.position, _center.position);
        float dis = Vector3.Distance(go.transform.position, _center.position);
        if (dis < oldDis)
        {
          //Debug.Log("center: " + go.name);
          //Debug.Log("new center: " + go.name + " dis: " + dis + " old dis: " + oldDis);
          m_tempCenterIndex = i;
        }

      }
    }
    m_centerCellIndex = m_tempCenterIndex;
    //Debug.Log("Center index: " + m_centerCellIndex);
    CalculateDragOffset();
  }

  void CalculateDragOffset()
  {
    if (m_cellContainer.Count == 0)
      return;
    if (m_centerCellIndex < 0 || m_centerCellIndex >= m_cellContainer.Count)
      return;
    RectTransform center = m_cellContainer[m_centerCellIndex].GetComponent<RectTransform>();


    m_dragStartPos = _panel.position + (_center.position - center.position);

  }

  void CheckBoundary()
  {
    if (_cell == null)
      return;
    if (!_shouldLoop)
      return;
    if (m_cellContainer.Count == 0)
      return;
    RectTransform rt = _cell.GetComponent<RectTransform>();
    if (rt == null)
      return;

    // Calculate the boundaries 
    m_boundary.x = m_cellContainer.Count * (rt.rect.width + _cellGap);
    m_boundary.y = m_cellContainer.Count * (rt.rect.height + _cellGap);
    float leftBoundary = _center.position.x - m_boundary.x / 2;
    float rightBoundary = _center.position.x + m_boundary.x / 2;
    float upBoundary = _center.position.y + m_boundary.y / 2;
    float downBoundary = _center.position.y - m_boundary.y / 2;

    // We only check the right most or left most 
    if (_isHorizontal)
    {
      //if ((_panel.position - m_dragDir).x < 0)
      {

        GameObject go = m_cellContainer[0];
        if (go.transform.position.x < leftBoundary)
        {
          //Debug.Log("left");
          UpdateBoundaryCell(go, new Vector3(rightBoundary, go.transform.position.y, go.transform.position.z), false);
        }
      }
      //else if ((_panel.position - m_dragDir).x > 0)
      {

        GameObject go = m_cellContainer[m_cellContainer.Count - 1];
        if (go.transform.position.x > rightBoundary)
        {
          //Debug.Log("right");
          UpdateBoundaryCell(go, new Vector3(leftBoundary, go.transform.position.y, go.transform.position.z), true);
        }
      }
    }
    else
    {
      //if ((_panel.position - m_dragDir).y < 0)
      {

        GameObject go = m_cellContainer[0];
        if (go.transform.position.y < downBoundary)
        {
          //Debug.Log("down");
          UpdateBoundaryCell(go, new Vector3(go.transform.position.x, upBoundary, go.transform.position.z), false);
        }
      }

      //else if ((_panel.position - m_dragDir).y > 0)
      {

        GameObject go = m_cellContainer[m_cellContainer.Count - 1];
        if (go.transform.position.y > upBoundary)
        {
          //Debug.Log("up");
          UpdateBoundaryCell(go, new Vector3(go.transform.position.x, downBoundary, go.transform.position.z), true);
        }
      }
    }
  }

  void UpdateBoundaryCell(GameObject passGO, Vector3 newPos, bool isInsert)
  {
    //Debug.Log("panel pos: " + _panel.position + " center pos: " + _center.position);
    passGO.transform.position = newPos;
    //int newCenterIndex = m_centerCellIndex;
    GameObject selectGO = null;
    if (m_selectIndex != -1)
      selectGO = m_cellContainer[m_selectIndex];
    if (isInsert)
    {
      m_cellContainer.Remove(passGO);
      m_cellContainer.Insert(0, passGO);

    }
    else
    {
      m_cellContainer.Remove(passGO);
      m_cellContainer.Add(passGO);
    }
    // If there is a select index, we need to update it as well 
    // because container array has been updated
    if (selectGO)
    {
      m_selectIndex = m_cellContainer.IndexOf(selectGO);
    }
  }
}