using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 3;
    int amount;
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;
    [SerializeField] float cooldownTime = 1f;//发射冷却时间
    bool isReady = true;   


    private void Awake()
    {
        amount=defaultAmount;
    }
    private void Start()
    {
        MissileDisplay.UpdateText(amount);
    }
    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0||!isReady) return;
        isReady = false;
        PoolManager.Release(missilePrefab,muzzleTransform.position);
        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;
        MissileDisplay.UpdateText(amount);
        if(amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            //进入发射冷却时间
            StartCoroutine(nameof(CooldownCorotine));
        }


    }

    IEnumerator CooldownCorotine()
    {
        var cooldownValue=cooldownTime;
        while (cooldownValue > 0f)
        {
            cooldownValue=Mathf.Max(cooldownValue-Time.deltaTime,0);
            MissileDisplay.UpdateCooldownImage(cooldownValue/cooldownTime);
            yield return null;
        }
        
        isReady = true;
    }

    //导弹数量增加
    public void PickUp()
    {
        amount++;
        MissileDisplay.UpdateText(amount);
        if(amount == 1)
        {
            MissileDisplay.UpdateCooldownImage(0f);
            isReady = true;
        }
    }
}
