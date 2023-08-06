using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    AudioSource bgm;

    void Awake()
    {
        I = this;
        bgm = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SwitchBGM(AudioClip clip)
    {
        bgm.DOFade(0, 0.5f).OnComplete(()=>
        {
            bgm.clip = clip;
            bgm.DOFade(1, 0.5f);
        });
    }
}
