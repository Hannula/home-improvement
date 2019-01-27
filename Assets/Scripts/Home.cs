using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public bool PlayerHome;
    public GameObject FloorPrefab;
    public List<Floor> Floors;
    public SpriteRenderer RoofSprite;

    public HomeData homeData;

    private float deathSoundDelay = 1f;
    private bool dead;
    private bool exiting;
    private AudioSource deathSound;

    public void Awake()
    {
        if (PlayerHome)
        {
            homeData = GameManager.Instance.PlayerHome;
            homeData.Prune();
        }
        else
        {
            homeData = GameManager.Instance.EnemyHome;
        }
    }

    public void Init(HomeData homeData)
    {
        this.homeData = homeData;
    }

    public void Start()
    {
        if(homeData != null)
        {
            Floor prevFloor = null;
            for (int i = 0; i < homeData.Floors.Count; i++)
            {
                FloorData fd = homeData.Floors[i];
                Floor floor = GameObject.Instantiate(FloorPrefab, transform).GetComponent<Floor>();
                floor.FloorData = fd;
                floor.Index = i;
                floor.FloorLower = prevFloor;
                floor.MyHome = this;
                Floors.Add(floor);

                if (prevFloor != null)
                {
                    prevFloor.FloorUpper = floor;
                }

                prevFloor = floor;

            }
            prevFloor.RoofSprite = RoofSprite;
        }
    }

    private void Update()
    {
        if (!exiting && Floors.Count == 0)
        {
            Die();
        }

        if (exiting && !deathSound.isPlaying &&
            GameManager.Instance.gameRunning)
        {
            LeaveBattle();
        }
    }

    private void Die()
    {
        if (!dead)
        {
            MusicPlayer.Instance.StartFadeOut();
        }

        dead = true;

        if (deathSoundDelay > 0f)
        {
            deathSoundDelay -= GameManager.Instance.DeltaTime;
            return;
        }

        exiting = true;
        if (PlayerHome)
        {
            deathSound = SFXPlayer.Instance.Play(Sound.Asdfghj);
        }
        else
        {
            deathSound = SFXPlayer.Instance.Play(Sound.GetRekt2);
        }
    }

    private void LeaveBattle()
    {
        if (PlayerHome)
        {
            GameManager.Instance.EndGame(false);
        }
        else
        {
            GameManager.Instance.WinBattle();
        }
    }

    public void UpdateHome()
    {
        if (homeData != null)
        {
            Floor prevFloor = null;
            foreach(Floor f in Floors)
            {
                Destroy(f.gameObject);
            }
            Floors.Clear();

            for (int i = 0; i < homeData.Floors.Count; i++)
            {
                FloorData fd = homeData.Floors[i];
                Floor floor = GameObject.Instantiate(FloorPrefab, transform).GetComponent<Floor>();
                floor.FloorData = fd;
                floor.Index = i;
                floor.FloorLower = prevFloor;
                Floors.Add(floor);

                if (prevFloor != null)
                {
                    prevFloor.FloorUpper = floor;
                }

                prevFloor = floor;

            }
            prevFloor.RoofSprite = RoofSprite;
        }
    }
}
