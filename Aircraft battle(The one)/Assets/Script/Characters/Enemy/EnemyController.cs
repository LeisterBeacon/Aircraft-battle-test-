using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("----Move----")]
    [SerializeField] protected float paddingX;
    [SerializeField] protected float paddingY;
    [SerializeField] protected float moveSpeed=2f;
    [SerializeField] float moveRotationAngle = 2f;

    protected Vector3 targetPos;


    [Header("----Fire----")]
    //������
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected  float maxFireInterval;

    [SerializeField] protected ParticleSystem muzzleVFX;
    //�ӵ�Ԥ����
    [SerializeField] protected GameObject[] projectiles;
    //ǹ��
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected AudioData[] protectileSFX;
    WaitForFixedUpdate waitForFixedUpdate=new WaitForFixedUpdate();

    protected virtual void Awake()
    {
        
    }
    
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position =Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        targetPos=Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
        //�ƶ�����
        while (gameObject.activeSelf)
        {
            //�Ƿ񵽴�Ŀ��λ��
            if(Vector3.Distance(targetPos, transform.position) >= moveSpeed * Time.fixedDeltaTime)
            {
                //����ǰ��Ŀ��λ��
                transform.position=Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPos - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                
                //�����µ�Ŀ��λ��
                targetPos= Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
            }
            yield return waitForFixedUpdate;
        }
    }

    //�������
    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));
            if (GameManager.GameState == GameState.GameOver) yield break;
            foreach(var projectile in projectiles)
            {
                PoolManager.Release(projectile,muzzle.position);
            }
            muzzleVFX.Play();
            AudioManager.Instance.PlayRandomSFX(protectileSFX);
        }
    }
}
