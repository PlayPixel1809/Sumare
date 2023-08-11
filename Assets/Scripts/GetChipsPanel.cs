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

    public InappPurchaseNotice inappPurchaseNotice;

    public AudioClip creditMoneySound;
    private Action onCloseShopPanel;

    void Start()
    {
        
    }

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
        BuyProduct("chips1000", "Do you want to buy 1000 chips for $.99 ?");
    }

    public void Buy2500Chips()
    {
        BuyProduct("chips2500", "Do you want to buy 2500 chips for $1.99 ?");
    }

    public void Buy10000Chips()
    {
        BuyProduct("chips10000", "Do you want to buy 10000 chips for $4.99 ?");
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
        BuyProduct("remove_ads", "Do you want to unlock Remove Ads for $4.99 ?");
    }

    void BuyProduct(string productId, string purchaseMsg)
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        inappPurchaseNotice.ShowNotice(purchaseMsg, (i) =>
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

                    if (id == "chips2500")
                    {
                        AddChips(2500);
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
