using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPage : MonoBehaviour
{
    [SerializeField] Transform tLabelParent;
    [SerializeField] Transform tLineParent;
    [SerializeField] Vector2[] defaultPos;


    [SerializeField]Object objLabel;
    [SerializeField]Object objLine;

    List<GameObject> lLael;
    List<GameObject> lLine;

    [SerializeField] GameObject goMaster;

    List<Sequence> listSequence;

    [SerializeField] GameObject[] goUnderCtrl;

    void Awake()
    {

        if (lLael == null)
        {
            lLael = new List<GameObject>();
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
        if(lLael==null)
        {
            lLael = new List<GameObject>();
            lLine = new List<GameObject>();
            listSequence=new List<Sequence>();
        }
        GameObject goLabel = Instantiate(objLabel) as GameObject;
        goLabel.transform.SetParent(tLabelParent);
        goLabel.transform.localScale = Vector3.zero;
        goLabel.transform.localPosition = Vector3.zero;
        goLabel.GetComponent<Label>().InitLabel(name, intro);
        lLael.Add(goLabel);

        GameObject goLine = Instantiate(objLine) as GameObject;
        goLine.transform.SetParent(tLineParent);
        goLine.GetComponent<Line>().InitLine(goMaster, lLael[lLael.Count - 1]);
        lLine.Add(goLine);


        Sequence sequence = DOTween.Sequence();
        sequence.Append(lLael[listSequence.Count].transform.DOScale(1, 1).SetAutoKill(false));
        sequence.Join(lLael[listSequence.Count].transform.DOLocalMove(defaultPos[listSequence.Count], 1).SetAutoKill(false));
        sequence.SetAutoKill(false);
        listSequence.Add(sequence);
    }

    public void OpenPage()
    {
        for (int i = 0; i < lLael.Count; i++)
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
        for (int i = 0; i < lLael.Count; i++)
        {
            //listSequence[i].Pause();
            lLael[i].transform.localScale = Vector3.zero;
            lLael[i].transform.localPosition = Vector3.zero;
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
}
