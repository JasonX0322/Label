using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image imgCard;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("parent");
    }

    public void MoveTo(Vector3 pos,float scale,float alpha)
    {
        transform.SetAsFirstSibling();
        transform.DOLocalMove(pos, 0.1f).OnComplete(()=>
        {
        });
        transform.DOScale(scale, 0.1f);
        imgCard.DOFade(alpha, 0.1f);
    }
}
