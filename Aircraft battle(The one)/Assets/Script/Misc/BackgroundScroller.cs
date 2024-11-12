using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    Material material;
    //´æ´¢Æ«ÒÆÖµ
    [SerializeField] Vector2 scollVelocity;


    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        while (GameManager.GameState!=GameState.GameOver)
        {
            material.mainTextureOffset += scollVelocity * Time.deltaTime;
            yield return null;
        }
    }
}
