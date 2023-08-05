using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck ins;
    void Awake() 
    { 
        ins = this;
        CreateCards();
    }

    public Image cardBack;
    public List<int> values;
    public List<Sprite> suits;

    public List<TextOverride> cardTextOverrides;
    public List<ValueOverride> cardValueOverrides;


    public Transform deckModel;
    public AudioClip cardSound;

    [Space(20)]
    public List<CardInfo> allCards;
    public List<CardInfo> drawableCards;



    [System.Serializable]
    public class TextOverride
    {
        public string text;
        public string newText;
    }

    [System.Serializable]
    public class ValueOverride
    {
        public int value;
        public int newValue;
    }

    

    private Vector3 deckScale;
    void Start()
    {
        deckScale = deckModel.localScale;
    }

    public void CreateCards()
    {
        allCards = new List<CardInfo>();
        drawableCards = new List<CardInfo>();
        for (int i = 0; i < suits.Count; i++)
        {
            for (int j = 0; j < values.Count; j++)
            {
                CardInfo cardInfo = new CardInfo();
                cardInfo.cardValue = values[j];
                cardInfo.cardSuit = suits[i];
                cardInfo.cardText = values[j].ToString();
                cardInfo.cardBack = cardBack.sprite;
                ApplyTextOverrides(cardInfo);
                ApplyValueOverrides(cardInfo);

                allCards.Add(cardInfo);
                drawableCards.Add(cardInfo);
            }
        }
    }


    public void ApplyTextOverrides(CardInfo cardInfo)
    {
        for (int i = 0; i < cardTextOverrides.Count; i++)
        {
            if (cardInfo.cardText == cardTextOverrides[i].text)
            {
                cardInfo.cardText = cardTextOverrides[i].newText;
            }
        }
    }


    public void ApplyValueOverrides(CardInfo cardInfo)
    {
        for (int i = 0; i < cardValueOverrides.Count; i++)
        {
            if (cardInfo.cardValue == cardValueOverrides[i].value)
            {
                cardInfo.cardValue = cardValueOverrides[i].newValue;
            }
        }
    }


    
    public CardInfo GetRandomCard()
    {
        AudioSource.PlayClipAtPoint(cardSound, Camera.main.transform.position);

        CardInfo card = drawableCards[Random.Range(0, drawableCards.Count)];
        drawableCards.Remove(card);

        int cardsCount = allCards.Count;
        int cardsDrawnCount = allCards.Count - drawableCards.Count;
        deckModel.localScale = new Vector3(deckModel.localScale.x, Mathf.Lerp(0, deckScale.y, (float)(cardsCount - cardsDrawnCount) / cardsCount), deckModel.localScale.z);
        return card;
    }
    

    public void ResetDeck()
    {
        drawableCards = new List<CardInfo>();
        for (int i = 0; i < allCards.Count; i++) { drawableCards.Add(allCards[i]); }

        deckModel.localScale = deckScale;

        CardsHolder[] cardsHolders = FindObjectsOfType<CardsHolder>();
        for (int i = 0; i < cardsHolders.Length; i++){cardsHolders[i].RemoveCards();}
    }

    public void SetCardsBack(Sprite sprite)
    {
        cardBack.sprite = sprite;

        for (int i = 0; i < allCards.Count; i++) { allCards[i].cardBack = sprite; }

        Card[] cards = FindObjectsOfType<Card>();
        for (int i = 0; i < cards.Length; i++)  { cards[i].Refresh(); }
    }
}
