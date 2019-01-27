using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtSceneStart : MonoBehaviour
{
    [SerializeField]
    private Sound sound;

    [SerializeField, Range(0f, 2f)]
    private float volume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SFXPlayer.Instance.Play(sound, volumeFactor: volume);
    }
}
