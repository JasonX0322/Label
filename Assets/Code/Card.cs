using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void TurnOverEvent();

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image imgCard;
    public Image imgOutline;
    Material matCard;

    public bool interactable = true;
    // Start is called before the first frame update
    void Awake()
    {
        imgOutline = GetComponentsInChildren<Image>()[0];
        imgCard = GetComponentsInChildren<Image>()[1];
        matCard = new Material(Resources.Load<Material>("card"));
        imgCard.material = matCard;
    }

    public virtual void InitCard(int cardIndex)
    {

    }

    public virtual void ClickEvent()
    {

    }

    /// <summary>
    /// 卡牌移动
    /// </summary>
    public void MoveTo(Vector3 pos,float scale,float alpha,bool _interactable=false)
    {
        transform.localScale = Vector3.zero;
        imgCard.color = new Color(1, 1, 1, 0);
        transform.SetAsFirstSibling();
        transform.localPosition = pos;
        transform.DOScale(scale, 0.5f);
        imgCard.DOFade(alpha, 0.5f);
        SetInteractable(_interactable);
    }
    /// <summary>
    /// 翻面
    /// </summary>
    public void TurnOver(TurnOverEvent turnOverEvent)
    {
        transform.DORotate(Vector3.zero, 0.5f).OnComplete(() => turnOverEvent());
    }

    public void SetImage(Sprite sp)
    {
        imgCard.sprite = sp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enter");
        if (!interactable)
            return;
        imgOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("exit");
        if (!interactable)
            return;
        imgOutline.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;

        ClickEvent();
    }

    public void SetInteractable(bool b)
    {
        interactable = b;
    }

    public void HideCard()
    {
        interactable = false;
        imgOutline.enabled = false;
        imgCard.DOFade(0, 0.5f);
    }

    public void ShowCard()
    {
        interactable = true;
        imgCard.DOFade(1, 0.5f);
    }

}
