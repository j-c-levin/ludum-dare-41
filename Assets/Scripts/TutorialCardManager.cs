using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialCardManager : MonoBehaviour
{
    // Reference to the player
    public Runner runner;
    // Delegate for listening for drawing cards
    public delegate void DrawCardDelegate(Card drawnCard);
    // Event listener for drawing cards
    public DrawCardDelegate drawCardDelegate;
    // List of current hand
    private List<Card> hand = new List<Card>();
    // List of current deck
    private Stack<Card> deck = new Stack<Card>();
    // Starting number of cards in hand
    private int startingHandSize = 1;

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

    // Function to consume a card in hand, returns true if card is removed
    public bool useCardInHand(Card selectedCard)
    {
        // Use ability
        selectedCard.use(runner);
        // Check if the card is to be removed from the hand
        if (selectedCard.canRemoveCard == false)
        {
            return false;
        }
        // Remove from hand
        hand.Remove(selectedCard);
        // If the card has anything it needs to do to reset, do so
        selectedCard.reset();
        return true;
    }

    // Function to draw a new card from deck to hand
    public void draw(int drawNumber)
    {
        // Check if deck needs discard pile to be shuffled in
        for (int i = 0; i < drawNumber; i++)
        {
            // Draw a card from deck to hand
            if (deck.Count > 0)
            {
                Card topCard = deck.Pop();
                hand.Add(topCard);
                if (drawCardDelegate != null)
                {
                    drawCardDelegate(topCard);
                }
            }
        }
    }

    public void AddRockToHand()
    {
        deck.Push(new RockCard());
        draw(1);
    }

    // Initalise the deck with the same number of basic cards
    private void setUpDeck()
    {
        deck.Push(new ChangeFloorCard());
        deck.Push(new DuckCard());
        deck.Push(new JumpCard());
        // Draw the starting cards
        draw(startingHandSize);
    }
}