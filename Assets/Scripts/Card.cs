public class Card
{
    public Card(CardType type)
    {
        this.type = type;
    }
    public CardType type;
    public virtual void use(Runner runner) { }
}