using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] AudioData fullHealthPickUpSFX;
    [SerializeField] int fullHealthScoreBonus = 150;
    [SerializeField] float shieldBonus = 20;
    protected override void PickUp()
    {
        if (player.IsFullHealth)
        {
            pickUpSFX = fullHealthPickUpSFX;
            lootMessage.text =$"SCORE+{fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(fullHealthScoreBonus);
        }
        else
        {
            pickUpSFX = DefaultPickUpSFX;
            lootMessage.text = $"SHIELD+{shieldBonus}";
            player.RestoreHealth(shieldBonus);
        }
        base.PickUp();
    }
}

