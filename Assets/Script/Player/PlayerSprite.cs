using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer torsoSprite;
    [SerializeField] private SpriteRenderer legSprite;
    [SerializeField] private SpriteRenderer eyeSprite;
    [SerializeField] private SpriteRenderer armSprite1;
    [SerializeField] private SpriteRenderer armSprite2;

    private Dictionary<PlayerAction, Sprite> bodyParts;
    private Dictionary<KeyCode, Sprite> defaultParts;

    private void Awake()
    {
        loadBodyParts();
    }

    private void loadBodyParts()
    {
        bodyParts = new Dictionary<PlayerAction, Sprite>();
        defaultParts = new Dictionary<KeyCode, Sprite>();
        //load the body parts (and animations?) from resources
    }

    public void adjustBodyParts(Dictionary<KeyCode, PlayerAction> equippedActions)
    {
        if (!equippedActions.Keys.Contains(KeyCode.Z))
        {
            legSprite.sprite = defaultParts[KeyCode.Z];
        } else
        {
            legSprite.sprite = bodyParts[equippedActions[KeyCode.Z]];
        }
        if (!equippedActions.Keys.Contains(KeyCode.I))
        {
            eyeSprite.sprite = defaultParts[KeyCode.I];
        }
        else
        {
            eyeSprite.sprite = bodyParts[equippedActions[KeyCode.I]];
        }
    }
}
