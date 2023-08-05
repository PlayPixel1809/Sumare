using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    public static GameplayScreen ins;
    void Awake() { ins = this; }


    public Text playerBalance;

    public CasinoSumare casinoSumare;
    public Homescreen homescreen;
    public Shop shop;

    void OnEnable()
    {
        playerBalance.text = PlayerPrefs.GetInt("coins").ToString();
        casinoSumare.StartCoroutine("StartGame");
    }

    public void Back()
    {
        NoticeUtils.ins.ShowTwoBtnAlert("Are you sure you want to quit the game ? ", (i) =>
        {
            if (i == 0) 
            {
                CasinoSumare.ins.Reset();
                ScreensManager.ins.ActivateScreen(gameObject, homescreen.gameObject); 
            }
        });

    }

    public void OpenShopBtn()
    {
        Shop.ins.OpenShop(false);

    }
}
