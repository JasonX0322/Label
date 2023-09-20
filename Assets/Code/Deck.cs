using DG.Tweening;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Deck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] Image imgCover;
    [SerializeField] GameObject goBorder;
    [SerializeField] Sprite spBattleFieldBG;

    [SerializeField] Deck[] allDecks;
    [SerializeField] AudioClip myBGM;

    Material mat;

    public bool interactable = true;

    public bool tempClose;

    //[SerializeField] int nFieldWidth;
    //[SerializeField] int nFieldDepth;
    [SerializeField] string strBattleName;

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

        foreach (var item in allDecks)
        {
            item.interactable = false;
        }

        imgCover.gameObject.transform.DOScale(1.5f, 1);
        imgCover.DOFade(0, 1);

        Debug.LogWarning("选择战场");

        MainManager.I.targetScene = "battle";

        MainManager.BattleInfo info = new MainManager.BattleInfo();
        info.SetBattle(strBattleName);
        MainManager.I.battleInfo = info;

        BlackProcess.I.BlackScene(() =>
        {
            SceneManager.LoadScene("loading");
        });

        //imgCover.DOFade(0, 2).OnComplete(() => License());

        //BackImg.I.OpenBG(spBattleFieldBG);

        //for (int i = 0; i < allDecks.Length; i++)
        //{
        //    if (allDecks[i] != this)
        //    {
        //        allDecks[i].HideDeck();
        //    }
        //}

        //interactable = false;
        //UnHighLightCards();
        //AudioManager.I.SwitchBGM(myBGM);
    }

    /// <summary>
    /// 发牌
    /// </summary>
    //void License()
    //{
    //    Debug.Log("License");
    //    //StartCoroutine(ienuLicense());
    //    //BattleFieldManager.I.SetBattleField(nFieldWidth, nFieldDepth, strBattleName);
    //    BattleFieldManager.I.StartBattleField();
    //}
}
