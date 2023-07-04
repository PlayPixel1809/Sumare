using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreensManager : MonoBehaviour
{
    public static ScreensManager ins;
    void Awake() { ins = this; }

    public Action<GameObject> onScreenDeactivate;
    public Action<GameObject> onScreenActivate;

    void Start()
    {
    }

    public void ActivateScreen(GameObject screenToDeactivate, GameObject screenToActivate,Action onDeactivate = null, Action onActivate = null)
    {
        StartCoroutine(ActivateScreenCoroutine(screenToDeactivate, screenToActivate, onDeactivate, onActivate));
    }

    IEnumerator ActivateScreenCoroutine(GameObject screenToDeactivate, GameObject screenToActivate, Action onDeactivate, Action onActivate)
    {
        if (screenToDeactivate != null)
        {
            if (screenToDeactivate.GetComponent<Animation>() != null)
            {yield return StartCoroutine(PlayAnim(screenToDeactivate.GetComponent<Animation>(), screenToDeactivate.name, true));}

            screenToDeactivate.SetActive(false);
            onDeactivate?.Invoke();
        }


        if (screenToActivate != null)
        {
            screenToActivate.SetActive(true);
            if (screenToActivate.GetComponent<Animation>() != null)
            {yield return StartCoroutine(PlayAnim(screenToActivate.GetComponent<Animation>(), screenToActivate.name));}

            onActivate?.Invoke();
        }
    }

    IEnumerator PlayAnim(Animation animation, string clipName, bool reverse = false)
    {
        animation.Play();
        float time = animation[clipName].length;
        float val = 0;
        while (val < 1)
        {
            val += Time.deltaTime / time;

            if (!reverse)
            { animation[clipName].time = Mathf.Lerp(0, animation[clipName].length, val); }
            else
            { animation[clipName].time = Mathf.Lerp(animation[clipName].length, 0, val); }

            yield return null;
        }
        animation.Stop();
        
    }
}
