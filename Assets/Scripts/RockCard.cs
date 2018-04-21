using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCard : Card
{
    private const int clicksToRemove = 3;
    private int currentHealth = clicksToRemove;
    public RockCard() : base(CardType.Rock)
    {
        canRemoveCard = false;
    }

    public override void use(Runner runner)
    {
        currentHealth -= 1;
        Debug.Log("health:" + currentHealth + " " + canRemoveCard);
        // Remove the card if it has been clicked the required amount of times
        if (currentHealth <= 0)
        {
            canRemoveCard = true;
        }
    }

    public override void reset()
    {
        currentHealth = clicksToRemove;
        Debug.Log("resetting: " + currentHealth);
        canRemoveCard = false;
    }
}
