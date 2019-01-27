using System.Collections.Generic;

[System.Serializable]
public class EventType
{
    public string Name;
    public string Description;
    public List<EventChoice> Choices;
    public bool MustFight;
    public EventGain Gain;
    public int RiskTier = 1;
}
