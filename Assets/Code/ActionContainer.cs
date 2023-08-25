using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public delegate void LicenseFinish();
    [SerializeField]
    GameObject[] lActionem;
    [SerializeField]
    GameObject[] lActionDeck;
    Queue<GameObject> qActionDeck;
    GameObject[] lActionHand;
    [SerializeField] float lDefaultActPosY;
    [SerializeField]
    Transform[] defaultHandPos;
    //[SerializeField] Vector3 chosenPosLeft;
    [SerializeField] Transform[] myBlocks;

    Sequence sequenceFillcontainer;

    //[SerializeField] float fActInterval;

    GameObject[] chosenAct;

    // Start is called before the first frame update
    void Start()
    {
        //sequenceFillcontainer =  DOTween.Sequence();
        //sequenceFillcontainer.Pause();
        //for (int i = 0; i < lActionem.Length; i++)
        //{
        //    sequenceFillcontainer.Insert(0.2f * i, lActionem[i].transform.DOLocalMoveY(lDefaultActPosY, 0.5f).SetAutoKill(false)).SetAutoKill(false);
        //}
        qActionDeck = new Queue<GameObject>();
        for (int i = 0; i < lActionDeck.Length; i++)
        {
            qActionDeck.Enqueue(lActionDeck[i]);
        }
        lActionHand = new GameObject[5];
    }

    public void UpdateActionContainer()
    {
        Card_Enemy.rawEnemy rawEnemy = BattleManager.I.GetRawEnemyNow();
        int nActPoint = rawEnemy.actPoint;
        chosenAct=new GameObject[nActPoint];
        //TODO
    }

    public void FillContainer(LicenseFinish licenseFinish = null)
    {
        Debug.Log("EnemyFillContainer");

        //sequenceFillcontainer.OnComplete(() =>
        //{
        //    licenseFinish();

        //}).Restart();
        StartCoroutine(ienuFillContainer(licenseFinish));

    }

    IEnumerator ienuFillContainer(LicenseFinish licenseFinish=null)
    {
        for (int i = 0; i < lActionHand.Length; i++)
        {
            if (lActionHand[i] == null)
            {
                yield return new WaitForSeconds(1);
                lActionHand[i] = qActionDeck.Dequeue();
                if (TryGetComponent<Actionem>(out Actionem act))
                    act.LicenseActionem(defaultHandPos[i].position);
                else
                    lActionHand[i].transform.DOMove(defaultHandPos[i].position, 0.5f);
            }
        }
        if (licenseFinish != null)
            licenseFinish();
    }

    //public void FillContainer()
    //{
    //    Debug.Log("PlayerFillContainer");
    //    sequenceFillcontainer.Restart();
    //}

    public void ChooseAction(int index)
    {
        Debug.Log("AIChooseAction    " + chosenAct.Length);
        //Vector3 targetPos = chosenPosLeft;
        for (int i = 0; i < chosenAct.Length; i++)
        {
            if (chosenAct[i] == null)
            {
                //targetPos.x += ((lActionem[0].GetComponent<RectTransform>().sizeDelta.x + 10) * i);
                chosenAct[i] = lActionHand[index];
                lActionHand[index].transform.DOMove(myBlocks[i].position, 0.5f).OnComplete(() =>
                {
                    EnemyAI.I.ChooseAction();
                });
                break;
            }
        }
    }

    public void LockActions()
    {
        foreach (var item in lActionHand)
        {
            item.GetComponent<Actionem>().Lock();
        }

    }

    public void UnlockActions()
    {
        foreach (var item in lActionHand)
        {
            item.GetComponent<Actionem>().Unlock();
        }
    }
}
