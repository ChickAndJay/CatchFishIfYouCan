using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoatScriptInLobby : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,  IPointerEnterHandler, IPointerExitHandler
{
    public GameObject _character;
    public GameObject _lobbySceneManager;

    bool _onBoat = false;
    bool _dragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
        _onBoat = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_dragging)
            _onBoat = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_dragging)
            _onBoat = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_dragging && _onBoat)
            TakeCharacterThenGo();
        _dragging = false;
        _onBoat = false;
    }

    public void TakeCharacterThenGo()
    {
        _character.GetComponent<Animator>().SetBool("JumpIntoBoat", true);
        StartCoroutine(DepartBoat());
        StartCoroutine(CallSceneLoad());
    }

    IEnumerator DepartBoat()
    {
        yield return new WaitForSeconds(2.0f);
        
        Vector3 startPosition = transform.position;

        while(Vector3.Distance(startPosition, transform.position) < 10)
        {
            transform.Translate(transform.right * Time.deltaTime * 2, Space.World);
            yield return null;
        }        
    }


    IEnumerator CallSceneLoad()
    {
        yield return new WaitForSeconds(1.0f);
        _lobbySceneManager.GetComponent<SceneLoader>().LoadGameScene();
    }
}
