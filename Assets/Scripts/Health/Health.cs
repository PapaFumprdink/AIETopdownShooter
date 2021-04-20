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
        if (Time.time > LastDamageTime + m_RegenerationDelay && NormalizedHealth < m_RegenerationPercentCap)
        {
            if ((m_CurrentHealth + m_RegenerationRate * Time.deltaTime) / m_MaxHealth > m_RegenerationPercentCap)
            {
                m_CurrentHealth = m_RegenerationPercentCap * m_MaxHealth;
                HealthChangeEvent?.Invoke();
            }
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

        if (m_CurrentHealth <= 0)
        {
            Kill(damager, damage, point, direction);
        }
    }

    public void Kill(GameObject killer, float damage, Vector3 point, Vector3 direction)
    {
        DeathEvent?.Invoke(killer, damage, point, direction);

        gameObject.SetActive(false);
    }
}
