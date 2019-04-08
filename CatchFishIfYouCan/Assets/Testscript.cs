using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{
    List<Vector2> positionsList = new List<Vector2>();
    Vector2[] positionsArray;
    EdgeCollider2D edgeCollider;
    PolygonCollider2D polygonCollider;

    float time = 0;
    private void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;

        foreach(Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.gameObject.CompareTag("NetPoint"))
            {
                positionsList.Add(tr.localPosition);

            }
        }

        positionsArray = new Vector2[positionsList.Count];
        for(int i=0; i<positionsList.Count; i++)
        {
            positionsArray[i] = positionsList[i];
        }

        edgeCollider.points = positionsArray;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            edgeCollider.enabled = false;
            polygonCollider.enabled = true;

            polygonCollider.points = positionsArray;
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
    }
}
