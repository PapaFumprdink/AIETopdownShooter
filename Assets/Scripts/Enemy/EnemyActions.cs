using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(IDamagable))]
public class EnemyActions : MonoBehaviour, IMovementProvider, IWeaponInputProvider
{
    public Vector2 MovementDirection { get; set; }
    public bool WantsToFire { get; set; }
    public bool UseCursor => false;
    public Vector2 FaceDirection { get; set; }

    public event Action FireEvent;
    public event Action ReloadEvent;

    private IDamagable m_Damagable;

    public Targetable LastTarget { get; private set; }

    private void Awake()
    {
        m_Damagable = GetComponent<IDamagable>();
    }

    void Start()
    {
        m_Damagable.DamageEvent += OnDamage;
    }

    private void OnDamage(GameObject damager, int damage, Vector3 point, Vector3 direction)
    {
        if (damage >= 0)
        {
            if (damager)
            {
                // If the object that damaged us is targetable, set that as the perfered target.
                // Adds a retaliation behaviour.
                var target = damager.GetComponent<Targetable>();
                LastTarget = target;
            }
        }
    }

    public bool GetBestTarget (TargetType[] possibleTargetTypes, float range, out Targetable bestTarget)
    {
        // Checks if the last target is still valid, if so use it.
        if (LastTarget)
        {
            if ((LastTarget.transform.position - transform.position).sqrMagnitude < range * range)
            {
                bestTarget = LastTarget;
                return true;
            }
        }

        // Otherwise, loop through each layer and find a possible target.
        foreach (TargetType targetType in possibleTargetTypes)
        {
            if (Targetable.TryFindTarget(targetType, transform.position, range, out Targetable target))
            {
                bestTarget = target;
                return true;
            }
        }

        bestTarget = null;
        return false;
    }
}
