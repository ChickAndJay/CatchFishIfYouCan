using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Camera _cam;

    public GameObject _targetImg;
    public LayerMask _targetMask;

    public GameObject _catObj;
    Cat _catScript;

    public Testscript testscript;

    public bool _isEndGame = false;
    public GameObject _boat;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _catScript = _catObj.GetComponent<Cat>();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (_isEndGame)
        {
            ;
        }
        else
        {
            Ray ray = _cam.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, _targetMask);

            if (hit && hit.collider != null)
            {
                _targetImg.transform.position = hit.point;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isEndGame)
        {

        }
        else
        {
            Ray ray = _cam.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, _targetMask);

            if (hit && hit.collider != null)
            {
                _targetImg.SetActive(true);
                _targetImg.transform.position = hit.point;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isEndGame)
        {
            _boat.GetComponent<BoatScriptInGame>().GoBackToHome();
        }
        else
        {
            _targetImg.SetActive(false);
            Ray ray = _cam.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, _targetMask);

            if (hit && hit.collider != null)
            {
                _catScript.Fire(hit.point);
            }
        }
    }


}
