using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public int cardValue;
    public string cardText;
    public CardSuit cardSuit;
    public Sprite cardBack;

    [System.Serializable]
    public class CardSuit
    {
        public Sprite suitSprite;
        public Color suitColor;


    }
}
