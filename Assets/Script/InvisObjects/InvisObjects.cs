using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Required for TilemapRenderer

public class InvisObjects : MonoBehaviour
{
    public static InvisObjects instance;

    [SerializeField] private List<TilemapRenderer> invisibleObjects;

    private void Awake()
    {
        instance = this;
    }

    public void setObjectVisibility(bool visible)
    {
        foreach (TilemapRenderer t in invisibleObjects)
        {
            t.enabled = visible;
        }
    }
}
