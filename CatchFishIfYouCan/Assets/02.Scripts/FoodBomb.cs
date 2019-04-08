using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBomb : MonoBehaviour
{
    public GameObject _renderObj;
    public SpriteRenderer _electricBombExplode;
    public GameObject _electricBombParticlePrefab;

    float time = 0;
    bool _fire = false;
    bool _deceleration = false;
    Vector2 _startVelocity;
    Rigidbody2D _rigidbody2D;

    Vector3 _startPos;
    Vector3 _endPos;

    bool _isFood;
    // Start is called before the first frame update
    public void TheStart(Vector2 route, bool isFood)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
               
        _startPos = transform.position;
        _endPos = route;

        _isFood = isFood;
        if (!_isFood)
            StartCoroutine(ExplodeBomb());

        float x = (_endPos - _startPos).x;
        float y = (_endPos - _startPos).y;
        y = y + 4.9f;

        _force = new Vector2(x, y);
        _fire = true;
    }

    Vector2 _force;
    float _velocityTime = 0;
    private void FixedUpdate()
    {
        if (_fire)
        {
            _fire = false;
            _rigidbody2D.AddForce(_force, ForceMode2D.Impulse);
        }
    }

    bool showed = false;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1 && !showed)
        {
            showed = true;
            time = 0;

            
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,-0);
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Collider2D>().isTrigger = true;

            GetComponent<Rigidbody2D>().gravityScale = 0;

            if (_isFood)
            {
                //GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.5f);

            }
            else
            {
                Instantiate(_electricBombParticlePrefab, transform.position, Quaternion.identity);
                _renderObj.SetActive(false);
                _electricBombExplode.enabled = false;
            }
            StartCoroutine(SizeDown());
        }
    }

    IEnumerator SizeDown()
    {
        float time = 0;
        float duration = 3f;
        Vector3 startScale = _renderObj.transform.localScale;
        Vector3 endScale = Vector3.zero;

        while(time < duration)
        {
            time += Time.deltaTime;
            _renderObj.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);                
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator ExplodeBomb()
    {
        float time = 0;
        float duration = 1.0f;
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);

        while (time < duration)
        {
            time += Time.deltaTime;
            Color color = new Color(1, 1, 1, Mathf.Lerp(0, 1, time / duration));
            _electricBombExplode.color = color;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish") && !_isFood)
        {
            collision.gameObject.GetComponent<FishMoving>().ElectricShock();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
