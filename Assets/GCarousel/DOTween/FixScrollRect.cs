using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixScrollRect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerClickHandler
{
    public ScrollRect MainScroll;

    bool m_isDragging = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDragging = true;
        MainScroll.OnBeginDrag(eventData);
    }


    public void OnDrag(PointerEventData eventData)
    {
        this.gameObject.transform.parent.parent.parent.Find("_carouselManager").GetComponent<GCarouselController>().onDrag(eventData);
        MainScroll.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        /*if(this.gameObject.transform.parent.parent.parent.name == "CarouselBrandsNEW")
        {

        }*/

        m_isDragging = false;
        this.gameObject.transform.parent.parent.parent.Find("_carouselManager").GetComponent<GCarouselController>().onEndDrag(eventData);
        MainScroll.OnEndDrag(eventData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_isDragging)
        {
            //print("clickedCellName: " + eventData.pointerClick.name);
            //print("parent name: " + this.gameObject.transform.parent.parent.parent.parent.name);
            this.gameObject.transform.parent.parent.parent.Find("_carouselManager").GetComponent<GCarouselController>().onCellClicked(eventData);
            //GameObject.Find("CarouselController").GetComponent<GCarouselController>().onCellClicked(eventData);
        }
    }

    public void OnScroll(PointerEventData data)
    {

        this.gameObject.transform.parent.parent.parent.Find("_carouselManager").GetComponent<GCarouselController>().onScroll(data);
    }


}
