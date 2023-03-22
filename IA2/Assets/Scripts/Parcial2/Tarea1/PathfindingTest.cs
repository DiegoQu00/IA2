//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen2 --- IDVMI 
//
// Clase PathfindingTest
//
// Clase encargada de inicializar el Grid y  pathfinding en escena.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class PathfindingTest : MonoBehaviour
{
    [Header("Grid")]
    public int Height=5;
    public int Width=5;
    public float fTileSize = 10.0f;

    [Header("StartPoints&EndPoints")]
    public int Initx=0;
    public int Inity=0;
    public int Endx=4;
    public int Endy=4;

    public Transform t;

    public int2 StartPosition = int2.zero;
    public int2 EndPosition = int2.zero;

    public GameObject go_InitialPoint;
    public GameObject go_EndPoint;
    public bool b_InitialPoint = false;
    public bool b_EndPoint = false;
    public bool b_Ready = false;
    public ClassGrid myTest;
    public List<Node> Pathfinding_result;
    public bool b_PathR = false;



    private void Awake()
    {
        // Creacion de Grid
         myTest = new ClassGrid(Height, Width, fTileSize);

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
        }

        // Booleana que nos avisa si ya se seleccionaron los 2 puntos.
        if (b_InitialPoint == true && b_EndPoint == true)
            b_Ready = true;

        // Inicio de pathfinding con espacio
        if (b_Ready == true && Input.GetKeyDown("space"))
        {
            GridTile s_InitialPoint = go_InitialPoint.GetComponent<GridTile>();
            GridTile s_EndPoint = go_EndPoint.GetComponent<GridTile>();
            Pathfinding_result = myTest.AStarSearch(s_InitialPoint.i_x, s_InitialPoint.i_y, s_EndPoint.i_x, s_EndPoint.i_y);
            List<Vector3> WorldPositionPathfinding = new List<Vector3>();

            foreach (Node n in Pathfinding_result)
            {
                WorldPositionPathfinding.Add(myTest.GetWorldPosition(n.x, n.y));
            }

            // Pathfinding listo.
            b_PathR = true;
        }

        
    }

    // Reiniciar escena
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
