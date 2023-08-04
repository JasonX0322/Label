using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void LicenseFinish();
public delegate void EnemyFinish();

public class BattleManager : MonoBehaviour
{
    [SerializeField] ActionContainer playerContainer;
    [SerializeField] ActionContainer enemyContainer;
    [SerializeField] GameObject battlePanel;
    [SerializeField] Button btnSelectFinish;

    Card_Enemy enemy;

    battleState stateNow;

    enum battleState
    {
        license,
        enemyTurn,
        playerTurn,
        determination
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartBattle()
    {
        stateNow = battleState.license;
        battlePanel.SetActive(true);
        enemyContainer.FillContainer();
        playerContainer.FillContainer(() =>
        {
            EnemyAct();
        });
    }


    void EnemyAct()
    {
        stateNow = battleState.enemyTurn;
        EnemyAI.I.ChooseAction(() =>
        {
            PlayerAct();
        });
    }

    void PlayerAct()
    {
        stateNow = battleState.playerTurn;
        btnSelectFinish.interactable=true;
        playerContainer.UnlockActions();
    }
    /// <summary>
    /// 按钮事件,回合结束
    /// </summary>
    public void PlayerSelectFinish()
    {
        stateNow = battleState.determination;
        btnSelectFinish.interactable = false;
        playerContainer.LockActions();


    }


    public void SetEnemy(Card_Enemy _enemy)
    {
        enemy = _enemy;
    }




}
