using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //public Vector3 _destiny;
    public float _destinyLength;
    public float _speedMultiplier = 5;

    public GameObject _nodePrefab;
    public float _distance = 0.1f;
    public GameObject _pivot;

    public GameObject _lastNode;
    //GameObject _firstNode;

    protected bool _done = false;
    protected bool _hit = false;
    protected bool _fire = false;

    public GameObject _impactSpritePrefab;

    public Material _transparentSpriteMat;
    public Material _transparentLineMat;

    protected SpriteRenderer _spriteRenderer;
    protected LineRenderer _lineRenderer;
    protected int _vertexCount;
    protected List<GameObject> _nodes = new List<GameObject>();

    protected Rigidbody2D _rigidbody2D;
    protected HingeJoint2D _hingeJoint;
    protected Collider2D _collider2D;

    public ArrowCategory _arrow;
    Vector3 _startPos;

    public Transform _nodeCreationPos;
    public List<GameObject> _catchedFish = new List<GameObject>();

    public Transform _arrowPoint;

    Vector2 _startSpeed;
    Vector2 _reducedSpeed;
    float time = 0;

    public LayerMask _fishLayer;

    AudioSource _audioSource;

    // Start is called before the first frame update
    public void TheStart(GameObject pivot)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _hingeJoint = GetComponent<HingeJoint2D>();
        _collider2D = GetComponentInChildren<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

        _hingeJoint.enabled = false;

        _destinyLength = _arrow._ropeLength;
        _startPos = transform.position;

        _pivot = pivot;

        GameObject go = Instantiate(_nodePrefab, _pivot.transform);
        _lastNode = go;
        _lastNode.GetComponent<HingeJoint2D>().enabled = false;
        _lastNode.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        _nodes.Add(_lastNode);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _vertexCount = 2; // for hook and _gun 

        Vector3 force = transform.right.normalized;
        force *= 10;
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        _fire = true;

        //Time.timeScale = 0.1f;
    }   

    // Update is called once per frame
    void Update()
    {
        if (!_fire) return;
        

        if (Vector3.Distance(_startPos, transform.position) < _destinyLength &&
            !_hit &&
            _rigidbody2D.velocity.magnitude >= 2f
            )
        {

            if (Vector3.Distance(transform.position, _lastNode.transform.position) > _distance)
            {
                CreateNode();
            }
        }
        else if (!_done)
        {
            _done = true;

            _rigidbody2D.velocity = Vector3.zero;
            _collider2D.isTrigger = true;
            while (Vector3.Distance(transform.position, _lastNode.transform.position) > _distance)
            {
                CreateNode();
            }
            _hingeJoint.enabled = true;
            _hingeJoint.connectedBody = _lastNode.GetComponent<Rigidbody2D>();
            _hingeJoint.autoConfigureConnectedAnchor = false;
            _hingeJoint.connectedAnchor = Vector2.zero;
            _hingeJoint.anchor = _lastNode.transform.position - transform.position;
            StartCoroutine(RollBackArrow());
        }

        RenderLine();
    }

    IEnumerator RollBackArrow()
    {
        for (int i = 1; i < _nodes.Count; i++)
            _nodes[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _spriteRenderer.material = _transparentSpriteMat;
        _lineRenderer.material = _transparentLineMat;

        _collider2D.enabled = false;

        for (int i = 0; i < _catchedFish.Count; i++)
        {
            _catchedFish[i].GetComponent<Fish>().NearestNode(_nodes);
        }

        yield return new WaitForSeconds(0.5f);
        HingeJoint2D hinge;
        float time = 0, duration = 0.1f;
        Vector2 startconnectedAnchor;

        Vector3 startScale = _pivot.transform.localScale;

        for (int i = 1; i < _nodes.Count; i++)
        {
            _nodes[i - 1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hinge = _nodes[i].GetComponent<HingeJoint2D>();
            hinge.autoConfigureConnectedAnchor = false;
            startconnectedAnchor = hinge.connectedAnchor;
            time = 0;

            Fish fish = _nodes[i].GetComponentInChildren<Fish>();
            if (fish != null)
            {
                fish.transform.SetParent(null);
                fish.SizeDown();
            }

            while (time <= duration)
            {
                time += Time.deltaTime;
                hinge.connectedAnchor = Vector2.Lerp(startconnectedAnchor, Vector2.zero, time / duration);
                yield return null;
            }
        }

        if(Cat.instance._startOxygen <= 0 && Cat.instance._arrowCount == 1)
            Cat.instance.EndGame();

        Cat.instance._arrowCount--;
        Destroy(_pivot);
        Destroy(gameObject);
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
        _lineRenderer.positionCount = _nodes.Count;
        int i;
        for (i = 0; i < _nodes.Count; i++)
        {
            _lineRenderer.SetPosition(i, _nodes[i].transform.position);
        }
    }

    Vector2 _currentVelocity;
    private void FixedUpdate()
    {
        _currentVelocity = _rigidbody2D.velocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Fish") || collision.gameObject.CompareTag("JellyFish"))
            && !_hit)
        {
            Instantiate(_impactSpritePrefab, _arrowPoint.transform.position, Quaternion.identity).transform.position += new Vector3(0,0,-0.1f);            
            Cat.instance.HitCombo();
            _audioSource.Play();
            Fish fish = collision.gameObject.GetComponent<Fish>();
            if (fish._hp < _arrow._speed)
            {
                _rigidbody2D.velocity = _currentVelocity;
                collision.gameObject.GetComponent<Collider2D>().isTrigger = true;

                Vector2 newVelocity = _currentVelocity * (float)_arrow._transparent / 10f;
                _rigidbody2D.velocity = newVelocity;

                _catchedFish.Add(collision.gameObject);
                collision.gameObject.GetComponent<Fish>()._catched = true;

                if (collision.gameObject.CompareTag("JellyFish"))
                    Cat.instance.HitJellyFish(collision.gameObject);
            }
            else
            {
                fish._hp -= _arrow._speed;
                _hit = true;                
            }
        }
    }    
}
