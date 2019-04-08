using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    #region Singleton
    public static GameManagerScript instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion


    public int _gold = 5000;
    public int _foodCount = 0;
    public int _net = 0;
    public int _electricBombCount = 0;

    public AudioSource _audioSource;
    public AudioClip _introLobbyAudioClip;
    public AudioClip _inGameClip;

    // Start is called before the first frame update
    void Start()
    {
        PlayLobbyMusic();
    }

    public void PlayLobbyMusic()
    {
        if (_audioSource.clip == _introLobbyAudioClip)
            return;

        _audioSource.Stop();
        _audioSource.clip = _introLobbyAudioClip;
        _audioSource.Play();
    }

    public void PlayGameMusic()
    {
        if (_audioSource.clip == _inGameClip)
            return;

        _audioSource.Stop();
        _audioSource.clip = _inGameClip;
        _audioSource.Play();
    }


}
