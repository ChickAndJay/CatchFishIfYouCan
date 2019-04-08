using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject _arrowPrefab;
    public GameObject _arrowPivotPrefab;

    float _time;
    public float _reloadTime;

    public GameObject _foodPrefab;
    public GameObject _electricBombPrefab;
    public GameObject _netPrefab;


    public int _foodCount;
    public int _net;
    public int _electricBombCount;

    AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _reloadTime = _arrowPrefab.GetComponent<Arrow>()._arrow._reloadTime;
        _time = 2f;

        _net = GameManagerScript.instance._net;
        _foodCount = GameManagerScript.instance._foodCount;
        _electricBombCount = GameManagerScript.instance._electricBombCount;

        InGameCanvas.instance._foodCountTxt.text = _foodCount.ToString();
        InGameCanvas.instance._netCountTxt.text = _net.ToString();
        InGameCanvas.instance._electricBombCountTxt.text = _electricBombCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
    }

    virtual public int Fire(Vector3 firePoint  ) {
        if (_time <= _reloadTime || Cat.instance._startOxygen <= 0) return -1;

        _time = 0;
        Vector3 dir = (firePoint - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.right);
        if (firePoint.y < transform.position.y)
            angle *= -1;

        _audioSource.Play();
        Cat.instance._arrowCount++;
        GameObject projectile = Instantiate(_arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        GameObject pivot = Instantiate(_arrowPivotPrefab, transform);

        projectile.GetComponent<Arrow>().TheStart(pivot);

        return projectile.GetComponent<Arrow>()._arrow._oxygenComsuming;
    }

    public int FireFoodAndBomb(bool isFood, Vector3 firePoint)
    {
        Vector2 route = firePoint - transform.position;
        GameObject bomb;
        if (isFood)
        {
            bomb = Instantiate(_foodPrefab, transform.position, Quaternion.identity);
            _foodCount--;
            InGameCanvas.instance._foodCountTxt.text = _foodCount.ToString();
            GameManagerScript.instance._foodCount--;

            bomb.GetComponent<FoodBomb>().TheStart(firePoint, isFood);
            return _foodCount;
        }
        else
        {
            bomb = Instantiate(_electricBombPrefab, transform.position, Quaternion.identity);
            _electricBombCount--;
            InGameCanvas.instance._electricBombCountTxt.text = _electricBombCount.ToString();
            GameManagerScript.instance._electricBombCount--;

            bomb.GetComponent<FoodBomb>().TheStart(firePoint, isFood);
            return _electricBombCount;
        }

    }

    public  int FireNetBomb(Vector3 firePoint)
    {
        Vector3 dir = (firePoint - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.right);
        if (firePoint.y < transform.position.y)
            angle *= -1;

        GameObject netprojectile = Instantiate(_netPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
        GameObject pivot = Instantiate(_arrowPivotPrefab, transform);

        _net--;
        InGameCanvas.instance._netCountTxt.text = _net.ToString();
        GameManagerScript.instance._net--;
        netprojectile.GetComponent<Net>().TheStart(pivot, firePoint);

        return _net;
    }
}
