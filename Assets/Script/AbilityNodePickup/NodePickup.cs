using UnityEngine;

public class NodePickup : MonoBehaviour
{
    [SerializeField] PlayerAction.AbilityName _abilityName;
    public PlayerAction.AbilityName abilityName => _abilityName;
    [SerializeField] string nodeType;

    public string GetNodeType()
    {
        return nodeType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControls>().addAvailableAction(_abilityName);
            Destroy(gameObject);
        }
    }
}
