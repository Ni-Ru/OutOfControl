using UnityEngine;

public class NodePickup : MonoBehaviour
{
    [SerializeField] string nodeType;

    public string GetNodeType()
    {
        return nodeType;
    }
}
