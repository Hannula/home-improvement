using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public SpriteRenderer BackgroundImage;
    public List<Transform> FurnitureSlots;
    public FloorData Data;

    void Start()
    {
        BackgroundImage.sprite = Data.Type.BackgroundImage;
    }

}
