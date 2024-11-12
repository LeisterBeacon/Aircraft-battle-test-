using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    protected Player player;
    [SerializeField] float minSpeed = 5f;
    [SerializeField] float maxSpeed = 15f;
    Animator animator;
    int pickUpStateID = Animator.StringToHash("PickUp");
    [SerializeField] protected AudioData DefaultPickUpSFX;
    protected AudioData pickUpSFX;

    protected Text lootMessage;
    private void Awake()
    {
        player=FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        //获取处于禁用状态组件
        lootMessage=GetComponentInChildren<Text>(true);
        pickUpSFX = DefaultPickUpSFX;
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(MoveCoroutine));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
    protected virtual void PickUp()
    {
        StopAllCoroutines();
        animator.Play(pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(pickUpSFX);
    }
    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 direction=Vector3.left;
        while (true)
        {
            if(player.isActiveAndEnabled)
            {
                direction=(player.transform.position-transform.position).normalized;
            }
            transform.Translate(direction*speed*Time.deltaTime );
            yield return null;
        }
    }
}
