using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCamera : MonoBehaviour
{
    Camera _camera;
    public GameObject _resultCanvas;
    public TouchPad _touchPad;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        _touchPad = GetComponentInChildren<TouchPad>();

        StartCoroutine(CloseUpCamera());
    }

    IEnumerator CloseUpCamera()
    {
        float time = 0;
        float duration = 1.5f;
        Vector3 startPos = new Vector3(-3.5f,18,-10);
        Vector3 endPos = new Vector3(0, 0, -10);
        float startCameraSize = 3;
        float endCameraSize = 5;

        while(time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            _camera.orthographicSize = Mathf.Lerp(startCameraSize, endCameraSize, time / duration);
            yield return null;
        }
    }

    public void EndGame()
    {
        StartCoroutine(EndGameMove());
    }

    IEnumerator EndGameMove()
    {
        float time = 0;
        float duration = 2;
        Vector3 startPos = new Vector3(0, 0, -10);
        Vector3 endPos = new Vector3(0, 30, -10);

        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            yield return null;
        }

        _touchPad._isEndGame = true;
        _resultCanvas.SetActive(true);
    }
}
