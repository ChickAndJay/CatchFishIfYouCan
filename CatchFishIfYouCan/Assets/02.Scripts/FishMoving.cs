using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMoving : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    Fish _fish;
    bool _hit = false;

    float _lifeTime = 0;
    float _lifeTimeLimit = 60;

    float _time = 2;
    float _duration = 1f;
    float _radius = 0;
    bool _clockWise = true;
    
    float _forwardTime = 0;
    float _forwardTimeDuration;
    float _forwardSpeed;
    float _speedRange;

    float _rotationTime = 0;
    float _rotationTimeDuration;
    public float _rotationSpeed = 45;
    private float _desiredRot;

    float _stunTime = 0;
    float _stunDuration = 0.5f;
    
    public bool _foodBomb = false;
    GameObject _foodTarget;
    public bool _electricBomb = false;
    float _electricStunDuration = 4f;

    public GameObject _speechBubblePrefab;

    public GameObject _spawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        _fish = GetComponent<Fish>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _speedRange = _fish._fishCategory._speed;

        _forwardTimeDuration = Random.Range(0, 1f);
        _forwardSpeed = Random.Range(0, _speedRange);

        _rotationTimeDuration = Random.Range(0, 1f);
        _rotationSpeed = Random.Range(-90f, 90f);

    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime += Time.deltaTime;

        if (_fish._catched)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody2D.freezeRotation = true;
            return;
        }

        if (_electricBomb)
        {
            _stunTime += Time.deltaTime;
            if (_stunTime >= _electricStunDuration)
            {
                _stunTime = 0;
                _electricBomb = false;
                _hit = false;
            }
            return;
        }

        if (_hit)
        {
            _stunTime += Time.deltaTime;
            if (_stunTime >= _stunDuration)
            {
                _stunTime = 0f;
                _hit = false;
            }
            return;
        }        

        if (_foodBomb)
        {
            Vector3 dir = _foodTarget.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            if (transform.right.x <= 0)
                GetComponent<SpriteRenderer>().flipY = true;
            else
                GetComponent<SpriteRenderer>().flipY = false;
            if(Vector3.Distance(_foodTarget.transform.position, transform.position) >= 0.5f)
                transform.Translate(transform.right.normalized * Time.deltaTime * _fish._fishCategory._speed, Space.World);

            return;
        }

        if(_lifeTime > _lifeTimeLimit)
        {
            Vector3 dir = _spawnPoint.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
            if (transform.right.x <= 0)
                GetComponent<SpriteRenderer>().flipY = true;
            else
                GetComponent<SpriteRenderer>().flipY = false;
            if (Vector3.Distance(_spawnPoint.transform.position, transform.position) >= 0.5f)
                transform.Translate(transform.right.normalized * Time.deltaTime * _fish._fishCategory._speed/2, Space.World);
            else
                Destroy(gameObject);
        }

        if (_forwardTime > _forwardTimeDuration)
        {
            _forwardTime = 0;
            _forwardTimeDuration = Random.Range(0,5f);
            _forwardSpeed = Random.Range(0, _speedRange);
        }
        if (_rotationTime > _rotationTimeDuration)
        {
            _rotationTime = 0;
            _rotationTimeDuration = Random.Range(0, 2f);
            _rotationSpeed = Random.Range(0, 360f);
        }

        _forwardTime += Time.deltaTime;
        _rotationTime += Time.deltaTime;

        if (transform.right.x >= 0)
            GetComponent<SpriteRenderer>().flipY = false;
        else
            GetComponent<SpriteRenderer>().flipY = true;

        _desiredRot += _rotationSpeed * Time.deltaTime;
        Quaternion desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, _desiredRot);

        transform.Translate(transform.right.normalized * Time.deltaTime * _forwardSpeed, Space.World);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime);
    }

    bool _speechBubbleSpawnedBefore = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Arrow") && _fish._catched == false)
        {
            _hit = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            //_rigidbody2.drag = 10;

            if (!_speechBubbleSpawnedBefore)
            {
                _speechBubbleSpawnedBefore = true;
                GameObject bubble = Instantiate(_speechBubblePrefab, transform.position, Quaternion.identity);
                bubble.GetComponent<SpeechBubble>().TheStart(gameObject);
            }
        }
    }

    public void ElectricShock()
    {
        GameObject bubble = Instantiate(_speechBubblePrefab, transform.position, Quaternion.identity);
        bubble.GetComponent<SpeechBubble>().TheStart(gameObject);

        _electricBomb = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            _foodBomb = true;
            _foodTarget = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            _foodBomb = false;
            _foodTarget = null;
        }
    }
}
