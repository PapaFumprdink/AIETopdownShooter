using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public static Dictionary<TargetType, List<Targetable>> TargetInstances = new Dictionary<TargetType, List<Targetable>>();

    [SerializeField] private TargetType m_TargetType;

    private void Awake()
    {
        // Instance a list if there isnt one for this objects target type.
        if (!TargetInstances.ContainsKey(m_TargetType))
        {
            TargetInstances.Add(m_TargetType, new List<Targetable>());
        }

        // Add this object to its target type list.
        TargetInstances[m_TargetType].Add(this);
    }

    public static bool TryFindTarget (TargetType type, Vector3 position, float range, out Targetable bestTarget)
    {
        bestTarget = FindTarget(type, position, range);
        return bestTarget;
    }

    public static Targetable FindTarget (TargetType type, Vector3 position, float range)
    {
        Targetable bestTarget = null;

        if (TargetInstances.ContainsKey(type))
        {
            // Loop through each target in the layer to see which is best.
            foreach (Targetable target in TargetInstances[type])
            {
                if (target != null)
                {
                    var vectorToTarget = (target.transform.position - position);

                    // Check that target is active and it is within range.
                    // Vector2.sqrMagnitude skips a square root call, used for performance.
                    if (target.isActiveAndEnabled && vectorToTarget.sqrMagnitude < range * range)
                    {
                        if (!bestTarget)
                        {
                            bestTarget = target;
                        }
                        else
                        {
                            var vectorToBestTarget = (bestTarget.transform.position - position);

                            // Choose this target if it is closest.
                            // Vector2.sqrMagnitude skips a square root call, used for performance.
                            if (vectorToTarget.sqrMagnitude < vectorToBestTarget.magnitude)
                            {
                                bestTarget = target;
                            }
                        }
                    }
                }
            }
        }

        return bestTarget;
    }
}
