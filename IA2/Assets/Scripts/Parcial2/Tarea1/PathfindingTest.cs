using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        //ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.DepthFirstSearch(0, 0, 4, 4);

        //ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.BreadthFirstSearch(2, 2, 1, 1);

        //ClassGrid myTest = new ClassGrid(5, 5);
        //myTest.BestFirstSearch(0, 0, 4, 4);

        GridQueue myTest = new GridQueue(Height, Width);
        myTest.BreadthFirstSearch(Initx, Inity, Endx, Endy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
