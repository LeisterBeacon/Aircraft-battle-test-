using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    InputSystemUIInputModule UIInputModule;
    [SerializeField] PlayerInput playerInput;
    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        //UIobject UI可被选中的基类

        //选中UI
        UIObject.Select();
        //将UI设置为正确状态
        UIObject.OnSelect(null);

        UIInputModule.enabled=true;
        
        
    }
    //防止多次响应
    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled=false;   
    }
}
