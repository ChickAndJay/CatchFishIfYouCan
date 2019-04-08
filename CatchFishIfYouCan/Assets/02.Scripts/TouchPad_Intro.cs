using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad_Intro : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject _sceneLoader;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");

        _sceneLoader.GetComponent<SceneLoader>().LoadLobbyScene();
    }

}
