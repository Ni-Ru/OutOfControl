using UnityEngine;

public class Walk : PlayerAction
{
    public static readonly float walkingSpeed = 250;
    public bool isRight;
    private PlayerMovement movement;

    public override void execute(GameObject player)
    {
        if (movement == null) movement = player.GetComponent<PlayerMovement>();
        movement.addHorizontalMovement(Time.fixedDeltaTime * walkingSpeed * (isRight ? 1 : -1) * player.GetComponent<PlayerControls>().maxEnergy / 100f);
    }
}
