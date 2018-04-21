public class JumpCard : Card
{
    public JumpCard() : base(CardType.Jump) { }

     public override void use(Runner runner) {
        runner.Jump();
    }
}
