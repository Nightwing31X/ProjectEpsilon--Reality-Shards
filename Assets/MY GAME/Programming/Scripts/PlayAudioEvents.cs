using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioEvents : MonoBehaviour
{
    private AudioSource _objSound;
    void Awake()
    {
        _objSound = GetComponent<AudioSource>();
    }

    private void play_sound()
    {
        _objSound.Play();
    }
}
