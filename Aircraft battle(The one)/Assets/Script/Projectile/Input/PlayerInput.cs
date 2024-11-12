using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IUnPauseActions, InputActions.IGameOverScreenActions
{
    InputActions inputActions;
    public event UnityAction<Vector2> onMove=delegate { };
    public event UnityAction onStopMove = delegate { };
    public event UnityAction onFire=delegate { };
    public event UnityAction onStopFire = delegate { };
    public event UnityAction onDodge = delegate { };
    public event UnityAction onOverdrive = delegate { };
    public event UnityAction onPause = delegate { };
    public event UnityAction onUnPause = delegate { };
    public event UnityAction onLaunchMissle = delegate { };
    public event UnityAction onConfirmGameOver=delegate { };

    //ʵ���ƶ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        //�ɳ�������
        if (context.performed)
        {
           onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }
    //ʵ�ֿ�����
    public void OnFire(InputAction.CallbackContext context)
    {
        //�ɳ�������
        if (context.performed)
        {
            onFire.Invoke();
        }
        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }
    //ʵ�����ܹ���
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    //��������
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();   
        }
    }
    //���䵼��
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissle.Invoke();
        }
    }
    //��ͣ����
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }
    //ȡ����ͣҳ��
    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }
    //����ҳ��
    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }
    private void OnEnable()
    {
        inputActions = new InputActions();
        //�Ǽ�gameplay������Ļص�����
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.UnPause.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableAllInputs();
    }

    //���ö�����
    public void DisableAllInputs()
    {
        inputActions.Disable();
        
    }
    //�����������ö�����
    public void EnableGameplayInput()=>SwitchActionMap(inputActions.GamePlay,false);
    public void EnableUnPauseInput() => SwitchActionMap(inputActions.UnPause, true);

    public void EnableGameOverScreenInput()=>SwitchActionMap(inputActions.GameOverScreen,false);
        //�л�����ģʽ��ֹ��Ϸ����
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    //�л�������
    void SwitchActionMap(InputActionMap actionMap,bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible=false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    

    
}
