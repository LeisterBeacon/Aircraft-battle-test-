using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    BossHealthBar healthBar;
    Canvas canvas;
    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<BossHealthBar>();
        canvas = healthBar.GetComponentInChildren<Canvas>();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Init(health,maxHealth);
        canvas.enabled = true;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();         
        }
    }
    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        canvas.enabled = false;
        base.Die();
       
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateState(health, maxHealth);
    }
    
}
