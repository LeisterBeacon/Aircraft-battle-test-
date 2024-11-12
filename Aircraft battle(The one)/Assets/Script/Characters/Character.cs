using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("-----Health-----")]
    [SerializeField]protected float maxHealth;
    protected float health;

    [SerializeField]GameObject deathVFX;
    [SerializeField] StatsBar onHeadHealthBar;
    [SerializeField] bool showOnHeadHealthBar = true;
    [SerializeField] AudioData[] deathSFX;


    protected virtual void OnEnable()
    {
        health=maxHealth;
        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }
    public virtual void TakeDamage(float damage)
    {
        if (health == 0f) return;
        health-=damage;
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health,maxHealth);
        }
        if(health <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX,transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if(health==maxHealth) return;     
        health =Mathf.Clamp(health + value,0,maxHealth);
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth);
        }
    }

    //生命值再生协程
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float percent)
    {
        while(health<maxHealth)
        {
            yield return waitTime;
            RestoreHealth(maxHealth*percent);
        }
    }
    //持续受伤
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health >0)
        {
            yield return waitTime;
            TakeDamage(maxHealth * percent);
        }
    }

    #region UI_HealthBar
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Init(health,maxHealth);

    }
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }
    #endregion

}
