using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : Character
{
    //�Ƿ��������ֵ�ָ�
    [SerializeField]bool regenerateHealth=true;
    //����ֵ�ָ�ʱ��
    [SerializeField]float healthRegenerateTime;
    //����ֵ�ָ��ٷֱ�
    [SerializeField,Range(0f,1f)]float healthRegeneratePercent;

    [SerializeField] StatsBar_HUD statsBar_HUD;
    //�����ָ����ʱ��
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
    //����ʱ��
    [SerializeField] float acclerationTime = 3f;
    //����ʱ��|
    [SerializeField] float declerationTime = 3f;
    //��ת�Ƕ�
    [SerializeField] float moveRotationAngle = 50f;
    //�ٶ�
    [SerializeField] float moveSpeed = 10f;
    //�ƶ�Э��
    Coroutine moveCoroutine;

    float t;
   
    


    [Header("-----View-----")]
    //���Ͳ�ֵ
    [SerializeField] float paddingX=0.2f;
    [SerializeField] float paddingY=0.2f;

    [Header("-----Fire-----")]
    //�ӵ�Ԥ�Ƽ�
    [SerializeField] GameObject projectil1;
    [SerializeField] GameObject projectil2;
    [SerializeField] GameObject projectil3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField,Range(0,2)] int weaponPower=0;
    //�ӵ�������Ч
    [SerializeField] AudioData projectileSFX;
    [SerializeField] ParticleSystem muzzleVFX;

    //ǹ��
    [SerializeField] Transform muzzlTop;
    [SerializeField] Transform muzzlMiddle;
    [SerializeField] Transform muzzlBottom;
    //������
    [SerializeField] float fireInterval=0.2f;
    WaitForSeconds waitForFireInterval;
    
    [Header("----Dodge----")]
    [SerializeField] AudioData DodgeSFX;
    //������������ֵ
    [SerializeField,Range(0,100)] int dodgeEnergy = 25;
    //����ת��
    [SerializeField] float MaxRoll = 720f;
    float currentRoll;
    [SerializeField] float rollSpeed=360f;
    bool IsDodging;
    //����ֵ
    [SerializeField] Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    //���ܶ�������ʱ��
    float DodgeDuration;

    [Header("----Overdrive----")]
    bool isOverdriving=false;
    //������ǿ������
    [SerializeField] int overdriveDodgeFactor = 2;
    [SerializeField] float overdriveSpeedFactor = 1.2f;
    [SerializeField] float overdriveFireFactor = 1.2f;
    WaitForSeconds waitForOverdriveInterval;
    //�ӵ�ʱ��
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
        //����Gameplayer������
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
        //�����ƶ�
        input.onMove += Move;
        input.onStopMove += StopMove;

        //���п���
        input.onFire += Fire;
        input.onStopFire += StopFire;

        //���з���
        input.onDodge += Dodge;
        //���䵼��
        input.onLaunchMissle += LaunchMissile;

        //��������
        input.onOverdrive += onOverdrive;
        //������������
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
    //����ƶ�����
    void Move(Vector2 moveInput) 
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        Quaternion moveRotation = Quaternion.AngleAxis(moveRotationAngle * moveInput.y,Vector3.right);
        //moveInput * moveSpeed���ƶ���         
        moveCoroutine=StartCoroutine(MoveCoroutine(acclerationTime,moveInput.normalized * moveSpeed, moveRotation));
        //����λ������
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }
   void StopMove()
   {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine =StartCoroutine(MoveCoroutine(declerationTime, Vector2.zero, Quaternion.identity));
        //�ر�λ������
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
   //λ������Э��   
   IEnumerator MovePositionLimitCoroutine()
   {
        while (true)
        {
            //�����ƶ�
            transform.position = Viewport.Instance.PlayerMoveablePoint(transform.position,paddingX,paddingY);
            yield return null;
        }
   }
    //��ʼ��ֹͣ�ƶ�Э��
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
    //������
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
        //���ܺ���
        PlayerEnergy.Instance.Use(dodgeEnergy);
        //����޵�
        collider2d.isTrigger = true;

        currentRoll = 0f;

           
        //��ҷ���
        while (currentRoll < MaxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation=Quaternion.AngleAxis(currentRoll, Vector3.right);
            //��������
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
        //������ǿ
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
