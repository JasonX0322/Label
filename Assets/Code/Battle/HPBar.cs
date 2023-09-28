using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Text txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InitHPBar(int HPMax, int HPRemain)
    {
        float HPRate = (float)HPRemain / (float)HPMax;
        fill.fillAmount = 0;
        fill.DOFillAmount(HPRate, 2);
        txt.text = HPRemain.ToString() + "/" + HPMax.ToString();
    }

    public void UpdateHealth(int HPMax,int HPRemain)
    {
        float HPRate=(float)HPRemain/(float)HPMax;
        fill.DOFillAmount(HPRate, 1);
        txt.text = HPRemain.ToString() + "/" + HPMax.ToString();
    }
}
