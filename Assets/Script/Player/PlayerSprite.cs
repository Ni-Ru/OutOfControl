using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Sprite jumpingLegsSprite;
    [SerializeField] private Sprite noLegsSprite;
    [SerializeField] private Sprite jumpingLegsSpriteEyes;
    [SerializeField] private Sprite noLegsSpriteEyes;
    [SerializeField] private Sprite climbingLegsSpriteEyes;
    [SerializeField] private Sprite climbingLegsSprite;
    [SerializeField] private Animator animator;

    private Vector2 tallHitbox = new Vector2(-0.046875f, 0.90625f);
    private Vector2 shortHitbox = new Vector2(0.03125f, 0.75f);

    public void adjustBodyParts(Dictionary<KeyCode, PlayerAction> equippedActions)
    {
        animator.enabled = false;
        bool hasEyes = equippedActions.ContainsKey(KeyCode.I);
        animator.SetBool("hasEyes", hasEyes);
        if (equippedActions.Keys.Contains(KeyCode.Z))
        {
            playerSprite.sprite = hasEyes? jumpingLegsSpriteEyes: jumpingLegsSprite;
            animator.SetBool("hasJumpingLegs", true);
            animator.SetBool("hasClimbingLegs", false);
            playerCollider.offset = new Vector2(0, tallHitbox.x);
            playerCollider.size = new Vector2(playerCollider.size.x, tallHitbox.y);
        }
        else if (equippedActions.Keys.Contains(KeyCode.UpArrow))
        {
            playerSprite.sprite = hasEyes ? climbingLegsSpriteEyes : climbingLegsSprite;
            animator.SetBool("hasJumpingLegs", false);
            animator.SetBool("hasClimbingLegs", true);
            playerCollider.offset = new Vector2(0, tallHitbox.x);
            playerCollider.size = new Vector2(playerCollider.size.x, tallHitbox.y);
        } else
        {
            playerSprite.sprite = hasEyes? noLegsSpriteEyes : noLegsSprite;
            animator.SetBool("hasJumpingLegs", false);
            animator.SetBool("hasClimbingLegs", false);
            playerCollider.offset = new Vector2(0, shortHitbox.x);
            playerCollider.size = new Vector2(playerCollider.size.x, shortHitbox.y);
        }
        //animator.enabled = true;
        /*
        if (!equippedActions.Keys.Contains(KeyCode.Z) && !equippedActions.Keys.Contains(KeyCode.UpArrow))
        {
            //no legs, adapt Sprite, Collider, Animation
            legSprite.enabled = false;// defaultParts[KeyCode.Z];
            playerCollider.offset = new Vector2(0, shortHitbox.x);
            playerCollider.size = new Vector2(playerCollider.size.x, shortHitbox.y);
        } else
        {
            legSprite.enabled = true;
            playerCollider.offset = new Vector2(0, tallHitbox.x);
            playerCollider.size = new Vector2(playerCollider.size.x, tallHitbox.y);
            if (equippedActions.Keys.Contains(KeyCode.Z))
            {
                Debug.Log(equippedActions[KeyCode.Z] == null);
                legSprite.sprite = bodyParts[KeyCode.Z];
            }
            else legSprite.sprite = bodyParts[KeyCode.UpArrow];
        }
        /*
        if (!equippedActions.Keys.Contains(KeyCode.I))
        {
            eyeSprite.sprite = defaultParts[KeyCode.I];
        }
        else
        {
            eyeSprite.sprite = bodyParts[equippedActions[KeyCode.I]];
        }
        */
    }
}
