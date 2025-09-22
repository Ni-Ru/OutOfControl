using UnityEngine;

public abstract class AbilitySlot
{
    public readonly AbilitySlotPositions slotPosition;
    public readonly KeyCode keyboardInput;
    public readonly KeyCode controllerInput;
    public PlayerAction playerAction;

    public AbilitySlot(PlayerAction playerAction, AbilitySlotPositions slotPosition, KeyCode keyboardInput, KeyCode controllerInput)
    {
        this.playerAction = playerAction;
        this.slotPosition = slotPosition;
        this.keyboardInput = keyboardInput;
        this.controllerInput = controllerInput;
    }
}
