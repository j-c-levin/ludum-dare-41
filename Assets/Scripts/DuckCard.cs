public class DuckCard : Card
{
    public DuckCard() : base(CardType.Duck) { }

    public override void use(Runner runner) {
        runner.Duck();
    }
}