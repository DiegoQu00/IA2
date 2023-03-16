using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    // Start is called before the first frame update

    public PathfindingTest PathFinding;
    public int i_x;
    public int i_y;
    void Start()
    {
        PathFinding = FindObjectOfType<PathfindingTest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (PathFinding.b_InitialPoint == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Funciona 1");
                PathFinding.go_InitialPoint = gameObject;
                PathFinding.b_InitialPoint = true;
            }
        }
        


        if (PathFinding.b_EndPoint == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Funciona 2");
                PathFinding.go_EndPoint = gameObject;
                PathFinding.b_EndPoint = true;
            }
        }
    }

    public void GetIndex(int x, int y)
    {
        i_x = x;
        i_y = y;
    }
}
