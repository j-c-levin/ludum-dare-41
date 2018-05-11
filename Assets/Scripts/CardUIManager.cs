﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CardManager))]
public class CardUIManager : MonoBehaviour
{
    // Prefab for jump
    public GameObject jumpCard;
    // Prefab for duck
    public GameObject duckCard;
    // Prefab for change floor
    public GameObject changeFloorCard;
    // Prefab for rock
    public GameObject rockCard;
    // Prefab for shuffling card
    public GameObject shuffleCard;
    // The Scroll View for the hand
    public GameObject handView;
    // The deck for animating
    public GameObject deckImage;
    // The discard pilefor animating
    public GameObject discardPileImage;
    // Counter for the deck
    public Text deckCounterText;
    // Counter for the discard pile
    public Text discardCounterText;
    // Width of card
    public float cardWidth = 188f;
    // X positions of the cards
    public GameObject[] cardPositions;
    // Reference for CardManager
    private CardManager cardManager;
    // List of current cards
    private GameObject[] uiCards = new GameObject[3];
    // shuffle animation duration
    private float shuffleAnimationDuration = 1f;
    // shuffle animation delay
    private float shuffleAnimationdelay = 0.1f;

    public void Awake()
    {
        // Store card manager reference
        cardManager = GetComponent<CardManager>();
        // Subscribe to draw card event
        cardManager.drawCardDelegate += DrawCard;
    }

    public void DrawCard(Card newCard)
    {
        // Should the shuffling animation play
        if (deckImage.GetComponent<Image>().color == Color.gray)
        {
            shufflingAnimation();
        }
        deckCounterText.text = cardManager.deck.Count.ToString();
        discardCounterText.text = cardManager.discardPile.Count.ToString();
        GameObject newCardObject = null;
        switch (newCard.type)
        {
            case CardType.Jump:
                newCardObject = jumpCard;
                break;
            case CardType.Duck:
                newCardObject = duckCard;
                break;
            case CardType.ChangeFloor:
                newCardObject = changeFloorCard;
                break;
            case CardType.Rock:
                newCardObject = rockCard;
                break;
            default:
                Debug.LogError("Cannot match new card type: " + newCard.type + " to a prefab");
                return;
        }
        // Instantiate the card
        Vector3 spawnPosition = new Vector3(deckImage.transform.position.x, deckImage.transform.position.y, deckImage.transform.position.z + 0.2f);
        GameObject newUiCard = Instantiate(newCardObject, spawnPosition, Quaternion.identity);
        // Set it into the hand panel
        newUiCard.transform.SetParent(handView.transform);
        // Reset the scale (because it jumps to 100 or something)
        newUiCard.transform.localScale = Vector2.one;
        // Set the width and height
        float cardHeight = handView.GetComponent<RectTransform>().rect.height;
        newUiCard.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
        // Set anchor
        newUiCard.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
        newUiCard.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
        // Set the card's data
        newUiCard.GetComponent<UICard>().card = newCard;
        // Add an event handler for when it is clicked
        newUiCard.GetComponent<Button>()
         .onClick
         .AddListener(() =>
         {
             useCard(newUiCard.GetComponent<UICard>());
         });
        // Animate the card
        animateCardIn(newUiCard.GetComponent<RectTransform>());
        // Add to list
        uiCards[cardManager.hand.Count - 1] = newUiCard;
        // Change the deck colour
        deckImage.GetComponent<Image>().color = (cardManager.deck.Count == 0) ? Color.gray : Color.white;
        // Change the discard pile colour
        discardPileImage.GetComponent<Image>().color = (cardManager.discardPile.Count == 0) ? Color.gray : Color.white;
    }

    public void useCard(UICard uiCard)
    {
        // Check the card can be played
        if (cardManager.runner.canPlayCard(uiCard.card) == false)
        {
            return;
        }
        // Use the card's effect
        bool cardRemoved = cardManager.useCardInHand(uiCard.card);
        // Check if the card should remain in the hand
        if (cardRemoved == false)
        {
            return;
        }
        discardCounterText.text = cardManager.discardPile.Count.ToString();
        // Remove from the game ui
        animateCardToDiscard(uiCard.GetComponent<RectTransform>());
        // Redraw a new card
        cardManager.draw(1);
    }

