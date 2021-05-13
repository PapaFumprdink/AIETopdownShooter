using UnityEngine;
using UnityEngine.UIElements;

public class SentinelBehavior : EnemyBrain
{
    const float Tau = 2 * Mathf.PI;

    [SerializeField] private float m_TargetAngle;
    [SerializeField] private float m_SweepAngle;
    [SerializeField] private float m_SweepTime;

    public override void Think()
    {
        if (Target)
        {
            FaceTowards(Target);
            WantsToFire = DetectionPercent > 0.95f;
        }
        else
        {
            if (DetectionPercent < 0.05f)
            {
                Sweep();
            }
            WantsToFire = false;
        }
    }

    private void Sweep ()
    {
        // Oscillate around the target angle.
        float sweepAngle = m_TargetAngle + Mathf.Sin(Time.time * Tau / m_SweepTime) * m_SweepAngle;
        FaceTowardsAngle(sweepAngle);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle));

        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle + m_SweepAngle));
        Gizmos.DrawRay(transform.position, Util.VectorFromAngle(m_TargetAngle - m_SweepAngle));

        Gizmos.color = Color.white;
    }
}
