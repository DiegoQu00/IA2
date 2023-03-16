using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PathfindingTest : MonoBehaviour
{
    [Header("Grid")]
    public int Height=5;
    public int Width=5;

    [Header("StartPoints&EndPoints")]
    public int Initx=0;
    public int Inity=0;
    public int Endx=4;
    public int Endy=4;

    public Transform t;
    //public Transform t;

    public int2 StartPosition = int2.zero;
    public int2 EndPosition = int2.zero;

    public GameObject go_InitialPoint;
    public GameObject go_EndPoint;
    public bool b_InitialPoint = false;
    public bool b_EndPoint = false;
    public bool b_Ready = false;
    ClassGrid myTest;


    // Start is called before the first frame update
    void Start()
    {
         myTest = new ClassGrid(Height, Width);
        //myTest.DepthFirstSearch(0, 0, 4, 4);

        //ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.BreadthFirstSearch(2, 2, 1, 1);

        //ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.BestFirstSearch(0, 0, 4, 4);

        //GridQueue myTest = new GridQueue(Height, Width);
        //myTest.BreadthFirstSearch(Initx, Inity, Endx, Endy);

        //myTest.AStarSearch(0, 0, 4, 4);
        //myTest.AStarSearch(0, 1, 4, 0);
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (b_InitialPoint == true && b_EndPoint == true)
            b_Ready = true;

        if (b_Ready == true)
        {
            GridTile s_InitialPoint = go_InitialPoint.GetComponent<GridTile>();
            GridTile s_EndPoint = go_EndPoint.GetComponent<GridTile>();
            List<Node> Pathfinding_result = myTest.AStarSearch(s_InitialPoint.i_x, s_InitialPoint.i_y, s_EndPoint.i_x, s_EndPoint.i_y);
            List<Vector3> WorldPositionPathfinding = new List<Vector3>();

            foreach (Node n in Pathfinding_result)
            {
                WorldPositionPathfinding.Add(myTest.GetWorldPosition(n.x, n.y));
            }
        }
    }
}
