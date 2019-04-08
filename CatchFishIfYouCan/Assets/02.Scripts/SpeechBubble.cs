using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public GameObject _target;
    public Vector3 _offset = new Vector3(2, 2, 0);
    bool _follow;
    float _time = 0;
    float _destroyDuration = 1f;

    // Start is called before the first frame update
    public void TheStart(GameObject target)
    {
        _target = target;
        _follow = true;
        StartCoroutine(SizeUp());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_follow) return;

        if (_time > _destroyDuration)
            StartCoroutine(SizeDown());

        _time += Time.deltaTime;
        transform.position = _target.transform.position + _offset;
    }

    IEnumerator SizeUp()
    {
        float time = 0f;
        float duration = 0.2f;
        Vector3 startSize = Vector3.zero;
        Vector3 endSize = new Vector3(0.3f, 0.3f, 0.3f);

        while(time <= duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startSize, endSize, time / duration);
            yield return null;
        }
    }

    IEnumerator SizeDown()
    {
        float time = 0f;
        float duration = 0.2f;
        Vector3 startSize = new Vector3(0.3f, 0.3f, 0.3f);
        Vector3 endSize = Vector3.zero;

        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startSize, endSize, time / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
