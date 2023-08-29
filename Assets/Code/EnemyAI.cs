using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI I;
    [SerializeField] ActionContainer enemyActionContainer;

    int actPointNow;

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
    }

    public void ChooseAllAction(EnemyFinish enemyFinish)
    {
        Debug.Log("ChooseAll");
        actPointNow = myRawEnemy.actPoint;
        finishEvent = enemyFinish;
        ChooseAction();
    }

    int _index;
    public void ChooseAction()
    {
        Debug.Log("ChooseAct      "+actPointNow);
        actPointNow--;
        if (actPointNow < 0)
        {
            Debug.Log("ChooseFinish");
            finishEvent();
            return;
        }

        enemyActionContainer.AIChooseAction(_index++);
    }
}
