using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public int maxEnergy { get; private set; }

    private PlayerMovement movement;

    private Dictionary<KeyCode, bool> buttonAvailability;
    private List<PlayerAction> availableActions;
    private Dictionary<KeyCode, PlayerAction> buttonBindings;


    private void Awake()
    {
        buttonAvailability = new Dictionary<KeyCode, bool>();
        buttonAvailability.Add(KeyCode.RightArrow, true);
        buttonAvailability.Add(KeyCode.LeftArrow, true);
        buttonAvailability.Add(KeyCode.UpArrow, true);
        buttonAvailability.Add(KeyCode.DownArrow, false);
        buttonAvailability.Add(KeyCode.Z, true); // Y in german keyboard
<<<<<<< Updated upstream
        buttonAvailability.Add(KeyCode.X, false);
=======
        buttonAvailability.Add(KeyCode.X, true);
        buttonAvailability.Add(KeyCode.I, true);
>>>>>>> Stashed changes
        availableActions = new List<PlayerAction>();
        buttonBindings = new Dictionary<KeyCode, PlayerAction>();
        movement = GetComponent<PlayerMovement>();
        maxEnergy = 100;

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
            }

            if(nodeType == AbilityNodePickup.CLIMBUP.ToString()) 
            { 
                ClimbUp climbUp = new ClimbUp();
                addAvailableAction(climbUp);

                changeButtonBinding (KeyCode.UpArrow, climbUp);
            }

<<<<<<< Updated upstream
=======
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

>>>>>>> Stashed changes
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
    }
}
