using UnityEngine;

public class Jump : PlayerAction
{
    public static readonly float jumpSpeed = 500;
    private PlayerMovement movement;

    public override void execute(GameObject player)
    {
        if(movement == null) movement = player.GetComponent<PlayerMovement>();

        movement.addJump(Time.fixedDeltaTime * jumpSpeed);
    }
}
