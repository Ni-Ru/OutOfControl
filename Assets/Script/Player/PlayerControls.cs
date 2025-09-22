using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public static float maxEnergyLimit = 100;
    public float maxEnergy { get; private set; }
    public float currentEnergy;
    [SerializeField] GameObject abilityInventoryMenu;

    [SerializeField] Button[] abilityUIButtons;
    [SerializeField] GameObject[] batteryPips;

    private PlayerMovement movement;
    private PlayerSprite playerSprite;

    public List<PlayerAction.AbilityName> availableActions;
    private Dictionary<AbilitySlotPositions, AbilitySlot> buttonBindings;
    private float energyRechargeRate = 50;
    private Color32 batteryUnusedColor = new Color32(0, 201, 255, 255);
    private Color32 batteryHalfUsedColor = new Color32(254, 255, 0, 255);
    private Color32 batteryAlmostUsedColor = new Color32(255, 0, 0, 255);
    private Color32 batteryUsedColor = new Color32(91, 91, 91, 255);

    Jump jump;
    ClimbUp climbUp;
    SeeInvisibility seeInvis;

    private bool consumingEnergy = false;


    private void Awake()
    {
        availableActions = new List<PlayerAction.AbilityName>();
        buttonBindings = new Dictionary<AbilitySlotPositions, AbilitySlot>();
        movement = GetComponent<PlayerMovement>();
        maxEnergy = maxEnergyLimit;

        abilityInventoryMenu = GameObject.Find("AbilityInventoryMenu");

        abilityUIButtons = abilityInventoryMenu.GetComponentsInChildren<Button>();

        batteryPips = GameObject.FindGameObjectsWithTag("BatteryPip");
        System.Array.Reverse(batteryPips);
        /*
        foreach (Button btn in abilityUIButtons)
        {
            //btn.onClick.AddListener(() => onButtonClicked(btn.name));

            if (btn.name == "ClimbUpEquip") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "SeeInvisEquip") btn.interactable = false; //btn.gameObject.SetActive(false);

            if (btn.name == "SeeInvisInventory") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "ClimbUpInventory") btn.interactable = false; //btn.gameObject.SetActive(false);

            if (btn.name == "JumpInventory") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "JumpEquip") btn.interactable = true; //btn.gameObject.SetActive(true);

            if (btn.name == "NormalEyeInventory") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "NormalEyeEquip") btn.interactable = true; //btn.gameObject.SetActive(true);
        }
        */
        foreach (var batteryPip in batteryPips)
        {
            batteryPip.GetComponent<Image>().color = batteryUnusedColor;
        }

        jump = new Jump();
        addAvailableAction(PlayerAction.AbilityName.Jump);

        buttonBindings[AbilitySlotPositions.Right] = new WalkRight();
        buttonBindings[AbilitySlotPositions.Left] = new WalkLeft();
        buttonBindings[AbilitySlotPositions.Legs] = new LegSlot(jump);
        buttonBindings[AbilitySlotPositions.Eyes] = new EyeSlot(null);

        playerSprite = GetComponent<PlayerSprite>();

        abilityInventoryMenu.SetActive(false);
    }

    public void addAvailableAction(PlayerAction.AbilityName action)
    {
        availableActions.Add(action);
    }

    public void changeButtonBinding(AbilitySlotPositions slot, PlayerAction action)
    {
        if (buttonBindings.ContainsKey(slot) && buttonBindings[slot].playerAction != null) maxEnergy += buttonBindings[slot].playerAction.maxEnergyPenalty;

        buttonBindings[slot].playerAction = action;
        if(action != null) maxEnergy -= action.maxEnergyPenalty;
        UpdateBatteryUI();
        playerSprite.adjustBodyParts(buttonBindings);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NodePickup")) 
        {
            //addAvailableAction()
            /*
            string nodeType = collision.gameObject.GetComponent<NodePickup>().GetNodeType();

            foreach (Button btn in abilityUIButtons)
            {
                if (nodeType == AbilityNodePickup.JUMP.ToString() && (btn.name == "JumpInventory"))
                {
                    jump = new Jump();
                    addAvailableAction(PlayerAction.AbilityName.Jump);
                    btn.interactable = true;
                }

                if (nodeType == AbilityNodePickup.CLIMBUP.ToString() && btn.name == "ClimbUpInventory")
                {
                    climbUp = new ClimbUp();
                    addAvailableAction(PlayerAction.AbilityName.Climb);
                    btn.interactable = true;
                }
                /*
                if (nodeType == AbilityNodePickup.BOMB.ToString())
                {
                    SpawnBomb spawnBomb = new SpawnBomb();
                    addAvailableAction(spawnBomb);
                }
                if (nodeType == AbilityNodePickup.SEE_INVIS.ToString() && btn.name == "SeeInvisInventory")
                {
                    seeInvis = new SeeInvisibility();
                    addAvailableAction(PlayerAction.AbilityName.SeeInvis);
                    btn.interactable = true;
                }
            }

            foreach (var action in availableActions)
            {
                Debug.Log(action.ToString());
            }

            Debug.Log(collision.gameObject.GetComponent<NodePickup>().GetNodeType() + " node picked up");

            Destroy(collision.gameObject);
        }
        
    }
    */
    private void UpdateBatteryUI()
    {
        int activePipCount = Mathf.FloorToInt(currentEnergy / 10f);

        Color pipColor;

        if (currentEnergy > 50)
        {
            pipColor = batteryUnusedColor;
        }
        else if (currentEnergy > 20)
        {
            pipColor = batteryHalfUsedColor;
        }
        else
        {
            pipColor = batteryAlmostUsedColor;
        }

        for (int i = 0; i < batteryPips.Length; i++)
        {
            Image pipImage = batteryPips[i].GetComponent<Image>();

            if (pipImage == null) continue;

            if (i < activePipCount)
            {
                pipImage.color = pipColor;
            }
            else { 
                pipImage.color = batteryUsedColor;
            }

        }
    }

    private void Update()
    {
        movement.purgeHorizontalVelocity();
        if (Gamepad.current != null)
        {
            Vector2 leftStickValue = Gamepad.current.leftStick.ReadValue();
            if(leftStickValue.x > 0.5f)
            {
                buttonBindings[AbilitySlotPositions.Right].playerAction.execute(gameObject);
            } else if(leftStickValue.x < -0.5f)
            {
                buttonBindings[AbilitySlotPositions.Left].playerAction.execute(gameObject);
            }
            foreach(AbilitySlot slot in buttonBindings.Values)
            {
                if (Input.GetKey(slot.controllerInput) && slot.playerAction != null) slot.playerAction.execute(gameObject);
            }
        }
        else
        {
            foreach (AbilitySlot slot in buttonBindings.Values)
            {
                if (Input.GetKey(slot.keyboardInput) && slot.playerAction != null) slot.playerAction.execute(gameObject);
            }
        }

        if (consumingEnergy) consumingEnergy = false;
        else currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRechargeRate * Time.deltaTime * maxEnergy / maxEnergyLimit);

        UpdateBatteryUI();
    }

    public bool tryConsumeEnergy(float amount)
    {
        if (amount > currentEnergy) return false;
        currentEnergy -= amount;
        consumingEnergy = true;
        return true;
    }
}
