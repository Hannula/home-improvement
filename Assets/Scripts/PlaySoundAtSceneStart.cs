using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtSceneStart : MonoBehaviour
{
    [SerializeField]
    private Sound sound;

    [SerializeField, Range(0f, 2f)]
    private float volume = 1f;

    private AudioSource audioSrc;

    // Start is called before the first frame update
    private void Start()
    {
        audioSrc = SFXPlayer.Instance.Play(sound, volumeFactor: volume);
    }

    public AudioSource GetAudioSource()
    {
        return audioSrc;
    }
}
