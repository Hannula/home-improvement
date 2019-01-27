using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillHandler : MonoBehaviour
{
    public enum SkillType
    {
        Barricade = 0,
        Ventilate = 1,
        Flee = 2
    }

    public BattleSkill[] battleSkills;

    private float fleeSoundDelay = 1f;
    private bool fleeing;
    private AudioSource fleeSound;

    // Start is called before the first frame update
    public void Init()
    {
        battleSkills = FindObjectsOfType<BattleSkill>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!fleeing)
        {
            foreach (BattleSkill skill in battleSkills)
            {
                skill.UpdateSkill();
            }
        }
        else if (fleeSound != null && !fleeSound.isPlaying)
        {
            ExitBattle();
        }
    }

    public void ActivateSkill(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Barricade:
            {
                ActivateBarricadeSkill();
                break;
            }
            case SkillType.Ventilate:
            {
                ActivateVentilateSkill();
                break;
            }
            case SkillType.Flee:
            {
                ActivateFleeSkill();
                break;
            }
        }
    }

    private void ActivateBarricadeSkill()
    {
        // TODO
        Debug.Log("Player used Barricade! It's super effective!");
    }

    private void ActivateVentilateSkill()
    {
        // TODO
        Debug.Log("Player used Ventilate! It's super effective!");
    }

    private void ActivateFleeSkill()
    {
        fleeing = true;
        fleeSound = SFXPlayer.Instance.Play(Sound.Mamma);
    }

    private void ExitBattle()
    {
        GameManager.Instance.LoadMapScene();
    }
}
