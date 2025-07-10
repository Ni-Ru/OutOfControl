using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] private VolumeProfile normalVisionProfile;
    [SerializeField] private VolumeProfile ultravioletVisionProfile;
    
    public static float maxEnergyLimit = 100;
    public float maxEnergy { get; private set; }
    public float currentEnergy;
    [SerializeField] GameObject abilityInventoryMenu;
    //[SerializeField] TextMeshProUGUI currentEnergyText;
    //[SerializeField] TextMeshProUGUI maxEnergyText;

    [SerializeField] Button[] abilityUIButtons;
    [SerializeField] GameObject[] batteryPips;

    private PlayerMovement movement;
    private PlayerSprite playerSprite;

    private Dictionary<KeyCode, bool> buttonAvailability;
    private List<PlayerAction> availableActions;
    private Dictionary<KeyCode, PlayerAction> buttonBindings;
    private float energyRechargeRate = 50;
    private Color32 batteryUnusedColor = new Color32(0, 201, 255, 255);
    private Color32 batteryUsedColor = new Color32(91, 91, 91, 255);

    Jump jump;
    ClimbUp climbUp;
    SeeInvisibility seeInvis;

    bool insideWorkbench = false;
    private bool consumingEnergy = false;


    private void Awake()
    {
        buttonAvailability = new Dictionary<KeyCode, bool>();
        buttonAvailability.Add(KeyCode.RightArrow, true);
        buttonAvailability.Add(KeyCode.LeftArrow, true);
        buttonAvailability.Add(KeyCode.UpArrow, true);
        buttonAvailability.Add(KeyCode.DownArrow, false);
        buttonAvailability.Add(KeyCode.Z, true); // Y in german keyboard
        buttonAvailability.Add(KeyCode.X, true);
        buttonAvailability.Add(KeyCode.I, true);
        buttonAvailability.Add(KeyCode.Joystick1Button0, true); // A
        buttonAvailability.Add(KeyCode.Joystick1Button2, true); // X
        buttonAvailability.Add(KeyCode.Joystick1Button3, true); // Y
        availableActions = new List<PlayerAction>();
        buttonBindings = new Dictionary<KeyCode, PlayerAction>();
        movement = GetComponent<PlayerMovement>();
        maxEnergy = maxEnergyLimit;

        abilityInventoryMenu = GameObject.Find("AbilityInventoryMenu");

        abilityUIButtons = abilityInventoryMenu.GetComponentsInChildren<Button>();

        batteryPips = GameObject.FindGameObjectsWithTag("BatteryPip");
        System.Array.Reverse(batteryPips);

        foreach (Button btn in abilityUIButtons)
        {
            btn.onClick.AddListener(() => onButtonClicked(btn.name));

            if (btn.name == "ClimbUpEquip") btn.gameObject.SetActive(false);
            if (btn.name == "SeeInvisEquip") btn.gameObject.SetActive(false);

            if (btn.name == "SeeInvisInventory") btn.gameObject.SetActive(false);
            if (btn.name == "ClimbUpInventory") btn.gameObject.SetActive(false);

            if (btn.name == "JumpInventory") btn.gameObject.SetActive(false);
            //if (btn.name == "JumpEquip") btn.interactable = false; //btn.gameObject.SetActive(true);

            if (btn.name == "NormalEyeInventory") btn.gameObject.SetActive(false);
            if (btn.name == "NormalEyeEquip") btn.gameObject.SetActive(true);
        }

        foreach (var batteryPip in batteryPips)
        {
            batteryPip.GetComponent<Image>().color = batteryUnusedColor;
        }

        Walk right = new Walk();
        Walk left = new Walk();
        jump = new Jump();
        //climbUp = new ClimbUp();
        //seeInvis = new SeeInvisibility();

        right.isRight = true;
        playerSprite = GetComponent<PlayerSprite>();

        changeButtonBinding(KeyCode.RightArrow, right);
        changeButtonBinding(KeyCode.LeftArrow, left);

        changeButtonBinding(KeyCode.Z, jump);
        changeButtonBinding(KeyCode.Joystick1Button2, jump);

        abilityInventoryMenu.SetActive(false);
    }

    private void onButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "JumpInventory":
                changeButtonBinding(KeyCode.Z, jump);
                changeButtonBinding(KeyCode.Joystick1Button2, jump);
                changeButtonBinding(KeyCode.UpArrow, null);
                break;

            case "JumpEquip":
                changeButtonBinding(KeyCode.Z, null);
                changeButtonBinding(KeyCode.Joystick1Button2, null);
                break;

            case "ClimbUpInventory":
                changeButtonBinding(KeyCode.UpArrow, climbUp);
                changeButtonBinding(KeyCode.Z, null);
                changeButtonBinding(KeyCode.Joystick1Button2, null);
                break;

            case "ClimbUpEquip":
                changeButtonBinding(KeyCode.UpArrow, null);
                break;

            case "NormalEyeInventory":
                changeButtonBinding(KeyCode.I, null);
                changeButtonBinding(KeyCode.Joystick1Button3, null);
                break;

            case "NormalEyeEquip":
                changeButtonBinding(KeyCode.I, null);
                changeButtonBinding(KeyCode.Joystick1Button3, null);
                break;

            case "SeeInvisInventory":
                changeButtonBinding(KeyCode.I, seeInvis);
                changeButtonBinding(KeyCode.Joystick1Button3, seeInvis);
                break;

            case "SeeInvisEquip":
                changeButtonBinding(KeyCode.I, null);
                changeButtonBinding(KeyCode.Joystick1Button3, null);
                break;

            default:
                break;
        }
    }

    public void enableButton(KeyCode button)
    {
        buttonAvailability[button] = true;
    }

    public void addAvailableAction(PlayerAction action)
    {
        availableActions.Add(action);
    }

    public void changeButtonBinding(KeyCode button, PlayerAction action)
    {
        if (!buttonAvailability[button]) return;
        if (buttonBindings.ContainsKey(button)) maxEnergy += buttonBindings[button].maxEnergyPenalty;
        if (action == null)
        {
            buttonBindings.Remove(button);
            UpdateBatteryUI();
            playerSprite.adjustBodyParts(buttonBindings);
            return;
        }
        buttonBindings[button] = action;
        maxEnergy -= action.maxEnergyPenalty;

        //currentEnergy = Mathf.Min(currentEnergy, maxEnergy);

        UpdateBatteryUI();

        Debug.Log(maxEnergy);
        playerSprite.adjustBodyParts(buttonBindings);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Workbench"))
        {
            insideWorkbench = true;
        }

        if (collision.gameObject.CompareTag("NodePickup")) 
        {
            string nodeType = collision.gameObject.GetComponent<NodePickup>().GetNodeType();

            foreach (Button btn in abilityUIButtons)
            {
                if (nodeType == AbilityNodePickup.LEFT.ToString())
                {
                    Walk left = new Walk();
                    addAvailableAction(left);

                    changeButtonBinding(KeyCode.LeftArrow, left);

                }

                if (nodeType == AbilityNodePickup.JUMP.ToString() && (btn.name == "JumpInventory"))
                {
                    Jump jump = new Jump();
                    addAvailableAction(jump);

                    //changeButtonBinding(KeyCode.Z, jump);
                    //changeButtonBinding(KeyCode.Joystick1Button2, jump);
                    btn.gameObject.SetActive(true);
                }

                if (nodeType == AbilityNodePickup.CLIMBUP.ToString() && btn.name == "ClimbUpInventory")
                {
                    climbUp = new ClimbUp();
                    addAvailableAction(climbUp);

                    //changeButtonBinding(KeyCode.UpArrow, climbUp);
                    btn.gameObject.SetActive(true);
                }

                if (nodeType == AbilityNodePickup.BOMB.ToString())
                {
                    SpawnBomb spawnBomb = new SpawnBomb();
                    addAvailableAction(spawnBomb);

                    //changeButtonBinding(KeyCode.X, spawnBomb);
                }
                if (nodeType == AbilityNodePickup.SEE_INVIS.ToString() && btn.name == "SeeInvisInventory")
                {
                    seeInvis = new SeeInvisibility(globalVolume,normalVisionProfile,ultravioletVisionProfile);
                    addAvailableAction(seeInvis);
                    //changeButtonBinding(KeyCode.I, seeInvis);
                    //changeButtonBinding(KeyCode.Joystick1Button3, seeInvis);
                    btn.gameObject.SetActive(true);
                }
            }

            Debug.Log(collision.gameObject.GetComponent<NodePickup>().GetNodeType() + " node picked up");

            Destroy(collision.gameObject);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Workbench"))
        {
            insideWorkbench = false;
        }
    }

    private void UpdateBatteryUI()
    {
        int activePipCount = Mathf.FloorToInt(currentEnergy / 10f);

        for (int i = 0; i < batteryPips.Length; i++)
        {
            Image pipImage = batteryPips[i].GetComponent<Image>();

            if (pipImage == null) continue;

            pipImage.color = (i < activePipCount) ? batteryUnusedColor : batteryUsedColor;
        }
    }

    private void Update()
    {
        movement.purgeHorizontalVelocity();
        foreach(KeyCode button in buttonBindings.Keys)
        {
            if (Input.GetKey(button) && button != KeyCode.I)
            {
                buttonBindings[button].execute(gameObject);
                //Debug.Log(GetComponent<Rigidbody2D>().linearVelocityX);
            }
        }
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Joystick1Button3)) buttonBindings[KeyCode.I].execute(gameObject);

        if (Gamepad.current != null)
        {
            Vector2 dpadValue = Gamepad.current.dpad.ReadValue();

            if (dpadValue.x > 0.5f)
            {
                buttonBindings[KeyCode.RightArrow]?.execute(gameObject);
            }
            else if (dpadValue.x < -0.5f)
            {
                buttonBindings[KeyCode.LeftArrow]?.execute(gameObject);
            }

            if (buttonBindings.TryGetValue(KeyCode.UpArrow, out var upAction) && dpadValue.y > 0.5f)
            {
                //buttonBindings[KeyCode.UpArrow]?.execute(gameObject);
                upAction.execute(gameObject);

            }
        }
        if (consumingEnergy) consumingEnergy = false;
        else currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRechargeRate * Time.deltaTime * maxEnergy / maxEnergyLimit);

        UpdateBatteryUI();

        if (insideWorkbench && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            if (!abilityInventoryMenu.activeSelf)
            {
                abilityInventoryMenu.SetActive(true);
                Time.timeScale = 0;
                UpdateBatteryUI();
            }
            else
            {
                abilityInventoryMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public bool tryConsumeEnergy(float amount)
    {
        if (amount > currentEnergy) return false;
        currentEnergy -= amount;
        consumingEnergy = true;
        return true;
    }
}
