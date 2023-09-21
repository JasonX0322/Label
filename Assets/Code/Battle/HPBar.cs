using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

    void OnEnable()
    {
        fill.fillAmount = 0;
        fill.DOFillAmount(1, 2);
    }

    public void SubHealth(int HPMax,int HPRemain)
    {
        float HPRate=(float)HPRemain/(float)HPMax;
        fill.DOFillAmount(HPRate, 1);
        txt.text = HPRemain.ToString() + "/" + HPMax.ToString();
    }
}
