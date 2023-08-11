using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image cardBg;
    public Image cardBack;
    public Image cardSuit;
    public Text cardTxt;

    public GameObject outline;

    [Space(20)]
    public CardInfo cardInfo;

    public void SetCard(CardInfo cardInfo)
    {
        this.cardInfo = cardInfo;

        cardBack.sprite = cardInfo.cardBack;
        //cardNoTxt.font = deck.cardFront.font;
        cardSuit.sprite = cardInfo.cardSuit.suitSprite;
        cardTxt.color = cardInfo.cardSuit.suitColor;
        cardTxt.text = cardInfo.cardText;
    }

    public void Refresh()
    {
        SetCard(cardInfo);
    }



    public void HideCard()
    {
        cardBg.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(true);
        cardSuit.gameObject.SetActive(false);
        cardTxt.gameObject.SetActive(false);
        RemoveHighlight();
    }

    public void RevealCard()
    {
        gameObject.SetActive(true);
        cardBg.gameObject.SetActive(true);
        cardBack.gameObject.SetActive(false);
        cardSuit.gameObject.SetActive(true);
        cardTxt.gameObject.SetActive(true);
    }

    public void HighlightCard()
    {
        outline.SetActive(true);
    }

    public void RemoveHighlight()
    {
        outline.SetActive(false);
    }

    int GetCardNo(int cardIndex)
    {
        int cardNo = cardIndex - 13 * Mathf.FloorToInt(cardIndex / 13.01f);
        if (cardNo == 1) { cardNo = 14; }
        return cardNo;
    }
    int GetCardSuit(int cardIndex) { return Mathf.FloorToInt(cardIndex / 13.01f); }
}
