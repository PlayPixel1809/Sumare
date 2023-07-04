using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    public static Homescreen ins;
    void Awake() { ins = this; }

    public GameObject muteBtn, unmuteBtn;

    public Text playerBalance;


    public GameplayScreen gameplayScreen;

    void Start()
    {
        if (!PlayerPrefs.HasKey("coins")) { PlayerPrefs.SetInt("coins", 1000); }
        ScreensManager.ins.ActivateScreen(null, gameObject);
    }

    void OnEnable()
    {
        playerBalance.text = PlayerPrefs.GetInt("coins", 1000).ToString();
    }

    public void Play()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        if (PlayerPrefs.GetInt("coins") < 100)
        {
            NoticeUtils.ins.ShowOneBtnAlert("Your balance is low.");
        }
        else 
        {
            ScreensManager.ins.ActivateScreen(gameObject, gameplayScreen.gameObject);
        }
    }

    public void BuyCoins()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        NoticeUtils.ins.ShowTwoBtnAlert("Are you sure you want to purchase coins ? ", (i) =>
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 5000);
            playerBalance.text = PlayerPrefs.GetInt("coins").ToString();
        });
    }



    public void Mute()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        AudioListener.volume = 0;
        muteBtn.SetActive(false);
        unmuteBtn.SetActive(true);
    }

    public void Unmute()
    {
        AudioListener.volume = 1;
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        muteBtn.SetActive(true);
        unmuteBtn.SetActive(false);
    }

    public void Exit()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        NoticeUtils.ins.ShowTwoBtnAlert("Are you sure you want to exit the game ? ", (i)=> 
        {
            if (i == 0) { Application.Quit(); }
        });
        
    }
}
