using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience + Movement Speed
    public int xpValue = 1;


    // Logic
    public float triggerLength = 0.5f;
    public float chaseLength = 1.25f;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox (enemy weapon)
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D filter;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

        ySpeed = 0.375f;
        xSpeed = 0.5f;
    }

    private void FixedUpdate()
    {
        // Is the player in range of chase? Should be bigger than the beginning trigger to "notice" the player
        if(Vector3.Distance(playerTransform.position, transform.position) < chaseLength)
        {
            // Is the player within trigger length to begin chase?
            if (Vector3.Distance(playerTransform.position, transform.position) < triggerLength)
                chasing = true;

            // Start chasing
            if (chasing)
            {
                // Stop chasing when you're colliding, just stop moving as damage is already being done
                if (!collidingWithPlayer)
                    UpdateMotor((playerTransform.position - transform.position).normalized);
            }
            else
            {
                UpdateMotor((startingPosition - transform.position)); // eventually this will reach 0 and the enemy returns to the starting position if player out of radius range
            }
            
        } 
        else 
        {
            UpdateMotor((startingPosition - transform.position));
            chasing = false;
        }

        // Check for overlap with the player
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits); // Get a list of results that overlap this collider (looking for other colliders beneath or above it)
        for (int i = 0; i < hits.Length; i++)
        {
            // we hit nothing
            if(hits[i] == null)
                continue;

            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
                collidingWithPlayer = true;

            // The array is not cleaned up every time so we have to go ahead and do it ourselves
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+" + xpValue + " exp!", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);

    }
}
