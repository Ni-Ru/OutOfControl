using UnityEngine;

public class Jump : PlayerAction
{
    public static readonly float jumpSpeed = 500;
    private PlayerMovement movement;
    private PlayerControls controls;

    public override void execute(GameObject player)
    {
        if (movement == null)
        {
            movement = player.GetComponent<PlayerMovement>();
            controls = player.GetComponent<PlayerControls>();
        }
        if(movement.isGrounded && controls.tryConsumeEnergy(30)) movement.addJump(Time.fixedDeltaTime * jumpSpeed);
    }
}
