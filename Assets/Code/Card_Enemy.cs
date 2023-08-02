using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Enemy : Card
{
    public struct rawEnemy
    {
        public string name;
        public int[] personalityPool;
        public int[] appearancePool;
        public int[] externalPool;
    }

    rawEnemy myRawEnemy;

    void Start()
    {
        Debug.Log("son");
    }

    public void InitEnemy(int depth,int width)
    {
        int enemyIndex = int.Parse(ReadCSV.I.GetFieldElement(depth, width).Split('/')[1]);
        myRawEnemy.name = ReadCSV.I.GetEnemyElement(enemyIndex, 0);

        string strPersonalityPool = ReadCSV.I.GetEnemyElement(enemyIndex, 1);
    }

    public rawEnemy GetEnemyElement(int enemyIndex)
    {

        string[] strSplitPerPool = strPersonalityPool.Split('/');
        int[] nSplitPerPool = new int[strSplitPerPool.Length];
        List<int> listRandom = new List<int>();
        for (int i = 0; i < dtPersonalityPool.Rows.Count; i++)
        {
            listRandom.Add(i);
        }
        for (int i = 0; i < strSplitPerPool.Length; i++)
        {
            if (strSplitPerPool[i] == "r")
            {
                int r = Random.Range(0, listRandom.Count);
                nSplitPerPool[i] = listRandom[r];
                listRandom.RemoveAt(r);
            }
            else
            {
                nSplitPerPool[i] = int.Parse(strSplitPerPool[i]);
            }
        }
        enemy.externalPool = nSplitPerPool;

        //TODO
        return enemy;
    }
}
