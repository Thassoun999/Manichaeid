using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoveryspeed = 0.2f;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage and Die

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmount;

            // we want the direction you should be pushed TOWARDS to, so where you currently are, you need to be pushed away from where the damage CAME from! (Creating a vector between two points)
            // Then normalize the value and add it to the pushforce
            // To normalize a vector means to change it so that it points in the same direction (think of that line from the origin) but its length is one ( |v| = sqrt(x^2 + y^2 + z^2))
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce; 

            if(dmg.color == 0) // player getting damage
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 40, Color.red, transform.position + (new Vector3(0.0f, 0.1f, 0.0f)), Vector3.zero, 0.5f, 1);
            else if(dmg.color == 1) // enemy getting damage
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 40, Color.magenta, transform.position + (new Vector3(0.0f, 0.1f, 0.0f)), Vector3.zero, 0.5f, 1);

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }

    // for the future, make this abstract!!
    protected virtual void Death()
    {

    }
}
