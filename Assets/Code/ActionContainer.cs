using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public delegate void LicenseFinish();
    [SerializeField] GameObject[] lActionem;
    [SerializeField] float lDefaultActPosY;
    [SerializeField] Vector3 chosenPosLeft;

    Sequence sequenceFillcontainer;

    [SerializeField] float fActInterval;

    GameObject[] chosenAct;

    // Start is called before the first frame update
    void Start()
    {
        sequenceFillcontainer =  DOTween.Sequence();
        sequenceFillcontainer.Pause();
        for (int i = 0; i < lActionem.Length; i++)
        {
            sequenceFillcontainer.Insert(0.2f * i, lActionem[i].transform.DOLocalMoveY(lDefaultActPosY, 0.5f).SetAutoKill(false)).SetAutoKill(false);
        }
    }

    public void UpdateActionContainer()
    {
        Card_Enemy.rawEnemy rawEnemy = BattleManager.I.GetRawEnemyNow();
        int nActPoint = rawEnemy.actPoint;
        chosenAct=new GameObject[nActPoint];
        //TODO
    }

    public void FillContainer(LicenseFinish licenseFinish)
    {
        Debug.Log("EnemyFillContainer");

        sequenceFillcontainer.OnComplete(() =>
        {
            licenseFinish();

        }).Restart();
    }

    public void FillContainer()
    {
        Debug.Log("PlayerFillContainer");
        sequenceFillcontainer.Restart();
    }

    public void ChooseAction(int index)
    {
        Debug.Log("AIChooseAction    "+ chosenAct.Length);
        Vector3 targetPos=chosenPosLeft;
        for (int i = 0; i < chosenAct.Length; i++)
        {
            if (chosenAct[i] == null)
            {
                targetPos.x += ((lActionem[0].GetComponent<RectTransform>().sizeDelta.x + 10) * i);
                chosenAct[i] = lActionem[index];
                break;
            }
        }
        lActionem[index].transform.DOLocalMove(targetPos, 0.5f).OnComplete(() =>
        {
            EnemyAI.I.ChooseAction();
        });
    }

    public void LockActions()
    {

    }

    public void UnlockActions()
    {

    }
}
