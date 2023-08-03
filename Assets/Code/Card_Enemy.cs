using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Card_Enemy : Card
{
    public struct rawEnemy
    {
        public string name;
        public int[] personalityPool;
        public int[] appearancePool;
        public int[] internality;
    }

    enum PoolType
    {
        personality,
        appearance,
        internality
    }

    rawEnemy myRawEnemy;

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
            myRawEnemy.internality = nSplitPool;
    }

    public override void ClickEvent()
    {
        Debug.Log("ClickEvent");

        BattleFieldManager.I.ExitBattleField(this.gameObject);

        MoveTo(new Vector3(900, 400, 0), 1, 1, false);

        LabelMaster labelMaster = this.gameObject.AddComponent<LabelMaster>();

        GameObject dataPage = BattleFieldManager.I.GetEnemyDataPage();

        labelMaster.SetMaster(dataPage, imgOutline);
        labelMaster.InitMaster();
    }
}
