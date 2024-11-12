using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    [SerializeField]float damage;
    [SerializeField]protected  float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;
    protected GameObject target;
    [SerializeField] AudioData[] hitSFX;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf) 
        {
            Move();
            yield return null;  
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            //����ײ��֮��ĽӴ���
            var contactPoint = collision.GetContact(0);
            //�ͷ���Ч
            PoolManager.Release(hitVFX,contactPoint.point,Quaternion.LookRotation(contactPoint.normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);    
            gameObject.SetActive(false);
        }
    }
    protected void SetTarget(GameObject target)
    {
        this.target = target;   
    }

    public void Move()
    {

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
