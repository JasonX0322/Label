using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Actionem : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    bool bLock = true;
    bool bDragging = false;
    Vector3 defaultPos;
    Vector3 mouseDis;

    Vector3 targetPos;
    GameObject targetBlock;
    bool bSelected;

    public int nHandIndex=-1;
    public int nSelectIndex = -1;

    [SerializeField] ActionContainer container;
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
        if(bDragging)
        {
            //Debug.Log(Input.mousePosition);
            transform.position = Input.mousePosition - mouseDis;
        }
    }

    public void MoveAction(Vector3 pos,int handIndex)
    {
        defaultPos = pos;
        nHandIndex = handIndex;
        transform.DOMove(pos, 0.5f);
    }
    public void MoveAction(Vector3 pos, bool selected,int selectIndex)
    {
        defaultPos = pos;
        bSelected = selected;
        nSelectIndex = selectIndex;
        transform.DOMove(pos, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bLock)
            return;
        transform.SetAsLastSibling();
        mouseDis = Input.mousePosition;
        mouseDis = mouseDis - transform.position;
        bDragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (bLock)
            return;
        bDragging = false;
        if(targetBlock != null)
        {
            container.PlayerChooseAction(nHandIndex, targetBlock, nSelectIndex);
        }
        else
        {
            container.PlayerUnchooseAction(nHandIndex, nSelectIndex);
        }
        //transform.DOMove(targetPos, 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bDragging)
            return;
        if (!bSelected)
        {
            Vector3 newpos = defaultPos;
            newpos.y += 100;
            transform.DOMove(newpos, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bDragging)
            return;
        transform.DOMove(defaultPos, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        if(col.tag=="SelectedActionPos")
        {
            targetBlock = col.gameObject;

            targetPos = col.gameObject.transform.position;
            defaultPos = targetPos;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "SelectedActionPos")
        {
            targetBlock = null;
        }
    }
}
