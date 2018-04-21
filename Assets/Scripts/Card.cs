public class Card
{
    public Card(CardType type)
    {
        this.type = type;
    }
    public CardType type;
    public bool canRemoveCard = true;
    public virtual void use(Runner runner) { }
    public virtual void reset() { }
}