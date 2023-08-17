using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducationalModes : MonoBehaviour
{
    public static EducationalModes ins;
    void Awake() { ins = this; }

    public enum Modes { BasicNumbers, EvenNumbers , OddNumbers , NegativeNumbers, DivisibleBy, Range, RangeAndDivisibleBy}


    public List<CardInfo.CardSuit> suits;

    public Text selectedMode, selectedRange, modeWinnings;

    public Deck deck;

    public Sprite selected;
    public Sprite unselected;

    public Transform modesBtns;
    

    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedEducationalMode")) { PlayerPrefs.SetString("selectedEducationalMode", "BasicNumbers"); }

        for (int i = 0; i < modesBtns.childCount; i++)
        {
            EducatiopnalModeBtn educatiopnalModeBtn = modesBtns.GetChild(i).GetComponent<EducatiopnalModeBtn>();
            if (educatiopnalModeBtn.mode.ToString() == PlayerPrefs.GetString("selectedEducationalMode")) 
            {
                educatiopnalModeBtn.Start();
                educatiopnalModeBtn.Select();
                break;
            }
        }

        CasinoSumare.ins.onBetWon += (amountWon) => 
        {
            modeWinnings.DOCounter(int.Parse(modeWinnings.text), int.Parse(modeWinnings.text) + amountWon, 1, false);
            PlayerPrefs.SetInt(PlayerPrefs.GetString("selectedEducationalMode") + "ModeWinnings", int.Parse(modeWinnings.text) + amountWon);
        };
    }

    public void DeselectAll()
    {
        for (int i = 0; i < modesBtns.childCount; i++)
        {
            modesBtns.GetChild(i).GetComponent<EducatiopnalModeBtn>().Deselect();
        }
    }

    public void GenerateCardsAccordingToRange(int lowerLimit, int upperLimit, int divisibleBy, int maxCards, int suitCount, string modeName, string rangeName)
    {
        upperLimit += 1;

        List<int> numbers = new List<int>();
        for (int i = lowerLimit; i < upperLimit; i++) 
        {
            if (i != 0 && i %divisibleBy == 0) { numbers.Insert(Random.Range(0, numbers.Count), i); }
        }

        if (maxCards > numbers.Count) { maxCards = numbers.Count; }
        numbers.RemoveRange(maxCards, numbers.Count - maxCards);

        GenerateCards(numbers, suitCount, modeName, rangeName);
    }

    public void GenerateCards(List<int> numbers, int suitCount, string modeName, string rangeName)
    {
        deck.values = numbers;

        deck.suits.Clear();
        for (int i = 0; i < suitCount; i++) { deck.suits.Add(suits[i]); }

        selectedMode.text = modeName;
        selectedRange.text = rangeName;

        deck.CreateCards();
    }
}
