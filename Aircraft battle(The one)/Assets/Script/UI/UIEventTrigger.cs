using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : 
    MonoBehaviour, 
    IPointerEnterHandler,
    IPointerDownHandler,
    ISelectHandler,
    ISubmitHandler
{
    [SerializeField] AudioData selectSFX;
    [SerializeField] AudioData submitSFX;
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.playSFX(selectSFX);
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.playSFX(submitSFX);
    }
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.playSFX(selectSFX);
    }
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.playSFX(submitSFX);
    }

}
