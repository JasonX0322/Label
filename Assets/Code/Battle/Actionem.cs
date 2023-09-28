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
    int dir;

    [SerializeField] ActionContainer container;

    public enum ActionType
    {
        ATK,
        DEF
        //CounterATK
    }
    public ActionType actionType;
    public int def;
    public int atk;

    public string cardName;

    Image img;

    Transform parentBattle;
    Transform parentDefault;

    // Start is called before the first frame update
    void Start()
    {
        dir = isEnemy ? -1 : 1;
        img = GetComponent<Image>();
        parentBattle = BattleManager.I.parentBattle;
        parentDefault = transform.parent;
    }

    public void InitAction(string groupName)
    {
        Sprite sp = Resources.Load<Sprite>("actionem/" + groupName + "/" + cardName);
        img.sprite = sp;
    }

    public void Unlock()
    {
        bLock = false;
        Debug.Log("Unlock");
    }

    public void Lock()
    {
        bLock = true;
        Debug.Log("Lock");
    }

    public void Update()
    {
        if(bDragging)
        {
            //Debug.Log(Input.mousePosition);
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDis;
        }
    }


    public void FillAction(Vector3 pos, int handIndex)
    {
        Vector3 newPos = pos;
        newPos.y += -2 * dir;
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
        if (isEnemy)
            return;
        if (bLock)
            return;
        transform.SetAsLastSibling();
        mouseDis = Input.mousePosition;
        mouseDis=Camera.main.ScreenToWorldPoint(mouseDis);
        mouseDis = mouseDis - transform.position;
        bDragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEnemy)
            return;
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
        if (isEnemy)
            return;
        if (bDragging)
            return;
        if (!bSelected)
        {
            Vector3 newpos = defaultPos;
            newpos.y += 0.5f;
            transform.DOMove(newpos, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnemy)
            return;
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
                default:
                    break;
            }
        }
    }





    void AtkSuccessAnim(int damage, bool isNull = false)
    {
        BattleManager.I.Attack(this, damage);
        Sequence sqcAtkSuccess;
        transform.SetParent(parentBattle);
        sqcAtkSuccess = DOTween.Sequence();
        sqcAtkSuccess.Append(transform.DOLocalMoveY(transform.localPosition.y + 400 * dir, 0.1f).SetEase(Ease.InSine));
        sqcAtkSuccess.AppendInterval(1);
        sqcAtkSuccess.Append(img.DOFade(0, 0.5f).OnComplete(()=>transform.SetParent(parentDefault)));
    }

    void AtkFailAnim()
    {
        Sequence sqcAtkFail;
        sqcAtkFail = DOTween.Sequence();
        sqcAtkFail.Append(transform.DOLocalMoveY(transform.localPosition.y + 50 * dir, 0.1f));
        sqcAtkFail.Append(transform.DOLocalMoveY(transform.localPosition.y - 50 * dir, 1).SetEase(Ease.OutSine));
        sqcAtkFail.Join(img.DOFade(0, 1));
    }

    void DefSuccessAnim()
    {
        Sequence sqcDefSuccess;
        sqcDefSuccess = DOTween.Sequence();
        sqcDefSuccess.Append(transform.DOLocalMoveY(transform.localPosition.y - 20 * dir, 0.1f));
        sqcDefSuccess.AppendInterval(1);
        sqcDefSuccess.Append(img.DOFade(0, 0.5f));
    }

    void DefFailAnim()
    {
        Sequence sqcDefFail;
        sqcDefFail = DOTween.Sequence();
        sqcDefFail.Append(transform.DOLocalMoveY(transform.localPosition.y - 20 * dir, 0.1f));
        sqcDefFail.Join(img.DOFade(0, 0.5f));
    }

    public delegate void TurnOverEvent();
    public void TurnOver(TurnOverEvent turnOverEvent)
    {
        transform.DOLocalRotate(Vector3.zero, 0.5f).OnComplete(()=>
        {
            turnOverEvent();
        });
    }

}
