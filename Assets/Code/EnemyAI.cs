using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    public static EnemyAI I;

    [SerializeField] ActionContainer enemyActionContainer;

    EnemyFinish finishEvent;

    Card_Enemy.rawEnemy myRawEnemy;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetRawEnemy(Card_Enemy.rawEnemy raw)
    {
        myRawEnemy = raw;
        HP_Max = myRawEnemy.health;
        HP_Remain = myRawEnemy.health;
        ActPoint_Max = myRawEnemy.actPoint;
    }

    public void ChooseAllAction(EnemyFinish enemyFinish)
    {
        Debug.Log("ChooseAll");
        finishEvent = enemyFinish;
        ChooseAction();
    }

    int _index;
    public void ChooseAction()
    {
        int ActPoint_Remain = ActPoint_Max;
        Debug.Log("ChooseAct      "+ActPoint_Remain);
        ActPoint_Remain--;
        if (ActPoint_Remain < 0)
        {
            Debug.Log("ChooseFinish");
            finishEvent();
            return;
        }

        enemyActionContainer.AIChooseAction(_index++);
    }
}
