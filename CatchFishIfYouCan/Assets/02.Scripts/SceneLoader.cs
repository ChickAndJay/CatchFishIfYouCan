using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject _fadeInCanvas;
    public GameObject _fadeInPanel;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInCamera());
    }

    IEnumerator FadeInCamera()
    {
        float time = 0f;
        float duration = 1f;

        _fadeInCanvas.SetActive(true);
        _fadeInPanel.SetActive(true);
        _fadeInPanel.GetComponent<Image>().enabled = true;

        while (time <= duration)
        {
            time += Time.deltaTime;
            _fadeInPanel.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time / duration));
            yield return null;
        }

        _fadeInCanvas.SetActive(false);
        _fadeInPanel.SetActive(false);
        _fadeInPanel.GetComponent<Image>().enabled = false;
    }

    IEnumerator FadeOutCamera(int sceneIndex)
    {
        float time = 0f;
        float duration = 1f;

        _fadeInCanvas.SetActive(true);
        _fadeInPanel.SetActive(true);
        _fadeInPanel.GetComponent<Image>().enabled = true;

        while (time <= duration)
        {
            time += Time.deltaTime;
            _fadeInPanel.GetComponent<Image>().color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time / duration));
            yield return null;
        }

        //_fadeInCanvas.SetActive(false);
        //_fadeInPanel.SetActive(false);
        //_fadeInPanel.GetComponent<Image>().enabled = false;

        SceneManager.LoadScene(sceneIndex);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLobbyScene()
    {
        StartCoroutine(FadeOutCamera(1));
    }

    public void LoadGameScene()
    {
        StartCoroutine(FadeOutCamera(2));
    }

    public void LoadChestScene()
    {
        StartCoroutine(FadeOutCamera(3));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        if (scene.name == "GameScene")
            GameManagerScript.instance.PlayGameMusic();
        else if (scene.name == "LobbyScene")
            GameManagerScript.instance.PlayLobbyMusic();

    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

}
