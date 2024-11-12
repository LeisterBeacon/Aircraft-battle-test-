using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoosController :EnemyController
{
    [SerializeField] float ContinuousFireDuration = 1.5f;
    WaitForSeconds waitForContinuousFireInterval;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForBeamCoolDown;
    Animator animator;
    Transform PlayerTransform;
    [Header("=======Player Dir=======")]
    [SerializeField] Transform PlayerDetectionTransfrom;
    [SerializeField] Vector3 PlayerDetectionSize;
    [SerializeField] LayerMask PlayerLayerMask;
    //µØº–
    List<GameObject> magazine;
    AudioData launchSFX;
    [Header("=======BEAM=======")]
    [SerializeField] float beamCooldownTime = 12f;
    bool isBeamReady;
    int launchBeamID = Animator.StringToHash("launchBeam");
    [SerializeField] AudioData beamChargingSFX;
    [SerializeField] AudioData beamLaunchSFX;

   


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        waitForContinuousFireInterval=new WaitForSeconds(minFireInterval);
        waitForFireInterval=new WaitForSeconds(maxFireInterval);
        magazine=new List<GameObject>(projectiles.Length);
        waitForBeamCoolDown=new WaitForSeconds(beamCooldownTime);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void OnEnable()
    {
        isBeamReady=false;
        muzzleVFX.Stop();
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(PlayerDetectionTransfrom.position, PlayerDetectionSize);
    }
    //∑¢…‰º§π‚
    void ActivateBeamWeapon()
    {
        isBeamReady = false;
        animator.SetTrigger(launchBeamID);
        AudioManager.Instance.playSFX(beamChargingSFX);
    }
    void AE_LaunchBeam()
    {
        AudioManager.Instance.playSFX(beamLaunchSFX);
    }
    void AE_stopBeam()
    {
        moveSpeed /= 1.5f;
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }
    void LoadProjectiles()
    {
        magazine.Clear();
        if (Physics2D.OverlapBox(PlayerDetectionTransfrom.position, PlayerDetectionSize, 0f, PlayerLayerMask))
        {
            
            magazine.Add(projectiles[0]);
            launchSFX = protectileSFX[0];
        }
        else
        {
            if (Random.value < 0.5f)
            {
                magazine.Add(projectiles[1]);
                launchSFX = protectileSFX[1];
            }
            else
            {
                for (int i = 2; i < projectiles.Length; i++)
                {
                    magazine.Add(projectiles[i]);
                }
                launchSFX = protectileSFX[2];
            }
        }
    }
    protected override IEnumerator RandomlyFireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (GameManager.GameState == GameState.GameOver) yield break;
            if (isBeamReady)
            {

                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }

    }
    
    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectiles();
        muzzleVFX.Play();
        float ContinuousFireTimer = 0f;
        while(ContinuousFireTimer < ContinuousFireDuration)
        {
            foreach (var projectile in magazine)
            {
                PoolManager.Release(projectile,muzzle.position);
            }
            ContinuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(launchSFX);
            yield return waitForContinuousFireInterval;
        }
        muzzleVFX.Stop();

    }
    IEnumerator BeamCooldownCoroutine()
    {
        yield return waitForBeamCoolDown ;
        isBeamReady = true;

    }
    IEnumerator ChasingPlayerCoroutine()
    {
        moveSpeed *= 1.5f;
        while (isActiveAndEnabled)
        {
            
            targetPos.x = Viewport.Instance.MaxX - paddingX;
            targetPos.y =PlayerTransform.position.y;
            yield return null;
        }
    }
}
