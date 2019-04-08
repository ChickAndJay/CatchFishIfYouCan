using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum ItemCategory{
    FoodBomb,
    ElectircBomb,
    NetBomb
}
public class ItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemCategory _itemCategory;
    public GameObject _targetImg;
    public LayerMask _targetMask;
    RectTransform _rectTr;

    Button button;

    bool _isDragging = false;
    bool _drawImg = false;
    bool _outOfAmmo = false;

    bool _isCooling = false;
    float _coolTimeDuration = 2f;
    public Image _coolTimeImg;
    public TextMeshProUGUI _count;
    public GameObject _gun;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_drawImg || _isCooling || _outOfAmmo) return;
        _targetImg.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);

        if (hit && hit.collider != null)
        {
            _targetImg.transform.position = hit.point;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isDragging || _isCooling || _outOfAmmo) return;

        _drawImg = false;
        _targetImg.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isDragging) return;

        _drawImg = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isCooling && _outOfAmmo) return;

        if (_targetImg.activeSelf)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);
            
            if (hit && hit.collider != null)
            {
                int count = 0;

                switch (_itemCategory)
                {
                    case ItemCategory.FoodBomb:
                        count = _gun.GetComponent<Gun>().FireFoodAndBomb(true, hit.point);
                        break;
                    case ItemCategory.ElectircBomb:
                        count = _gun.GetComponent<Gun>().FireFoodAndBomb(false, hit.point);
                        break;
                    case ItemCategory.NetBomb:
                        count = _gun.GetComponent<Gun>().FireNetBomb(hit.point);
                        break;
                }

                if (count == 0)
                {
                    _coolTimeImg.fillAmount = 1f;
                    _coolTimeImg.enabled = true;
                    _outOfAmmo = true;
                }
                else
                    StartCoroutine(WaitCoolTime());
            }
        }
        _isDragging = false;
        _drawImg = false;
        _targetImg.SetActive(false);
    }

    IEnumerator WaitCoolTime()
    {
        _isCooling = true;
        
        float time = 0f;
        float duration = _coolTimeDuration;

        _coolTimeImg.fillAmount = 1f;
        _coolTimeImg.enabled = true;
        
        while(time < duration)
        {
            time += Time.deltaTime;
            _coolTimeImg.fillAmount = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }

        _isCooling = false;
        _coolTimeImg.enabled = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        _rectTr = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
