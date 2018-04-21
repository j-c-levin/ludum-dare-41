using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockObject : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        // Add a card to the discard pile in the card manager
        CardManager manager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();
        manager.pickUpCard(new RockCard());
    }
}
