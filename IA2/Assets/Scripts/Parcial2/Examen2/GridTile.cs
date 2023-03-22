using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    // Start is called before the first frame update

    public PathfindingTest PathFinding;
    public int i_x;
    public int i_y;
    public bool b_initPoint;
    public bool b_endPoint;
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
                if(b_endPoint == true)
                {
                    PathFinding.go_EndPoint = null;
                    PathFinding.b_EndPoint = false;
                    b_endPoint = false;
                }
                Debug.Log("Funciona 1");
                PathFinding.go_InitialPoint = gameObject;
                PathFinding.b_InitialPoint = true;
                b_initPoint = true;
                
            }
        }
        


        if (PathFinding.b_EndPoint == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (b_initPoint == true)
                {
                    PathFinding.go_InitialPoint = null;
                    PathFinding.b_InitialPoint = false;
                    b_initPoint = false;
                }
                Debug.Log("Funciona 2");
                PathFinding.go_EndPoint = gameObject;
                PathFinding.b_EndPoint = true;
                b_endPoint = true;
            }
        }
    }

    public void GetIndex(int y, int x)
    {
        i_x = x;
        i_y = y;
    }

    private void OnDrawGizmos()
    {
        if(b_initPoint==true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 1);
        }
        if(b_endPoint==true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}
