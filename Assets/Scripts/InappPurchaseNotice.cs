using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InappPurchaseNotice : MonoBehaviour
{
    public Text discription;

    private Action<int> action;


    public void ShowNotice(string notice, Action<int> callBack)
    {
        action = null;
        gameObject.SetActive(true);
        discription.text = notice;
        action = callBack; 
    }

    public void NoticeBtn(Transform btn)
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);

        gameObject.SetActive(false);
        action?.Invoke(btn.GetSiblingIndex()); 
    }
}
