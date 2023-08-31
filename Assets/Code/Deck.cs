using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] Image imgCover;
    [SerializeField] GameObject goBorder;
    [SerializeField] Sprite spBattleFieldBG;

    [SerializeField] Deck[] allDecks;
    [SerializeField] AudioClip myBGM;

    Material mat;

    bool interactable = true;

    public bool tempClose;

    [SerializeField] int nFieldWidth;
    [SerializeField] int nFieldDepth;
    [SerializeField] string strFieldName;

    // Start is called before the first frame update
    void Start()
    {
        mat = imgCover.material;
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
        goBorder.SetActive(true);
        GetComponent<AudioSource>().Play();
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
        goBorder.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable|| tempClose)
            return;

        Debug.LogWarning("选择战场");

        imgCover.DOFade(0, 2).OnComplete(() => License());

        BackImg.I.OpenBG(spBattleFieldBG);

        for (int i = 0; i < allDecks.Length; i++)
        {
            if (allDecks[i] != this)
            {
                allDecks[i].HideDeck();
            }
        }

        interactable = false;
        UnHighLightCards();
        AudioManager.I.SwitchBGM(myBGM);
    }

    /// <summary>
    /// 隐藏卡组
    /// </summary>
    public void HideDeck()
    {
        interactable = false;
        imgCover.DOFade(0, 0.5f).OnComplete(() => imgCover.enabled = false);
    }
    /// <summary>
    /// 显示卡组
    /// </summary>
    public void ShowDeck()
    {
        interactable = true;
        imgCover.DOFade(1, 0.5f);
    }

    /// <summary>
    /// 发牌
    /// </summary>
    void License()
    {
        Debug.Log("License");
        //StartCoroutine(ienuLicense());
        BattleFieldManager.I.SetBattleField(nFieldWidth, nFieldDepth, strFieldName);
        BattleFieldManager.I.StartBattleField();
    }
}
