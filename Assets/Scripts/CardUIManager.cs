using System.Collections;
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
    // The Scroll View for the hand
    public GameObject handView;
    // The deck for animating
    public GameObject deckImage;
    // The discard pilefor animating
    public GameObject discardPileImage;
    // Width of card
    public float cardWidth = 188f;
    // X positions of the cards
    private float[] cardPositions = new float[] { 680f, 951f, 1223f };
    // Reference for CardManager
    private CardManager cardManager;

    public void Awake()
    {
        // Store card manager reference
        cardManager = GetComponent<CardManager>();
        // Subscribe to draw card event
        cardManager.drawCardDelegate += DrawCard;
    }

    public void DrawCard(Card newCard)
    {
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
        // Remove from the game ui
        Destroy(uiCard.gameObject);
        // Redraw a new card
        cardManager.draw(1);
    }

    private void animateCardIn(RectTransform card)
    {
        float xPosition = cardPositions[cardManager.hand.Count - 1];
        card.DOLocalMoveX(xPosition, 1f).SetEase(Ease.OutQuad);
    }

    private void animateCardToDiscard(RectTransform card)
    {

    }
}
