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

    List<GameObject> listCard;

    public static BattleFieldManager I;

    void Awake()
    {
        listCard = new List<GameObject>();
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 设置战场大小，以此设置卡牌位置
    /// </summary>
    /// <param name="fieldShape">[3,3,3,3,3,1]代表深度为6，每一层三张牌，最后一层一张牌</param>
    public void SetBattleField(int fieldWidth,int fieldDepth,string battleName)
    {
        nDepthNow = 0;
        nWidthNow = 0;
        nFieldWidth = fieldWidth;
        nFieldDepth = fieldDepth;
        listCard.Clear();
        fieldNow = battleName;

        ReadCSV.I.LoadBattleCSV(battleName);
    }

    int nDepthNow;
    int nWidthNow;

    public void AddCard(GameObject card)
    {
        Debug.Log("width"+nWidthNow);
        Debug.Log("depth" + nDepthNow);
        listCard.Add(card);
        card.transform.SetParent(transform);
        string element = ReadCSV.I.GetFieldElement(nDepthNow, nWidthNow);
        Debug.Log(element);
        string cardType = element.Split('_')[0];
        int cardIndex = int.Parse(element.Split('_')[1]);
        if(cardType == "enemy")
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
        card.GetComponent<Card>().MoveTo(targetPos, targetScale, taegetAlpha,interactable);
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
}
