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
        // Disable the hit fx in case I forgot in editor, its more than likley.
        m_HitFX.SetActive(false);
    }

    private void FixedUpdate()
    {
        // store the speed, as a magnitude call is expensive.
        float speed = m_Rigidbody.velocity.magnitude;

        // perform the raycast operation for collision.
        var hit = Physics2D.Raycast(m_Rigidbody.position, m_Rigidbody.velocity, speed * Time.deltaTime + SkinWidth);
        if (hit)
        {
            // if the hit object has a health component, damage it.
            var health = hit.transform.GetComponent<IDamagable>();
            if (health != null)
            {
                health.Damage(Shooter, m_Damage, hit.point, m_Rigidbody.velocity.normalized);
            }

            // Place and show the hit fx.
            m_HitFX.transform.parent = null;
            m_HitFX.SetActive(true);
            m_HitFX.transform.position = hit.point;
            m_HitFX.transform.rotation = Quaternion.Euler(hit.normal);

            Destroy(gameObject);
        }
    }
}
