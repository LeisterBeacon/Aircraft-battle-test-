using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField]Text waveText;
    private void Awake()
    {
        //GetComponent<Canvas>().worldCamera=Camera.main;
        //waveText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        waveText.text= "- WAVE "+EnemyManager.Instance.WaveNumber+" -";
    }
}
