using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyGetCanvas : MonoBehaviour
{
    public TextMeshProUGUI _moneyTxt;
    public Image _coinImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TheStart(int money)
    {
        _moneyTxt.text = "+" + money;
        StartCoroutine(RiseUp());
    }

    IEnumerator RiseUp()
    {
        float time = 0;
        float duration = 1f;
        float alphaValue;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0.6f, 0);

        while(time < duration)
        {
            time += Time.deltaTime;

            alphaValue = Mathf.Lerp(1, 0, time / duration);
            _coinImage.color = new Color(_coinImage.color.r, _coinImage.color.g, _coinImage.color.b, alphaValue);
            _moneyTxt.color = new Color(_coinImage.color.r, _coinImage.color.g, _coinImage.color.b, alphaValue);

            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
                       
            yield return null;
        }

        Destroy(gameObject);
    }
}
