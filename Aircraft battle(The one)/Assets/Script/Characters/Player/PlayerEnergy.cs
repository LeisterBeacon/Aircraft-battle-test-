using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField]EnergyBar energyBar;
    public const int MAX = 100;
    public const int PERCENT = 1;
    //当前能量
    int energy=100;

    bool availble = true;

    //能量爆发耗精间隔
    [SerializeField] float overdriveInterval = 0.1f;
    WaitForSeconds waitForoverdriveInterval;
    protected override void Awake()
    {
        base.Awake();
        waitForoverdriveInterval=new WaitForSeconds(overdriveInterval);
    }
    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }
    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    private void Start()
    {
        energyBar.Init(energy,MAX);
       
        
    }

    //获取能量
    public void Obtain(int value)
    {
        if(energy == MAX||!availble||!gameObject.activeSelf)
        {
            return;
        }
        
        energy=Mathf.Clamp(energy+value, 0, MAX);

        energyBar.UpdateState(energy,MAX);

    }

    //使用能量
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy,MAX);

        if (energy == 0 && !availble)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    //判断能量是否满足
    public bool IsEnough(int value)=>energy>=value; 

    //能量爆发状态
    void PlayerOverdriveOn()
    {
        availble = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverdriveOff()
    {
        availble=true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForoverdriveInterval;
            Use(PERCENT);
        }
    }

}
