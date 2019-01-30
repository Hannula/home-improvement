using UnityEngine;
using UnityEngine.UI;

public class FadeToColor : MonoBehaviour
{
    public Image screenCoverImage;

    [SerializeField]
    private Color _color = Color.black;

    [SerializeField]
    private Color _altColor = Color.red;

    [SerializeField]
    private float _fadeOutTime = 1;

    [SerializeField]
    private float _fadeInTime = 1;

    private bool _fadeOut;
    private bool _useAltColor;
    private float _fadeProgress;
    private float _elapsedTime;
    private bool _wait;
    private short _maxWaitFrames = 2;
    private short _waitedFrames;

    public bool Active { get; private set; }

    public bool FadedOut
    {
        get
        {
            return (_fadeOut && _fadeProgress == 1);
        }
    }

    public bool FadedIn
    {
        get
        {
            return (!_fadeOut && _fadeProgress == 1);
        }
    }

    /// <summary>
    /// Initializes the object.
    /// </summary>
    private void Start()
    {
        if (screenCoverImage == null)
        {
            Debug.LogError("Screen cover image is null.");
        }
    }

    /// <summary>
    /// Starts fading in or out, opposite of whichever was done last.
    /// </summary>
    public void StartNextFade()
    {
        _fadeOut = !_fadeOut;

        if (_fadeOut)
        {
            StartFadeOut(false);
        }
        else
        {
            StartFadeIn(false);
        }
    }

    /// <summary>
    /// Starts fading out.
    /// </summary>
    public void StartFadeOut(bool useAltColor)
    {
        _fadeOut = true;
        _useAltColor = useAltColor;
        _wait = false;
        StartFade();
    }

    /// <summary>
    /// Starts fading in.
    /// </summary>
    public void StartFadeIn(bool wait)
    {
        _fadeOut = false;
        _wait = wait;
        _waitedFrames = 0;
        StartFade();
    }

    /// <summary>
    /// Starts the fading process.
    /// </summary>
    private void StartFade()
    {
        _fadeProgress = 0;
        _elapsedTime = 0;
        Active = true;
        UpdateTransparency();
    }

    /// <summary>
    /// Finishes the fading process.
    /// </summary>
    private void FinishFade()
    {
        _fadeProgress = 1;
        Active = false;
    }

    /// <summary>
    /// Updates the object once per frame.
    /// </summary>
    private void Update()
    {
        if (Active)
        {
            // If the framerate is very low when faded out, 
            // wait a few frames to give it time to go back
            // to normal and only then continue to fade in
            if (_wait)
            {
                _waitedFrames++;
                if (_waitedFrames >= _maxWaitFrames)
                {
                    _wait = false;
                }

                return;
            }

            // Increases the elapsed time
            _elapsedTime += Time.deltaTime;

            // Updates the fade's progress
            UpdateFadeProgress();

            // Updates the fade object's transparency
            UpdateTransparency();
        }
    }

    /// <summary>
    /// Updates the fade's progress.
    /// </summary>
    private void UpdateFadeProgress()
    {
        if (_fadeOut)
        {
            if (_fadeOutTime <= 0)
            {
                _fadeProgress = 1;
            }
            else
            {
                _fadeProgress = _elapsedTime / _fadeOutTime;
            }
        }
        else
        {
            if (_fadeInTime <= 0)
            {
                _fadeProgress = 1;
            }
            else
            {
                _fadeProgress = _elapsedTime / _fadeInTime;
            }
        }

        if (_fadeProgress >= 1f)
        {
            FinishFade();
        }
    }

    /// <summary>
    /// Updates the UI image's transparency.
    /// </summary>
    private void UpdateTransparency()
    {
        if (screenCoverImage != null)
        {
            Color newColor = (_useAltColor ? _altColor : _color);

            if (_fadeOut)
            {
                newColor.a = _fadeProgress;
            }
            else
            {
                newColor.a = 1f - _fadeProgress;
            }

            screenCoverImage.color = newColor;
        }
    }

    private void InstantFadeOut()
    {
        Color newColor = _color;
        newColor.a = 1f;
        screenCoverImage.color = newColor;
        Debug.Log("Instant fade-out");
    }
}
