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

    [SerializeField] private int m_MaxHealth;
    [SerializeField] private int m_CurrentHealth;

    [Space]
    [SerializeField] private GameObject m_DeathFX;

    private float m_NextRegenTime;

    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;
    public float NormalizedHealth => m_CurrentHealth / m_MaxHealth;
    public GameObject LastDamager { get; private set; }
    public float LastDamageTime { get; private set; }

    private void OnEnable()
    {
        if (m_DeathFX)
        {
            // If, for some reason revived, reparent the fx.
            m_DeathFX.transform.parent = transform;
            m_DeathFX.transform.localPosition = Vector3.zero;

            // Disable death fx If I forgot to in editor.
            if (m_DeathFX) m_DeathFX.SetActive(false);
        }
    }

    public void Damage(GameObject damager, int damage, Vector3 point, Vector3 direction)
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

    public void Kill(GameObject killer, int damage, Vector3 point, Vector3 direction)
    {
        DeathEvent?.Invoke(killer, damage, point, direction);

        if (m_DeathFX)
        {
            // Detach so it does not disable with this object.
            m_DeathFX.transform.parent = null;
            m_DeathFX.transform.right = direction;
            // Enable death fx.
            m_DeathFX.SetActive(true);
        }

        // Disable object instead of destroy, as not to break any references to it.
        gameObject.SetActive(false);
    }
}
