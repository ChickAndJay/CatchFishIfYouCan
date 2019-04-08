using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScriptInGame : MonoBehaviour
{
    public GameObject _sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBackToHome()
    {
        StartCoroutine(GoBackToHomeRoutine());
        StartCoroutine(CallSceneLoad());
    }

    IEnumerator CallSceneLoad()
    {
        yield return new WaitForSeconds(1.0f);
        _sceneLoader.GetComponent<SceneLoader>().LoadLobbyScene();
    }

    IEnumerator GoBackToHomeRoutine()
    {
        Vector3 startPosition = transform.position;

        while (Vector3.Distance(startPosition, transform.position) < 10)
        {
            transform.Translate(transform.right * Time.deltaTime * 2, Space.World);
            yield return null;
        }

    }
}
