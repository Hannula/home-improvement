using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkill : MonoBehaviour
{
    public bool onCooldown;
    public float cooldownTime = 1f;
    public BattleSkillHandler.SkillType skillType;

    private Button button;
    private float elapsedCooldown;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void UpdateSkill()
    {
        if (onCooldown)
        {
            elapsedCooldown += GameManager.Instance.DeltaTime;
            if (elapsedCooldown >= cooldownTime)
            {
                onCooldown = false;
                button.interactable = true;
            }
        }
    }

    public void Activate()
    {
        elapsedCooldown = 0f;
        onCooldown = true;
        button.interactable = false;
        GameManager.Instance.battleSkillHandler.ActivateSkill(skillType);

        // TODO: The skill does something.
    }

    public void ResetSkill()
    {
        elapsedCooldown = 0f;
        onCooldown = false;
        button.interactable = true;
    }
}
