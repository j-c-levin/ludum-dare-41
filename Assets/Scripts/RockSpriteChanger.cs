using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class RockSpriteChanger : MonoBehaviour
{
    public Sprite[] rockSprites;
    int health = 1;
    public void changeSprite()
    {
        // Change the sprite attached to the rock card.
        /*
            Remember that the '--' operator decriments after the variable has been read.  So writing as health-- reads the value of 'health' and access the array at that index, and after that it decriments the value of health.
         */
        if (health >= 0)
        {
            GetComponent<Image>().sprite = rockSprites[health--];
        }
    }
}
