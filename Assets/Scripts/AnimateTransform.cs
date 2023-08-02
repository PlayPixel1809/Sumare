using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTransform : MonoBehaviour
{
    public enum TransformTypes {Position, Rotation, Scale}
    public enum RepeatModes { Loop, PingPong }

    public TransformTypes transformType;
    public Space space;
    
    
    public Vector3 from;
    public Vector3 to;

    public float time;

    public Ease ease;

    public RepeatModes repeatMode;

    private void OnEnable()
    {
        StartCoroutine("Animation");
    }

    /*IEnumerator Animation()
    {

    }*/

    void DoTweenAnimation(Vector3 from, Vector3 to)
    {

    }
}
