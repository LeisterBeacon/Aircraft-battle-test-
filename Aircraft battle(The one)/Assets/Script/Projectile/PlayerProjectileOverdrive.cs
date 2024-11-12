using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    TrailRenderer trail;
    ProjectileGuidanceSystem guidanceSystem;
    GameObject enemy;
    [SerializeField] GameObject boss;
    
    protected override void Awake()
    {
        guidanceSystem = GetComponent<ProjectileGuidanceSystem>();
        trail = GetComponentInChildren<TrailRenderer>();
        if (moveDirection != Vector2.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirection);
        }    

    }
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomeEnemy);
        
        //if (EnemyManager.Instance.enemyList.Count == 1)
        //{
        //    SetTarget(boss);
        //}
        //else
        //{
        //    SetTarget(EnemyManager.Instance.RandomeEnemy);
        //}
        transform.rotation = Quaternion.identity;
        if(target == null)
        {
            
            base.OnEnable();
        }
        else
        {
            
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }
       
    }
     void OnDisable()
    {
        //子弹被禁用时去除子弹轨迹
        trail.Clear();  
    }
}
