using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    Vector3 targetDirection;

    [SerializeField] float minBallisticAngle=50f;
    [SerializeField] float maxBallisticAngle=75f;

    float ballisticAngle;

    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle=Random.Range(minBallisticAngle,maxBallisticAngle);
        while(gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                targetDirection=target.transform.position-transform.position;
                
                //求与对方的距离对x轴的夹角
                var angle=Mathf.Atan2(targetDirection.y, targetDirection.x)*Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0, 0, ballisticAngle);            
                projectile.Move();
                
            }
            else
            {
                
                projectile.Move();
            }
            yield return null;  
        }
    }
}
