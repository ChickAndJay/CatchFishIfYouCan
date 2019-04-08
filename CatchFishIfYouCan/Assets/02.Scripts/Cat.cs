using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Cat : MonoBehaviour
{
    #region Singleton
    public static Cat instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    public int _startOxygen { get; private set; }

    Gun _gun;

    List<Fish> _catchedFish = new List<Fish>();
    public int _gold;

    public int _arrowCount = 0;
    int _fishCount = 0;

    public int _comboCount = 0;
    float _comboTime = 3f;
    float _comboLimitTime = 2f;

    float _waveTime = 0;    
    float _autoWaveReduceTime = 4f;
    int _waveCount = 1;
    float _currentWaveRatio = 1;
    int _MaxValueForWave = 500;
    float _remainingValue = 0;

    public GameObject _spawnObject;

    Coroutine _decreaseGageRoutine;
    Coroutine _waveSliderRoutine;
    Coroutine _comboPopUpRoutine;

    public GameObject _moneyGetCanvasPrefab;
    public GameObject _moneyCanvasSpawnPos;

    public GameObject _comboCanvas;
    public GameObject _comboParticlePrefab;

    bool _start = false;

    public GameObject _endGameBoat;
    public GameObject _mainCamera;
    public GameObject _catchedFishListPanel;
    public GameObject _catchedFishUIPrefab;

    public TextMeshProUGUI _totalCoin;
    public TextMeshProUGUI _totalFishCount;

    class FishResultScore {
        public Sprite sprite;
        public int category;
        public int count;
        public int money;
        public float maxSize;
    }

    Dictionary<int, FishResultScore> _fishResultScores = new Dictionary<int, FishResultScore>();

    AudioSource _audioSource;
    public AudioClip _coinClip;
    public AudioClip _jellyFishHitClip;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _startOxygen = 99;
        _gun = GetComponentInChildren<Gun>();

        _gold = GameManagerScript.instance._gold;

        GoldCanvas.instance._goldTxt.text = _gold.ToString();

        InGameCanvas.instance._fishCountTxt.text = _fishCount.ToString();
        InGameCanvas.instance._oxygenTxt.text = _startOxygen.ToString();
        InGameCanvas.instance._oxygenImg.fillAmount = (float)_startOxygen / 100;
        InGameCanvas.instance._waveSlider.value = 0;
        InGameCanvas.instance._waveTxt.text = "WAVE " + _waveCount;

        _waveTime = 2.5f;

        StartCoroutine(GoDownUnderSea());

    }

    IEnumerator GoDownUnderSea ()
    {
        float time = 0;
        float duration = 1.5f;
        Vector3 startPos = new Vector3(-6, 18, 0);
        Vector3 endPos = new Vector3(-6, -1.65f, 0);
        while(time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            yield return null;
        }

        _start = true;
    }


    public void EndGame()
    {
        Debug.Log("Cat.EndGame()");
        InGameCanvas.instance.gameObject.SetActive(false);
        StartCoroutine(EndGameMoveRoutine());
        _mainCamera.GetComponent<InGameCamera>().EndGame();


        // make result canvas 
        for (int i=0; i<_catchedFish.Count; i++)
        {
            if (!_fishResultScores.ContainsKey(_catchedFish[i]._fishCategory._category))
            {
                _fishResultScores.Add(_catchedFish[i]._fishCategory._category, new FishResultScore());
                _fishResultScores[_catchedFish[i]._fishCategory._category].sprite = _catchedFish[i]._fishCategory._sprite;
                _fishResultScores[_catchedFish[i]._fishCategory._category].category = _catchedFish[i]._fishCategory._category;
                _fishResultScores[_catchedFish[i]._fishCategory._category].count = 1;
                _fishResultScores[_catchedFish[i]._fishCategory._category].money = _catchedFish[i]._gold;
                _fishResultScores[_catchedFish[i]._fishCategory._category].maxSize = _catchedFish[i]._size;

            }
            else
            {
                _fishResultScores[_catchedFish[i]._fishCategory._category].count++;
                _fishResultScores[_catchedFish[i]._fishCategory._category].money += _catchedFish[i]._gold;
                if (_fishResultScores[_catchedFish[i]._fishCategory._category].maxSize < _catchedFish[i]._size)
                {
                    _fishResultScores[_catchedFish[i]._fishCategory._category].maxSize = _catchedFish[i]._size;
                }

            }
        }

        int j = 0;
        int totalGold = 0;
        int totalFishCount = 0;

        Dictionary<int, FishResultScore>.Enumerator enumerator = _fishResultScores.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, FishResultScore> keyValuePair = enumerator.Current;

            GameObject catchedFishUI = Instantiate(_catchedFishUIPrefab, _catchedFishListPanel.transform);
            float height = 70;
            Debug.Log(j);
            catchedFishUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -height * j );
            j++;

            catchedFishUI.GetComponent<CatchedFish>()._count.text = " x " + keyValuePair.Value.count.ToString();
            catchedFishUI.GetComponent<CatchedFish>()._fishIcon.sprite = keyValuePair.Value.sprite;
            float truncate = Mathf.Round( keyValuePair.Value.maxSize * 100)  / 100;
            catchedFishUI.GetComponent<CatchedFish>()._longestFish.text = truncate.ToString() + " m";
            catchedFishUI.GetComponent<CatchedFish>()._money.text = keyValuePair.Value.money.ToString();

            totalGold += keyValuePair.Value.money;
            totalFishCount += keyValuePair.Value.count;
        }
        //GameManagerScript.instance._gold;
        _totalCoin.text = totalGold.ToString();        
        _totalFishCount.text = " x " + totalFishCount.ToString();

    }

    IEnumerator EndGameMoveRoutine()
    {
        float time = 0;
        float duration = 0.75f;
        Vector3 startPos = new Vector3(-6, -1.65f, 0);
        Vector3 midPos = new Vector3(-6, 28.6f, 0);
        Vector3 endPos = new Vector3(-1, 28.6f, 0);

        yield return new WaitForSeconds(0.25f);

        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, midPos, time / duration);
            yield return null;
        }
        time = 0;
        while (time <= duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(midPos, endPos, time / duration);
            yield return null;
        }

        transform.SetParent(_endGameBoat.transform);
    }

    private void Update()
    {
        if (!_start) return;

        _comboTime += Time.deltaTime;
        _waveTime += Time.deltaTime;

        if (_waveTime > _autoWaveReduceTime)
        {
            _waveTime = 0f;
            if (_waveSliderRoutine != null)
                StopCoroutine(_waveSliderRoutine);
            _waveSliderRoutine = StartCoroutine(DecreaseWaveSlider(0.1f));
        }
    }

    public void Fire(Vector3 firePoint)
    {
        int consumingOxygen = _gun.Fire(firePoint);
        if (consumingOxygen == -1)
            return;        
        _startOxygen -= consumingOxygen;
        if (_startOxygen <= 0)
        {
            _startOxygen = 0;
        }
        InGameCanvas.instance._oxygenTxt.text = _startOxygen.ToString();
        InGameCanvas.instance._oxygenImg.fillAmount = (float)_startOxygen / 100;

    }

    public void AddFish(GameObject fish)
    {
        int gold = fish.GetComponent<Fish>()._gold;

        if (_audioSource.isPlaying)
            _audioSource.Stop();
        _audioSource.clip = _coinClip;
        _audioSource.Play();

        _catchedFish.Add(fish.GetComponent<Fish>());
        _gold += gold;
        GameManagerScript.instance._gold = _gold;
        GoldCanvas.instance._goldTxt.text = _gold.ToString();
        GameObject moneyCanvas = Instantiate(_moneyGetCanvasPrefab, _moneyCanvasSpawnPos.transform.position, Quaternion.identity);
        moneyCanvas.GetComponent<MoneyGetCanvas>().TheStart(gold);
        
        _fishCount++;
        InGameCanvas.instance._fishCountTxt.text = _fishCount.ToString();
        float waveAmount = (float)gold / _MaxValueForWave;

        if (_waveSliderRoutine != null)
            StopCoroutine(_waveSliderRoutine);
        StartCoroutine(DecreaseWaveSlider(waveAmount));
    }

    public void AddFishes(List<GameObject> fishes)
    {
        int gold = 0;

        for (int i = 0; i < fishes.Count; i++)
        {
            gold += fishes[i].GetComponent<Fish>()._gold;
            _catchedFish.Add(fishes[i].GetComponent<Fish>());
            _fishCount++;
        }

        _audioSource.Play();

        GameManagerScript.instance._gold = _gold;
        GoldCanvas.instance._goldTxt.text = _gold.ToString();
        GameObject moneyCanvas = Instantiate(_moneyGetCanvasPrefab, _moneyCanvasSpawnPos.transform.position, Quaternion.identity);
        moneyCanvas.GetComponent<MoneyGetCanvas>().TheStart(gold);

        InGameCanvas.instance._fishCountTxt.text = _fishCount.ToString();

        float waveAmount = (float)gold / _MaxValueForWave;

        if (_waveSliderRoutine != null)
            StopCoroutine(_waveSliderRoutine);
        StartCoroutine(DecreaseWaveSlider(waveAmount));

    }

    public void HitJellyFish(GameObject jellyFish)
    {
        Debug.Log("HitJellyFish");
        int consumingOxygen = jellyFish.GetComponent<Fish>()._fishCategory._jellyFishDamage;
        if (consumingOxygen == -1)
            return;

        if (_audioSource.isPlaying)
            _audioSource.Stop();
        _audioSource.clip = _jellyFishHitClip;
        _audioSource.Play();

        _startOxygen -= consumingOxygen;
        InGameCanvas.instance._oxygenTxt.text = _startOxygen.ToString();
        InGameCanvas.instance._oxygenImg.fillAmount = (float)_startOxygen / 100;
    }

    public void HitCombo()
    {

        if (_comboTime > _comboLimitTime)
        {
            _comboCount = 1;
            _comboTime = 0;
        }
        else
        {                      
            _comboCount++;
            _comboTime = 0;

            if (_comboCount < 10)
                _comboCanvas.GetComponent<ComboCanvas>()._comboCountTxt.text = _comboCount.ToString();
            else
                _comboCanvas.GetComponent<ComboCanvas>()._comboCountTxt.text = "MAX";

            _comboCanvas.SetActive(true);
            _comboCanvas.GetComponent<ComboCanvas>()._comboFillImg.fillAmount = 1f;

            if (_comboPopUpRoutine != null)
                StopCoroutine(_comboPopUpRoutine);
            _comboPopUpRoutine = StartCoroutine(PopUpComboUI());

            if (_decreaseGageRoutine != null)
                StopCoroutine(_decreaseGageRoutine);
            _decreaseGageRoutine = StartCoroutine(DecreaseComboGage());
        }
    }

    IEnumerator PopUpComboUI()
    {
        float time = 0, duration = 0.1f;

        Instantiate(_comboParticlePrefab, _comboCanvas.transform.position + new Vector3(0,0,-2), Quaternion.identity);

        _comboCanvas.GetComponent<ComboCanvas>()._comboBGImg.color = new Color(1, 1 - (float)_comboCount / 10, 0, 1);
        _comboCanvas.GetComponent<ComboCanvas>()._comboCountTxt.color = new Color(1, 1 - (float)_comboCount / 10, 0, 1);
        _comboCanvas.GetComponent<ComboCanvas>()._comboFillImg.color = new Color(1, 1 - (float)_comboCount / 10, 0, 1);
        _comboCanvas.GetComponent<ComboCanvas>()._comboTxt.color = new Color(1, 1 - (float)_comboCount / 10, 0, 1);

        while (time <= duration)
        {
            time += Time.deltaTime;
            _comboCanvas.transform.localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1.3f), new Vector3(0.9f , 0.9f, 0.9f), time / duration);
            yield return null;
        }

        while (time <= duration)
        {
            time += Time.deltaTime;
            _comboCanvas.transform.localScale = Vector3.Lerp(new Vector3(0.9f, 0.9f, 0.9f), new Vector3(1, 1, 1), time / duration);
            yield return null;
        }

        _comboCanvas.transform.localScale = new Vector3(1, 1, 1);

    }

    IEnumerator PopDownComboUI()
    {
        float time = 0;
        float duration = 0.2f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            _comboCanvas.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), time / duration);
            yield return null;
        }

        _comboCanvas.SetActive(false);

    }

    IEnumerator DecreaseComboGage()
    {
        float time = 0;

        while(time <= _comboLimitTime)
        {
            time += Time.deltaTime;
            _comboCanvas.GetComponent<ComboCanvas>()._comboFillImg.fillAmount = Mathf.Lerp(1, 0, time / _comboLimitTime);

            yield return null;
        }
        StartCoroutine(PopDownComboUI());
    }

    

    IEnumerator DecreaseWaveSlider(float amount)
    {
        _remainingValue += amount;
        float time = 0, duration =0.3f ;
        float startValue = InGameCanvas.instance._waveSlider.value;
        float endValue = _remainingValue;
        //duration = 5f * (endValue - startValue);

        if (_remainingValue >= 1)
            endValue = 1f;
        else
            endValue = _remainingValue;

        while (time <= duration)
        {
            time += Time.deltaTime;
            _currentWaveRatio = Mathf.Lerp(
                startValue,
                endValue,
                time / duration);
            
            InGameCanvas.instance._waveSlider.value = _currentWaveRatio;            
            yield return null;            
        }
        if ( InGameCanvas.instance._waveSlider.value == 1f)
        {
            _spawnObject.GetComponent<SpawningFish>().SpawnFishes();
            InGameCanvas.instance._waveSlider.value = 0f;
            _remainingValue -= 1f;
            _waveCount++;
            InGameCanvas.instance._waveTxt.text = "WAVE " + _waveCount;
            _waveSliderRoutine = StartCoroutine(DecreaseWaveSlider(0));
        }
    }
}
