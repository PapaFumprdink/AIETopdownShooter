using UnityEngine;
using UnityEngine.UIElements;

public class SentinelBehavior : EnemyBrain
{
    const float Tau = 2 * Mathf.PI;

    [SerializeField] private float m_TargetAngle;
    [SerializeField] private float m_SweepAngle;
    [SerializeField] private float m_SweepTime;
    [SerializeField] private float m_SightDot;

    [Space]
    [SerializeField] private float m_ReactionTime;
    [SerializeField] private float m_AttentionSpan;
    [SerializeField] private float m_AlertPoint;

    [Space]
    [SerializeField] private SpriteRenderer m_AlertRenderer;
    [SerializeField] private SpriteRenderer m_AlertBackgroundRenderer;
    [SerializeField] private Sprite m_AlertIcon;
    [SerializeField] private Sprite m_ActiveIcon;
    [SerializeField] private Sprite m_AlertBackgroundIcon;
    [SerializeField] private Sprite m_ActiveBackgroundIcon;

    private float m_DetectionPercent;

    public override void Think()
    {
        Targetable target = GetTarget();

        bool isTargetViable = false;
        if (target)
        {
            if (Vector3.Dot((target.transform.position - transform.position).normalized, EnemyActions.FaceDirection) > m_SightDot)
            {
                isTargetViable = true;
            }
        }

        if (isTargetViable)
        {
            FaceTowards(target);
            WantsToFire = m_DetectionPercent > 0.95f;

            m_DetectionPercent = Mathf.Clamp01(m_DetectionPercent + Time.deltaTime / m_ReactionTime);
        }
        else
        {
            if (m_DetectionPercent < 0.05f)
            {
                Sweep();
            }
            WantsToFire = false;

            m_DetectionPercent = Mathf.Clamp01(m_DetectionPercent - Time.deltaTime / m_AttentionSpan);
        }

        m_AlertRenderer.sprite = m_DetectionPercent > m_AlertPoint ? m_AlertIcon : m_ActiveIcon;
        m_AlertRenderer.size = new Vector2(1f, m_DetectionPercent > m_AlertPoint ? (m_DetectionPercent - m_AlertPoint) / (1f - m_AlertPoint) : m_DetectionPercent / m_AlertPoint);

        m_AlertBackgroundRenderer.sprite = m_DetectionPercent > m_AlertPoint ? m_AlertBackgroundIcon : m_ActiveBackgroundIcon;
    }

    private void Sweep ()
    {
        // Oscillate around the target angle.
        float sweepAngle = m_TargetAngle + Mathf.Sin(Time.time * Tau / m_SweepTime) * m_SweepAngle;
        FaceTowardsAngle(sweepAngle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle));

        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle + m_SweepAngle));
        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle - m_SweepAngle));

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, Mathf.Asin(Mathf.Clamp(1f - m_SightDot, -1f, 1f)) * Mathf.Rad2Deg) * transform.right * m_TargetMaxRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -Mathf.Asin(Mathf.Clamp(1f - m_SightDot, -1f, 1f)) * Mathf.Rad2Deg) * transform.right * m_TargetMaxRange);

        Gizmos.color = Color.white;
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -m_ObstacleAvoidanceMaxAngle) * Vector3.right * m_ObstacleAvoidanceRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, m_ObstacleAvoidanceMaxAngle) * Vector3.right * m_ObstacleAvoidanceRange);

        Gizmos.color = Color.white;
    }
}
