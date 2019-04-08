using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    GameObject _pivot;


    GameObject[] _netDots = new GameObject[7];
    public GameObject _netDotPrefab;
    float _angleDifference = 15;

    LineRenderer[] _lineRenderers;
    EdgeCollider2D _edgeCollider;
    PolygonCollider2D _polygonCollider;

    bool _netFire = false;

    protected Rigidbody2D _rigidbody2D;
    protected HingeJoint2D _hingeJoint;
    public float _destinyLength;
    float _maxDestinyLength = 5f;
    Vector3 _startPos;
    Vector3 _endPos;

    public GameObject _nodePrefab;
    public GameObject _lastNode;
    protected List<GameObject> _nodes = new List<GameObject>();
    protected LineRenderer _lineRenderer;
    int _vertexCount;
    float _distance = 0.1f;
    bool _done = false;
    bool _fire = false;
    bool _rollBackStart = false;
    public Transform _nodeCreationPos;

    float _time = 0f;
    float _duration = 0.2f;

    List<GameObject> _catchedFish = new List<GameObject>();

    // Start is called before the first frame update
    public void TheStart(GameObject pivot, Vector3 firePoint)
    {
        _pivot = pivot;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = false;
        _hingeJoint = GetComponent<HingeJoint2D>();
        _hingeJoint.enabled = false;
        
        //_destinyLength = Mathf.Clamp( Vector3.Distance(transform.position, firePoint), 0, _maxDestinyLength);
        _destinyLength = _maxDestinyLength;
        _startPos = transform.position;
        _endPos = _startPos + transform.right * _destinyLength;
        _duration = _duration * _destinyLength / _maxDestinyLength;

        GameObject go = Instantiate(_nodePrefab, _pivot.transform);
        _lastNode = go;
        _lastNode.GetComponent<HingeJoint2D>().enabled = false;
        _lastNode.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        _nodes.Add(_lastNode);
        _lineRenderer = GetComponent<LineRenderer>();
        _vertexCount = 2; // for hook and _gun 

        _lineRenderers = GetComponentsInChildren<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _edgeCollider.enabled = true;
        _edgeCollider.isTrigger = false;

        _polygonCollider = GetComponent<PolygonCollider2D>();
        _polygonCollider.enabled = true;
        _polygonCollider.isTrigger = true;

        _rigidbody2D.AddForce(transform.right * 10, ForceMode2D.Impulse);

        _fire = true;
        FireNet();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_fire) return;

        //_time += Time.deltaTime;
        // transform.position = Vector3.Lerp(_startPos, _endPos, _time / _duration);

        if (Vector3.Distance(_startPos, transform.position) < _destinyLength &&
            _rigidbody2D.velocity.magnitude >= 2f)
        {
            if (Vector3.Distance(transform.position, _lastNode.transform.position) > _distance)
            {
                CreateNode();
            }
        }
        else if (!_done)
        {
            _done = true;
            
            while (Vector3.Distance(transform.position, _lastNode.transform.position) >= _distance)
            {
                CreateNode();
            }
            VelocityZero();

            _hingeJoint.enabled = true;
            _hingeJoint.connectedBody = _lastNode.GetComponent<Rigidbody2D>();
            _hingeJoint.autoConfigureConnectedAnchor = false;
            _hingeJoint.connectedAnchor = Vector2.zero;
            _hingeJoint.anchor = _lastNode.transform.position - transform.position;

            StartCoroutine(RollBackArrow());
        }

        RenderLine();

        if (_netFire)
        {
            RenderNet();
            
            Vector2[] polygonPoints = new Vector2[8];
            polygonPoints[0] = Vector2.zero;
            for (int i = 0; i < 7; i++)
            {
                polygonPoints[i + 1] = transform.InverseTransformPoint(_netDots[i].transform.position );
            }
            _polygonCollider.points = polygonPoints;

            if (_rollBackStart)
            {
                _edgeCollider.points = polygonPoints;
            }
            else
            {
                Vector2[] edgePoints = new Vector2[3];
                edgePoints[0] = transform.InverseTransformPoint(_netDots[0].transform.position);
                edgePoints[1] = Vector2.zero;
                edgePoints[2] = transform.InverseTransformPoint(_netDots[6].transform.position);
                _edgeCollider.points = edgePoints;
            }
        }
    }

    void VelocityZero()
    {
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.gravityScale = 0f;
        //_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        for (int i = 1; i < _nodes.Count; i++)
        {
            //_nodes[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //_nodes[i].GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        }
    }

    void CreateNode()
    {
        GameObject go = Instantiate(_nodePrefab, _nodeCreationPos.transform.position, Quaternion.identity);
        go.transform.SetParent(_pivot.transform);
        go.GetComponent<HingeJoint2D>().connectedBody = _lastNode.GetComponent<Rigidbody2D>();
        go.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        _lastNode = go;
        _nodes.Add(_lastNode);
        _vertexCount++;
    }

    void RenderLine()
    {
        _lineRenderers[0].positionCount = _nodes.Count;
        int i;
        for (i = 0; i < _nodes.Count; i++)
        {
            _lineRenderers[0].SetPosition(i, _nodes[i].transform.position);
        }
    }


    void FireNet()
    {
        Debug.Log(transform.rotation.eulerAngles);

        _netFire = true;

        for (int i = 0; i < 7; i++)
        {
            float angle = transform.rotation.eulerAngles.z + _angleDifference*3 - _angleDifference * i;
            _netDots[i] = Instantiate(_netDotPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            _netDots[i].transform.SetParent(transform);
            _netDots[i].GetComponent<NetPoint>().TheStart(gameObject);
        }
    }

    void RenderNet()
    {
        _lineRenderers[1].positionCount = 7; // two for pivot, 7 for net dots
        for(int i=0; i<7; i++)
        {
            _lineRenderers[1].SetPosition(i, _netDots[i].transform.position);
        }

        for (int i = 2; i < 5; i++)
        {
            _lineRenderers[i].positionCount = 7;
            for(int j=0; j<7; j++)
            {
                Vector3 temp = _netDots[j].transform.position - transform.position;
                temp *= ((float)
                    i / 4);
                _lineRenderers[i].SetPosition(j, transform.position + temp);
            }
        }
    }

    public void StartRollBack()
    {

    }

    IEnumerator RollBackArrow()
    {
        _rollBackStart = true;

        for (int i = 1; i < _nodes.Count; i++)
            _nodes[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //_edgeCollider.enabled = false;

        HingeJoint2D hinge;
        float time = 0, duration = 0.02f;
        Vector2 startconnectedAnchor;

        Vector3 startScale = _pivot.transform.localScale;

        for(int i=0; i<_catchedFish.Count; i++)
        {
            _catchedFish[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _catchedFish[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            _catchedFish[i].transform.SetParent(transform);
        }

        for (int i = 1; i < _nodes.Count; i++)
        {
            _nodes[i - 1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hinge = _nodes[i].GetComponent<HingeJoint2D>();
            hinge.autoConfigureConnectedAnchor = false;
            startconnectedAnchor = hinge.connectedAnchor;
            time = 0;
            
            while (time <= duration)
            {
                time += Time.deltaTime;
                hinge.connectedAnchor = Vector2.Lerp(startconnectedAnchor, Vector2.zero, time / duration);
                yield return null;
            }
        }

        StartCoroutine(SizeDownNet());
    }

    IEnumerator SizeDownNet()
    {
        float time = 0;
        float duration = 0.1f;

        Vector3 startSize = transform.localScale;

        Cat.instance.AddFishes(_catchedFish);
        while(time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startSize, Vector3.zero, time / duration);
            yield return null;
        }

        Destroy(_pivot);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            if (collision.gameObject.GetComponent<Fish>()._hp < 5 && !_rollBackStart)
            {
                _catchedFish.Add(collision.gameObject);
                //collision.gameObject.GetComponent<Fish>()._catched = true;
                //collision.gameObject.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Fish") && !_rollBackStart)
        {
            _catchedFish.Remove(collision.gameObject);
        }
    }
}
