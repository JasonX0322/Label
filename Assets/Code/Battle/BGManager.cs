using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGManager : MonoBehaviour
{
    public static BGManager I;
    Image img;

    void Awake()
    {
        Debug.Log("awake");
        I = this;
        img=GetComponent<Image>();
        string battleName=MainManager.I.battleInfo.battleName;
        Sprite sp = Resources.Load<Sprite>(battleName + "/bg");
        Debug.Log(battleName + "/bg");
        Debug.Log(sp);
        img.sprite = sp;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlack(bool toBlack)
    {
        if(toBlack)
        {
            img.color = new Color(0.3f, 0.3f, 0.3f, 1);
        }
        else
        {
            img.color = Color.white;
        }
    }
}
