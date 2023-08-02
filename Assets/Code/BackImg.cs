using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackImg : MonoBehaviour
{
    [SerializeField] Material matBG;
    [SerializeField] Texture texCity;

    public static BackImg I;

    void Awake()
    {
        I = this;
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
}
