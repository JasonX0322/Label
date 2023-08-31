using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        StartCoroutine(AsyncLoading());
        float fillAmount = 0;
        for(int i=0;i!=50;i++)
        {
            fillAmount += 0.01f;
            slider.value = fillAmount;
            yield return new WaitForSeconds(0.04f);
        }
        while (true)
        {
            float real = 0.5f + asyncOperation.progress * 0.5f;
            fillAmount = Mathf.Lerp(fillAmount, real, 0.5f);
            slider.value = fillAmount;
            if (fillAmount >= 0.9)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator AsyncLoading()
    {
        yield return new WaitForSeconds(1);
        asyncOperation = SceneManager.LoadSceneAsync(MainManager.I.targetScene);
        asyncOperation.allowSceneActivation = false;
        yield return asyncOperation;
    }
}
