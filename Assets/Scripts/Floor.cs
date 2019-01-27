using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : SelectableItem
{

    public GameObject ProjectilePrefab;
    public TextMesh HealthTex;
    public SpriteRenderer RoofSprite;
    public bool IsGroundFloor { get { return Index == 0; } }
    public int Index;
    public Floor FloorLower;
    public Floor FloorUpper;
    public SpriteRenderer BackgroundImage;
    public SpriteRenderer WallImage;
    public SpriteRenderer BorderImage;
    public UpgradeSlot[] UpgradeSlots;
    public FloorData FloorData;
    public float Height;
    public float FallSpeed = 4f;
    public float TargetY;
    public ProgressBar ReloadBar;
    private float Cooldown;
    public Home MyHome;

    public Color SelectedColor;
    public Color EmptyColor;
    public Color HoverColor;

    public bool DestroyThis;

    void Start()
    {
        BackgroundImage.sprite = FloorData.Type.BackgroundImage;
        WallImage.sprite = FloorData.WallType.BackgroundImage;
        for(int i = 0; i < FloorData.HomeUpgrades.Length; i++)
        {
            UpgradeSlots[i].HomeUpgrade = FloorData.HomeUpgrades[i];
            UpgradeSlots[i].ParentFloor = this;
            UpgradeSlots[i].ParentFloorSlotIndex = i;
        }
    }

    public void Update()
    {
        Cooldown += GameManager.Instance.DeltaTime;
        ReloadBar.SetProgress(Cooldown / FloorData.MaxCooldown);
        if ( Cooldown > FloorData.MaxCooldown)
        {
            Fire();
        }

        if (IsGroundFloor)
        {
            TargetY = 0f;
        }
        else
        {
            TargetY = FloorLower.TargetY + FloorLower.Height;
        }
        float temporalMovementSpeed = GameManager.Instance.DeltaTime * FallSpeed;
        if (Mathf.Abs(TargetY - transform.localPosition.y) <= temporalMovementSpeed)
        {

            transform.localPosition = new Vector3(transform.localPosition.x, TargetY);
        }
        else
        {
            transform.localPosition += Vector3.up * temporalMovementSpeed * Mathf.Sign(TargetY - transform.localPosition.y);
        }

        if (RoofSprite)
        {
            RoofSprite.transform.localPosition = transform.localPosition + Vector3.up * (Height + 0.25f);
        }
        if (DestroyThis)
        {
            DestroyFloor();
        }
        BackgroundImage.sortingOrder = Index;
        WallImage.sortingOrder = Index;



        if (Selected)
        {
            BorderImage.color = SelectedColor;
        }
        else if (HoveringOver)
        {
            BorderImage.color = HoverColor;
        }
        else
        {
            BorderImage.color = EmptyColor;
        }

        if (FloorData.Health <= 0)
        {
            DestroyFloor();
        }
        HealthTex.text = FloorData.Health.ToString();


    }

    public void Fire()
    {
        if (FloorData.MaxCooldown > 0 && GameManager.Instance.State == GameManager.GameState.Battle)
        {
            foreach (UpgradeSlot slot in UpgradeSlots)
            {
                HomeUpgrade item = slot.HomeUpgrade;
                if (item != null && item.UpgradeData != null && item.UpgradeData.IsWeapon && item.UpgradeData.BaseDamage != null)
                {
                    Home targetHome = FindObjectsOfType<Home>().Where(x => x != MyHome).First();
                    int baseTargetIndex = Index;
                    switch(item.UpgradeData.TargetingModule.TargetingType)
                    {
                        case TargetTypes.Bottom: baseTargetIndex = 0;
                            break;
                        case TargetTypes.Top: baseTargetIndex = targetHome.Floors.Count - 1;
                            break;
                    }
                    List<StatBonus> baseDamage = item.UpgradeData.BaseDamage;
                    foreach(DamageTypes type in  FloorData.DamageBonuses.Keys)
                    {
                        for(int i = 0; i < baseDamage.Count; i++)
                        {
                            if (baseDamage[i].DamageType == type)
                            {
                                baseDamage[i] = new StatBonus(baseDamage[i].DamageType, baseDamage[i].Bonus + FloorData.DamageBonuses[type]);
                                break;
                            }
                            baseDamage.Add(new StatBonus(type, FloorData.DamageBonuses[type]));
                        }
                    }
                    foreach (int targetIndex in item.UpgradeData.TargetingModule.Targets)
                    {
                        int projectileTargetIndex = targetIndex + baseTargetIndex;

                        Projectile proj = Instantiate(ProjectilePrefab).GetComponent<Projectile>();
                        proj.transform.position = slot.transform.position;
                        proj.TargetFloorIndex = projectileTargetIndex;
                        proj.TargetHome = targetHome;
                        proj.Damage = baseDamage;
                    }
                }
            }
        }
        Cooldown -= FloorData.MaxCooldown;

    }

    public void DestroyFloor()
    {
        MyHome.Floors.Remove(this);
        if (FloorUpper)
        {
            FloorUpper.FloorLower = FloorLower;
            FloorUpper.ReduceIndex();
        }
        if (FloorLower)
        {
            FloorLower.FloorUpper = FloorUpper;
            FloorLower.RoofSprite = RoofSprite;
        }
        else
        {
            if (RoofSprite)
            {
                RoofSprite.transform.localPosition -= Vector3.up * (Height);
            }
        }
        Destroy(gameObject);
    }

    public void ReduceIndex()
    {
        Index -= 1;
        if (FloorUpper)
        {
            FloorUpper.ReduceIndex();
        }
    }

}
