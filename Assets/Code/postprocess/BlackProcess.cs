using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using DG.Tweening;

public class BlackProcess : PostEffectsBase
{
    public Shader briSatConShader;
    private Material briSatConMaterial;
    public static BlackProcess I;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        WhiteScene();
    }
    public Material material
    {
        get
        {
            briSatConMaterial = CheckShaderAndCreateMaterial(briSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }

    //[Range(0.0f, 1.0f)]
    //public float black = 0.0f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            //material.SetFloat("_black", black);
            //使用这个函数进行将source传入material的_MainTex（必须保证有这个字段）纹理进行用material的着色器渲染
            //注意：它会执行所有Pass，进行渲染，相当于第四个参数为-1
            Graphics.Blit(src, dest, material);//第三个参数是material材质，还用它身上的着色器shader进行渲染
        }
        else
        {
            //否则，只是将原色source图像输出到屏幕,实际上是啥都没干。因为没有着色器传入（第三个参数material身上的着色器)
            Graphics.Blit(src, dest);
        }
    }

    public void BlackScene(GotoScene sceneEvent)
    {
        material.DOFloat(0, "_black", 2).OnComplete(() =>
        {
            sceneEvent();
        });
    }
    public void WhiteScene()
    {
        material.SetFloat("_black", 0);
        material.DOFloat(1, "_black", 2);
    }
}
