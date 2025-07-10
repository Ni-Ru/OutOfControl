using UnityEngine;

public class ClimbUp : PlayerAction
{
    public static readonly float climbSpeed = 300;
    private PlayerMovement movement;
    private PlayerControls controls;
    new public int maxEnergyPenalty = 10;

    public override void execute(GameObject player)
    {
        if (movement == null) {
            movement = player.GetComponent<PlayerMovement>();
            controls = player.GetComponent<PlayerControls>();
        }
        if(movement.isTouchingWall && controls.tryConsumeEnergy(15 * Time.deltaTime)) movement.addClimbingUp(climbSpeed * Time.fixedDeltaTime);
    }
}
