using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void GotoScene();

public class MainManager : MonoBehaviour
{
    
    public static MainManager I;

    public string targetScene;

    public struct BattleInfo
    {
        public string battleName;
        public void SetBattle(string name)
        {
            battleName = name;
        }
    }
    public BattleInfo battleInfo;

    public bool testMode;

    void Awake()
    {
        I = this;
        DontDestroyOnLoad(gameObject);

        if(testMode)
        {
            battleInfo=new BattleInfo();
            battleInfo.SetBattle("tavern");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(battleInfo.battleName);
    }


}
