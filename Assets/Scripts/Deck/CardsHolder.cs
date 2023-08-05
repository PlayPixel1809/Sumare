using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardsHolder : MonoBehaviour
{
    public float cardAnimTime = 1;
    public bool revealCard;

    public Transform deckPoint;
    
    public List<Card> allCards;

    [Space(20)]
    public List<Card> activeCards = new List<Card>();

    public void DrawCardFromDeck(Deck deck)
    {
        CardInfo cardInfo = deck.GetRandomCard();
        Card card = GetNewCard();
        card.SetCard(cardInfo);

        Vector3 cardLocalPos = card.transform.localPosition;
        Vector3 cardLocalScale = card.transform.localScale;
        card.transform.position = deckPoint.position;
        card.transform.localScale = deckPoint.localScale;
        card.transform.DOScale(cardLocalScale, cardAnimTime);
        card.transform.DOLocalMove(cardLocalPos, cardAnimTime).OnComplete(delegate 
        {
            if (revealCard) { card.RevealCard(); }
        });
    }


    public void RevealCards()
    {
        for (int i = 0; i < activeCards.Count; i++) { activeCards[i].RevealCard(); }
    }

    public void RevealCard(int index)
    {
        activeCards[index].RevealCard(); 
    }

    public void HighlightCards()
    {
        for (int i = 0; i < activeCards.Count; i++) { activeCards[i].HighlightCard(); }
    }

    public void RemoveCardsHighlight()
    {
        for (int i = 0; i < activeCards.Count; i++) { activeCards[i].RemoveHighlight(); }
    }

    public void HideCards()
    {
        for (int i = 0; i < activeCards.Count; i++) { activeCards[i].HideCard(); }
    }

    public void RemoveCards()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].gameObject.activeInHierarchy)
            {
                Card activeCard = allCards[i];
                activeCard.RemoveHighlight();
                activeCard.gameObject.SetActive(false);
                activeCards.Remove(activeCard);
            }
        }
    }

    public Card GetCard(int index)
    {
        return activeCards[index];
    }

    public void CopyCards(CardsHolder copyFrom, bool revealCards = false)
    {
        for (int i = 0; i < copyFrom.activeCards.Count; i++)
        {
            Card card = GetNewCard();
            card.SetCard(copyFrom.activeCards[i].cardInfo);

            if (revealCards) { card.RevealCard(); } 
        }
    }


    public Card GetNewCard()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
            {
                Card inActiveCard = allCards[i];
                inActiveCard.gameObject.SetActive(true);
                inActiveCard.HideCard();
                activeCards.Add(inActiveCard);
                return inActiveCard;
            }
        }

        return null;
    }


    
}
