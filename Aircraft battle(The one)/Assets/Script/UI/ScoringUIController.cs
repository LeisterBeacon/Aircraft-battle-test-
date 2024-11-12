using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI    ;

public class ScoringUIController : MonoBehaviour
{

    [SerializeField] Image background;
    [SerializeField] Sprite[] sp;

    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;
    
    private void Start()
    {
        showRandomBackgroud();
        ShowScoringScreen();
        ButtonPressBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.name, onButtonMainMenuClicked);
        GameManager.GameState=GameState.Scoring;

    }
    private void OnDisable()
    {
        ButtonPressBehavior.buttonFunctionTable.Clear();
    }
    void showRandomBackgroud()
    {
        background.sprite = sp[Random.Range(0,sp.Length)];
    }
    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;   
    }
    void onButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainScene();
    }

}