    private void shufflingAnimation()
    {
        // Loop every card in the shuffle
        for (int i = 0; i < cardManager.deck.Count; i++)
        {
            // Instantiate a 'card back' for them
            GameObject tempCard = Instantiate(shuffleCard, Vector2.zero, Quaternion.identity);
            float delay = i * shuffleAnimationdelay;
            // Parent
            tempCard.transform.SetParent(discardPileImage.transform);
            // Set the width and height
            float cardHeight = handView.GetComponent<RectTransform>().rect.height;
            tempCard.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
            // Reset the scale (because it jumps to 100 or something)
            tempCard.transform.localScale = Vector2.one;
            tempCard.transform.localPosition = Vector2.zero;
            tempCard.transform.rotation = discardPileImage.transform.rotation;
            // Set them to animate in a loop back to the deck
            DOTween.Sequence()
            // Move X and give them a delay based on their index
            .Insert(delay,
                tempCard.transform.DOMoveX(
                    deckImage.transform.position.x + 1,
                    shuffleAnimationDuration
                )
                .SetEase(Ease.OutQuad)
            )
            // Rotate
            .Insert(delay,
                tempCard.transform.DOLocalRotate(new Vector3(0, 0, -90), shuffleAnimationDuration * 0.8f)
            )
            // Move y up
            .Insert(delay,
                tempCard.transform.DOMoveY(deckImage.transform.position.y + 3, shuffleAnimationDuration / 2)
                .SetEase(Ease.OutQuad)
            )
            // Move y down
            .Insert(delay + shuffleAnimationDuration / 2,
                tempCard.transform.DOMoveY(deckImage.transform.position.y + 1.5f, shuffleAnimationDuration / 2)
                .SetEase(Ease.InQuad)
            )
            .AppendCallback(() =>
            {
                Destroy(tempCard);
            });
        }
    }

    private void animateCardIn(RectTransform card)
    {
        float xPosition = cardPositions[cardManager.hand.Count - 1].transform.localPosition.x;
        Sequence s = DOTween.Sequence();
        s.Append(
            card.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad)
        );
        // Set opacity
        card.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        // Fade in
        s.Insert(0, card.GetComponent<Image>().DOFade(1, 0.5f));
    }

    private void animateCardToDiscard(RectTransform card)
    {
        // Remove the used card
        Sequence s = DOTween.Sequence();
        s.Insert(0,
            card.DOLocalMoveX(discardPileImage.transform.localPosition.x, 1f)
            .SetEase(Ease.OutQuad)
        )
        // Destroy the card on end
        .AppendCallback(() =>
        {
            Destroy(card.gameObject);
            if (cardManager.discardPile.Count != 0)
            {
                discardPileImage.GetComponent<Image>().color = Color.white;
            }
        });
        // Bounce the card's y
        float yBounceTime = 0.3f;
        s.Insert(0,
            card.DOLocalMoveY(card.transform.localPosition.y + 100, yBounceTime)
            .SetEase(Ease.OutQuad)
        );
        s.Insert(yBounceTime,
            card.DOLocalMoveY(card.transform.localPosition.y, yBounceTime)
            .SetEase(Ease.InQuad)
        );
        // Fade out the card
        s.Insert(0.4f,
            card.GetComponent<Image>().DOFade(0, 0.6f)
        );
        // Rotate the card
        s.Insert(0, card.DOLocalRotate(new Vector3(0, 0, -90), 0.5f));
        // Shuffle along the other cards in the array
        // The right one
        if (card.gameObject == uiCards[0])
        {
            float xPosition = cardPositions[0].transform.localPosition.x;
            uiCards[0] = uiCards[1];
            uiCards[0].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
            xPosition = cardPositions[1].transform.localPosition.x;
            uiCards[1] = uiCards[2];
            uiCards[1].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
        }
        // The middle one
        else if (card.gameObject == uiCards[1])
        {
            float xPosition = cardPositions[1].transform.localPosition.x;
            uiCards[1] = uiCards[2];
            uiCards[1].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
        }
    }
}
