using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] UnityEngine.UI.Image transitionImage;
    [SerializeField] float fadeTime = 3f;
    Color fadeColor;

    IEnumerator LoadCoroutine(string sceneName)
    {
        var loadScene=SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = false;
        transitionImage.gameObject.SetActive(true);
        //µ≠»Î
        while (fadeColor.a < 1f)
        {
            fadeColor.a=Mathf.Clamp01(fadeColor.a+ Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = fadeColor;
            yield return null;
        }
        yield return new WaitUntil(()=>loadScene.progress>=0.9f);  

        loadScene.allowSceneActivation = true;
        //µ≠≥ˆ
        while (fadeColor.a > 0f)
        {
            fadeColor.a = Mathf.Clamp01(fadeColor.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = fadeColor;
            yield return null;
        }
    }
    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine("GamePlay"));
    }

    public void LoadMainScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine("MainMenu"));
    }
    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine("Scoring"));
    }

}
