using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DestructableProp : MonoBehaviour, IDamagable
{
    [SerializeField] private Sprite m_DestroyedSprite;

    private SpriteRenderer m_SpriteRenderer;
    private bool m_Destroyed = false;

    public float CurrentHealth => m_Destroyed ? 0f : 1f;

    public float MaxHealth => 1f;

    public float NormalizedHealth => CurrentHealth;

    public event Action HealthChangeEvent;
    public event IDamagable.DamageDelegate DamageEvent;
    public event IDamagable.DamageDelegate DeathEvent;

    private void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Damage(GameObject damager, int damage, Vector3 point, Vector3 direction)
    {
        Kill(damager, damage, point, direction);
    }

    public void Kill(GameObject killer, int damage, Vector3 point, Vector3 direction)
    {
        m_Destroyed = true;

        m_SpriteRenderer.sprite = m_DestroyedSprite;

        HealthChangeEvent?.Invoke();
        DamageEvent?.Invoke(killer, damage, point, direction);
        DeathEvent?.Invoke(killer, damage, point, direction);
    }
}
