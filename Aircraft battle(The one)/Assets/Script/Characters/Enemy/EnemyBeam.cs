using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] float damage=1f;
    [SerializeField] GameObject hitVFX;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
            //����ײ��֮��ĽӴ���
            var contactPoint = collision.GetContact(0);
            //�ͷ���Ч
            PoolManager.Release(hitVFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
        }
    }

}
