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
    // Counter for the deck
    public Text deckCounterText;
    // Counter for the discard pile
    public Text discardCounterText;
    // Width of card
    public float cardWidth = 188f;
    // X positions of the cards
    private float[] cardPositions = new float[] { 1223f, 951f, 680f };
    // Reference for CardManager
    private CardManager cardManager;
    // List of current cards
    private GameObject[] uiCards = new GameObject[3];

    public void Awake()
    {
        // Store card manager reference
        cardManager = GetComponent<CardManager>();
        // Subscribe to draw card event
        cardManager.drawCardDelegate += DrawCard;
    }

    public void DrawCard(Card newCard)
    {
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

    private void animateCardIn(RectTransform card)
    {
        float xPosition = cardPositions[cardManager.hand.Count - 1];
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
        if (card.position.x > 1)
        {
            float xPosition = cardPositions[0];
            uiCards[0] = uiCards[1];
            uiCards[0].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
            xPosition = cardPositions[1];
            uiCards[1] = uiCards[2];
            uiCards[1].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
        }
        // The middle one
        else if (card.position.x > -1)
        {
            float xPosition = cardPositions[1];
            uiCards[1] = uiCards[2];
            uiCards[1].transform.DOLocalMoveX(xPosition, 1f)
            .SetEase(Ease.OutQuad);
        }
    }
}
