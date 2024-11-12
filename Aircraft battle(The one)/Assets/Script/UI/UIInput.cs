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
        //UIobject UI�ɱ�ѡ�еĻ���

        //ѡ��UI
        UIObject.Select();
        //��UI����Ϊ��ȷ״̬
        UIObject.OnSelect(null);

        UIInputModule.enabled=true;
        
        
    }
    //��ֹ�����Ӧ
    public void DisableAllUIInputs()
    {
        playerInput.DisableAllInputs();
        UIInputModule.enabled=false;   
    }
}
