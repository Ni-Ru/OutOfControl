using UnityEngine;

public class NodePickup : MonoBehaviour
{
    [SerializeField] string nodeType;

    public string GetNodeType()
    {
        nodeType = AbilityNodePickup.LEFT.ToString();
        return nodeType;
    }
}
