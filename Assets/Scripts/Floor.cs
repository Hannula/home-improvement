using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : SelectableItem
{

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


    }

    public void DestroyFloor()
    {

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
