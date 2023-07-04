using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetSlot : MonoBehaviour
{
    public GameObject chips, highlight;
    public Text betAmount;

    private int index;

    void Start()
    {
        index = transform.GetSiblingIndex();
    }

    public void BetSlotBtn()
    {
        if (!CasinoSumare.ins.betTypeBtns[0].interactable) { return; }
        CasinoSumare.ins.BetTypeBtn(CasinoSumare.ins.betTypeBtns[transform.GetSiblingIndex()]);
    }

    public void MakeBet(int amount)
    {
        chips.gameObject.SetActive(true);
        betAmount.gameObject.SetActive(true);
        betAmount.text = amount.ToString();
    }

    public void Deactivate()
    {
        chips.gameObject.SetActive(false);
        betAmount.gameObject.SetActive(false);
    }

  
    public int GetBetAmount()
    {
        return int.Parse(betAmount.text);
    }

    public void DecreaseBet()
    {
        chips.gameObject.SetActive(false);
        betAmount.gameObject.SetActive(false);
    }

    public void Highlight()
    {
        highlight.SetActive(true);
    }

    public void RemoveHighlight()
    {
        highlight.SetActive(false);
    }

   
    public IEnumerator EvaluateBetCoroutine()
    {
        Highlight();
        AudioSource.PlayClipAtPoint(CasinoSumare.ins.select, Camera.main.transform.position, .75f);
        yield return new WaitForSeconds(1);

        int dealerCard1No = CasinoSumare.ins.dealerCards.GetCard(0).cardNo;
        int dealerCard2No = CasinoSumare.ins.dealerCards.GetCard(1).cardNo;
        int playerCardNo = CasinoSumare.ins.playerCards.GetCard(0).cardNo;

        if (playerCardNo == dealerCard1No && playerCardNo == dealerCard2No)
        {
            CasinoSumare.ins.SetBigText("DRAW");
            CasinoSumare.ins.CreditMoneyToPlayer(int.Parse(this.betAmount.text));
            betAmount.DOCounter(int.Parse(betAmount.text), 0, 1);
            yield return new WaitForSeconds(2);
        }
        else
        {
            if (index == 0)
            {
                if (playerCardNo > dealerCard1No && playerCardNo > dealerCard2No) 
                { yield return StartCoroutine("BetWon"); }
                else
                { yield return StartCoroutine("BetLost"); }
            }

            if (index == 1)
            {
                if ((playerCardNo > dealerCard1No && playerCardNo < dealerCard2No) || (playerCardNo < dealerCard1No && playerCardNo > dealerCard2No))
                { yield return StartCoroutine("BetWon"); }
                else
                { yield return StartCoroutine("BetLost"); }
            }

            if (index == 2)
            {
                if (playerCardNo < dealerCard1No && playerCardNo < dealerCard2No)
                { yield return StartCoroutine("BetWon"); }
                else
                { yield return StartCoroutine("BetLost"); }

            }
        }

        RemoveHighlight();

    }

    public IEnumerator BetWon()
    {
        CasinoSumare.ins.amountWon += int.Parse(this.betAmount.text) ;
        AudioSource.PlayClipAtPoint(CasinoSumare.ins.wonSound, Camera.main.transform.position, .3f);
        CasinoSumare.ins.SetBigText("YOU WON");

        betAmount.text = (int.Parse(this.betAmount.text) * 2).ToString();
        yield return new WaitForSeconds(1);
        CasinoSumare.ins.CreditMoneyToPlayer(int.Parse(betAmount.text));
        betAmount.DOCounter(int.Parse(betAmount.text), 0, 1, false);
        yield return new WaitForSeconds(1);
        CasinoSumare.ins.SetBigText("");
        yield return new WaitForSeconds(1);
    }

    public IEnumerator BetLost()
    {
        CasinoSumare.ins.amountWon -= int.Parse(betAmount.text);

        AudioSource.PlayClipAtPoint(CasinoSumare.ins.lostSound, Camera.main.transform.position, .3f);
        CasinoSumare.ins.SetBigText("YOU LOST");
        yield return new WaitForSeconds(1.25f);
        CasinoSumare.ins.SetBigText("");
    }
}
