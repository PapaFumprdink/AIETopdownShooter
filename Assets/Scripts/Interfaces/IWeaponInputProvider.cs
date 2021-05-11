using System;
using UnityEngine;

public interface IWeaponInputProvider
{
    event Action<bool> FireEvent;
    event Action ReloadEvent;
    bool UseCursor { get; } 
}
