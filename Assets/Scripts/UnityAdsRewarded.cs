using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsRewarded : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAdsRewarded ins;
    void Awake() { ins = this; }

    public bool testMode = true;

    public string myGameIdAndroid = "YOUR_GAME_ID_HERE";
    public string myGameIdIOS = "YOUR_GAME_ID_HERE";

    public string adUnitIdAndroid = "Interstitial_Android";
    public string adUnitIdIOS = "Interstitial_iOS";

    [HideInInspector] public bool adLoaded;

    private string myAdUnitId;

    


    private Action onAdComplete;


    void Start()
    {
#if UNITY_IOS
        Advertisement.Initialize(myGameIdIOS, testMode, this);
        myAdUnitId = adUnitIdIOS;
#else
        Advertisement.Initialize(myGameIdAndroid, testMode, this);
        myAdUnitId = adUnitIdAndroid;
#endif
        
        
    }


    public void OnInitializationComplete()
    {
        Debug.Log("OnInitializationComplete");
        Advertisement.Load(myAdUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("OnInitializationFailed , error = " + error + " , message = " + message);
    }




    public void Show(Action onComplete)
    {
        adLoaded = false;
        Advertisement.Load(myAdUnitId, this);
        onAdComplete = onComplete;
        Advertisement.Show(myAdUnitId, this);

    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded");
        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("OnUnityAdsFailedToLoad , error = " + error  + " , message = " + message);
        Advertisement.Load(myAdUnitId, this);
    }






    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure , error = " + error + " , message = " + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        
        onAdComplete?.Invoke();
    }

    
}