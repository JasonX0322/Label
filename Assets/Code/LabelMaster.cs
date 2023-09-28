using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabelMaster : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject goDataPage;
    [SerializeField] Image imgOutline;

    public struct TagInfo
    {
        public string tagName;
        public string tagIntro;
    }

    List<TagInfo> myTags = new List<TagInfo>();

    bool bLocked = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitMaster(string[] tagName, string[] tagIntro)
    {
        if (tagName.Length != tagIntro.Length)
            Debug.LogError("标签数量有误");


        for (int i = 0; i < tagName.Length; i++)
        {
            TagInfo newtag = new TagInfo();
            newtag.tagName = tagName[i];
            newtag.tagIntro = tagIntro[i];
            myTags.Add(newtag);
            goDataPage.GetComponent<DataPage>().AddLabel(tagName[i], tagIntro[i]);
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

    public void AddTag(TagInfo tagInfo)
    {
        myTags.Add(tagInfo);
        goDataPage.GetComponent<DataPage>().AddLabel(tagInfo.tagName, tagInfo.tagIntro);
    }
}
