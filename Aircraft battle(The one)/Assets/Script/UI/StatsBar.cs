using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    //延迟开关
    [SerializeField] bool delayFill=true;
    //填充延迟时间
    [SerializeField] float fillDelay = 0.5f;
    //缓冲填充速度
    [SerializeField] float fillSpeed=0.1f;

    float currentFillAmount;
    protected float targetFillAmount;
    float previousFillAmount;
    //线性插值
    float t;

    WaitForSeconds WaitForDelayFill;

    Coroutine bufferCoroutine;

    Canvas canvas;
    private void Awake()
    {
        
        if(TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
            
        }
        WaitForDelayFill = new WaitForSeconds(fillDelay);

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public virtual void Init(float currentValue,float maxValue)
    {
        currentFillAmount= currentValue/maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;
    }

    //更新状态条
    public void UpdateState(float currentValue, float maxValue)
    {
        targetFillAmount= currentValue/maxValue;    

        //if(bufferCoroutine != null)
        //{
        //    StopCoroutine(bufferCoroutine);
        //}

        //状态值减少
        if(currentFillAmount>targetFillAmount)
        {
            fillImageFront.fillAmount= targetFillAmount;
            bufferCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        //状态值增加
        else if(currentFillAmount<targetFillAmount)
        {
            fillImageBack.fillAmount= targetFillAmount;
            bufferCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }
    //缓冲更新
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if(delayFill)
        {
            yield return WaitForDelayFill;
        }
        previousFillAmount = currentFillAmount;
        t = 0;
        while(t < 1f)
        {
            t += Time.deltaTime * fillSpeed;

            currentFillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;

            yield return null;
        }
        
        
    }

}
