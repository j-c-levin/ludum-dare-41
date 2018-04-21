using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Holds the card types
public enum CardType
{
    Jump,
    Duck,
    Dash,
    PickupThree,
    HandSizeIncrease,
    WildCard,
    WildDraw
}

public class Card
{
    public Card(CardType type)
    {
        this.type = type;
    }
    public CardType type;
    public void use() { }
}

public class CardManager : MonoBehaviour
{
    // List of current deck
    private Stack<Card> deck = new Stack<Card>();

    // Discard pile
    private Stack<Card> discardPile = new Stack<Card>();

    // List of current hand
    private List<Card> hand = new List<Card>();

    // Starting number of cards
    private int startingBasicCardCount = 3;

    // Initialisation
    public void Start()
    {
        setUpDeck();
    }

    // Function to add a card to a deck
    public void pickUpCard(Card newCard)
    {
        discardPile.Push(newCard);
    }

    // Function to consume a card in hand
    public void useCardInHand(int index)
    {
        // Get card used
        Card selectedCard = hand[index];
        // Remove from hand
        hand.RemoveAt(index);
        // Use ability
        // Add to discard
        discardPile.Push(selectedCard);
    }

    // Function to draw a new card from deck to hand
    public void draw(int drawNumber = 1)
    {
        // Check if deck needs discard pile to be shuffled in
        for (int i = 0; i < drawNumber; i++)
        {
            if (deck.Count == 0)
            {
                shuffleDiscardIntoDeck();
                if (deck.Count == 0)
                {
                    Debug.LogError("Should have shuffled the discard pile into the deck but the deck is still empty!!!");
                }
            }
            // Draw a card from deck to hand
            Card topCard = deck.Pop();
            hand.Add(topCard);
        }
    }

    // Initalise the deck with the same number of basic cards
    private void setUpDeck()
    {
        for (int i = 0; i < startingBasicCardCount; i++)
        {
            deck.Push(new Card(CardType.Dash));
            deck.Push(new Card(CardType.Duck));
            deck.Push(new Card(CardType.Jump));
        }
    }

    // Shuffle the discard pile back into the deck when the deck pile is empty and something is drawn
    private void shuffleDiscardIntoDeck()
    {
        Card[] tempDeck = discardPile.ToArray();
        new System.Random().Shuffle(tempDeck);
        discardPile.Clear();
        deck = new Stack<Card>(tempDeck);
    }
}

static class RandomExtensions
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}