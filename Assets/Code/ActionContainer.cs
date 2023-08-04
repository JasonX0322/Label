using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContainer : MonoBehaviour
{
    public delegate void LicenseFinish();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FillContainer(LicenseFinish licenseFinish)
    {
        licenseFinish();
    }

    public void FillContainer()
    {

    }

    public void LockActions()
    {

    }

    public void UnlockActions()
    {

    }
}
