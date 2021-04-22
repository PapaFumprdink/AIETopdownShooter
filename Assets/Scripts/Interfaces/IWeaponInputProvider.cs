using System;
using UnityEngine;

public interface IWeaponInputProvider
{
    event Action FireEvent;
    event Action ReloadEvent;
    bool WantsToFire { get; }
    bool UseCursor { get;  } 
    Vector2 Direction { get; }
}
