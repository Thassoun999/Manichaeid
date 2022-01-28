using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected float moveSpeed = 1;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;

    private RaycastHit2D hit;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input) 
    {
        // Reset moveDelta -- Difference between current position and where I'm going to be
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        // Swap sprite direction, whether you're going left or right
        if (moveDelta.x < 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Add push vector, if any.
        moveDelta += pushDirection;

        // Reduce the push force every frame with linear interpolation and recovery speed.
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoveryspeed); // what are we affecting, to what end point, with what modifiable value using a LERP formula
    
        // Make sure we can move in this direction by casting a box there first, if the box returns null we are free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Now we are making it move
            transform.Translate(0, moveDelta.y * Time.deltaTime * moveSpeed, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Now we are making it move

            transform.Translate(moveDelta.x * Time.deltaTime * moveSpeed, 0, 0);
        }
    }
}
