using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object for the player stats holds all relevant values for the player plug into the wall jump and player movement scripts
/// </summary>
[CreateAssetMenu(menuName = "CreateNewPlayerStats", fileName = "NewPlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Drag settings")]
    public float groundDrag; // drag for ground movement prevents slippery sliding
    public float airDrag; // drag when in the air to prevent slow falling

    [Header("Movement Stats")]
    public float speed; // grounded movement speed
    public float movementMultiplier; // overcome drag
    public float airMultiplier; // account for less drag in the air
    public float jumpPower; // how much force is applied when the player jumps
    public float maxAcceleration; // acceleration

    [Header("Wall Jump Stats")]
    public float wallDistance; // how far the player needs to be from the wall
    public float minimumJumpHeight;
    public float wallRunGrav;
    public float wallJumpForce;
}
