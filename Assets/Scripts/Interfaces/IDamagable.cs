using System;
using UnityEngine;

public interface IDamagable
{
    delegate void DamageDelegate(GameObject damager, float damage, Vector3 point, Vector3 direction);

    event Action HealthChangeEvent;
    event DamageDelegate DamageEvent;
    event DamageDelegate DeathEvent;

    float CurrentHealth { get; }
    float MaxHealth { get; }
    float NormalizedHealth { get; }

    GameObject gameObject { get; }
    Transform transform { get; }

    void Damage(GameObject damager, float damage, Vector3 point, Vector3 direction);
    void Kill(GameObject killer, float damage, Vector3 point, Vector3 direction);
}
