using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ActionContainer : MonoBehaviour
{
    public delegate void LicenseFinish();
    [SerializeField]
    GameObject[] lActionem;
    [SerializeField]
    GameObject[] lActionDeck;
    Queue<GameObject> qActionDeck;
    GameObject[] lActionHand;
    GameObject[] lActionSelected;
    [SerializeField] float lDefaultActPosY;
    [SerializeField] Transform[] defaultHandPos;
    //[SerializeField] Vector3 chosenPosLeft;
    [SerializeField] Transform[] myBlocks;

    Sequence sequenceFillcontainer;

    //[SerializeField] float fActInterval;


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
        lActionSelected=new GameObject[nActPoint];
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
                Actionem act = lActionHand[i].GetComponent<Actionem>();
                act.FillAction(defaultHandPos[i].position, i);
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

    public void AIChooseAction(int index)
    {
        Debug.Log("AIChooseAction    " + lActionSelected.Length);
        //Vector3 targetPos = chosenPosLeft;
        for (int i = 0; i < lActionSelected.Length; i++)
        {
            if (lActionSelected[i] == null)
            {
                lActionSelected[i] = lActionHand[index];
                lActionHand[index].transform.DOMove(myBlocks[i].position, 0.5f).OnComplete(() =>
                {
                    EnemyAI.I.ChooseAction();
                });
                break;
            }
        }
    }

    public void PlayerChooseAction(int myHandIndex, GameObject targetBlock,int mySelectIndex)
    {
        int targetSelectIndex = -1;
        for (int i = 0; i < myBlocks.Length; i++)
        {
            if (myBlocks[i].gameObject==targetBlock)
            {
                targetSelectIndex = i;
                break;
            }
        }
        Debug.Log(targetSelectIndex);
        if (lActionSelected[targetSelectIndex] ==null)
        {
            lActionSelected[targetSelectIndex] = lActionHand[myHandIndex];
            lActionHand[myHandIndex] = null;
            lActionSelected[targetSelectIndex].GetComponent<Actionem>().MoveAction(myBlocks[targetSelectIndex].transform.position,true, targetSelectIndex);
        }
        else if(mySelectIndex!=-1)
        {
            SwitchAction(mySelectIndex, targetSelectIndex);
        }
        else
        {
            Actionem UnselectOne = lActionSelected[targetSelectIndex].GetComponent<Actionem>();
            PlayerUnchooseAction(UnselectOne.nHandIndex, UnselectOne.nSelectIndex);

            lActionSelected[targetSelectIndex] = lActionHand[myHandIndex];
            lActionHand[myHandIndex] = null;
            lActionSelected[targetSelectIndex].GetComponent<Actionem>().MoveAction(myBlocks[targetSelectIndex].transform.position,true, targetSelectIndex);
        }
    }

    public void SwitchAction(int originIndex,int TargetIndex)
    {
        GameObject go=lActionSelected[originIndex];
        Vector3 pos = go.transform.position;
        lActionSelected[originIndex] = lActionSelected[TargetIndex];
        lActionSelected[originIndex].GetComponent<Actionem>().MoveAction(lActionSelected[TargetIndex].transform.position,true, TargetIndex);
        lActionSelected[TargetIndex] = go;
        lActionSelected[TargetIndex].GetComponent<Actionem>().MoveAction(pos,true,originIndex);

    }

    public void PlayerUnchooseAction(int myHandIndex, int mySelectIndex)
    {
        if (mySelectIndex != -1)
        {
            lActionHand[myHandIndex] = lActionSelected[mySelectIndex];
            lActionSelected[mySelectIndex] = null;
        }
        lActionHand[myHandIndex].GetComponent<Actionem>().MoveAction(defaultHandPos[myHandIndex].transform.position,false,-1);
    }

    public void LockActions()
    {
        foreach (var item in lActionHand)
        {
            if (item != null)
                item.GetComponent<Actionem>().Lock();
        }

    }

    public void UnlockActions()
    {
        Debug.Log(name + "Unlock");
        foreach (var item in lActionHand)
        {
            if (item != null)
                item.GetComponent<Actionem>().Unlock();
        }
    }

    public GameObject[] GetLActionSelected()
    {
        return lActionSelected;
    }
}
