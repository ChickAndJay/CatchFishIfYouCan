using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishCategory _fishCategory;

    public int _hp;
    public int _speed;
    public int _gold;
    public float _size;
    public float _sizeRatio;
    public int _jellyFishDamage;

    HingeJoint2D _hingeJoint2D;
    public bool _catched = false;
    GameObject _cat;
    Coroutine _sizeDownRoutine;
    
    // Start is called before the first frame update
    void Start()
    {
        _size = Random.Range(_fishCategory._standardSize, _fishCategory._maxSize);
        _sizeRatio = _size / _fishCategory._standardSize;
        transform.localScale = new Vector3(_size, _size, _size);
               
        _hp = (int)((float)_fishCategory._hp * _sizeRatio);
        _speed = _fishCategory._speed;
        _gold =(int)((float) _fishCategory._gold * _sizeRatio);

        if (gameObject.CompareTag("JellyFish"))
        {
            _hp = 2;
            _jellyFishDamage = (int)((float)_fishCategory._jellyFishDamage * _sizeRatio);
        }
    }
    
    public void NearestNode(List<GameObject> nodes)
    {
        GameObject nearestNode = nodes[0];
        for (int i = 1; i < nodes.Count; i++)
        {
            if(Vector3.Distance(transform.position, nearestNode.transform.position) >
                Vector3.Distance(transform.position, nodes[i].transform.position))
            {
                nearestNode = nodes[i];
            }
        }
        transform.SetParent(nearestNode.transform);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
        

    public void SizeDown()
    {
        StartCoroutine(SizeDownRoutine());
    }

    IEnumerator SizeDownRoutine()
    {
        Cat.instance.AddFish(gameObject);

        float time = 0, duration = 0.25f;
        Vector3 startScale = transform.localScale;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / duration);
            yield return null;
        }
    }
}
