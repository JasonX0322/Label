using DG.Tweening;
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

    [SerializeField] GameObject playerInfo;
    [SerializeField] HPBar playerHPBar;
    [SerializeField] GameObject enemyInfo;
    [SerializeField] HPBar enemyHPBar;

    GameObject goEnemy;

    public Transform parentBattle;

    [SerializeField] DataPage enemyDataPage;
    [SerializeField] LabelMaster playerTagMaster;
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

    public void StartBattle(Card_Enemy.rawEnemy rawEnemy,GameObject go)
    {
        Debug.Log("StartBattle");
        goEnemy = go;
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

        EnemyAI.I.SetRawEnemy(rawEnemyNow);
        enemyContainer.UpdateActionContainer(true,"person");
        playerContainer.UpdateActionContainer(false,"taotie");
        OpenPlayerInfo();
        FillContainer();
    }

    /// <summary>
    /// 打开双方数据页面，生命条
    /// </summary>
    void OpenPlayerInfo()
    {
        playerInfo.SetActive(true);
        PlayerManager.I.myHPBar = playerHPBar;
        playerHPBar.InitHPBar(PlayerManager.I.HP_Max, PlayerManager.I.HP_Remain);
        enemyInfo.SetActive(true);
        EnemyAI.I.myHPBar = enemyHPBar;
        enemyHPBar.InitHPBar(EnemyAI.I.HP_Max, EnemyAI.I.HP_Remain);
    }

    /// <summary>
    /// 填充手牌
    /// </summary>
    void FillContainer()
    {
        enemyContainer.FillContainer(() =>
        {
            EnemyAct();
        });
        playerContainer.FillContainer();
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
        GameObject[] enemySelected = enemyContainer.GetLActionSelected();
        int maxIndex = (playerSelected.Length > enemySelected.Length) ? playerSelected.Length : enemySelected.Length;
        Actionem[] playerAction = new Actionem[maxIndex];
        Actionem[] enemyAction = new Actionem[maxIndex];
        for (int i = 0; i < playerSelected.Length; i++)
        {
            if (playerSelected[i] != null)
                playerAction[i] = playerSelected[i].GetComponent<Actionem>();
        }
        for (int i = 0; i < playerSelected.Length; i++)
        {
            if (enemySelected[i] != null)
                enemyAction[i] = enemySelected[i].GetComponent<Actionem>();
        }
        for (int i = 0; i < maxIndex; i++)
        {
            enemyAction[i].TurnOver(() =>
            {
                if (playerAction[i] != null)
                    playerAction[i].ActionCollision(enemyAction[i]);
                if (enemyAction[i] != null)
                    enemyAction[i].ActionCollision(playerAction[i]);
            });
            yield return new WaitForSeconds(1);
        }
        Debug.Log("ienuActionemAtk finish");
        playerContainer.ClearSelectedAction();
        enemyContainer.ClearSelectedAction();
        Determin();
    }

    /// <summary>
    /// 判断胜负
    /// </summary>
    void Determin()
    {
        int playerHP = PlayerManager.I.HP_Remain;
        int enemyHP = EnemyAI.I.HP_Remain;
        if (playerHP <= 0)
        {
            BattleFail();
        }
        else if(enemyHP<=0)
        {
            BattleSuccess();
        }
        else
        {
            Debug.Log("Game continue");
            FillContainer();
        }
    }

    [SerializeField] GameObject pageGameFail;
    void BattleFail()
    {
        Debug.Log("Game fail");
        pageGameFail.SetActive(true);
    }

    void BattleSuccess()
    {
        Debug.Log("Game success");
        ShowReward();


    }
    /// <summary>
    /// 获胜后选择获取
    /// </summary>
    void ShowReward()
    {
        goEnemy.transform.SetParent(BattleFieldManager.I.GetOverAll());
        goEnemy.GetComponent<Card_Enemy>().UnbindLabelPage();
        goEnemy.transform.DOLocalMove(Vector3.zero, 1).OnComplete(() =>
        {
            enemyDataPage.OpenReward();
        });
    }
    /// <summary>
    /// 选取结束，关闭奖励，下一层
    /// </summary>
    [SerializeField] ParticleSystem particleReward;
    void CloseReward()
    {
        particleReward.Play();
        enemyDataPage.ClosePage();
        goEnemy.GetComponent<Image>().DOFillAmount(0, 1);
        particleReward.transform.localPosition = new Vector3(0, -1.1f, 0);
        particleReward.transform.DOLocalMoveY(1.1f, 1).OnComplete(() => particleReward.Stop());
        //TODO
    }

    /// <summary>
    /// 攻击，造成伤害
    /// </summary>
    /// <param name="damageSource">目标</param>
    /// <param name="damage">伤害</param>
    public void Attack(Actionem damageSource,int damage)
    {
        Character target;
        if (damageSource.isEnemy)
            target = PlayerManager.I;
        else
            target = EnemyAI.I;

        target.SubHealth(damage);
    }

    public Card_Enemy.rawEnemy GetRawEnemyNow()
    {
        return rawEnemyNow;
    }

    public void SelectRewardTag(LabelMaster.TagInfo tagInfo)
    {
        playerTagMaster.AddTag(tagInfo);
    }


}
