using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card_Enemy;

public class Card_Obstacle : Card
{
    string strObstacleName;
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void InitCard(int enemyIndex)
    {
        enemyIndex--;
        //Debug.Log(enemyIndex);
        strObstacleName = ReadCSV.I.GetObstacleElement(enemyIndex, "name");
        SetInteractable(false);
        transform.localEulerAngles = Vector3.zero;
        this.gameObject.name = strObstacleName;

        string spPath = BattleFieldManager.I.GetFieldNow();
        spPath = spPath + "/" + strObstacleName;
        SetImage(Resources.Load<Texture>(spPath));
    }
}
