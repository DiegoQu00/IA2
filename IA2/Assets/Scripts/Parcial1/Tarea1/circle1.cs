using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle1 : MonoBehaviour
{
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
