using UnityEngine;

public class Walk : PlayerAction
{
    public static readonly float walkingSpeed = 250;
    public bool isRight;
    private PlayerMovement movement;
    private PlayerControls playerControls;
    new public int maxEnergyPenalty = 0;

    public override void execute(GameObject player)
    {
        if (movement == null) {
            movement = player.GetComponent<PlayerMovement>();
            playerControls = player.GetComponent<PlayerControls>();
        }
        movement.addHorizontalMovement(Time.fixedDeltaTime * walkingSpeed * (isRight ? 1 : -1) * playerControls.maxEnergy / PlayerControls.maxEnergyLimit);
    }
}
