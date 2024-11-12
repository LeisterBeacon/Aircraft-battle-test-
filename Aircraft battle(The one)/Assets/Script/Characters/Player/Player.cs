using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : Character
{
    //是否进行生命值恢复
    [SerializeField]bool regenerateHealth=true;
    //生命值恢复时间
    [SerializeField]float healthRegenerateTime;
    //生命值恢复百分比
    [SerializeField,Range(0f,1f)]float healthRegeneratePercent;

    [SerializeField] StatsBar_HUD statsBar_HUD;
    //生命恢复间隔时间
    WaitForSeconds waitHealthRegenerateTime;
    Coroutine healthRegenerateCoroutine;

    public bool IsFullHealth=>health==maxHealth;
    public bool IsFullPower => weaponPower == 2;

    [Header("-----Component-----")]
    [SerializeField]PlayerInput input;
    Rigidbody2D rb;
    Collider2D collider2d;
    PlayerMissileSystem missileSystem;
    [Header("-----Move-----")]
    //加速时间
    [SerializeField] float acclerationTime = 3f;
    //减速时间|
    [SerializeField] float declerationTime = 3f;
    //旋转角度
    [SerializeField] float moveRotationAngle = 50f;
    //速度
    [SerializeField] float moveSpeed = 10f;
    //移动协程
    Coroutine moveCoroutine;

    float t;
   
    


    [Header("-----View-----")]
    //体型差值
    [SerializeField] float paddingX=0.2f;
    [SerializeField] float paddingY=0.2f;

    [Header("-----Fire-----")]
    //子弹预制件
    [SerializeField] GameObject projectil1;
    [SerializeField] GameObject projectil2;
    [SerializeField] GameObject projectil3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField,Range(0,2)] int weaponPower=0;
    //子弹发射音效
    [SerializeField] AudioData projectileSFX;
    [SerializeField] ParticleSystem muzzleVFX;

    //枪口
    [SerializeField] Transform muzzlTop;
    [SerializeField] Transform muzzlMiddle;
    [SerializeField] Transform muzzlBottom;
    //开火间隔
    [SerializeField] float fireInterval=0.2f;
    WaitForSeconds waitForFireInterval;
    
    [Header("----Dodge----")]
    [SerializeField] AudioData DodgeSFX;
    //闪避能量消耗值
    [SerializeField,Range(0,100)] int dodgeEnergy = 25;
    //最大滚转角
    [SerializeField] float MaxRoll = 720f;
    float currentRoll;
    [SerializeField] float rollSpeed=360f;
    bool IsDodging;
    //缩放值
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    //闪避动作持续时间
    float DodgeDuration;

    [Header("----Overdrive----")]
    bool isOverdriving=false;
    //能力增强与削弱
    [SerializeField] int overdriveDodgeFactor = 2;
    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;
    WaitForSeconds waitForOverdriveInterval;
    //子弹时间
    [SerializeField]float slowMotionDuration = 2f;
    [SerializeField] float getDamageDuration = 0.3f;

  
    readonly float InvincibleTime = 1f;
    WaitForSeconds waitInvincibleTime;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        missileSystem = GetComponent<PlayerMissileSystem>();
        DodgeDuration=MaxRoll/rollSpeed;
        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForOverdriveInterval = new WaitForSeconds(fireInterval /= overdriveFireFactor);
        waitInvincibleTime=new WaitForSeconds(InvincibleTime);
    }

    void Start()
    {
        rb.gravityScale = 0;
        //激活Gameplayer动作表
        input.EnableGameplayInput();
        
        statsBar_HUD.Init(health,maxHealth);
        

    }
    #region PlayerInvincible
    IEnumerator PlayerInvincible()
    {
        collider2d.isTrigger = true;    
        yield return waitInvincibleTime ;
        collider2d.isTrigger = false;
    }
    #endregion

    #region Input
    protected override void OnEnable()
    {
        base.OnEnable();
        //进行移动
        input.onMove += Move;
        input.onStopMove += StopMove;

        //进行开火
        input.onFire += Fire;
        input.onStopFire += StopFire;

        //进行翻滚
        input.onDodge += Dodge;
        //发射导弹
        input.onLaunchMissle += LaunchMissile;

        //能量爆发
        input.onOverdrive += onOverdrive;
        //能量爆发开关
        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        
        input.onFire -= Fire;
        input.onStopFire -= StopFire;

        input.onDodge -= Dodge;

        input.onLaunchMissle -= LaunchMissile;

        input.onOverdrive -= onOverdrive;
        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;
    }
    #endregion 

    #region Move
    //玩家移动功能
    void Move(Vector2 moveInput) 
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        //moveInput * moveSpeed是移动量         
        moveCoroutine=StartCoroutine(MoveCoroutine(acclerationTime,moveInput.normalized * moveSpeed, moveRotation));
        //启动位移限制
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }
   void StopMove()
   {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine =StartCoroutine(MoveCoroutine(declerationTime, Vector2.zero, Quaternion.identity));
        //关闭位移限制
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
   //位移限制协程   
   IEnumerator MovePositionLimitCoroutine()
   {
        while (true)
        {
            //限制移动
            transform.position = Viewport.Instance.PlayerMoveablePoint(transform.position,paddingX,paddingY);
            yield return null;
        }
   }
    //开始和停止移动协程
   IEnumerator MoveCoroutine(float time,Vector2 moveVelocity,Quaternion moveRotation)
   {
       t = 0f;
        
       while(t < 1f)
       {
           t+= Time.fixedDeltaTime/time;
           rb.velocity=Vector2.Lerp(rb.velocity, moveVelocity, t );
            transform.rotation=Quaternion.Lerp(transform.rotation, moveRotation,t);
           yield return null;
       }
   }
    #endregion

    #region Fire
    //开火功能
    void Fire()
    {
        muzzleVFX.Play();
       StartCoroutine(nameof(FireCoroutine));
        
    }
    void StopFire()
    {
        muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        
        while(true)
        {
           
            switch (weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving?projectileOverdrive:projectil1, muzzlMiddle.position);   
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectil1, muzzlTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectil2, muzzlBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectil2, muzzlTop.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectil1, muzzlMiddle.position);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectil3, muzzlBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlayRandomSFX(projectileSFX);
            
            yield return isOverdriving?waitForOverdriveInterval:waitForFireInterval;
        }
    }
    
    #endregion

    #region Health
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (health < maxHealth / 2) WeaponPowerDown();
        statsBar_HUD.UpdateState(health, maxHealth);
        if(gameObject.activeSelf)
        {
            TimeController.Instance.BulletTime(getDamageDuration);
            StartCoroutine(PlayerInvincible());
            if(regenerateHealth)
            {
                if (healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime,healthRegeneratePercent));
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsBar_HUD.UpdateState(health, maxHealth);

    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsBar_HUD.UpdateState(0f,maxHealth);
        base.Die(); 
    }

    #endregion

    #region Dodge
    void Dodge()
    {
        if (IsDodging ||! PlayerEnergy.Instance.IsEnough(dodgeEnergy)) return;
        StartCoroutine(nameof(DogeCoroutine));
    }
    IEnumerator DogeCoroutine()
    {

        IsDodging = true;
        AudioManager.Instance.PlayRandomSFX(DodgeSFX);
        //闪避耗能
        PlayerEnergy.Instance.Use(dodgeEnergy);
        //玩家无敌
        collider2d.isTrigger = true;

        currentRoll = 0f;

           
        //玩家翻滚
        while (currentRoll < MaxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation=Quaternion.AngleAxis(currentRoll, Vector3.right);
            //闪避缩放
            transform.localScale=BezierCurve.QuadraticPoint(Vector3.one,Vector3.one,dodgeScale,currentRoll/MaxRoll); 
            
            yield return null;
        }
        collider2d.isTrigger = false;
        
        IsDodging=false;

    }
    #endregion

    #region Overdrive
    void onOverdrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;
        PlayerOverdrive.on.Invoke();
    }
    void OverdriveOn()
    {
        isOverdriving = true;
        //能力增强
        dodgeEnergy*=overdriveDodgeFactor;
        moveSpeed*=overdriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration);

    }

    void OverdriveOff()
    {
        isOverdriving=false;
        dodgeEnergy /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }
    #endregion

    #region Missile
    void LaunchMissile()
    {
        missileSystem.Launch(muzzlMiddle);
    }
    public void PickUpMissile()
    {
        missileSystem.PickUp();
    }
    #endregion

    #region WeaponPowerUp
    public void WeaponPowerUp()
    {
        weaponPower = Mathf.Min(weaponPower + 1, 2);
    }
    public void WeaponPowerDown()
    {
        weaponPower = Mathf.Max(weaponPower - 1, 0);
    }
    #endregion
}
