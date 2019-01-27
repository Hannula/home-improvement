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

    public HomeUpgrade itemGain;

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
                itemGain = (node.Event.Gain.RandomItem ?
                    ContentManager.Instance.GetRandomHomeUpgrade(node.Event.RiskTier, node.Event.RiskTier) : null);
                string gainText = GetGainString(node.Event.Gain, node.Event.RiskTier);
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
                if (actionEvent.Gain.Money > 0)
                {
                    GameManager.Instance.money += actionEvent.Gain.Money;
                }
                else if (actionEvent.Gain.Money < 0)
                {
                    GameManager.Instance.money -= actionEvent.Gain.Money;
                    if (GameManager.Instance.money < 0)
                    {
                        GameManager.Instance.money = 0;
                    }
                }
                else if (actionEvent.Gain.RandomItem)
                {
                    inventoryManager.AddItem(itemGain);
                }
                else if (actionEvent.Gain.NewFloor)
                {
                    FloorData fd = new FloorData(
                        ContentManager.Instance.GetRandomFloorType(actionEvent.RiskTier),
                        ContentManager.Instance.GetRandomWallType(actionEvent.RiskTier));
                    GameManager.Instance.PlayerHome.Floors.Add(fd);
                    GameManager.Instance.home.UpdateHome();
                    SFXPlayer.Instance.Play(Sound.Repair);
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

        if (choice.SkipConfirm)
        {
            EndEventWithoutResults();
        }
        else
        {
            string confirmText = "OK";
            string gainText = GetGainString(actionEvent.Gain, actionEvent.RiskTier);
            ui.eventDialog.ShowResults(choice.ResultText, gainText, confirmText);
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

    private string GetGainString(EventGain gain, int tier)
    {
        if (gain.Money > 0)
        {
            return gain.Money + " Junk";
        }
        else if (gain.RandomItem)
        {
            return itemGain.GetName();
        }
        else
        {
            return "nothing";
        }
    }
}
