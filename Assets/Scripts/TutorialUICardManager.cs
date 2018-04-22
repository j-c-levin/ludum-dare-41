using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TutorialCardManager))]
public class TutorialUICardManager : MonoBehaviour
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
    // Reference for CardManager
    private TutorialCardManager cardManager;
    // Width of card
    public float cardWidth = 200;

    public void Awake()
    {
        // Store card manager reference
        cardManager = GetComponent<TutorialCardManager>();
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
        GameObject newUiCard = Instantiate(newCardObject, Vector2.zero, Quaternion.identity);
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
        // For tutorial purposes, hold off on drawing before getting the rock
        if (uiCard.card.type == CardType.Duck)
        {
            return;
        }
        // Redraw a new card
        cardManager.draw(1);
    }
}
