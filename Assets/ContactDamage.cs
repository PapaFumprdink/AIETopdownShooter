using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public event System.Action<IDamagable> DamageDeltEvent;

    [SerializeField] private float m_Damage;
    [SerializeField] private GameObject m_Damager;

    public float LastDamageDeltTime { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.transform.GetComponent<IDamagable>();
        if (health != null)
        {
            LastDamageDeltTime = Time.time;
            DamageDeltEvent?.Invoke(health);
            health.Damage(m_Damager, m_Damage, transform.position, (collision.transform.position - transform.position).normalized);
        }
    }
}
