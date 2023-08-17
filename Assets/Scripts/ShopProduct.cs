using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopProduct : MonoBehaviour
{
    public Shop shop;

    public Shop.ProductTypes productType;

    public string productName;
    public int productPrice;

    public Image bg;
    public GameObject unlockTab, selectTab;
    public Text productPriceText, selectText;

    public Image cardBack;

    void Awake()
    {
        if (string.IsNullOrEmpty(productName)) { productName = transform.GetSiblingIndex().ToString(); }
        if (PlayerPrefs.HasKey(productType.ToString() + productName)) { Unlock(); } else { Lock(); }
    }

    
    public void ProductBtn()
    {
        if (unlockTab.activeInHierarchy)
        {
            if (int.Parse(CasinoSumare.ins.playerBalance.text) < productPrice) { NoticeUtils.ins.ShowOneBtnAlert("Your balance is low."); }
            else
            {
                NoticeUtils.ins.ShowTwoBtnAlert("Do you wanna unlock this product?",(i)=> 
                {
                    if(i == 0)
                    {
                        CasinoSumare.ins.CreditMoneyToPlayer(-productPrice);
                        Select();
                    }
                });
            }
        }

        if (selectTab.activeInHierarchy)
        {
            Select();
        }
    }

    public void Lock()
    {
        unlockTab.SetActive(true);
        selectTab.SetActive(false);
        productPriceText.text = productPrice.ToString();
    }


    public void Unlock()
    {
        if (string.IsNullOrEmpty(productName)) { productName = transform.GetSiblingIndex().ToString(); }
        PlayerPrefs.SetInt(productType.ToString() + productName, 1);
        unlockTab.SetActive(false);
        selectTab.SetActive(true);
    }



    public void Select(bool userSelected = true)
    {
        Unlock();

        PlayerPrefs.SetInt("selected" + productType.ToString() + "Index", transform.GetSiblingIndex());
        for (int i = 0; i < transform.parent.childCount; i++) { transform.parent.GetChild(i).GetComponent<ShopProduct>().Deselect(); }
        
        selectText.text = "SELECTED";
        bg.color = shop.selectedProductColor ;


        if (productType == Shop.ProductTypes.CardBack) { shop.deck.SetCardsBack(cardBack.sprite); }

        if (productType == Shop.ProductTypes.Music) 
        {
            if (userSelected && Homescreen.ins != null) { Homescreen.ins.Unmute(); }
            shop.gameMusic.clip = Resources.Load<AudioClip>(productName);
            shop.gameMusic.Play();
        }

        if (productType == Shop.ProductTypes.Table)
        {
            for (int i = 0; i < shop.tables.childCount; i++) { shop.tables.GetChild(i).gameObject.SetActive(false); }
            shop.tables.GetChild(transform.GetSiblingIndex()).gameObject.SetActive(true);
        }

        if (productType == Shop.ProductTypes.Dealer)
        {
            for (int i = 0; i < shop.dealers.childCount; i++) {shop.dealers.GetChild(i).gameObject.SetActive(false);}
            shop.dealers.GetChild(transform.GetSiblingIndex()).gameObject.SetActive(true);
        }
    }

    public void Deselect()
    {
        bg.color = shop.unselectedProductColor;
        selectText.text = "SELECT";
    }
}
