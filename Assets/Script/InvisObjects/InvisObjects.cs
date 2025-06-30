using System.Collections.Generic;
using UnityEngine;

public class InvisObjects : MonoBehaviour
{
    public static InvisObjects instance;
    [SerializeField] private List<SpriteRenderer> invisibleObjects;

    private void Awake()
    {
        instance = this;
    }

    public void setObjectVisibility(bool visible)
    {
        foreach (SpriteRenderer g in invisibleObjects) g.enabled = visible;
    }
}
