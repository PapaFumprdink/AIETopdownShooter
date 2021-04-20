using System;
using UnityEngine;

public interface IMovementProvider
{
    event Action JumpEvent;
    Vector2 MovementDirection { get; }
}
