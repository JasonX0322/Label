using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image imgCard;
    [SerializeField] Image imgOutline;
    Material matCard;

    bool interactable = true;
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
    public void MoveTo(Vector3 pos,float scale,float alpha,bool interactable=false)
    {
        transform.SetAsFirstSibling();
        transform.DOLocalMove(pos, 0.1f).OnComplete(()=>
        {
        });
        transform.DOScale(scale, 0.1f);
        imgCard.DOFade(alpha, 0.1f);

        SetInteractable(interactable);
    }
    /// <summary>
    /// 翻面
    /// </summary>
    public void TurnOver()
    {
        transform.DORotate(Vector3.zero, 0.5f).OnComplete(() => ClickEvent());
    }

    public void SetImage(Texture tex)
    {
        matCard.SetTexture("_front", tex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        if (!interactable)
            return;
        imgOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        if (!interactable)
            return;
        imgOutline.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;
        TurnOver();
    }

    public void SetInteractable(bool b)
    {
        interactable = b;
    }



}
