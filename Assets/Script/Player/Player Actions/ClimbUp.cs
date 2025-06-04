using UnityEngine;

public class ClimbUp : PlayerAction
{
    public static readonly float climbSpeed = 300;
    private PlayerMovement movement;

    public override void execute(GameObject player)
    {
        if (movement == null) movement = player.GetComponent<PlayerMovement>();

        movement.addClimbingUp(climbSpeed * Time.fixedDeltaTime);
    }
}
