using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContactDamage))]
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

        m_ContactDamage.DamageDeltEvent += (target) => AttachedRigidbody.velocity += (Vector2)(transform.position - target.transform.position).normalized * m_DamageDeltKnockback;
    }

    public override void Think()
    {
        var target = GetTarget();

        if (target)
        {
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
