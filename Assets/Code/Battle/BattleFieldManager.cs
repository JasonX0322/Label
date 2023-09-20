using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldManager : MonoBehaviour
{
    int nFieldWidth;
    int nFieldDepth;

    string fieldNow;

    int nDepthNow;
    int nWidthNow;

    List<GameObject> listCard;

    public static BattleFieldManager I;

    [SerializeField] LabelMaster playerMaster;
    [SerializeField] GameObject enemyDataPage;
    [SerializeField] Transform tOverAll;
    [SerializeField] Transform tCardList;

    void Awake()
    {
        listCard = new List<GameObject>();
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] arrayName = new string[1] { "邪兽" };
        string[] arrayIntro = new string[1] { "吞天食地，只进不出" };
        playerMaster.InitMaster(arrayName, arrayIntro);
        Debug.Log("BatlleFieldStart");
        InitBattleField();
    }

    /// <summary>
    /// 设置战场大小，以此设置卡牌位置
    /// </summary>
    public void InitBattleField()
    {
        ReadCSV.I.LoadBattleCSV(MainManager.I.battleInfo.battleName);
        nDepthNow = 0;
        nWidthNow = 0;
        ReadCSV.fieldSize fSize = ReadCSV.I.GetFieldSize();
        nFieldWidth = fSize.nWidth;
        nFieldDepth = fSize.nDepth;
        Debug.Log("size" + nFieldDepth + " " + nFieldWidth);
        listCard.Clear();
        fieldNow = MainManager.I.battleInfo.battleName;
        StartCoroutine(ienuAddCard());
    }
    /// <summary>
    /// 控制按时间出现卡牌
    /// </summary>
    /// <returns></returns>
    IEnumerator ienuAddCard()
    {

        Object objCard = Resources.Load<Object>("prefab/Card");
        yield return new WaitForSeconds(1);
        for (int i = 0; i < nFieldDepth; i++)
        {
            for (int j = 0; j < nFieldWidth; j++)
            {
                GameObject go = GameObject.Instantiate(objCard) as GameObject;
                AddCard(go);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 生成牌
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(GameObject card)
    {
        listCard.Add(card);
        card.transform.SetParent(tCardList);

        string element = ReadCSV.I.GetFieldElement(nDepthNow, nWidthNow);
        if (element != "")
        {
            string cardType = element.Split('_')[0];
            int cardIndex = int.Parse(element.Split('_')[1]);
            if (cardType == "enemy")
            {
                card.AddComponent<Card_Enemy>();
            }
            if (cardType == "food")
            {
                card.AddComponent<Card_Food>();
            }
            if (cardType == "obstacle")
            {
                card.AddComponent<Card_Obstacle>();
            }
            card.GetComponent<Card>().InitCard(cardIndex);
            float fIntervalX = (Screen.width - 200) / (nFieldWidth - 1 + 2);
            float fStartPosX = -Screen.width / 2 + fIntervalX + 100;
            bool interactable = (nDepthNow == 0);
            Vector3 targetPos = new Vector3((fStartPosX + nWidthNow * fIntervalX) * (1.0f - 0.1f * nDepthNow), -Screen.height / 2 + 200 + 100 * nDepthNow, 0);
            float targetScale = Mathf.Clamp01(1.0f - 0.1f * nDepthNow);
            float taegetAlpha = Mathf.Clamp01(1.0f - 0.2f * nDepthNow);
            card.GetComponent<Card>().MoveTo(targetPos, targetScale, taegetAlpha, interactable);

        }

        nWidthNow++;
        if (nWidthNow >= nFieldWidth)
        {
            nDepthNow++;
            nWidthNow = 0;
        }
    }

    public string GetFieldNow()
    {
        return fieldNow;
    }

    public void ExitBattleField(GameObject exception)
    {
        for (int i = 0; i < listCard.Count; i++)
        {
            if (listCard[i] == exception)
                continue;
            listCard[i].GetComponent<Card>().HideCard();
        }
    }

    public GameObject GetEnemyDataPage()
    {
        return enemyDataPage;
    }

    public Transform GetOverAll()
    {
        return tOverAll;
    }
}
