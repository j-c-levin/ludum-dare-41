using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloorCard : Card
{

    public ChangeFloorCard() : base(CardType.ChangeFloor) { }

    public override void use(Runner runner)
    {
        runner.ChangeFloor();
    }
}
