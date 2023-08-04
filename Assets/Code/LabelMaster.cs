using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabelMaster : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject goDataPage;
    [SerializeField] Image imgOutline;

    bool bLocked = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitMaster(string[] labelName, string[] labelIntro)
    {
        if (labelName.Length != labelIntro.Length)
            Debug.LogError("标签数量有误");
        for(int i = 0; i < labelName.Length; i++)
        {
            goDataPage.GetComponent<DataPage>().AddLabel(labelName[i], labelIntro[i]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bLocked)
            return;
        Debug.Log("enter"+name);
        imgOutline.enabled = true;
        goDataPage.GetComponent<DataPage>().OpenPage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bLocked)
            return;
        Debug.Log("exit"+name);
        imgOutline.enabled = false;
        goDataPage.GetComponent<DataPage>().ClosePage();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click" + name);
        bLocked = !bLocked;
    }

    public void SetMaster(GameObject page,Image outline)
    {
        goDataPage = page;
        imgOutline = outline;
        goDataPage.GetComponent<DataPage>().SetPage(this.gameObject);
    }
}
