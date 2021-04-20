using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private const float SkinWidth = 0.1f;

    [SerializeField] private float m_Velocity;
    [SerializeField] private float m_Damage;
    [SerializeField] private AnimationCurve m_VelocityDamageScale;
    [SerializeField] private GameObject m_HitFX;

    private Rigidbody2D m_Rigidbody;

    public GameObject Shooter { get; set; }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_Rigidbody.velocity = transform.right * m_Velocity;
    }

    private void Start()
    {
        m_HitFX.SetActive(false);
    }

    private void FixedUpdate()
    {
        float speed = m_Rigidbody.velocity.magnitude;

        var hit = Physics2D.Raycast(m_Rigidbody.position, m_Rigidbody.velocity, speed * Time.deltaTime + SkinWidth);
        if (hit)
        {
            var health = hit.transform.GetComponent<IDamagable>();
            if (health != null)
            {
                health.Damage(Shooter, m_Damage * m_VelocityDamageScale.Evaluate(speed / m_Velocity), hit.point, m_Rigidbody.velocity.normalized);
            }

            m_HitFX.transform.parent = null;
            m_HitFX.SetActive(true);
            m_HitFX.transform.position = hit.point;
            m_HitFX.transform.rotation = Quaternion.Euler(hit.normal);

            Destroy(gameObject);
        }
    }
}
