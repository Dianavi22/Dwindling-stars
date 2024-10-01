using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsObjects : MonoBehaviour
{
    [SerializeField] Vector3 originalPosition;
    [SerializeField] float lerpTime;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator LerpFunction(Quaternion endValue, float duration, Transform transf)
    {
        float time = 0;
        Quaternion startValue = transf.rotation;

        while (time < duration)
        {
            transf.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transf.rotation = endValue;
    }

    public void SandFall(Transform transf, float speed)
    {
        transf.localScale = Vector3.Lerp(originalPosition, originalPosition + new Vector3(6, 6, 5), lerpTime * speed); ;
        lerpTime += Time.deltaTime;
    }
}
