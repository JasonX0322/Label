using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string labelName;
    string labelIntro;

    [SerializeField] GameObject goIntroBlock;
    [SerializeField] Text txtName;
    [SerializeField] Text txtIntro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitLabel(string strName,string strIntro)
    {
        labelName = strName;
        labelIntro = strIntro;

        txtName.text = labelName;
        txtIntro.text= labelIntro;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter" + name);
        goIntroBlock.SetActive(true);
        goIntroBlock.transform.SetParent(BattleFieldManager.I.GetOverAll());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit" + name);
        goIntroBlock.SetActive(false);
        goIntroBlock.transform.SetParent(transform);
    }
}
