using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPage : MonoBehaviour
{
    [SerializeField] Transform tLabelParent;
    [SerializeField] Transform tLineParent;
    [SerializeField] Vector3[] defaultPos;


    Object objTag;
    Object objLine;

    List<GameObject> lTag;
    List<GameObject> lLine;

    [SerializeField] GameObject goMaster;

    List<Sequence> listSequence;

    [SerializeField] GameObject[] goUnderCtrl;

    void Awake()
    {
        objTag = Resources.Load("prefab/Tag");
        objLine = Resources.Load("prefab/Line");
        if (lTag == null)
        {
            lTag = new List<GameObject>();
            lLine = new List<GameObject>();
            listSequence = new List<Sequence>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddLabel(string name,string intro)
    {
        if(lTag==null)
        {
            lTag = new List<GameObject>();
            lLine = new List<GameObject>();
            listSequence=new List<Sequence>();
        }
        GameObject goLabel = Instantiate(objTag) as GameObject;
        goLabel.transform.SetParent(tLabelParent);
        goLabel.transform.localScale = Vector3.zero;
        goLabel.transform.localPosition = Vector3.zero;
        goLabel.GetComponent<Tag>().InitLabel(name, intro);
        lTag.Add(goLabel);

        GameObject goLine = Instantiate(objLine) as GameObject;
        goLine.transform.SetParent(tLineParent);
        goLine.GetComponent<Line>().InitLine(goMaster, lTag[lTag.Count - 1]);
        lLine.Add(goLine);


        Debug.Log(listSequence.Count);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(lTag[listSequence.Count].transform.DOScale(1, 1).SetAutoKill(false));
        sequence.Join(lTag[listSequence.Count].transform.DOLocalMove(defaultPos[listSequence.Count], 1).SetAutoKill(false));
        sequence.SetAutoKill(false);
        listSequence.Add(sequence);
    }

    public void OpenPage()
    {
        for (int i = 0; i < lTag.Count; i++)
        {
            listSequence[i].Restart();
        }
        foreach (var item in goUnderCtrl)
        {
            item.SetActive(true);
        }
    }

    public void ClosePage()
    {
        for (int i = 0; i < lTag.Count; i++)
        {
            //listSequence[i].Pause();
            lTag[i].transform.localScale = Vector3.zero;
            lTag[i].transform.localPosition = Vector3.zero;
        }

        for (int i = 0; i < lLine.Count; i++)
        {
            lLine[i].GetComponent<Line>().HideLine();
        }
        foreach (var item in goUnderCtrl)
        {
            item.SetActive(false);
        }

    }

    public void SetPage(GameObject master)
    {
        goMaster = master;
    }


    public void OpenReward()
    {
        Debug.Log("OpenReward");
        foreach (var item in goUnderCtrl)
        {
            item.SetActive(true);
        }
        foreach (var item in lTag)
        {
            item.transform.localScale = Vector3.zero;
            item.transform.position=transform.position;
            item.GetComponent<Tag>().interactable = true;
        }
        int rewardNum = Random.Range(1, lTag.Count);
        Vector3[] newTagPos = new Vector3[rewardNum];
        List<float> lAngle = new List<float>(new float[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330});
        List<int> lRewardTag = new List<int>();
        for (int i = 0; i < lTag.Count; i++)
        {
            lRewardTag.Add(i);
        }
        for (int i = 0; i < rewardNum; i++)
        {
            int rIndex=Random.Range(0,lAngle.Count);
            float angle = lAngle[rIndex];
            lAngle.RemoveAt(rIndex);
            angle += Random.Range(-10.0f, 10.0f);
            angle *= Mathf.Deg2Rad;
            float dis = Random.Range(3f, 5.0f);
            newTagPos[i] = new Vector3(transform.position.x + Mathf.Cos(angle) * dis, transform.position.y + Mathf.Sin(angle) * dis, 100);
            int rRewardTagIndex=Random.Range(0,lRewardTag.Count);
            lRewardTag.Remove(rRewardTagIndex);
            lTag[rRewardTagIndex].transform.DOScale(1, 1);
            lTag[rRewardTagIndex].transform.DOMove(newTagPos[i], 1);

            Debug.LogWarning(newTagPos[i] + "  " + dis);
        }
    }
}
