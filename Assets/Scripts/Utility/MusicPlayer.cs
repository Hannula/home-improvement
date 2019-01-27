using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// README:
// In the AudioSource component, leave AudioClip empty and set
// all bools (from Mute to Loop) to false. The volume can be controlled
// with this script but it's not necessary. In our game, another
// Singleton object called GameManager handles audio settings.
// You need to adjust this script a bit to fit your game.

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    #region Statics
    private static MusicPlayer instance;

    /// <summary>
    /// Gets or sets the Singleton instance.
    /// </summary>
    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                // NOTE:
                // There must be a Resources folder under Assets and
                // MusicPlayer there for this to work. Not necessary if
                // a MusicPlayer object is present in a scene from the
                // get-go.

                instance =
                    Instantiate(Resources.Load<MusicPlayer>("MusicPlayer"));
            }

            return instance;
        }
    }
    #endregion Statics

    /// <summary>
    /// The track list
    /// </summary>
    [SerializeField,
        Tooltip("The track list")]
    private List<AudioClip> tracks;

    /// <summary>
    /// The time between tracks
    /// </summary>
    [SerializeField, Tooltip("How much time must pass before " +
        "the next track starts playing")]
    private float timeBetweenTracks;

    /// <summary>
    /// The playback progress
    /// </summary>
    [SerializeField, Range(0, 1),
        Tooltip("The playback progress")]
    private float progress;

    /// <summary>
    /// Is playback paused
    /// </summary>
    [SerializeField,
        Tooltip("Is playback paused")]
    private bool paused;

    /// <summary>
    /// Is the music fading out
    /// </summary>
    [SerializeField,
        Tooltip("Is the music fading out")]
    private bool fadeOut;

    /// <summary>
    /// The current track
    /// </summary>
    public int currentTrack;
    public float fadeTime;

    private AudioSource audioSrc;
    private float baseVolume;
    private float oldProgress;
    private float waitStartTime;
    private float elapsedFadeTime;

    public bool IsPlaying
    {
        get
        {
            return audioSrc.isPlaying;
        }
    }

    /// <summary>
    /// Lets a new Singleton instance be created.
    /// </summary>
    public void Create()
    {
        // Does nothing; only used for creating a new Singleton instance
    }

    /// <summary>
    /// The object is initialized on awake.
    /// </summary>
    private void Awake()
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

        Init();
    }

    /// <summary>
    /// Initializes the music player.
    /// </summary>
    private void Init()
    {
        // Initializes the audio source
        audioSrc = GetComponent<AudioSource>();

        // Initializes the volume
        audioSrc.volume = 1f;
        baseVolume = audioSrc.volume;

        // Corrects timeBetweenTracks' invalid value
        if (timeBetweenTracks < 0)
        {
            timeBetweenTracks = 0;
        }

        // Sets the music player to not be destroyed when changing scene
        DontDestroyOnLoad(gameObject);

        // Starts playing the first track
        Play();
    }

    /// <summary>
    /// Updates the object once per frame.
    /// </summary>
    private void Update()
    {
        // The track is playing
        if (IsPlaying)
        {
            UpdateWhenPlaying();
        }
        // The track is unpaused or over
        else if (!paused)
        {
            UpdateWhenNotPlaying();
        }
        // Waiting for the next track
        else if (progress == 1)
        {
            UpdateBetweenTracks();
        }
    }

    /// <summary>
    /// Updates when the track is playing.
    /// </summary>
    private void UpdateWhenPlaying()
    {
        // The playback is paused in the editor
        if (paused)
        {
            Pause();
            return;
        }

        // The playback progresses normally
        if (progress == oldProgress)
        {
            progress = audioSrc.time / tracks[currentTrack].length;
            oldProgress = progress;
        }
        // If the playback progress has been changed in the
        // editor, the playback time is adjusted accordingly
        else
        {
            SetProgress(progress);
        }

        // The track fades out
        if (fadeOut)
        {
            UpdateFadeOut();
        }
    }

    /// <summary>
    /// Updates when the track is unpaused or over.
    /// </summary>
    private void UpdateWhenNotPlaying()
    {
        // The playback is unpaused in the editor
        if (progress < 0.99f)
        {
            Unpause();

            // If the track had ended to a
            // fade-out, it's restarted
            if (!audioSrc.isPlaying)
            {
                Play();
            }
        }
        // The track is over
        else
        {
            Finish();
        }
    }

    /// <summary>
    /// Updates between tracks.
    /// </summary>
    private void UpdateBetweenTracks()
    {
        // The next track starts if enough time has passed
        if (timeBetweenTracks <= 0 ||
            (Time.time - waitStartTime) >= timeBetweenTracks)
        {
            //Debug.Log("[MusicPlayer]: Next track starts");

            Reset();
            NextTrack();
            Play();
        }
        // Otherwise prints debug info
        //else
        //{
        //    Debug.Log("[MusicPlayer]: Next track in: " +
        //        (timeBetweenTracks - (Time.time - waitStartTime)));
        //}
    }

    /// <summary>
    /// Sets the value of the progress bar.
    /// </summary>
    /// <param name="progress">the value of the progress bar</param>
    private void SetProgress(float progress)
    {
        audioSrc.time = progress * tracks[currentTrack].length;
        oldProgress = progress;
    }

    /// <summary>
    /// Starts playing the currently selected track.
    /// </summary>
    public void Play()
    {
        Play(currentTrack);
    }

    /// <summary>
    /// Starts playing a certain track.
    /// </summary>
    /// <param name="trackNum">the track's number in the rack list</param>
    public void Play(int trackNum)
    {
        if (trackNum >= 0 &&
            trackNum < tracks.Count)
        {
            if (fadeOut)
            {
                FinishFadeOut();
            }

            if (paused)
            {
                paused = false;
            }

            currentTrack = trackNum;
            audioSrc.clip = tracks[currentTrack];
            audioSrc.Play();
            //Debug.Log("[MusicPlayer] Playing track " + currentTrack);
        }
    }

    /// <summary>
    /// Stops playback and resets the track.
    /// </summary>
    public void Stop()
    {
        audioSrc.Stop();
        paused = true;
        Reset();
    }

    /// <summary>
    /// Resets the track.
    /// </summary>
    private void Reset()
    {
        audioSrc.time = 0f;
        progress = 0f;
        oldProgress = 0f;
        elapsedFadeTime = 0f;
    }

    /// <summary>
    /// Finishes the currently playing track.
    /// </summary>
    private void Finish()
    {
        //Debug.Log("[MusicPlayer]: Track finished");

        progress = 1;
        paused = true;
        waitStartTime = Time.time;
    }

    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause()
    {
        //Debug.Log("[MusicPlayer]: Track paused");

        audioSrc.Pause();
        paused = true;
    }

    /// <summary>
    /// Unpauses playback.
    /// </summary>
    public void Unpause()
    {
        //Debug.Log("[MusicPlayer]: Track unpaused");

        audioSrc.UnPause();
        paused = false;
    }

    /// <summary>
    /// Selects the next track in the list.
    /// </summary>
    private void NextTrack()
    {
        if (tracks.Count > 0)
        {
            currentTrack++;
            if (currentTrack >= tracks.Count)
            {
                currentTrack = 0;
            }
        }
    }

    /// <summary>
    /// Selects the previous track in the list.
    /// </summary>
    private void PrevTrack()
    {
        if (tracks.Count > 0)
        {
            currentTrack--;
            if (currentTrack < 0)
            {
                currentTrack = tracks.Count - 1;
            }
        }
    }

    /// <summary>
    /// Sets the AudioSource's volume.
    /// </summary>
    /// <param name="volume">volume level</param>
    public void SetVolume(float volume)
    {
        // Allows any changes to the volume if fade-out is not
        // active, and if it is, allows only decreasing the volume
        if (!fadeOut || volume < audioSrc.volume)
        {
            audioSrc.volume = volume;

            if (!fadeOut)
            {
                baseVolume = volume;
            }
        }
    }

    /// <summary>
    /// Starts fading out the track.
    /// </summary>
    public void StartFadeOut()
    {
        Debug.Log("[MusicPlayer] Fade out starting");
        fadeOut = true;
        elapsedFadeTime = 0f;
    }

    /// <summary>
    /// Starts fading out the track if the next track is different.
    /// </summary>
    /// <param name="nextTrack">The next track's number</param>
    public void StartFadeOut(int nextTrack)
    {
        if (currentTrack != nextTrack)
        {
            StartFadeOut();
        }
    }

    /// <summary>
    /// Updates the fade-out by decreasing the volume until it reaches 0.
    /// After that, playback will be stopped and the volume reset.
    /// </summary>
    private void UpdateFadeOut()
    {
        elapsedFadeTime += Time.deltaTime;
        float ratio = 1;
        if (fadeTime > 0)
        {
            ratio = elapsedFadeTime / fadeTime;
        }

        float newVolume = Mathf.Lerp(baseVolume, 0, ratio);
        if (newVolume <= 0)
        {
            FinishFadeOut();
        }
        else
        {
            SetVolume(newVolume);
        }
    }

    /// <summary>
    /// Finishes the fade-out by stopping playback
    /// and setting the volume back to normal.
    /// </summary>
    private void FinishFadeOut()
    {
        Debug.Log("[MusicPlayer] Fade out finished");
        Stop();
        fadeOut = false;
        SetVolume(baseVolume);
    }
}
