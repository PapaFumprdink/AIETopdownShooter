using System;
using UnityEngine;

public interface IWeaponInputProvider
{
    event Action<int> CycleWeaponEvent;
    event Action FireEvent;
    event Action ReloadEvent;
    bool WantsToFire { get; }
    bool IsAimingDownSights { get; }
    bool UseCursor { get;  } 
}
