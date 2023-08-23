using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackImg : MonoBehaviour
{
    [SerializeField] Material matBG;
    [SerializeField] Texture texCity;
    Image img;

    public static BackImg I;

    void Awake()
    {
        I = this;
        img=GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        matBG.SetTexture("_NowTex", texCity);
        matBG.SetFloat("_OpacityNow", 1);
    }

    public void SwitchBG(Texture texNext)
    {
        matBG.SetTexture("_NextTex", texNext);
        matBG.DOFloat(0, "_OpacityNow", 2).OnComplete(() =>
        {
            matBG.SetTexture("_NowTex", texNext);
            matBG.SetFloat("_OpacityNow", 1);
        });
    }

    public void SetBlack(bool b)
    {
        if (b)
            img.DOColor(new Color(0.3f, 0.3f, 0.3f, 1), 0.5f);
        else
            img.DOColor(Color.white, 0.5f);
    }
}
