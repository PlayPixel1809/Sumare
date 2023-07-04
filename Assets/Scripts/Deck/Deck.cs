﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public static Deck ins;
    void Awake() { ins = this; }

    public Transform deckModel;

    public Card cardPrefab;
    public Transform cardSpawnPoint;
    public AudioClip cardSound;

    public Sprite cardBack;
    public CardFront cardFront;
    public List<Sprite> cardsFront;

    public Vector3 cardScale = Vector3.one;

    private List<CardsHolder> cardsHolders = new List<CardsHolder>();
    private int cardsDrawnCount;

    private List<int> cardsIndexes = new List<int>();

    [System.Serializable]
    public class CardFront
    {
        public Font font;

        public Sprite spade;
        public Sprite heart;
        public Sprite diamond;
        public Sprite club;
    }

    private Vector3 deckScale;
    void Start()
    {
        deckScale = deckModel.localScale;
        for (int i = 0; i < 52; i++) { cardsIndexes.Add(i + 1);  }
    }

    public void CreateNewCard(int cardIndex, CardsHolder cardsHolder, bool animateCard = true)
    {
        cardsDrawnCount += 1;
        deckModel.localScale = new Vector3(deckModel.localScale.x, Mathf.Lerp(0, deckScale.y, (float)(52 - cardsDrawnCount )/ 52), deckModel.localScale.z);
        cardsHolder.AddCard(cardIndex, this, animateCard);
        cardsHolders.Add(cardsHolder);
    }

    public void CreateRandomCard(CardsHolder cardsHolder, bool animateCard = true)
    {
        int randomCard = cardsIndexes[Random.Range(0, cardsIndexes.Count)];
        cardsIndexes.Remove(randomCard);

        CreateNewCard(randomCard, cardsHolder, animateCard);
    }

    public void Reset()
    {
        cardsIndexes = new List<int>();
        for (int i = 0; i < 52; i++) { cardsIndexes.Add(i + 1); }

        cardsDrawnCount = 0;
        deckModel.localScale = deckScale;
        for (int i = 0; i < cardsHolders.Count; i++) { cardsHolders[i].RemoveCards(); }
        cardsHolders.Clear();
    }
}
