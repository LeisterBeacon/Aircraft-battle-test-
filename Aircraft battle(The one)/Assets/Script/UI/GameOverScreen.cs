using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] Canvas HUD;
    Animator animator;
    Canvas canvas;
    [SerializeField] AudioData audioData;
    int exitID = Animator.StringToHash("GameOverScreenExit");
    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        animator.enabled = false; 
    }
    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        input.onConfirmGameOver += OnConfirmGameOver;
    }
    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConfirmGameOver;
    }
    void OnGameOver()
    {
        AudioManager.Instance.BGM_Player.Pause();
        AudioManager.Instance.AmBient_Player.Pause();
        HUD.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisableAllInputs();
    }

    public void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
    void OnConfirmGameOver()
    {
        AudioManager.Instance.playSFX(audioData);
        input.DisableAllInputs() ;
        animator.Play(exitID);
        SceneLoader.Instance.LoadScoringScene();

    }

}
