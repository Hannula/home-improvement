using Action = data.Action;

[System.Serializable]
public class EventChoice
{
    public string Name;
    public string ResultText;
    public Action Action;
    public bool SkipConfirm;
}
