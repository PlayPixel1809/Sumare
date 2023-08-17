using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public enum ProductTypes { CardBack, Music, Table, Dealer }

    public static Shop ins;
    void Awake() { ins = this; }

    public Color selectedProductColor;
    public Color unselectedProductColor;

    public GetChipsPanel getChipsPanel;
    public GameplayScreen gameplayScreen;
    public GameObject dimmer;
    public RectTransform shopPanel;

    public Transform allProducts;
    public Transform productsBtns;

    public Transform cardBackProducts;
    public Transform musicProducts;
    public Transform tableProducts;
    public Transform dealerProducts;

    public AudioSource gameMusic;
    public Deck deck;
    public Transform tables;
    public Transform dealers;

    

    void Start()
    {
        shopPanel.DOAnchorPosY(-1500, .00005f);

        cardBackProducts.GetChild(0).GetChild(0).GetChild(PlayerPrefs.GetInt("selectedCardBackIndex",0)).GetComponent<ShopProduct>().Select();
        musicProducts.GetChild(0).GetChild(0).GetChild(PlayerPrefs.GetInt("selectedMusicIndex",0)).GetComponent<ShopProduct>().Select(false);
        tableProducts.GetChild(0).GetChild(0).GetChild(PlayerPrefs.GetInt("selectedTableIndex",0)).GetComponent<ShopProduct>().Select();
        dealerProducts.GetChild(0).GetChild(0).GetChild(PlayerPrefs.GetInt("selectedDealerIndex",0)).GetComponent<ShopProduct>().Select();
    }

    public void ShowProducts(Transform btn)
    {
        for (int i = 0; i < allProducts.childCount; i++) { allProducts.GetChild(i).gameObject.SetActive(false); }
        for (int i = 0; i < productsBtns.childCount; i++) { productsBtns.GetChild(i).GetChild(0).gameObject.SetActive(true); }

        allProducts.GetChild(btn.GetSiblingIndex()).gameObject.SetActive(true);
        btn.GetChild(0).gameObject.SetActive(false);
    }

    public void OpenShop(bool showDimmer)
    {
        GameUtils.ins.PlayBtnSound();
        shopPanel.gameObject.SetActive(true);
        dimmer.SetActive(showDimmer);
        shopPanel.DOAnchorPosY(-690, .5f);
    }

    public void CloseShop()
    {
        GameUtils.ins.PlayBtnSound();
        
        shopPanel.DOAnchorPosY(-1500, .5f).OnComplete( delegate 
        { 
            dimmer.SetActive(false);
            shopPanel.gameObject.SetActive(false);
        });
    }

    public void OpenGetChipsPanel()
    {
        deck.gameObject.SetActive(false);
        getChipsPanel.OpenBuyChipsPanel(delegate 
        {
            if (gameplayScreen.gameObject.activeInHierarchy) { deck.gameObject.SetActive(true); }
        });
    }
}
