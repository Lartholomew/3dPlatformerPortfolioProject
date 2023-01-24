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
    [HideInInspector] public float runTimeSpeed; // speed that should be adjusted when changing the value of the players speed
    public float movementMultiplier; // overcome drag
    public float airMultiplier; // account for less drag in the air
    public float jumpPower; // how much force is applied when the player jumps
    public float maxAcceleration; // acceleration

    [Header("Wall Jump Stats")]
    public float wallDistance; // how far the player needs to be from the wall
    public float minimumJumpHeight; // how far from the ground the player needs to be to wall jump
    public float wallRunGrav; // how much the player is pulled down while wall running
    [Range(300f, 500f)]
    public float wallJumpForce; // how much force is applied to the player when wall jumping (Should be from 300-500)

    private void OnEnable()
    {
        runTimeSpeed = speed;
    }
}
