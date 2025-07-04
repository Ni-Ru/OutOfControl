using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public static float maxEnergyLimit = 100;
    public float maxEnergy { get; private set; }
    public float currentEnergy;
    [SerializeField] GameObject abilityInventoryMenu;
    [SerializeField] TextMeshProUGUI currentEnergyText;
    [SerializeField] TextMeshProUGUI maxEnergyText;
    [SerializeField] Button[] abilityUIButtons;

    private PlayerMovement movement;

    private Dictionary<KeyCode, bool> buttonAvailability;
    private List<PlayerAction> availableActions;
    private Dictionary<KeyCode, PlayerAction> buttonBindings;
    private float energyRechargeRate = 50;

    Jump jump;
    ClimbUp climbUp;
    SeeInvisibility seeInvis;

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
        availableActions = new List<PlayerAction>();
        buttonBindings = new Dictionary<KeyCode, PlayerAction>();
        movement = GetComponent<PlayerMovement>();
        maxEnergy = maxEnergyLimit;

        abilityInventoryMenu = GameObject.Find("AbilityInventoryMenu");

        abilityUIButtons = abilityInventoryMenu.GetComponentsInChildren<Button>();

        currentEnergyText = GameObject.Find("BatteryCurrentNumber").GetComponent<TextMeshProUGUI>();
        maxEnergyText = GameObject.Find("BatteryMaxNumber").GetComponent<TextMeshProUGUI>();


        foreach (Button btn in abilityUIButtons)
        {
            btn.onClick.AddListener(() => onButtonClicked(btn.name));

            if (btn.name == "ClimbUpEquip") btn.gameObject.SetActive(false);
            if (btn.name == "SeeInvisEquip") btn.gameObject.SetActive(false);

            if (btn.name == "JumpInventory") btn.gameObject.SetActive(false);
            if (btn.name == "JumpEquip") btn.gameObject.SetActive(true);

            if (btn.name == "NormalEyeInventory") btn.gameObject.SetActive(false);
            if (btn.name == "NormalEyeEquip") btn.gameObject.SetActive(true);
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

        maxEnergyText.text = ((int)maxEnergyLimit / 10).ToString();

    }

    void onButtonClicked(string buttonName)
    {
        switch (buttonName)
        {
            case "JumpInventory":
                changeButtonBinding(KeyCode.Z, jump);
                changeButtonBinding(KeyCode.UpArrow, null);
                break;

            case "JumpEquip":
                changeButtonBinding(KeyCode.Z, null);
                break;

            case "ClimbUpInventory":
                changeButtonBinding(KeyCode.UpArrow, climbUp);
                changeButtonBinding(KeyCode.Z, null);
                break;

            case "ClimbUpEquip":
                changeButtonBinding(KeyCode.UpArrow, null);
                break;

            case "NormalEyeInventory":
                changeButtonBinding(KeyCode.I, null);
                break;

            case "NormalEyeEquip":
                changeButtonBinding(KeyCode.I, null);
                break;

            case "SeeInvisInventory":
                changeButtonBinding(KeyCode.I, seeInvis);
                break;

            case "SeeInvisEquip":
                changeButtonBinding(KeyCode.I, null);
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
        Debug.Log(maxEnergy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string nodeType = collision.gameObject.GetComponent<NodePickup>().GetNodeType();

        if (collision.gameObject.CompareTag("NodePickup")) 
        {
            if(nodeType == AbilityNodePickup.LEFT.ToString())
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
        }
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRechargeRate * Time.deltaTime * maxEnergy / maxEnergyLimit);

        currentEnergyText.text = ((int)currentEnergy / 10).ToString();
    }

    public bool tryConsumeEnergy(float amount)
    {
        if (amount > currentEnergy) return false;
        currentEnergy -= amount;
        return true;
    }
}
