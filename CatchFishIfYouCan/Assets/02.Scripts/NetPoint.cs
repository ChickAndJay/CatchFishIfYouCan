using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPoint : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    LineRenderer _lineRenderer;
    public GameObject _pivot;

    

    Vector3 _startPos;
    Vector3 _endPos;

    float _duration = 0.5f;
    float _length = 3.5f;
    float _time = 0;
    public bool _fire = false;

    // Start is called before the first frame update
   public  void TheStart(GameObject pivot)
    {
        _pivot = pivot;

        //GetComponent<HingeJoint2D>().enabled = false;
        //_rigidbody2D = GetComponent<Rigidbody2D>();
        //_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;

        _startPos = Vector3.zero;
        _endPos = pivot.transform.InverseTransformDirection(transform.right) * _length;
        
        _fire = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_fire) return;

        _time += Time.deltaTime;
        if(_time < _duration)
        {
            transform.localPosition = Vector3.Lerp(_startPos, _endPos, _time / _duration);
            //transform.localPosition = new Vector3(1, 0, 0);

        }
        else
        {

        }
        if (_pivot != null)
        {
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _pivot.transform.position);
            

            if(Vector3.Distance(_pivot.transform.position, transform.position) >= 5)
            {
                //GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
