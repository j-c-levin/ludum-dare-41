using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class RockSpriteChanger : MonoBehaviour
{
    public Sprite[] rockSprites;
    public void changeSprite()
    {
        int health = GetComponent<RockCard>().currentHealth;
        GetComponent<SpriteRenderer>().sprite = rockSprites[health - 1];
    }
}
