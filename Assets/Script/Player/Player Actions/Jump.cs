using UnityEngine;

public class Jump : PlayerAction
{
    public static readonly float jumpSpeed = 500;
    public bool isGrounded;
    private PlayerMovement movement;

    public override void execute(GameObject player)
    {
        if(movement == null) movement = player.GetComponent<PlayerMovement>();

        movement.addVerticalMovement(Time.fixedDeltaTime * jumpSpeed);
    }
}
