using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public static float maxEnergyLimit = 100;
    public float maxEnergy { get; private set; }
    public float currentEnergy;
    [SerializeField] GameObject abilityInventoryMenu;
    //[SerializeField] TextMeshProUGUI currentEnergyText;
    //[SerializeField] TextMeshProUGUI maxEnergyText;

    [SerializeField] Button[] abilityUIButtons;
    [SerializeField] GameObject[] batteryPips;

    private PlayerMovement movement;

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

        //currentEnergyText = GameObject.Find("BatteryCurrentNumber").GetComponent<TextMeshProUGUI>();
        //maxEnergyText = GameObject.Find("BatteryMaxNumber").GetComponent<TextMeshProUGUI>();


        foreach (Button btn in abilityUIButtons)
        {
            btn.onClick.AddListener(() => onButtonClicked(btn.name));

            if (btn.name == "ClimbUpEquip") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "SeeInvisEquip") btn.interactable = false; //btn.gameObject.SetActive(false);

            if (btn.name == "JumpInventory") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "JumpEquip") btn.interactable = true; //btn.gameObject.SetActive(true);

            if (btn.name == "NormalEyeInventory") btn.interactable = false; //btn.gameObject.SetActive(false);
            if (btn.name == "NormalEyeEquip") btn.interactable = true; //btn.gameObject.SetActive(true);
        }

        foreach (var batteryPip in batteryPips)
        {
            batteryPip.GetComponent<Image>().color = batteryUnusedColor;
        }

        Walk right = new Walk();
        Walk left = new Walk();
        jump = new Jump();
        climbUp = new ClimbUp();
        seeInvis = new SeeInvisibility();

        right.isRight = true;

        changeButtonBinding(KeyCode.RightArrow, right);
        changeButtonBinding(KeyCode.LeftArrow, left);

        changeButtonBinding(KeyCode.Z, jump);
        changeButtonBinding(KeyCode.Joystick1Button2, jump);


        //maxEnergyText.text = ((int)maxEnergyLimit / 10).ToString();

        abilityInventoryMenu.SetActive(false);

    }

    void onButtonClicked(string buttonName)
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

            return;
        }
        
        buttonBindings[button] = action;
        maxEnergy -= action.maxEnergyPenalty;

        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
        UpdateBatteryUI();

        Debug.Log(maxEnergy);
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

            if (nodeType == AbilityNodePickup.LEFT.ToString())
            {
                Walk left = new Walk();
                addAvailableAction(left);

                changeButtonBinding(KeyCode.LeftArrow, left);

            }

            if(nodeType == AbilityNodePickup.JUMP.ToString()) 
            {
                Jump jump = new Jump();
                addAvailableAction(jump);

                changeButtonBinding(KeyCode.Z, jump);
                Debug.Log("Jump-Fähigkeit aktiviert. Taste Z zum Springen.");

            }

            if(nodeType == AbilityNodePickup.CLIMBUP.ToString()) 
            { 
                ClimbUp climbUp = new ClimbUp();
                addAvailableAction(climbUp);

                changeButtonBinding (KeyCode.UpArrow, climbUp);
            }

            if (nodeType == AbilityNodePickup.BOMB.ToString())
            {
                SpawnBomb spawnBomb = new SpawnBomb();
                addAvailableAction(spawnBomb);

                changeButtonBinding(KeyCode.X, spawnBomb);
            }
            if(nodeType == AbilityNodePickup.SEE_INVIS.ToString())
            {
                SeeInvisibility seeInvis = new SeeInvisibility();
                addAvailableAction(seeInvis);
                changeButtonBinding(KeyCode.I, seeInvis);
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
            if (Input.GetKey(button))
            {
                buttonBindings[button].execute(gameObject);
                //Debug.Log(GetComponent<Rigidbody2D>().linearVelocityX);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Z-Taste wurde gedrückt!");
            }
        }

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

            if(buttonBindings.TryGetValue(KeyCode.UpArrow, out var upAction) && dpadValue.y > 0.5f) 
            {
                //buttonBindings[KeyCode.UpArrow]?.execute(gameObject);
                upAction.execute(gameObject);

            }
        }

        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRechargeRate * Time.unscaledDeltaTime * maxEnergy / maxEnergyLimit);

        UpdateBatteryUI();
        
        //currentEnergyText.text = ((int)currentEnergy / 10).ToString();

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
        return true;
    }
}
