using Action = data.Action;

[System.Serializable]
public class EventGain
{
    public enum GainType
    {
        Upgrade,
        Floor,
        Money,
        Score
    }

    public GainType Type;
    public int Amount;
}
