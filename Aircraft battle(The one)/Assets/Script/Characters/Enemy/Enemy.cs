using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //��ҵ÷�
    [SerializeField] int scorePoint = 100;
    //������������������
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
        //����Ҽӷ�
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBouns);
        //����ʱ�Ƴ��б�
        EnemyManager.Instance.RemoveList(gameObject);
        lootspawner.Spawn(transform.position);
        base.Die();
    }
    void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }
}
