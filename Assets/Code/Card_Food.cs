using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card_Enemy;

public class Card_Food : Card
{
    string strFoodName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void InitCard(int enemyIndex)
    {
        enemyIndex--;
        //Debug.Log(enemyIndex);
        strFoodName = ReadCSV.I.GetFoodElement(enemyIndex, "name");
        this.gameObject.name = strFoodName;

        string spPath = BattleFieldManager.I.GetFieldNow();
        spPath = spPath + "/" + strFoodName;
        SetImage(Resources.Load<Texture>(spPath));
    }
}
