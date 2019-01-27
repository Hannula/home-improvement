using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Is an event active.
    /// </summary>
    public bool eventActive;

    /// <summary>
    /// Is an event ending.
    /// </summary>
    public bool eventResultsSeen;

    public HomeUpgrade promisedRewardItem;

    private UIManager ui;
    private InventoryManager inventoryManager;

    // Start is called before the first frame update
    private void Start()
    {
        ui = FindObjectOfType<UIManager>();
        ui.SetEventManager(this);
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void StartEvent(Node node)
    {
        if (GameManager.Instance.State == GameManager.GameState.Map)
        {
            if (!eventActive)
            {
                eventActive = true;
                SetEventRiskTier(node.Event);
                SetPromisedRewardUpgrade(node.Event);
                string gainText = GetGainString(node.Event.Choices[0].Gain);
                ui.eventDialog.ShowEvent(node.Event, gainText);

                if (node.Event.MustFight)
                {
                    SFXPlayer.Instance.Play(Sound.Alarm);
                }
            }
        }
        else
        {
            Debug.LogError("Events can only happen in the Map screen.");
        }
    }

    private void EventResults(EventType actionEvent, int actionIndex)
    {
        eventResultsSeen = true;
        EventChoice choice = actionEvent.Choices[actionIndex];

        switch (choice.Action)
        {
            case Action.Fight:
            {
                GameManager.Instance.LoadBattleScene();
                break;
            }
            case Action.Gain:
            {
                if (choice.Gain.Type == EventGain.GainType.Upgrade)
                {
                    //HomeUpgrade newItem = ContentManager.Instance.
                    //    GetRandomHomeUpgrade(actionEvent.RiskTier, actionEvent.RiskTier);
                    inventoryManager.AddItem(promisedRewardItem);
                }
                else if (choice.Gain.Type == EventGain.GainType.Floor)
                {
                    FloorData fd = new FloorData(
                        ContentManager.Instance.GetRandomFloorType(actionEvent.Tier),
                        ContentManager.Instance.GetRandomWallType(actionEvent.Tier));
                    GameManager.Instance.PlayerHome.Floors.Add(fd);
                    GameManager.Instance.home.UpdateHome();
                    SFXPlayer.Instance.Play(Sound.Repair);
                }
                else if (choice.Gain.Type == EventGain.GainType.Money)
                {
                    GameManager.Instance.money += choice.Gain.Amount;
                }
                break;
            }
            case Action.Lose:
            {
                if (choice.Gain.Type == EventGain.GainType.Upgrade)
                {
                    // TODO
                }
                else if (choice.Gain.Type == EventGain.GainType.Floor)
                {
                    // TODO
                }
                else if (choice.Gain.Type == EventGain.GainType.Money)
                {
                    GameManager.Instance.money -= choice.Gain.Amount;
                    if (GameManager.Instance.money < 0)
                    {
                        GameManager.Instance.money = 0;
                    }
                }
                break;
            }
            case Action.Advance:
            {
                // TODO: Load next map.
                GameManager.Instance.EndGame(true); // testing
                break;
            }
            default:
            {
                break;
            }
        }

        ShowConfirmScreen(choice);

        promisedRewardItem = null;
    }

    private void ShowConfirmScreen(EventChoice choice)
    {
        if (choice.SkipConfirm)
        {
            EndEventWithoutResults();
        }
        else
        {
            string confirmText = "OK";
            string gainOrLostText = "";
            if (choice.Action == Action.Gain)
            {
                gainOrLostText = GetGainString(choice.Gain);
            }
            else if (choice.Action == Action.Lose)
            {
                gainOrLostText = GetLostString(choice.Gain);
            }

            if (!choice.ShowGainNameInResult)
            {
                gainOrLostText = "";
            }
            ui.eventDialog.ShowResults(choice.ResultText, gainOrLostText, confirmText);
        }
    }

    public void EndEvent(EventType actionEvent, int actionIndex)
    {
        if (eventActive && actionEvent != null)
        {
            EventResults(actionEvent, actionIndex);
        }
    }

    public void EndEventWithoutResults()
    {
        eventActive = false;
        eventResultsSeen = false;
        ui.eventDialog.Close();
    }

    private void SetEventRiskTier(EventType actionEvent)
    {
        if (actionEvent.Tier > 0)
        {
            int randomTier = Random.Range(1, GameManager.Instance.regionNum);
            actionEvent.Tier = randomTier;
        }
    }

    private void SetPromisedRewardUpgrade(EventType actionEvent)
    {
        if (actionEvent.Choices.Count > 0)
        {
            if (actionEvent.Choices[0].Action == Action.Gain
                && actionEvent.Choices[0].Gain.Type == EventGain.GainType.Upgrade)
            {
                promisedRewardItem = ContentManager.Instance.
                    GetRandomHomeUpgrade(actionEvent.Tier, actionEvent.Tier);
                //Debug.Log("Item promised: " + promisedRewardItem.GetName());
            }
            else
            {
                promisedRewardItem = null;
            }
        }
    }

    private string GetGainString(EventGain gain)
    {
        switch (gain.Type)
        {
            case EventGain.GainType.Upgrade:
            {
                if (promisedRewardItem != null)
                {
                    return promisedRewardItem.GetName();
                }
                else
                {
                    return "[NOT THE PROMISED ITEM]";
                }
            }
            case EventGain.GainType.Floor:
            {
                return "a new floor! Wow";
            }
            case EventGain.GainType.Money:
            {
                return gain.Amount + " Junk";
            }
            case EventGain.GainType.Score:
            {
                return gain.Amount + " points";
            }
            default:
            {
                return "nothing";
            }
        }
    }

    private string GetLostString(EventGain lost)
    {
        switch (lost.Type)
        {
            case EventGain.GainType.Upgrade:
            {
                return "[LOST UPGRADE]";
            }
            case EventGain.GainType.Floor:
            {
                return "[LOST FLOOR]";
            }
            case EventGain.GainType.Money:
            {
                return "[LOST " + lost.Amount + " JUNK]";
            }
            case EventGain.GainType.Score:
            {
                return "[LOST " + lost.Amount + " POINTS]";
            }
            default:
            {
                return "nothing";
            }
        }
    }
}
