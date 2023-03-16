using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarAgent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Selected = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Selected = true;
        Debug.Log("Click");
    }

    
}
