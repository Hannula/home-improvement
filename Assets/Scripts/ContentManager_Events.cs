using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using data;

public class ContentManager_Events : MonoBehaviour
{
    public static ContentManager_Events Instance;

    public List<EventType> EventTypes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject);
    }

    public EventType GetRandomEvent(int riskTier)
    {
        List<EventType> tierEvents = new List<EventType>();
        while (riskTier > 0 && tierEvents.Count <= 0)
        {
            tierEvents = EventTypes.Where(x => x.Tier == riskTier).ToList();
            riskTier -= 1;
        }
        if (tierEvents.Count <= 0)
        {
            return null;
        }
        return UtilityFunctions.GetRandomElement(tierEvents);
    }

    public EventType GetHomeEvent()
    {
        EventType homeEvent = null;
        try
        {
            homeEvent = EventTypes.First(x => x.Name.Equals("Home"));
        }
        catch (InvalidOperationException e)
        {
            Debug.LogError("No Home event found.");
        }

        return homeEvent;
    }

    public EventType GetGoalEvent()
    {
        EventType goalEvent = null;
        try
        {
            goalEvent = EventTypes.First(x => x.Name.Equals("Goal"));
        }
        catch (InvalidOperationException e)
        {
            Debug.LogError("No Goal event found.");
        }

        return goalEvent;
    }
}
