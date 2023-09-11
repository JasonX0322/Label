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

    [SerializeField] GameObject[] enemyBlocks;
    [SerializeField] GameObject[] playerBlocks;

    Card_Enemy.rawEnemy rawEnemyNow;

    battleState stateNow;

    public static BattleManager I;

    enum battleState
    {
        license,
        enemyTurn,
        playerTurn,
        determination
    }

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartBattle(Card_Enemy.rawEnemy rawEnemy)
    {
        Debug.Log("StartBattle");
        BGManager.I.SetBlack(true);
        rawEnemyNow = rawEnemy;
        stateNow = battleState.license;
        battlePanel.SetActive(true);
        foreach (var item in enemyBlocks)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < rawEnemy.actPoint; i++)
        {
            enemyBlocks[i].SetActive(true);
        }
        foreach (var item in playerBlocks)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < PlayerManager.I.GetActPoint(); i++)
        {
            playerBlocks[i].SetActive(true);
        }

        enemyContainer.UpdateActionContainer();
        playerContainer.UpdateActionContainer();
        EnemyAI.I.SetRawEnemy(rawEnemyNow);
        enemyContainer.FillContainer();
        playerContainer.FillContainer(() =>
        {
            EnemyAct();
        });
    }

    /// <summary>
    /// 敌方选择
    /// </summary>
    void EnemyAct()
    {
        Debug.Log("EnemyAct");
        stateNow = battleState.enemyTurn;
        EnemyAI.I.ChooseAllAction(() =>
        {
            PlayerAct();
        });
    }
    /// <summary>
    /// 玩家选择
    /// </summary>
    void PlayerAct()
    {
        Debug.Log("PlayerAct");
        stateNow = battleState.playerTurn;
        btnSelectFinish.interactable=true;
        playerContainer.UnlockActions();
    }
    /// <summary>
    /// 按钮事件,双方选择结束
    /// </summary>
    public void PlayerSelectFinish()
    {
        stateNow = battleState.determination;
        btnSelectFinish.interactable = false;
        playerContainer.LockActions();
    }

    public Card_Enemy.rawEnemy GetRawEnemyNow()
    {
        return rawEnemyNow;
    }


}
