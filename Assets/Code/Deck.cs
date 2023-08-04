using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] GameObject[] goTotalDeckCard;
    GameObject[] goDeckCard;
    Transform[] tDeckCard;
    Image[] imgDeckCard;
    [SerializeField] Image imgCover;
    [SerializeField] GameObject[] goBorder;
    [SerializeField] Texture texFull;
    [SerializeField] Texture texCover;

    [SerializeField] Material matUnselected;
    [SerializeField] Material matDefeated;


    [SerializeField] Deck[] allDecks;

    Material mat;

    bool interactable = true;

    public bool tempClose;

    [SerializeField] Object objCard;

    [SerializeField] int nFieldWidth;
    [SerializeField] int nFieldDepth;
    [SerializeField] string strFieldName;
    void Awake()
    {
        goDeckCard=new GameObject[nFieldDepth + 1];
        for(int i= 0; i < goTotalDeckCard.Length; i++)
        {
            if (i < nFieldDepth)
            {
                goTotalDeckCard[i].SetActive(true);
                goDeckCard[i] = goTotalDeckCard[i];
            }
            else
            {
                goTotalDeckCard[i].SetActive(false);
            }
        }
        goDeckCard[goDeckCard.Length - 1] = imgCover.gameObject;

        tDeckCard = new Transform[goDeckCard.Length];
        imgDeckCard = new Image[goDeckCard.Length];
        for (int i = 0; i < goDeckCard.Length; i++)
        {
            tDeckCard[i] = goDeckCard[i].transform;
        }
        for (int i = 0; i < goDeckCard.Length-1; i++)//最后一张是cover
        {
            imgDeckCard[i] = goDeckCard[i].GetComponentInChildren<Image>();
        }
        imgDeckCard[imgDeckCard.Length - 1] = imgCover;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tDeckCard.Length; i++)
        {
            tDeckCard[i].localPosition = new Vector3(-20,-20,0) + new Vector3(4, 4, 0) * i;
        }
        mat=new Material(matUnselected);
        imgCover.material = mat;
        mat.SetTexture("_NowTex", texCover);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable||tempClose)
            return;
        Debug.Log("enter");
        HightLightCards();
    }
    /// <summary>
    /// 高亮
    /// </summary>
    void HightLightCards()
    {
        for (int i = 0; i < tDeckCard.Length; i++)
        {
            tDeckCard[i].DOLocalMoveX(-20 + 10 * i + 4, 0.5f);
            goBorder[i].transform.DOLocalMoveX(-20 + 10 * i + 4, 0.5f);
        }
        foreach (var item in goBorder)
        {
            item.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable|| tempClose)
            return;
        Debug.Log("exit");
        UnHighLightCards();
    }
    /// <summary>
    /// 取消高亮
    /// </summary>
    void UnHighLightCards()
    {
        for (int i = 0; i < tDeckCard.Length; i++)
        {
            tDeckCard[i].DOLocalMoveX(-10 + 4 * i, 0.5f);
            goBorder[i].transform.DOLocalMoveX(-10 + 4 * i, 0.5f);
        }
        foreach (var item in goBorder)
        {
            item.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable|| tempClose)
            return;
        mat.DOFloat(0, "_OpacityNow", 2).OnComplete(()=> License());

        BackImg.I.SwitchBG(texFull);
        for (int i = 0; i < allDecks.Length; i++)
        {
            if (allDecks[i] != this)
            {
                allDecks[i].HideDeck();
            }
        }

        interactable = false;
        UnHighLightCards();
    }

    /// <summary>
    /// 隐藏卡组
    /// </summary>
    public void HideDeck()
    {
        interactable = false;
        foreach (var item in imgDeckCard)
        {
            item.DOFade(0, 0.5f).OnComplete(() => item.enabled = false) ;
        }
    }
    /// <summary>
    /// 显示卡组
    /// </summary>
    public void ShowDeck()
    {
        interactable = true;
        foreach (var item in imgDeckCard)
        {
            item.DOFade(1, 0.5f);
        }
    }

    /// <summary>
    /// 发牌
    /// </summary>
    void License()
    {
        Debug.Log("License");
        StartCoroutine(ienuLicense());
    }

    IEnumerator ienuLicense()
    {
        BattleFieldManager.I.SetBattleField(nFieldWidth, nFieldDepth, strFieldName);
        for (int i = 0; i < nFieldDepth; i++)
        {
            goDeckCard[i].SetActive(false);
            for (int j = 0; j < nFieldWidth; j++)
            {
                GameObject goCard = GameObject.Instantiate(objCard) as GameObject;
                goCard.transform.SetParent(transform);
                goCard.transform.localScale = Vector3.one;
                goCard.transform.SetAsFirstSibling();
                goCard.transform.position = goDeckCard[i].transform.position;
                BattleFieldManager.I.AddCard(goCard);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

}
