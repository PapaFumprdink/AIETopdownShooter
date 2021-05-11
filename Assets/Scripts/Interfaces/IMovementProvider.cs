using System;
using UnityEngine;

public interface IMovementProvider
{
    Vector2 MovementDirection { get; }
    Vector2 FaceDirection { get; }
}
