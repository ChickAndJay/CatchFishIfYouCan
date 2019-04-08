using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InLobbyCanvas : MonoBehaviour
{
    #region Singleton
    public static InLobbyCanvas instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public TextMeshProUGUI _goldText;
    public TextMeshProUGUI _foodCountText;
    public TextMeshProUGUI _foodGoldText;
    public TextMeshProUGUI _netCountText;
    public TextMeshProUGUI _netGoldText;
    public TextMeshProUGUI _electricBombCountText;
    public TextMeshProUGUI _electricBombGoldText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start in InLobbyCanvas");
        _goldText.text = GameManagerScript.instance._gold.ToString();
        _foodCountText.text = GameManagerScript.instance._foodCount.ToString();
        _foodGoldText.text = ((int.Parse(_foodCountText.text) + 1) * 200).ToString();
        _netCountText.text = GameManagerScript.instance._net.ToString();
        _netGoldText.text = ((int.Parse(_netCountText.text) + 1) * 200).ToString();
        _electricBombCountText.text = GameManagerScript.instance._electricBombCount.ToString();
        _electricBombGoldText.text = ((int.Parse(_electricBombCountText.text) + 1) * 200).ToString();
    }

    public void BuyFood()
    {
        if (GameManagerScript.instance._gold < int.Parse(_foodGoldText.text))
            return;
        GameManagerScript.instance._foodCount++;
        _foodCountText.text = GameManagerScript.instance._foodCount.ToString();
        _foodGoldText.text = ((int.Parse(_foodCountText.text) + 1) * 200).ToString();

        GameManagerScript.instance._gold -= GameManagerScript.instance._foodCount * 200;
        _goldText.text = GameManagerScript.instance._gold.ToString();
    }

    public void BuyNet()
    {
        if (GameManagerScript.instance._gold < int.Parse(_netGoldText.text))
            return;

        GameManagerScript.instance._net++;
        _netCountText.text = GameManagerScript.instance._net.ToString();
        _netGoldText.text = ((int.Parse(_netCountText.text) + 1) * 200).ToString();

        GameManagerScript.instance._gold -= GameManagerScript.instance._net * 200;
        _goldText.text = GameManagerScript.instance._gold.ToString();
    }

    public void BuyElectricBomb()
    {
        if (GameManagerScript.instance._gold < int.Parse(_electricBombGoldText.text))
            return;

        GameManagerScript.instance._electricBombCount++;
        _electricBombCountText.text = GameManagerScript.instance._electricBombCount.ToString();
        _electricBombGoldText.text = ((int.Parse(_electricBombCountText.text) + 1) * 200).ToString();

        GameManagerScript.instance._gold -= GameManagerScript.instance._electricBombCount * 200;
        _goldText.text = GameManagerScript.instance._gold.ToString();
    }
}
