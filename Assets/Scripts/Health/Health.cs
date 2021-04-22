using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour, IDamagable
{
    public event IDamagable.DamageDelegate DamageEvent;
    public event IDamagable.DamageDelegate DeathEvent;
    public event Action HealthChangeEvent;

    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_CurrentHealth;

    [Space]
    [SerializeField] private float m_RegenerationRate;
    [SerializeField] private float m_RegenerationDelay;
    [SerializeField][Range(0f, 1f)] private float m_RegenerationPercentCap;

    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;
    public float NormalizedHealth => m_CurrentHealth / m_MaxHealth;
    public GameObject LastDamager { get; private set; }
    public float LastDamageTime { get; private set; }

    private void Update()
    {
        // Only regen if enough time has passed since the last damage time and the percentage of our current health is below the regen cap.
        if (Time.time > LastDamageTime + m_RegenerationDelay && NormalizedHealth < m_RegenerationPercentCap)
        {
            // If the regen increase will go over the regeneration cap, clamp it at the cap
            if ((m_CurrentHealth + m_RegenerationRate * Time.deltaTime) / m_MaxHealth > m_RegenerationPercentCap)
            {
                m_CurrentHealth = m_RegenerationPercentCap * m_MaxHealth;
                HealthChangeEvent?.Invoke();
            }
            // Otherwise, apply the health regeneration
            else
            {
                m_CurrentHealth += m_RegenerationRate * Time.deltaTime;
                HealthChangeEvent?.Invoke();
            }
        }
    }

    public void Damage(GameObject damager, float damage, Vector3 point, Vector3 direction)
    {
        m_CurrentHealth -= damage;
        LastDamager = damager;
        LastDamageTime = Time.time;

        HealthChangeEvent?.Invoke();
        DamageEvent?.Invoke(damager, damage, point, direction);

        // If we have run out of health, die.
        if (m_CurrentHealth <= 0)
        {
            Kill(damager, damage, point, direction);
        }
    }

    public void Kill(GameObject killer, float damage, Vector3 point, Vector3 direction)
    {
        DeathEvent?.Invoke(killer, damage, point, direction);

        // Disable object, as not to break any references to it.
        gameObject.SetActive(false);
    }
}
