using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Cooldown
{
    [SerializeField] private float cooldownTime; // Time in seconds for the cooldown
    private float nextActionTime; // The time when the next action can be performed

    public bool isOnCooldown => Time.time < nextActionTime; // Check if the cooldown is active
    public void StartCooldown() => nextActionTime = Time.time + cooldownTime; // Start the cooldown

}
