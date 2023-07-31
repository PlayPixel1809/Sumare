using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasinoSumare : MonoBehaviour
{
    public static CasinoSumare ins;
    void Awake() { ins = this; }

    public Text playerBalance, smallText, bigText;

    public List<Deck> decks;

    public CardsHolder dealerCards;
    public CardsHolder playerCards;

    public List<Button> betTypeBtns;
    public List<Button> betBtns;
    public Button betSkipBtn;

    public Transform bettingSlots, rows, rowHighlighter;
    public AudioClip btnSound, creditMoneySound, wonSound, lostSound, select, netGain;

    public int amountWon;

    private List<BetSlot> betsMade = new List<BetSlot>();
    private int betIndex;
    private BetSlot activeBetSlot;
    


    public IEnumerator StartGame()
    {
        PlayerPrefs.SetInt("gamesPlayedInThisSession", PlayerPrefs.GetInt("gamesPlayedInThisSession") + 1);

        if (PlayerPrefs.GetInt("gamesPlayedInThisSession") == 6 && PlayerPrefs.GetInt("adsRemoved") == 0) 
        { 
            if (UnityAdsRewarded.ins.adLoaded) { UnityAdsRewarded.ins.Show(null); }
            PlayerPrefs.SetInt("gamesPlayedInThisSession", 0);
        }

        yield return new WaitForSeconds(1);

        decks[Random.Range(0, decks.Count)].CreateRandomCard(playerCards);
        yield return new WaitForSeconds(1);
        //decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);

        yield return new WaitForSeconds(1);


        for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = true; }
        SetSmallText("Tap on the Bet Area to place Low, Middle or High bet.");
        rowHighlighter.gameObject.SetActive(true);
        rowHighlighter.position = rows.GetChild(betIndex).position;
    }

    public void BetTypeBtn(Button button)
    {
        AudioSource.PlayClipAtPoint(btnSound, Camera.main.transform.position, .5f);
        rowHighlighter.gameObject.SetActive(false);

        if (betIndex == 0) { SetSmallText("Press '+' , '-' Buttons to raise or reduce the bet and BET Button to finalize the bet."); }

        for (int i = 0; i < bettingSlots.childCount; i++)
        {
            bettingSlots.GetChild(betIndex).GetChild(i).GetComponent<BetSlot>().Deactivate();
        }

        activeBetSlot = bettingSlots.GetChild(betIndex).GetChild(betTypeBtns.IndexOf(button)).GetComponent<BetSlot>();

        if (betIndex == 0) { activeBetSlot.MakeBet(ValidateBetAmount(100)); }
        if (betIndex == 1 || betIndex == 2) { activeBetSlot.MakeBet(ValidateBetAmount(betsMade[0].GetBetAmount()/2)); }

        for (int i = 0; i < betBtns.Count; i++) { betBtns[i].interactable = true; }

    }

    public void BetFinalizeBtn()
    {
        StartCoroutine("BetFinalizeBtnCoroutine");
        
    }

    public IEnumerator BetFinalizeBtnCoroutine()
    {
        AudioSource.PlayClipAtPoint(btnSound, Camera.main.transform.position);
        smallText.gameObject.SetActive(false);

        playerBalance.text = (int.Parse(playerBalance.text) - activeBetSlot.GetBetAmount()).ToString();
        PlayerPrefs.SetInt("coins", int.Parse(playerBalance.text));

        betSkipBtn.interactable = false;
        for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = false; }
        for (int i = 0; i < betBtns.Count; i++) { betBtns[i].interactable = false; }

        betsMade.Add(activeBetSlot);
        activeBetSlot = null;
        betIndex += 1;

        yield return new WaitForSeconds(1);
        
        if (betIndex == 1)
        {
            playerCards.RevealCard(0);


            if (int.Parse(this.playerBalance.text) > 0)
            {
                for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = true; }
                betSkipBtn.interactable = true;
                rowHighlighter.gameObject.SetActive(true);
                rowHighlighter.position = rows.GetChild(betIndex).position;
            }
            else 
            {
                decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
                decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
                yield return new WaitForSeconds(1);
                StartCoroutine("ShowdownCoroutine"); 
            }
                
        }

        if (betIndex == 2)
        {
            decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
            yield return new WaitForSeconds(1);

            if (int.Parse(this.playerBalance.text) > 0)
            {
                for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = true; }
                betSkipBtn.interactable = true;
                rowHighlighter.gameObject.SetActive(true);
                rowHighlighter.position = rows.GetChild(betIndex).position;
            }
            else 
            {
                decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
                yield return new WaitForSeconds(1);
                StartCoroutine("ShowdownCoroutine"); 
            }
        }

        if (betIndex == 3) 
        {
            decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
            rowHighlighter.gameObject.SetActive(false);
            StartCoroutine("ShowdownCoroutine");
        }


    }

    public void BetUpBtn()
    {
        AudioSource.PlayClipAtPoint(btnSound, Camera.main.transform.position);
        int betAmount = ValidateBetAmount(activeBetSlot.GetBetAmount() + 100);
        activeBetSlot.MakeBet (betAmount);
    }

    public void BetDownBtn()
    {
        AudioSource.PlayClipAtPoint(btnSound, Camera.main.transform.position);
        int betAmount = ValidateBetAmount(activeBetSlot.GetBetAmount() - 100);
        activeBetSlot.MakeBet(betAmount);
    }

    public void BetSkipBtn()
    {
        AudioSource.PlayClipAtPoint(btnSound, Camera.main.transform.position);
        StartCoroutine("BetSkipBtnCoroutine");
    }

    public IEnumerator BetSkipBtnCoroutine()
    {
        betSkipBtn.interactable = false;
        rowHighlighter.gameObject.SetActive(false);
        if (activeBetSlot != null) { activeBetSlot.Deactivate(); }

        for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = false; }
        for (int i = 0; i < betBtns.Count; i++) { betBtns[i].interactable = false; }


        if(betIndex == 1)
        {
            decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
            decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
            yield return new WaitForSeconds(1);
        }

        if (betIndex == 2)
        {
            decks[Random.Range(0, decks.Count)].CreateRandomCard(dealerCards);
            yield return new WaitForSeconds(1);
        }

        StartCoroutine("ShowdownCoroutine");
    }

  

    public IEnumerator ShowdownCoroutine()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < betsMade.Count; i++)
        {
            yield return betsMade[i].StartCoroutine(betsMade[i].EvaluateBetCoroutine());
        }
        if (amountWon > 0) { AudioSource.PlayClipAtPoint(netGain, Camera.main.transform.position); }

        yield return new WaitForSeconds(1);

        Reset();
        if (int.Parse(this.playerBalance.text) >= 100) { StartCoroutine("StartGame"); }
        else 
        {
            NoticeUtils.ins.ShowOneBtnAlert("Your balance is low.", (i) => { ScreensManager.ins.ActivateScreen(GameplayScreen.ins.gameObject, GameplayScreen.ins.homescreen.gameObject); });
        }
    }



    public void CreditMoneyToPlayer(int amount)
    {
        if (amount > 0) { AudioSource.PlayClipAtPoint(creditMoneySound, Camera.main.transform.position, .75f); }
        int playerBalance = int.Parse(this.playerBalance.text);
        this.playerBalance.DOCounter(playerBalance, playerBalance + amount, 1, false);
        PlayerPrefs.SetInt("coins", playerBalance + amount);
    }

    public void SetSmallText(string text)
    {
        bigText.gameObject.SetActive(false);
        smallText.gameObject.SetActive(true);
        smallText.text = text;
    }

    public void SetBigText(string text)
    {
        smallText.gameObject.SetActive(false);
        bigText.gameObject.SetActive(true);
        bigText.text = text;
    }

    public int ValidateBetAmount(int betAmount)
    {
        if (betIndex == 0) { betAmount = Mathf.Clamp(betAmount, 100, int.Parse(playerBalance.text)); }
        if (betIndex == 1) { betAmount = Mathf.Clamp(betAmount, 50, betsMade[0].GetBetAmount() / 2); }
        if (betIndex == 2) { betAmount = Mathf.Clamp(betAmount, 50, betsMade[1].GetBetAmount()); }

        //if (betAmount > 500) { betAmount = 500; }
        if (betAmount > int.Parse(playerBalance.text)) { betAmount = int.Parse(playerBalance.text); }

        return betAmount;
    }

    public void Reset()
    {
        amountWon = 0;
        betsMade.Clear();
        betIndex = 0;
        activeBetSlot = null;
        
        StopCoroutine("StartGame");
        StopCoroutine("BetFinalizeBtnCoroutine");
        StopCoroutine("BetSkipBtnCoroutine");
        StopCoroutine("ShowdownCoroutine");

        betSkipBtn.interactable = false;
        for (int i = 0; i < betTypeBtns.Count; i++) { betTypeBtns[i].interactable = false; }
        for (int i = 0; i < betBtns.Count; i++) { betBtns[i].interactable = false; }

        smallText.gameObject.SetActive(false);
        bigText.gameObject.SetActive(false);

        for (int i = 0; i < decks.Count; i++) { decks[i].Reset(); }

        BetSlot[] betSlots = FindObjectsOfType<BetSlot>();
        for (int i = 0; i < betSlots.Length; i++) { betSlots[i].Deactivate(); }
    }
}
