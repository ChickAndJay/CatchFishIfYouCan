using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameCanvas : MonoBehaviour
{
    #region Singleton
    public static InGameCanvas instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Image _oxygenImg;
    public TextMeshProUGUI _oxygenTxt;
    public Slider _waveSlider;
    public TextMeshProUGUI _waveTxt;
    
    public TextMeshProUGUI _fishCountTxt;

    public TextMeshProUGUI _foodCountTxt;
    public TextMeshProUGUI _netCountTxt;
    public TextMeshProUGUI _electricBombCountTxt;

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
