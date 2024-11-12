using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas HUD_Canvas;
    [SerializeField] Canvas menusCanvas;

    [Header("-----INPUT EVENT-----")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButton;

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnPause += UnPause;

        //resumeButton.onClick.AddListener(OnResumeButtonClick);   
        //optionButton.onClick.AddListener(OnOptionButtonClick);
        //mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);

        //采用以下方法，让按钮动画播放完后再实现功能，
        ButtonPressBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressBehavior.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClick);
        ButtonPressBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }
    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnPause -= UnPause;

        ButtonPressBehavior.buttonFunctionTable.Clear();
    }
    void Pause()
    {
        GameManager.GameState=GameState.Paused;
        TimeController.Instance.Pause();
        HUD_Canvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnableUnPauseInput();
        playerInput.SwitchToDynamicUpdateMode();
        AudioManager.Instance.BGM_Player.Pause();
        AudioManager.Instance.AmBient_Player.Pause();

        //自动选中UI按钮
        //UIInput.Instance.SelectUI(resumeButton);
    }
    void UnPause()
    {

        resumeButton.Select();
        resumeButton.animator.SetTrigger("Pressed");
        OnResumeButtonClick();
    }

    void OnResumeButtonClick()
    {
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.UnPause();
        HUD_Canvas.enabled = true;
        menusCanvas.enabled = false;
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
        AudioManager.Instance.BGM_Player.UnPause();
        AudioManager.Instance.AmBient_Player.UnPause();
    }

    void OnOptionButtonClick()
    {
        UIInput.Instance.SelectUI(optionButton);
        playerInput.EnableUnPauseInput();
    }
    
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        //切换到主菜单
        SceneLoader.Instance.LoadMainScene();

    }

}
