using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullyBehaviour : EnemyBrain
{
    [Space]
    [SerializeField] private float m_DamageDeltStunTime;
    [SerializeField] private float m_DamageDeltKnockback;

    private ContactDamage m_ContactDamage;

    protected override void Awake()
    {
        base.Awake();

        m_ContactDamage = GetComponent<ContactDamage>();

        if (m_ContactDamage)
        {
            m_ContactDamage.DamageDeltEvent += (target) =>
            {
                // If we did damage to a collided object, knock ourselves back.
                AttachedRigidbody.velocity += (Vector2)(transform.position - target.transform.position).normalized * m_DamageDeltKnockback;
            };
        }
    }

    public override void Think()
    {
        // Store the best target.
        Targetable target = GetTarget();

        if (target)
        {
            // If we have a target and not stunned, chase it down
            if (Time.time > m_ContactDamage.LastDamageDeltTime + m_DamageDeltStunTime)
            {
                MoveTowards(target);
                FaceTowards(target);
            }
            else
            {
                FaceTowards(target);
            }
        }
        else
        {
            ClearMovement();
        }
    }
}
