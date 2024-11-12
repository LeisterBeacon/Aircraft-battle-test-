using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //玩家得分
    [SerializeField] int scorePoint = 100;
    //敌人死亡奖励的能量
    [SerializeField] int deathEnergyBouns = 3;
    [SerializeField] protected int healthFactor;

    LootSpawner lootspawner;
    protected virtual void Awake()
    {
        lootspawner = GetComponent<LootSpawner>();
    }
    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }
    public override void Die()
    {
        //给玩家加分
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBouns);
        //死亡时移出列表
        EnemyManager.Instance.RemoveList(gameObject);
        lootspawner.Spawn(transform.position);
        base.Die();
    }
    void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
