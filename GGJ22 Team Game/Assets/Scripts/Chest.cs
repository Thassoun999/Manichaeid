using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount = 10;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.ShowText("+" + coinAmount + " coins!", 40, Color.yellow, transform.position, Vector3.up * 25, 0.5f, 1);
        }
    }
}
