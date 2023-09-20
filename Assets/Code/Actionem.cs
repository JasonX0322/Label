using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Actionem : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    bool bLock = true;
    bool bDragging = false;
    Vector3 defaultPos;
    Vector3 mouseDis;

    Vector3 targetPos;
    GameObject targetBlock;
    bool bSelected;

    public int nHandIndex=-1;
    public int nSelectIndex = -1;

    public bool isEnemy;

    [SerializeField] ActionContainer container;

    public enum ActionType
    {
        ATK,
        DEF,
        CounterATK
    }
    public ActionType actionType;
    int def;
    int atk;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Unlock()
    {
        bLock = false;
    }

    public void Lock()
    {
        bLock = true;
    }

    public void Update()
    {
        if(bDragging)
        {
            //Debug.Log(Input.mousePosition);
            transform.position = Input.mousePosition - mouseDis;
        }
    }


    public void FillAction(Vector3 pos, int handIndex)
    {
        int dir = isEnemy ? 1 : -1;
        Vector3 newPos = pos;
        newPos.y += 200 * dir;
        transform.position = newPos;
        defaultPos = pos;
        nHandIndex = handIndex;
        transform.DOMoveY(pos.y, 0.5f);
    }

    public void MoveAction(Vector3 pos, bool selected,int selectIndex)
    {
        defaultPos = pos;
        bSelected = selected;
        nSelectIndex = selectIndex;
        transform.DOMove(pos, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bLock)
            return;
        transform.SetAsLastSibling();
        mouseDis = Input.mousePosition;
        mouseDis = mouseDis - transform.position;
        bDragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (bLock)
            return;
        bDragging = false;
        if(targetBlock != null)
        {
            container.PlayerChooseAction(nHandIndex, targetBlock, nSelectIndex);
        }
        else
        {
            container.PlayerUnchooseAction(nHandIndex, nSelectIndex);
        }
        //transform.DOMove(targetPos, 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bDragging)
            return;
        if (!bSelected)
        {
            Vector3 newpos = defaultPos;
            newpos.y += 100;
            transform.DOMove(newpos, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bDragging)
            return;
        transform.DOMove(defaultPos, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        if(col.tag=="SelectedActionPos")
        {
            targetBlock = col.gameObject;

            targetPos = col.gameObject.transform.position;
            defaultPos = targetPos;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "SelectedActionPos")
        {
            targetBlock = null;
        }
    }


    //ATK ATK 大=伤害
    //ATK DEF 攻-防=伤害
    //ATK CounterATK 1.攻-防=伤害 2.防守反击=伤害
    //ATK NULL 攻击=伤害

    //DEF DEF 无效果
    //DEF CounterATK 1.无效果 2.攻-防=伤害
    //DEF NULL 无效果

    //CounterATK CounterATK 1.无效果 2.大=伤害
    //CounterATK NULL 1.无效果2.攻击=伤害
    public void ActionCollision(Actionem other)
    {
        if (actionType == ActionType.ATK)
        {
            if (other == null)
            {
                AtkSuccessAnim(atk, true);
                return;
            }
            switch (other.actionType)
            {
                case ActionType.ATK:
                    if (other.atk >= atk)
                    {
                        AtkFailAnim();
                    }
                    else
                    {
                        AtkSuccessAnim(atk);
                    }
                    break;
                case ActionType.DEF:
                    if (other.def >= atk)
                    {
                        AtkFailAnim();
                    }
                    else
                    {
                        AtkSuccessAnim(atk - other.def);
                    }
                    break;
                case ActionType.CounterATK:
                    if (other.def >= atk)
                    {
                        AtkFailAnim();
                    }
                    else
                    {
                        AtkSuccessAnim(atk - other.def);
                    }
                    break;
                default:
                    break;
            }
        }
        else if (actionType == ActionType.DEF)
        {
            if (other == null)
            {
                DefSuccessAnim();
                return;
            }
            switch (other.actionType)
            {
                case ActionType.ATK:
                    if (other.atk > def)
                        DefFailAnim();
                    else
                        DefSuccessAnim();
                    break;
                case ActionType.DEF:
                    DefFailAnim();
                    break;
                case ActionType.CounterATK:
                    DefCounterAnim(other.atk > def);
                    break;
                default:
                    break;
            }
        }
        else if (actionType == ActionType.CounterATK)
        {
            if (other == null)
            {
                CounterAnim(true, other);
                return;
            }
            switch (other.actionType)
            {
                case ActionType.ATK:
                    CounterAnim(true, other);
                    break;
                case ActionType.DEF:
                    if (other.def < atk)
                        CounterAnim(true, other);
                    else
                        CounterAnim(false);
                    break;
                case ActionType.CounterATK:
                    CounterAnim(true, other);
                    break;
                default:
                    break;
            }
        }

        void AtkSuccessAnim(int damage,bool isNull=false)
        {
            if(isNull)
            {
                transform.DOLocalMoveY(transform.localPosition.y + 100, 1).OnComplete(() =>
                {
                    BattleManager.I.Attack(other, damage);
                });
            }
            else
            {
                transform.DOLocalMoveY(transform.localPosition.y + 50, 1).OnComplete(() =>
                {
                    transform.DOLocalMoveY(transform.localPosition.y + 50, 1).OnComplete(() =>
                        BattleManager.I.Attack(other, damage)
                    );
                });
            }
        }

        void AtkFailAnim()
        {
            transform.DOLocalMoveY(transform.localPosition.y + 50, 1).OnComplete(() =>
                GetComponent<Image>().DOFade(0, 0.5f)
            );
        }

        void DefSuccessAnim()
        {
            transform.DOLocalMoveY(transform.localPosition.y - 20, 1);
        }

        void DefFailAnim()
        {
            transform.DOLocalMoveY(transform.localPosition.y - 20, 1).OnComplete(() =>
                GetComponent<Image>().DOFade(0, 0.5f)
            );
        }

        void DefCounterAnim(bool isSuccess)
        {
            transform.DOLocalMoveY(transform.localPosition.y, 1).OnComplete(() =>
             {
                 if (isSuccess)
                     DefSuccessAnim();
                 else
                     DefFailAnim();
             });
        }

        void CounterAnim(bool isSuccess, Actionem otherAct=null)
        {
            transform.DOLocalMoveY(transform.localPosition.y - 20, 1).SetEase(Ease.OutSine).OnComplete(() =>
            {
                transform.DOLocalMoveY(transform.localPosition.y + 70, 1).OnComplete(() =>
                {
                    if (isSuccess)
                        BattleManager.I.Attack(otherAct, atk - otherAct.def);
                    else
                        GetComponent<Image>().DOFade(0, 0.5f);
                });
            });
        }
    }
}
