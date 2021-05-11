using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UI/Custom Cursor")]
public class CustomCursor : ScriptableObject
{
    [SerializeField] private Sprite m_CursorIcon;
    [SerializeField] private float m_FillPercent;

    public Sprite CursorIcon 
    {
        get => m_CursorIcon; 
        set => m_CursorIcon = value;
    }

    public float FillPercent
    {
        get => m_FillPercent;
        set => m_FillPercent = value;
    }
}
