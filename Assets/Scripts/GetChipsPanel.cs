using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GetChipsPanel : MonoBehaviour
{
    public static GetChipsPanel ins;
    void Awake() { ins = this; }

    public Text playerBalance;
    public Text playerBalanceHomeScreen;
    public Text playerBalanceGameplayScreen;

    public AudioClip creditMoneySound;
    private Action onCloseShopPanel;

    void OnEnable()
    {
        playerBalance.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void OpenBuyChipsPanel(Action onCloseShopPanel = null)
    {
        this.onCloseShopPanel = onCloseShopPanel;
        gameObject.SetActive(true);
    }

    public void CloseBuyChipsPanel()
    {
        gameObject.SetActive(false);
        onCloseShopPanel?.Invoke();
    }


    public void Buy1000Chips()
    {
        BuyProduct("chips1000", "Are you sure you want to purchase 1000 chips ?");
    }

    public void Buy5000Chips()
    {
        BuyProduct("chips5000", "Are you sure you want to purchase 5000 chips ?");
    }

    public void Buy10000Chips()
    {
        BuyProduct("chips10000", "Are you sure you want to purchase 10000 chips ?");
    }

    public void WatchAd()
    {
        if (UnityAdsRewarded.ins.adLoaded)
        {
            UnityAdsRewarded.ins.Show(() =>
            {
                AddChips(40);
            });
        }
        else { NoticeUtils.ins.ShowOneBtnAlert("Ad is not available right now, Please check after some time"); }
    }

    public void RemoveAds()
    {
        BuyProduct("remove_ads", "Are you sure you want to purchase Remove Ads ?");
    }

    void BuyProduct(string productId, string purchaseMsg)
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        NoticeUtils.ins.ShowTwoBtnAlert(purchaseMsg, (i) =>
        {
            if (i == 0)
            {
                PaymentGateway.ins.BuyProduct(productId, (id) =>
                {
                    Debug.Log(id);
                    if (id == "chips1000")
                    {
                        AddChips(1000);
                    }

                    if (id == "chips5000")
                    {
                        AddChips(5000);
                    }

                    if (id == "chips10000")
                    {
                        AddChips(10000);
                    }

                    if (id == "remove_ads")
                    {
                        PlayerPrefs.SetInt("adsRemoved", 1);
                        NoticeUtils.ins.ShowOneBtnAlert("Ads are removed");
                    }
                });
            }
        });
    }

    public void AddChips(int amount)
    {
        int playerBalance = PlayerPrefs.GetInt("coins");
        PlayerPrefs.SetInt("coins", playerBalance + amount);
        playerBalanceHomeScreen.text = (playerBalance + amount).ToString();
        playerBalanceGameplayScreen.text = (playerBalance + amount).ToString();

        NoticeUtils.ins.ShowOneBtnAlert("CONGRATULATIONS you have got " + amount + " chips", delegate 
        {
            AudioSource.PlayClipAtPoint(creditMoneySound, Camera.main.transform.position, .75f);
            this.playerBalance.DOCounter(playerBalance, playerBalance + amount, 1, false);
        });
    }
}
