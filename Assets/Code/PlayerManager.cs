using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character
{
    public static PlayerManager I;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetActPoint()
    {
        return ActPoint_Max;
    }
}
