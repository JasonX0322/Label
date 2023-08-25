using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Actionem : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    bool bLock = true;
    bool bDrag = false;
    Vector3 defaultPos;
    Vector3 mouseDis;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Unlock()
    {
        bLock = false;
    }

    public void Lock()
    {
        bLock = true;
    }

    public void Update()
    {
        if(bDrag)
        {
            Debug.Log(Input.mousePosition);
            transform.position = Input.mousePosition - mouseDis;
        }
    }

    public void LicenseActionem(Vector3 pos)
    {
        defaultPos = pos;
        transform.DOMove(pos, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bLock)
            return;
        mouseDis = Input.mousePosition;
        Debug.Log(mouseDis);
        Debug.Log(transform.position);
        //mouseDis=Camera.main.ScreenToWorldPoint(mouseDis);
        //Debug.Log(mouseDis);
        mouseDis = mouseDis - transform.position;
        Debug.Log(mouseDis);
        bDrag = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (bLock)
            return;
        bDrag = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOLocalMoveY(transform.localPosition.y + 10, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOLocalMoveY(transform.localPosition.y + 10, 0.5f);
    }
}
