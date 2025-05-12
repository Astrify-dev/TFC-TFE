using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Custom/Input Settings")]
public class S_Commandes : ScriptableObject{
    [Header("Commandes de déplacement")]
    public KeyCode moveForward = KeyCode.Q;
    public KeyCode moveBackward = KeyCode.D;

    [Header("Commandes de saut")]
    public KeyCode jump = KeyCode.Space;

    [Header("Commandes spéciales")]
    public KeyCode dash = KeyCode.LeftShift;
}