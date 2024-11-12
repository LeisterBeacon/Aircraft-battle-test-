using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("=====CANVAS=====")]
    [SerializeField] Canvas mainMenuCanvas;
    [Header("=====BUTTON=====")]

    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;
    private void OnEnable()
    {
        ButtonPressBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name,onStartButtonClick);
        ButtonPressBehavior.buttonFunctionTable.Add(buttonOptions.gameObject.name, onOptionButtonClick);
        ButtonPressBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, onQuitButtonClick);
    }
    private void OnDisable()
    {
        ButtonPressBehavior.buttonFunctionTable.Clear();
    }
    //如果是暂停菜单回到主菜单，从主菜单回到游戏页面，游戏时间刻度为0
    private void Start()
    {
        Time.timeScale = 1.0f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    void onStartButtonClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();
    }
    void onOptionButtonClick()
    {
        UIInput.Instance.SelectUI(buttonOptions);

    }
    void onQuitButtonClick()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
