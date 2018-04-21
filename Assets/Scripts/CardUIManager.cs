using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIManager : MonoBehaviour
{

    // Prefab for jump
    public GameObject jumpCard;
    // Prefab for duck
    public GameObject duckCard;
    // The Scroll View for the hand
    public GameObject handView;
    // Reference for CardManager
    private CardManager cardManager;
    // Width of card
    public float cardWidth = 200;

    public Text cavnasText;

    public void Start()
    {
        cardManager = GetComponent<CardManager>();
        if (cardManager == null)
        {
            Debug.LogError("No card manager found on the same gameobject as CardUIManager");
        }
    }

    public void Update()
    {
        cavnasText.text = "Deck size: " + cardManager.deck.Count + '\n'
        + "Discard size: " + cardManager.discardPile.Count;
    }

    public void DrawCard()
    {
        // Only show a new card if one has been drawn
        if (cardManager.draw(1) == false)
        {
            return;
        }
        int cardIndex = cardManager.hand.Count - 1;
        Card newCard = cardManager.hand[cardIndex];
        GameObject newCardObject = null;
        switch (newCard.type)
        {
            case CardType.Jump:
                newCardObject = jumpCard;
                break;
            case CardType.Duck:
                newCardObject = duckCard;
                break;
            default:
                Debug.LogError("Cannot match new card type: " + newCard.type + " to a game object");
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
        Debug.Log("using card type: " + uiCard.card.type);
        cardManager.useCardInHand(uiCard.card);
        Destroy(uiCard.gameObject);
    }
}
