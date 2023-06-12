using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FD.HealthSystem
{
    public class Health : MonoBehaviour, IHittable
    {
        [field: SerializeField]
        public int currentHealth { get; private set; }
        public UnityEvent OnDeath, OnHit;
        public void GetHit(int damageValue, GameObject sender)
        {
            currentHealth -= damageValue;

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
            else
            {
                OnHit?.Invoke();
            }
        }

       

        public void InitializeHealth(int startingHealth)
        {
            if (startingHealth < 0)
                startingHealth = 0;
            currentHealth = startingHealth;
        }
    }
}