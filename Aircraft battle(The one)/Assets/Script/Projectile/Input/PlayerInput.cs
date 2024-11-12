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

    //实现移动功能
    public void OnMove(InputAction.CallbackContext context)
    {
        //可持续输入
        if (context.performed)
        {
           onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            onStopMove.Invoke();
        }
    }
    //实现开火功能
    public void OnFire(InputAction.CallbackContext context)
    {
        //可持续输入
        if (context.performed)
        {
            onFire.Invoke();
        }
        if (context.canceled)
        {
            onStopFire.Invoke();
        }
    }
    //实现闪避功能
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    //能量爆发
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();   
        }
    }
    //发射导弹
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissle.Invoke();
        }
    }
    //暂停界面
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }
    //取消暂停页面
    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnPause.Invoke();
        }
    }
    //结束页面
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
        //登记gameplay动作表的回调函数
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.UnPause.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }
    void OnDisable()
    {
        DisableAllInputs();
    }

    //禁用动作表
    public void DisableAllInputs()
    {
        inputActions.Disable();
        
    }
    //让其它类启用动作表
    public void EnableGameplayInput()=>SwitchActionMap(inputActions.GamePlay,false);
    public void EnableUnPauseInput() => SwitchActionMap(inputActions.UnPause, true);

    public void EnableGameOverScreenInput()=>SwitchActionMap(inputActions.GameOverScreen,false);
        //切换更新模式防止游戏卡死
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    //切换动作表
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
