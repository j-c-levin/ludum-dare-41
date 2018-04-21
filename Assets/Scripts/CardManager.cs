﻿using System.Collections;
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
    public Stack<Card> deck = new Stack<Card>();
    // Discard pile
    public Stack<Card> discardPile = new Stack<Card>();
    // Delegate for listening for drawing cards
    public delegate void DrawCardDelegate(Card drawnCard);
    // Event listener for drawing cards
    public DrawCardDelegate drawCardDelegate;
    // List of current hand
    public readonly List<Card> hand = new List<Card>();
    // Starting number of basic card types
    private int startingBasicCardCount = 3;
    // Starting number of cards in hand
    private int startingHandSize = 3;
    // Reference to the player
    private Runner runner;

    // Initialisation
    public void Start()
    {
        setUpDeck();
        runner = GameObject.FindGameObjectWithTag("Runner").GetComponent<Runner>();
        if (runner == null)
        {
            Debug.LogError("No runner found in scene");
        }
    }

    // Function to add a card to a deck
    public void pickUpCard(Card newCard)
    {
        discardPile.Push(newCard);
    }

    // Function to consume a card in hand
    public void useCardInHand(Card selectedCard)
    {
        // Remove from hand
        hand.Remove(selectedCard);
        // Use ability
        selectedCard.use(runner);
        // Add to discard
        discardPile.Push(selectedCard);
    }

    // Function to draw a new card from deck to hand
    public void draw(int drawNumber)
    {
        // Check if deck needs discard pile to be shuffled in
        for (int i = 0; i < drawNumber; i++)
        {
            // Check if the deck needs ot be reshuffled
            if (deck.Count == 0)
            {
                Debug.Log("Deck empty, shuffling");
                shuffleDiscardIntoDeck();
                if (deck.Count == 0)
                {
                    Debug.Log("Shuffled discard pile into deck but no cards remain to draw, deck is empty");
                    return;
                }
            }
            // Draw a card from deck to hand
            Card topCard = deck.Pop();
            hand.Add(topCard);
            if (drawCardDelegate != null)
            {
                drawCardDelegate(topCard);
            }
        }
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
        // Get the discard pile as an array
        Card[] tempDeck = discardPile.ToArray();
        // Shuffle the pile
        new System.Random().Shuffle(tempDeck);
        // Reset the discard pile
        discardPile.Clear();
        // Assign the deck the shuffled pile
        deck = new Stack<Card>(tempDeck);
        // Draw the starting cards
        draw(startingHandSize);
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