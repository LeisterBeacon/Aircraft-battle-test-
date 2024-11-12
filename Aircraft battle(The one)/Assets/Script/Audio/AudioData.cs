using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audio Data")]
public class AudioData :ScriptableObject
{
    public AudioClip audioClip;
    public float volume;
}
