using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishMoving : MonoBehaviour
{
    float _time = 0;
    float _duration = 0;
    float _speedRange;
    float _speed;
    float _pauseTime = 0;
    float _pauseDuration;

    float _stunTime = 0.5f;
    float _stunDuration = 0.5f;

    bool _hit = false;
    bool _pausing = false;

    Rigidbody2D _rigidbody2D;
    float _angle;

    public GameObject _speechBubblePrefab;
    bool _speechBubbleSpawnedBefore = false;

    Fish _fish;
    // Start is called before the first frame update
    void Start()
    {
        _fish = GetComponent<Fish>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _angle = Random.Range(-15f, 15f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        _speedRange = GetComponent<Fish>()._fishCategory._speed;

        _duration = Random.Range(0.5f, 2f);
        _pauseDuration = Random.Range(0.5f, 2f);

    }

    private void FixedUpdate()
    {
        // if (_pausing) return;

        if (_hit)
        {
            _stunTime += Time.deltaTime;
            if (_stunTime >= _stunDuration)
            {
                _stunTime = 0f;
                _rigidbody2D.drag = 1;
                _hit = false;
            }
            return;
        }

        _time += Time.deltaTime;
        if(_time > _duration)
        {
            _time = 0;
            _duration = Random.Range(2f, 5f);
            _speed = Random.Range(1f, _speedRange);
            _rigidbody2D.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OutLineBorder"))
        {
            Destroy(gameObject);
        }else if (collision.gameObject.CompareTag("Arrow") && _fish._catched == false)
        {
            _hit = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.drag = 10;

            if (!_speechBubbleSpawnedBefore)
            {
                _speechBubbleSpawnedBefore = true;
                GameObject bubble = Instantiate(_speechBubblePrefab, transform.position, Quaternion.identity);
                bubble.GetComponent<SpeechBubble>().TheStart(gameObject);
            }
        }
    }       
}
