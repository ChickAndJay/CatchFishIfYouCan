using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ArrowCategory _arrow;
    Vector3 _startPos;
    Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    public void Start()
    {
        _startPos = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(transform.right * _arrow._speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //float range = (transform.position - _startPos).magnitude;
        //if (range >= _arrow._ropeLength)
        //    _rigidbody2D.velocity = Vector2.zero;

    }
}
