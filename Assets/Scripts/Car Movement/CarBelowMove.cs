using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBelowMove : MonoBehaviour
{
    Vector3 startposition;
    // Start is called before the first frame update
    public float speed;
    void Start()
    {
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (transform.position.x > 53f)
            transform.position = startposition;
    }
}
