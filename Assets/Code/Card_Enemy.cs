using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class Card_Enemy : Card
{
    public struct rawEnemy
    {
        public string name;
        public int[] personalityPool;
        public int[] appearancePool;
        public int[] internalityPool;
    }

    enum PoolType
    {
        personality,
        appearance,
        internality
    }

    rawEnemy myRawEnemy;

    int maxHealth=100;
    int actPoint=1;

    void Start()
    {
    }

    public override void InitCard(int enemyIndex)
    {
        enemyIndex--;
        //Debug.Log(enemyIndex);
        myRawEnemy.name = ReadCSV.I.GetEnemyElement(enemyIndex, "name");
        //Debug.Log(myRawEnemy.name);
        SetPool(enemyIndex,PoolType.personality);
        SetPool(enemyIndex,PoolType.appearance);
        SetPool(enemyIndex,PoolType.internality);

        this.gameObject.name = myRawEnemy.name;

        string spPath = BattleFieldManager.I.GetFieldNow();
        spPath = spPath+"/"+ myRawEnemy.name;
        SetImage(Resources.Load<Texture>(spPath));
    }
    /// <summary>
    /// 生成技能池
    /// </summary>
    /// <param name="enemyIndex"></param>
    /// <param name="targetPool"></param>
    void SetPool(int enemyIndex,PoolType targetPool)
    {
        string strPool="";
        if (targetPool == PoolType.personality)
            strPool = ReadCSV.I.GetEnemyElement(enemyIndex, "personalityPool");
        else if (targetPool == PoolType.appearance)
            strPool = ReadCSV.I.GetEnemyElement(enemyIndex, "appearancePool");
        else if (targetPool == PoolType.internality)
            strPool = ReadCSV.I.GetEnemyElement(enemyIndex, "internalPool");

        if (strPool =="")
        {
            return;
        }
        string[] strSplitPool = strPool.Split('/');
        int[] nSplitPool = new int[strSplitPool.Length];
        List<int> listRandom = new List<int>();

        if (targetPool == PoolType.personality)
        {
            for (int i = 0; i < ReadCSV.I.GetPersonalityCount(); i++)
            {
                for (int j = 0; j < int.Parse(ReadCSV.I.GetPersonalityElement(i, "rate")); j++)
                {
                    listRandom.Add(i);
                }
            }
        }
        else if(targetPool==PoolType.appearance)
        {

            for (int i = 0; i < ReadCSV.I.GetAppearanceCount(); i++)
            {
                for (int j = 0; j < int.Parse(ReadCSV.I.GetAppearanceElement(i, "rate")); j++)
                {
                    listRandom.Add(i);
                }
            }
        }
        else if(targetPool==PoolType.internality)
        {

            for (int i = 0; i < ReadCSV.I.GetInternalityCount(); i++)
            {
                for (int j = 0; j < int.Parse(ReadCSV.I.GetInternalityElement(i, "rate")); j++)
                {
                    listRandom.Add(i);
                }
            }
        }

        for (int i = 0; i < strSplitPool.Length; i++)
        {
            if (strSplitPool[i] == "r")
            {
                int r = Random.Range(0, listRandom.Count);
                nSplitPool[i] = listRandom[r];
                listRandom.RemoveAt(r);
            }
            else
            {
                nSplitPool[i] = int.Parse(strSplitPool[i]);
            }
        }

        if (targetPool == PoolType.personality)
            myRawEnemy.personalityPool = nSplitPool;
        else if(targetPool == PoolType.appearance)
            myRawEnemy.appearancePool = nSplitPool;
        else if (targetPool == PoolType.internality)
            myRawEnemy.internalityPool = nSplitPool;
    }

    /// <summary>
    /// 点击事件 进入战斗
    /// </summary>
    public override void ClickEvent()
    {
        Debug.Log("ClickEvent");

        BattleFieldManager.I.ExitBattleField(this.gameObject);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(new Vector3(900, 400, 0), 1));
        sequence.Insert(0, transform.DOScale(1.5f, 0.5f));
        sequence.Insert(0.5f, transform.DOScale(1, 0.5f).SetEase(Ease.OutSine));
        sequence.OnComplete(() =>
        {
            BindLabelPage();
        });
        interactable = false;
        //MoveTo(new Vector3(900, 400, 0), 1, 1, false);


    }

    void BindLabelPage()
    {
        LabelMaster labelMaster = this.gameObject.AddComponent<LabelMaster>();
        Transform overall = BattleFieldManager.I.GetOverAll();
        GameObject dataPage = BattleFieldManager.I.GetEnemyDataPage();
        labelMaster.SetMaster(dataPage, imgOutline);
        transform.SetParent(overall);
        int countAll = myRawEnemy.personalityPool.Count() + myRawEnemy.appearancePool.Count() + myRawEnemy.internalityPool.Count();
        string[] arrayName = new string[countAll];
        string[] arrayIntro = new string[countAll];
        int startIndex = 0;
        for (int i = 0; i < myRawEnemy.personalityPool.Count(); i++)
        {
            Debug.Log("personality" + i);
            Debug.Log(myRawEnemy.personalityPool[i]);
            arrayName[i + startIndex] = ReadCSV.I.GetPersonalityElement(myRawEnemy.personalityPool[i], "name");
            arrayName[i + startIndex] = ReadCSV.I.GetPersonalityElement(myRawEnemy.personalityPool[i], "intro");
        }
        startIndex = myRawEnemy.personalityPool.Count();
        for (int i = 0; i < myRawEnemy.appearancePool.Count(); i++)
        {
            Debug.Log("appearance" + i);
            Debug.Log(myRawEnemy.appearancePool[i]);
            arrayName[i + startIndex] = ReadCSV.I.GetAppearanceElement(myRawEnemy.appearancePool[i], "name");
            arrayName[i + startIndex] = ReadCSV.I.GetAppearanceElement(myRawEnemy.appearancePool[i], "intro");
        }
        startIndex = myRawEnemy.personalityPool.Count() + myRawEnemy.appearancePool.Count();
        for (int i = 0; i < myRawEnemy.internalityPool.Count(); i++)
        {
            arrayName[i] = ReadCSV.I.GetInternalityElement(myRawEnemy.internalityPool[i], "name");
            arrayName[i] = ReadCSV.I.GetInternalityElement(myRawEnemy.internalityPool[i], "intro");
        }
        labelMaster.InitMaster(arrayName, arrayIntro);
    }
}
