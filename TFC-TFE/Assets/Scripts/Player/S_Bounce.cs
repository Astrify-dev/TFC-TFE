using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Bounce : MonoBehaviour{
    [Header("Param�tres de rebond")]
    [SerializeField] private LayerMask _bounceLayer;
    public LayerMask BounceLayer => _bounceLayer;
}