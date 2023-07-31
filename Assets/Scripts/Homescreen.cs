using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    public static Homescreen ins;
    void Awake() 
    { 
        ins = this;
        if (!PlayerPrefs.HasKey("coins")) { PlayerPrefs.SetInt("coins", 1000); }
        if (!PlayerPrefs.HasKey("dailyBonusCollectedTime")) { PlayerPrefs.SetString("dailyBonusCollectedTime", DateTime.Now.ToString()); }
        if (!PlayerPrefs.HasKey("dailyBonusesCollectedCount")) { PlayerPrefs.SetInt("dailyBonusesCollectedCount", 0); }
    }

    public GameObject muteBtn, unmuteBtn, inAppScreen;

    public Text playerBalance, dailyBonus, dailyBonusClock;

    public AudioClip creditMoneySound;
    public GameplayScreen gameplayScreen;

    void Start()
    {
        PlayerPrefs.SetInt("gamesPlayedInThisSession", 0);

        if (!PlayerPrefs.HasKey("adsRemoved")) { PlayerPrefs.SetInt("adsRemoved", 0); }
        ScreensManager.ins.ActivateScreen(null, gameObject);
    }

    void OnEnable()
    {
        playerBalance.text = PlayerPrefs.GetInt("coins", 1000).ToString();
        StartCoroutine("StartDailyBonusTimer");
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

    public void OpenGetChipsPanel()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        inAppScreen.SetActive(true);
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

    public void OpenShopBtn()
    {
        Shop.ins.OpenShop(true);
    }

    public void Exit()
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        NoticeUtils.ins.ShowTwoBtnAlert("Are you sure you want to exit the game ? ", (i)=> 
        {
            if (i == 0) { Application.Quit(); }
        });
        
    }

    public void CreditMoneyToPlayer(int amount)
    {
        if (amount > 0) { AudioSource.PlayClipAtPoint(creditMoneySound, Camera.main.transform.position, .75f); }
        int playerBalance = int.Parse(this.playerBalance.text);
        this.playerBalance.DOCounter(playerBalance, playerBalance + amount, 1, false);
        PlayerPrefs.SetInt("coins", playerBalance + amount);
    }


    public void CollectDailyBonus()
    {
        if (dailyBonusClock.text == "COLLECT")
        {
            PlayerPrefs.SetString("dailyBonusCollectedTime", DateTime.Now.ToString());

            PlayerPrefs.SetInt("dailyBonusesCollectedCount", PlayerPrefs.GetInt("dailyBonusesCollectedCount") + 1);
            if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 7) { PlayerPrefs.SetInt("dailyBonusesCollectedCount", 0); }

            CreditMoneyToPlayer(int.Parse(dailyBonus.text));
            dailyBonus.DOCounter(int.Parse(dailyBonus.text), 0, 1, false);

            Utils.InvokeDelayedAction(2, delegate { StartCoroutine("StartDailyBonusTimer"); });
        }
        else { NoticeUtils.ins.ShowOneBtnAlert("You collected the bonus for today , please check after some time"); }
    }

    IEnumerator StartDailyBonusTimer()
    {
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 0) { dailyBonus.text = "200"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 1) { dailyBonus.text = "500"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 2) { dailyBonus.text = "1000"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 3) { dailyBonus.text = "2000"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 4) { dailyBonus.text = "3000"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 5) { dailyBonus.text = "5000"; }
        if (PlayerPrefs.GetInt("dailyBonusesCollectedCount") == 6) { dailyBonus.text = "10000"; }

        int bonusCollectionTime = (int)Math.Floor(DateTime.Now.Subtract(DateTime.Parse(PlayerPrefs.GetString("dailyBonusCollectedTime"))).TotalSeconds);
        //int timer = 300 - bonusCollectionTime; //5 mins
        int timer = 86400 - bonusCollectionTime; //24 hours
        Debug.Log(bonusCollectionTime);
        while (timer > 0)
        {
            TimeSpan t = TimeSpan.FromSeconds(timer);
            dailyBonusClock.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            yield return new WaitForSeconds(1);
            timer -= 1;
        }
        dailyBonusClock.text = "COLLECT";
    }
}
