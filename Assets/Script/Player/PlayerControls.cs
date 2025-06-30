using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public static float maxEnergyLimit = 100;
    public float maxEnergy { get; private set; }
    public float currentEnergy;

    private PlayerMovement movement;

    private Dictionary<KeyCode, bool> buttonAvailability;
    private List<PlayerAction> availableActions;
    private Dictionary<KeyCode, PlayerAction> buttonBindings;
    private float energyRechargeRate = 50;


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

        Walk right = new Walk();
        right.isRight = true;
        changeButtonBinding(KeyCode.RightArrow, right);
        //changeButtonBinding(KeyCode.LeftArrow, new Walk());
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
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRechargeRate * Time.deltaTime * maxEnergy / maxEnergyLimit);
    }

    public bool tryConsumeEnergy(float amount)
    {
        if (amount > currentEnergy) return false;
        currentEnergy -= amount;
        return true;
    }
}
