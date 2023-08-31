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


    void Awake()
    {
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
