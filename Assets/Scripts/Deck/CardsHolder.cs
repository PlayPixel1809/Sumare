using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsHolder : MonoBehaviour
{
    public float cardAnimTime = 1;
    public bool revealCard;
    public AnimationCurve cardAnimCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    public List<Card> allCards;

    [Header("Assigned During Game -")]
    public List<Card> activeCards = new List<Card>();

    

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
            if (!allCards[i].gameObject.activeSelf) { AddCard(copyFrom.activeCards[i].cardIndex, null, false); }
        }
        if (revealCards) { RevealCards(); } else { HideCards(); }
    }

    public Card AddCard(int cardIndex, Deck deck, bool animate = true)
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeInHierarchy)
            {
                Card inActiveCard = allCards[i];
                inActiveCard.gameObject.SetActive(true);
                inActiveCard.SetCard(cardIndex, deck);
                activeCards.Add(inActiveCard);
                inActiveCard.HideCard();
                if (animate) { StartCoroutine(AnimateCard(inActiveCard.transform, deck)); } 
                else 
                {
                    if (revealCard) { inActiveCard.RevealCard(); } 
                }
                return inActiveCard;
            }
        }

        return null;
    }


    IEnumerator AnimateCard(Transform card, Deck deck)
    {
        AudioSource.PlayClipAtPoint(Deck.ins.cardSound, Camera.main.transform.position);

        Vector3 toPos = card.localPosition;
        Quaternion toRot = card.rotation;

        card.position = deck.cardSpawnPoint.position;
        card.rotation = deck.cardSpawnPoint.rotation;

        Vector3 fromPos = card.localPosition;
        
        Quaternion fromRot = card.rotation;
        Vector3 fromScale = deck.cardScale;
        Vector3 toScale = card.localScale;
        float val = 0;
        while (val < 1)
        {
            val += Time.deltaTime / cardAnimTime;

            card.localPosition = Vector3.Lerp(fromPos, toPos, cardAnimCurve.Evaluate(val));
            card.rotation = Quaternion.Lerp(fromRot, toRot, cardAnimCurve.Evaluate(val));
            card.localScale = Vector3.Lerp(fromScale, toScale, cardAnimCurve.Evaluate(val));
            yield return null;
        }
        if (revealCard) { card.GetComponent<Card>().RevealCard(); } 
    }

    public List<int> GetCardsIndexes()
    {
        List<int> cardsIndexes = new List<int>();
        for (int i = 0; i < activeCards.Count; i++)
        {
            cardsIndexes.Add(activeCards[i].cardIndex);
        }
        return cardsIndexes;
    }
}
