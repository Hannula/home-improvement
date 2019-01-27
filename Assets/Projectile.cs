using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 TargetPosition;
    public int TargetFloorIndex;
    public Home TargetHome;
    public List<StatBonus> Damage;

    public ParticleSystem PsysPhysical;
    public ParticleSystem PsysFire;
    public ParticleSystem PsysWater;
    public ParticleSystem PsysNoise;
    public ParticleSystem PsysStench;

    public float LerpSpeed = 5f;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, GameManager.Instance.DeltaTime * LerpSpeed);
        if (Vector3.Distance(transform.position, TargetPosition) < 0.75f)
        {
            if (TargetFloorIndex >= 0 && TargetHome && TargetHome.Floors.Count > TargetFloorIndex && TargetHome.Floors[TargetFloorIndex])
            {
                TargetHome.Floors[TargetFloorIndex].FloorData.DealDamage(Damage);
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (TargetHome)
        {
            if (TargetHome.Floors.Count > TargetFloorIndex && TargetFloorIndex >= 0)
            {
                TargetPosition = TargetHome.Floors[TargetFloorIndex].transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-1, 1));
            }
            else
            {
                TargetPosition = new Vector3(TargetHome.transform.position.x + TargetHome.transform.position.x - transform.position.x, transform.position.y + Random.Range(-2, 2));
            }
        }
        else
        {
            new Vector3( Random.Range(-10, 10), Random.Range(-10, 10));
        }

        if (Damage.Count > 0)
        {
            foreach(StatBonus stat in Damage)
            {
                DamageTypes type = stat.DamageType;

                switch (type)
                {
                    case DamageTypes.Physical: {PsysPhysical.gameObject.SetActive(true); LerpSpeed *= 1.5f; } break;
                    case DamageTypes.Fire: { PsysFire.gameObject.SetActive(true); LerpSpeed *= 0.9f; }
                 break;
                    case DamageTypes.Water: {PsysWater.gameObject.SetActive(true); LerpSpeed *= 1f; } break;
                    case DamageTypes.Stench: {PsysStench.gameObject.SetActive(true); LerpSpeed *= 0.5f; } break;
                    case DamageTypes.Noise: {PsysNoise.gameObject.SetActive(true); LerpSpeed *= 0.8f; } break;
                }
            }
        }
    }
}
