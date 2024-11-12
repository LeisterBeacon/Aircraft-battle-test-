using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField,Range(0f,1f)] float bulleTimeScale = 0.1f;
    float defualtFixedDeltaTime;

    float timeScaleBeforePause;


    protected override void Awake()
    {
        base.Awake();
        defualtFixedDeltaTime = Time.fixedDeltaTime;
    }
    public void Pause()
    {
        timeScaleBeforePause= Time.timeScale;   
        Time.timeScale = 0f;
        
    }
    public void UnPause()
    {
        Time.timeScale = timeScaleBeforePause;
        
    }
    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void BulletTime(float duration)
    {
        
        Time.timeScale = bulleTimeScale;
        Time.fixedDeltaTime = defualtFixedDeltaTime *Time.timeScale;
        StartCoroutine(slowOutCoroutine(duration));
    }

    IEnumerator slowOutCoroutine(float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            if(GameManager.GameState!= GameState.Paused)
            {
                t += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(bulleTimeScale, 1f, t);
                Time.fixedDeltaTime = defualtFixedDeltaTime * Time.timeScale;
            }
           
            yield return null;
        }
     
    } 
}            
             