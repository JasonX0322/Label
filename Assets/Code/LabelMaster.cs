using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabelMaster : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject goDataPage;
    [SerializeField] Image imgOutline;

    bool bPageOpened;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitMaster()
    {
        goDataPage.GetComponent<DataPage>().AddLabel("邪兽", "吞天食地，只进不出");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter"+name);
        imgOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit"+name);
        imgOutline.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click" + name);
        if(!bPageOpened)
        {
            bPageOpened = true;
            goDataPage.GetComponent<DataPage>().OpenPage();
        }
        else
        {
            bPageOpened = false;
            goDataPage.GetComponent<DataPage>().ClosePage();

        }
    }

    public void SetMaster(GameObject page,Image outline)
    {
        goDataPage = page;
        imgOutline = outline;
    }
}
