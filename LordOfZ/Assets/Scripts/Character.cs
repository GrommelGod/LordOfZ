using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int lifePoints;
    [SerializeField] protected int maxLifePoints;
    [SerializeField] protected float speed;
    [SerializeField] protected int damageDealt;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackDistance;
    public bool isDying;

    public event Action<float> OnHealthPctChanged = delegate { };

    private void OnEnable()
    {
        lifePoints = maxLifePoints;
    }

    public void Hit(int hitDamage)
    {
        lifePoints -= hitDamage;
        if (lifePoints < 0)
        {
            lifePoints = 0;
        }

        float currentHealthPct = (float)lifePoints / (float)maxLifePoints;

        OnHealthPctChanged(currentHealthPct);
    }
}
