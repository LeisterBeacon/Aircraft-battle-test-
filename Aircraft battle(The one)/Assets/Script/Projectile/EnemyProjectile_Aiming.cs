using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    private void Awake()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
        
    }
    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));
        
        base.OnEnable();
    }
    

    //稍等极短瞬间让引擎获取精确值
    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;
        if(target.activeSelf)
        {          
            moveDirection = (target.transform.position - transform.position).normalized;
        }
    }
}
