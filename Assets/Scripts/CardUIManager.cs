using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardManager))]
public class CardUIManager : MonoBehaviour
{
    // Prefab for jump
    public GameObject jumpCard;
    // Prefab for duck
    public GameObject duckCard;
    // Prefab for change floor
    public GameObject changeFloorCard;
    // The Scroll View for the hand
    public GameObject handView;
    // Reference for CardManager
    private CardManager cardManager;
    // Width of card
    public float cardWidth = 200;
    // Draw card event
    private CardManager.DrawCardDelegate drawCardHandler;

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
        cardManager.useCardInHand(uiCard.card);
        // Remove from the game ui
        Destroy(uiCard.gameObject);
        // Redraw a new card
        cardManager.draw(1);
    }
}
