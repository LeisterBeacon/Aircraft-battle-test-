using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissle : PlayerProjectileOverdrive
{
    [SerializeField] AudioData targetVoice ;
    [Header("=======Speed======")]
    [SerializeField]float lowSpeed=8f;
    [SerializeField]float highSpeed = 25f;
    //变速延迟时间
    [SerializeField] float variableSpeedDelay=0.5f;
    WaitForSeconds waitVariableSpeedDelay;
    [Header("=======Explosion======")]
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] LayerMask enemyLayerMask = default;
    [SerializeField] float explosionDamage = 100f;

    protected override void Awake()
    {
        base.Awake();
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }
    //执行范围伤害
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        var coliiders=Physics2D.OverlapCircleAll(collision.GetContact(0).point, explosionRadius,enemyLayerMask);
        foreach(var coli in coliiders)
        {
            if(coli.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed=lowSpeed;
        yield return waitVariableSpeedDelay;

        moveSpeed=highSpeed;
        if (target != null)
        {
            Debug.Log("www");
            AudioManager.Instance.PlayRandomSFX(targetVoice);
        }
    }

}
