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

    void OnEnable()
    {
        playerBalance.text = PlayerPrefs.GetInt("coins", 25000).ToString();

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
}
