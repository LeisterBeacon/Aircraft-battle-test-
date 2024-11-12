using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    //�ӳٿ���
    [SerializeField] bool delayFill=true;
    //����ӳ�ʱ��
    [SerializeField] float fillDelay = 0.5f;
    //��������ٶ�
    [SerializeField] float fillSpeed=0.1f;

    float currentFillAmount;
    protected float targetFillAmount;
    float previousFillAmount;
    //���Բ�ֵ
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

    //����״̬��
    public void UpdateState(float currentValue, float maxValue)
    {
        targetFillAmount= currentValue/maxValue;    

        //if(bufferCoroutine != null)
        //{
        //    StopCoroutine(bufferCoroutine);
        //}

        //״ֵ̬����
        if(currentFillAmount>targetFillAmount)
        {
            fillImageFront.fillAmount= targetFillAmount;
            bufferCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
        //״ֵ̬����
        else if(currentFillAmount<targetFillAmount)
        {
            fillImageBack.fillAmount= targetFillAmount;
            bufferCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));
        }
    }
    //�������
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
