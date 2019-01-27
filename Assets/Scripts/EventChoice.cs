using Action = data.Action;

[System.Serializable]
public class EventChoice
{
    public string Name;
    public string ResultText;
    public bool ShowGainNameInResult;
    public Action Action;
    public EventGain Gain;
    public bool SkipConfirm;
}
