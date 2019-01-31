using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkill : MonoBehaviour
{
    public bool onCooldown;
    public float cooldownTime = 1f;
    public BattleSkillHandler.SkillType skillType;
    public ProgressBar cooldownBar;

    private Button button;
    private float elapsedCooldown;

    private void Start()
    {
        button = GetComponent<Button>();
        PutSkillOnCooldown();
    }

    public void UpdateSkill()
    {
        if (onCooldown && GameManager.Instance.ActiveGame)
        {
            elapsedCooldown += GameManager.Instance.DeltaTime;
            if (elapsedCooldown >= cooldownTime)
            {
                onCooldown = false;
                button.interactable = true;
            }

            cooldownBar.SetProgress(elapsedCooldown / cooldownTime);
        }
    }

    public void Activate()
    {
        PutSkillOnCooldown();
        GameManager.Instance.battleSkillHandler.ActivateSkill(skillType);
    }

    public void SetButtonInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    public void PutSkillOnCooldown()
    {
        if (cooldownTime > 0f)
        {
            elapsedCooldown = 0f;
            onCooldown = true;
            button.interactable = false;
            cooldownBar.SetProgress(0f);
        }
    }

    public void ResetSkill()
    {
        elapsedCooldown = 0f;
        onCooldown = false;
        button.interactable = true;
    }
}
