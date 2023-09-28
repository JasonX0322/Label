using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int HP_Max;
    public int HP_Remain;
    public int ActPoint_Max;
    public int ActPoint_Remain;

    public HPBar myHPBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SubHealth(int amount)
    {
        HP_Remain-= amount;
        myHPBar.UpdateHealth(HP_Max,HP_Remain);
    }
}
