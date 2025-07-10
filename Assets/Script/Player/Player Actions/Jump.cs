using UnityEngine;

public class Jump : PlayerAction
{
    public static readonly float jumpSpeed = 500;
    private PlayerMovement movement;
    private PlayerControls controls;
    new public int maxEnergyPenalty = 20;

    public override void execute(GameObject player)
    {
        if (movement == null)
        {
            movement = player.GetComponent<PlayerMovement>();
            controls = player.GetComponent<PlayerControls>();
        }
        if(movement.isGrounded && controls.tryConsumeEnergy(40)) movement.addJump(Time.fixedDeltaTime * jumpSpeed);
    }
}
