using System.Collections.Generic;
using UnityEngine;

// README:
// Create a new game object with only an AudioSource component.
// Leave AudioClip empty and set all bools (from Mute to Loop) to false.
// Make it a prefab and give it to this in the editor. For each sound clip,
// there must be a corresponding name in the Sound enum in the correct order.
// The volume can be controlled with this script but it's not necessary.
// In our game, another Singleton object called GameManager handles
// audio settings. You need to adjust this script a bit to fit your game.

    /// <summary>
    /// The sound effects' names
    /// </summary>
    public enum Sound
    {
        // NOTE:
        // Sound clips must be assigned to SFXPlayer
        // in this specific order for the right sound
        // to be played at the right time

        Alarm = 0,
        GetRekt1 = 1,
        GetRekt2 = 2,
        Repair = 3,
        Splat1 = 4,
        Splat2 = 5
    }

public class SFXPlayer : MonoBehaviour
{
    #region Statics
    private static SFXPlayer instance;

    /// <summary>
    /// Gets or sets the Singleton instance .
    /// </summary>
    public static SFXPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                // NOTE:
                // There must be a Resources folder under Assets and
                // SFXPlayer there for this to work. Not necessary if
                // a SoundPlayer object is present in a scene from the
                // get-go.

                instance =
                    Instantiate(Resources.Load<SFXPlayer>("SFXPlayer"));
                instance.Init();
            }

            return instance;
        }
    }
    #endregion Statics

    /// <summary>
    /// The sound list
    /// </summary>
    [SerializeField,
        Tooltip("The sound list")]
    private List<AudioClip> sounds;

    /// <summary>
    /// The SFX volume
    /// </summary>
    [SerializeField, Range(0, 1),
        Tooltip("The SFX volume")]
    private float volume = 1;

    /// <summary>
    /// How many individual sounds can play at the same time
    /// </summary>
    [SerializeField,
        Tooltip("How many individual sounds can play at the same time")]
    private int audioSrcPoolSize = 5;

    /// <summary>
    /// Can new AudioSources be created if there are no unused left.
    /// </summary>
    [SerializeField, Tooltip("Can new AudioSources be created " +
        "if there are no unused left")]
    private bool flexiblePoolSize;

    /// <summary>
    /// The AudioSource pool
    /// </summary>
    private List<AudioSource> audioSrcPool;

    private float pitch;

    /// <summary>
    /// The object is initialized on start.
    /// </summary>
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Init();
    }

    /// <summary>
    /// Initializes the SFX player.
    /// </summary>
    private void Init()
    {
        // Initializes the AudioSource pool
        InitAudioSrcPool();

        volume = 1f;
        pitch = 1;
    }

    /// <summary>
    /// Initializes the AudioSource pool.
    /// </summary>
    public void InitAudioSrcPool()
    {
        audioSrcPool = new List<AudioSource>();

        for (int i = 0; i < audioSrcPoolSize; i++)
        {
            CreateNewAudioSrc();
        }
    }

    /// <summary>
    /// Adds new AudioSources to the pool.
    /// </summary>
    /// <param name="increase">the number of new AudioSources</param>
    /// <returns>the last created AudioSource</returns>
    private AudioSource IncreasePoolSize(int increase)
    {
        AudioSource audioSrc = null;

        if (increase > 0)
        {
            audioSrcPoolSize += increase;

            for (int i = 0; i < increase; i++)
            {
                audioSrc = CreateNewAudioSrc();
            }
        }

        return audioSrc;
    }

    /// <summary>
    /// Creates a new game object with an AudioSource
    /// component and adds it to the pool.
    /// </summary>
    /// <returns>an AudioSource</returns>
    private AudioSource CreateNewAudioSrc()
    {
        AudioSource audioSrc = new GameObject().AddComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.transform.position = transform.position;
        audioSrcPool.Add(audioSrc);
        return audioSrc;
    }

    /// <summary>
    /// Updates the object once per frame.
    /// </summary>
    private void Update()
    {
        if (GameManager.Instance.Transition == GameManager.SceneTransition.InScene)
        {
            // Returns any finished AudioSource to the pool to be used again
            ReturnFinishedAudioSrcsToPool();
        }
    }

    public void EmptyAudioSrcPool()
    {
        audioSrcPool.Clear();
    }

    /// <summary>
    /// Plays a sound clip which corresponds with the given name.
    /// </summary>
    /// <param name="sound">a sound's name</param>
    public AudioSource Play(Sound sound)
    {
        return Play((int) sound);
    }

    /// <summary>
    /// Plays a sound clip which corresponds with the given name
    /// and has the given volume and pitch.
    /// </summary>
    /// <param name="sound">a sound's name</param>
    /// <param name="pitch">sound's volume (optional)</param>
    /// <param name="pitch">sound's pitch (optional)</param>
    public AudioSource Play(Sound sound, float volumeFactor = 1f, float pitch = 1f)
    {
        float oldVolume = volume;
        if (volumeFactor != 1f)
        {
            volume = volume * volumeFactor;
        }
        this.pitch = pitch;
        AudioSource result = Play((int) sound);
        this.volume = oldVolume;
        return result;
    }

    /// <summary>
    /// Plays a sound clip with the given number.
    /// </summary>
    /// <param name="soundNum">a sound clip's number</param>
    public AudioSource Play(int soundNum)
    {
        if (soundNum >= 0 &&
            soundNum < sounds.Count)
        {
            // Plays the sound
            return Play(sounds[soundNum]);
        }
        else
        {
            Debug.LogError("[SoundPlayer]: The requested sound " +
                           "clip cannot be played");
        }

        return null;
    }

    /// <summary>
    /// Plays a sound clip.
    /// </summary>
    /// <param name="clip">a sound clip</param>
    private AudioSource Play(AudioClip clip)
    {
        AudioSource audioSrc = GetAudioSrcFromPool();

        // If there are no unused AudioSources
        // and the pool's size is flexible, a
        // new AudioSource is created
        if (audioSrc == null && flexiblePoolSize)
        {
            audioSrc = IncreasePoolSize(1);
            audioSrc.enabled = true;
        }

        // Plays a sound
        if (audioSrc != null)
        {
            audioSrc.clip = clip;
            audioSrc.volume = volume;
            audioSrc.pitch = pitch;
            audioSrc.Play();
            //audioSrc.PlayOneShot(clip, volume);

            pitch = 1;
        }
        // Otherwise prints debug data
        //else
        //{
        //    Debug.Log("[SoundPlayer]: All AudioSources are being used " +
        //              "and a new one could not be created");
        //}

        return audioSrc;
    }

    public AudioSource PlayLooped(Sound sound)
    {
        return PlayLooped((int) sound);
    }

    public AudioSource PlayLooped(Sound sound, float pitch)
    {
        this.pitch = pitch;
        return PlayLooped((int) sound);
    }

    public AudioSource PlayLooped(int soundNum)
    {
        if (soundNum >= 0 &&
            soundNum < sounds.Count)
        {
            // Plays the sound
            return PlayLooped(sounds[soundNum]);
        }
        else
        {
            Debug.LogError("[SoundPlayer]: The requested sound " +
                           "clip cannot be played");
        }

        return null;
    }

    private AudioSource PlayLooped(AudioClip clip)
    {
        AudioSource audioSrc = GetAudioSrcFromPool();

        // If there are no unused AudioSources
        // and the pool's size is flexible, a
        // new AudioSource is created
        if (audioSrc == null && flexiblePoolSize)
        {
            audioSrc = IncreasePoolSize(1);
            audioSrc.enabled = true;
        }

        // Plays a sound
        if (audioSrc != null)
        {
            audioSrc.clip = clip;
            audioSrc.volume = volume;
            audioSrc.pitch = pitch;
            audioSrc.loop = true;
            audioSrc.Play();
            //audioSrc.PlayOneShot(clip, volume);

            pitch = 1;
        }
        // Otherwise prints debug data
        //else
        //{
        //    Debug.Log("[SoundPlayer]: All AudioSources are being used " +
        //              "and a new one could not be created");
        //}

        return audioSrc;
    }

    /// <summary>
    /// Gets an unused AudioSource from the pool.
    /// </summary>
    /// <returns>an unused AudioSource</returns>
    private AudioSource GetAudioSrcFromPool()
    {
        foreach (AudioSource audioSrc in audioSrcPool)
        {
            if (!audioSrc.enabled)
            {
                audioSrc.enabled = true;
                return audioSrc;
            }
        }

        //Debug.Log("[SoundPlayer]: All AudioSources are being used");
        return null;
    }

    /// <summary>
    /// Makes all finished sound effects usable again.
    /// </summary>
    private void ReturnFinishedAudioSrcsToPool()
    {
        foreach (AudioSource audioSrc in audioSrcPool)
        {
            if (audioSrc.enabled && !audioSrc.isPlaying)
            {
                DeactivateAudioSrc(audioSrc);
            }
        }
    }

    /// <summary>
    /// Stops all sound effect playback.
    /// This is called when the scene changes.
    /// </summary>
    public void StopAllSFXPlayback()
    {
        foreach (AudioSource audioSrc in audioSrcPool)
        {
            audioSrc.Stop();
            DeactivateAudioSrc(audioSrc);
            audioSrc.loop = false;
        }
    }

    public void StopIndividualSFX(string clipName)
    {
        foreach (AudioSource audioSrc in audioSrcPool)
        {
            if (audioSrc.enabled && audioSrc.clip != null &&
                audioSrc.clip.name == clipName)
            {
                audioSrc.Stop();
                audioSrc.loop = false;
            }
        }
    }

    private void DeactivateAudioSrc(AudioSource audioSrc)
    {
        //RemoveForbiddenDuplicate(audioSrc.clip.name);
        audioSrc.enabled = false;
    }

    /// <summary>
    /// Sets the AudioSources' volume.
    /// </summary>
    /// <param name="volume">volume level</param>
    public void SetVolume(float volume)
    {
        this.volume = volume;
    }
}
