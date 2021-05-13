using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public class EnemyActions : MonoBehaviour, IMovementProvider, IWeaponInputProvider
{
    public Vector2 MovementDirection { get; set; }
    public bool WantsToFire { get; set; }
    public bool UseCursor => false;
    public Vector2 FaceDirection { get; set; }

    public event Action<bool> FireEvent;
    public event Action ReloadEvent;

    [SerializeField] private float m_FireFrequency;

    private IDamagable m_Damagable;
    private float m_LastFireTime;

    public Targetable LastTarget { get; private set; }

    private void Awake()
    {
        m_Damagable = GetComponent<IDamagable>();
    }

    void Start()
    {
        if (m_Damagable != null)
        {
            m_Damagable.DamageEvent += OnDamage;
        }
    }

    private void Update()
    {
        // If wants to fire, fire
        if (WantsToFire)
        {
            // If enough time has passed, call the event with the down field set to true
            // In case the weapon is single fire.
            if (Time.time + 1f / m_FireFrequency > m_LastFireTime)
            {
                FireEvent?.Invoke(true);
                m_LastFireTime = Time.time;
            }
            else
            {
                FireEvent?.Invoke(false);
            }
        }
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

    public bool GetBestTarget (TargetType[] possibleTargetTypes, float range, float sightDot, bool needsLineOfSight, out Targetable bestTarget)
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
            if (Targetable.TryFindTarget(targetType, transform.position, FaceDirection, range, sightDot, needsLineOfSight, out Targetable target))
            {
                bestTarget = target;
                return true;
            }
        }

        bestTarget = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, FaceDirection * 10f);
    }
}
