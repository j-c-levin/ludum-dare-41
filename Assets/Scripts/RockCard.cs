using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCard : Card
{
    private int clicksToRemove = 3;
    public RockCard() : base(CardType.Rock) { }

    public override void use(Runner runner)
    {
        clicksToRemove -= 1;
    }
}
