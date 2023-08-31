using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackImg : MonoBehaviour
{
    //[SerializeField] Material matBG;
    //[SerializeField] Texture texCity;
    //Image img;
    [SerializeField] Image img1;
    [SerializeField] Image img2;

    public static BackImg I;

    void Awake()
    {
        I = this;
        //img=GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //matBG.SetTexture("_NowTex", texCity);
        //matBG.SetFloat("_OpacityNow", 1);
    }

    //public void SwitchBG(Texture texNext)
    //{
    //    matBG.SetTexture("_NextTex", texNext);
    //    matBG.DOFloat(0, "_OpacityNow", 2).OnComplete(() =>
    //    {
    //        matBG.SetTexture("_NowTex", texNext);
    //        matBG.SetFloat("_OpacityNow", 1);
    //    });
    //}

    public void OpenBG(Sprite sp)
    {
        img2.sprite = sp;
        img2.gameObject.transform.DOScale(1, 1);
    }
    public void CloseBG()
    {
        img2.gameObject.transform.DOScale(0, 1);
    }

    public void SetBlack(bool b)
    {
        if (b)
            img2.DOColor(new Color(0.3f, 0.3f, 0.3f, 1), 0.5f);
        else
            img2.DOColor(Color.white, 0.5f);
    }
}
