using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldCanvas : MonoBehaviour
{
    #region Singleton
    public static GoldCanvas instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public TextMeshProUGUI _goldTxt;
}
