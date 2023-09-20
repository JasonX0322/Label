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
        ActionAnim();
    }

    /// <summary>
    /// 行动碰撞动画
    /// </summary>
    /// <returns></returns>
    public void ActionAnim()
    {
        StartCoroutine(ienuActionemAtk());
    }

    IEnumerator ienuActionemAtk()
    {
        GameObject[] playerSelected = playerContainer.GetLActionSelected();
        Debug.Log("player select " + playerSelected.Length + " actions");
        GameObject[] enemySelected = enemyContainer.GetLActionSelected();
        Debug.Log("enemy select " + enemySelected.Length + " actions");
        int maxIndex = (playerSelected.Length > enemySelected.Length) ? playerSelected.Length : enemySelected.Length;
        Actionem[] playerAction = new Actionem[maxIndex];
        Actionem[] enemyAction = new Actionem[maxIndex];
        for (int i = 0; i < playerSelected.Length; i++)
        {
            playerAction[i] = playerSelected[i].GetComponent<Actionem>();
        }
        for (int i = 0; i < playerSelected.Length; i++)
        {
            enemyAction[i] = enemySelected[i].GetComponent<Actionem>();
        }
        for (int i = 0; i < maxIndex; i++)
        {
            if (playerAction[i] != null)
                playerAction[i].ActionCollision(enemyAction[i]);
            if (enemyAction[i] != null)
                enemyAction[i].ActionCollision(playerAction[i]);
            yield return new WaitForSeconds(1);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="action">目标</param>
    /// <param name="damage">伤害</param>
    public void Attack(Actionem action,int damage)
    {

    }

    public Card_Enemy.rawEnemy GetRawEnemyNow()
    {
        return rawEnemyNow;
    }


}
