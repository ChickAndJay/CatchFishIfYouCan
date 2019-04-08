using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SizeDown());
    }

    IEnumerator SizeDown()
    {
        float time = 0;
        float duration = 0.3f;
        Vector3 startSize = transform.localScale;
       
        while(time <= duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startSize, Vector3.zero, time / duration);
            yield return null;
        }

        Destroy(gameObject);
    }
}
