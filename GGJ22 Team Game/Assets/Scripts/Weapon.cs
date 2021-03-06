using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int damagePoint;
    public float pushForce;

    // Weapon upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Cooldowns - both are tied to the same sword, will be setting priority to the projectile cooldown if it is up. Otherwise we are prioritizing the swing cooldown!
    private float swingCooldown = 0.5f;
    private float lastSwing; // Has push force and more damage
    private float projectileCooldown = 6.0f; // Has reduced damage, no push force, piercing, and range
    private float lastProjectile;
    private bool projectileBool;

    private bool swingHasHit;

    // Animator Controller
    private Animator animController; 

    // Projectile Prefab
    public Transform projectilePrefab;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        projectileBool = false;
        swingHasHit = false;
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - lastProjectile > projectileCooldown)
        {
            projectileBool = true;
            spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f, 0.8f);

        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (projectileBool)
            {
                lastProjectile = Time.time;
                projectileBool = false;
                SpecialSwing();
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            } 
            else if (Time.time - lastSwing > swingCooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    // This would have to be for the regular swing, for the special swing we're going to need to implement a separate object
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                return;
            }

            // Create a new damage object that we will then send over to the fighter 
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce,
                color = 1
            };

            if(!swingHasHit)
            {
                AkSoundEngine.PostEvent("Play_Take_Damage_Enemy", gameObject);
                swingHasHit = true;
            }

            coll.SendMessage("ReceiveDamage", dmg); // Is to be sent to the player / enemy class objects (the method ReceiveDamage needs to be implemented for this to work)
        }
    }

    private void Swing()
    {
        swingHasHit = false;
        animController.SetTrigger("Swing");
        AkSoundEngine.PostEvent("Play_Sword_Player", gameObject);
    }

    private void SpecialSwing()
    {
        swingHasHit = false;
        Debug.Log("Special Swing");
        animController.SetTrigger("Special");
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        AkSoundEngine.PostEvent("Play_Sword_Charge_Swing_Player", gameObject);
    }
}
