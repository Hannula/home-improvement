using System.Collections.Generic;

[System.Serializable]
public class EventType
{
    public string Name;
    public string Description;
    public List<EventChoice> Choices;
    public bool MustFight;
    public int Tier = 1;
}
