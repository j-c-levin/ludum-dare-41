using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Holds the card types
public enum CardType
{
    Jump,
    Duck
}

public class CardManager : MonoBehaviour
{
    // List of current deck
    private Stack<Card> deck = new Stack<Card>();

    // Discard pile
    private Stack<Card> discardPile = new Stack<Card>();

    // List of current hand
    public readonly List<Card> hand = new List<Card>();

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
        selectedCard.use();
        // Add to discard
        discardPile.Push(selectedCard);
    }

    // Function to draw a new card from deck to hand
    public bool draw(int drawNumber)
    {
        // Check if deck needs discard pile to be shuffled in
        for (int i = 0; i < drawNumber; i++)
        {
            // Check if the deck needs ot be reshuffled
            if (deck.Count == 0)
            {
                shuffleDiscardIntoDeck();
                if (deck.Count == 0)
                {
                    Debug.Log("Shuffled discard pile into deck but no cards remain to draw, deck is empty");
                    return false;
                }
            }
            // Draw a card from deck to hand
            hand.Add(deck.Pop());
        }
        return true;
    }

    // Initalise the deck with the same number of basic cards
    private void setUpDeck()
    {
        for (int i = 0; i < startingBasicCardCount; i++)
        {
            discardPile.Push(new JumpCard());
            discardPile.Push(new DuckCard());
        }
		shuffleDiscardIntoDeck();
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