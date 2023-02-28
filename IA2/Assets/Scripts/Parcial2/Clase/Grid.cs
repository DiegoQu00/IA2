using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Edge
//{
//    public Node A;
//    public Node B;
//    public float fCost;
//}

//public class Node
//{
//    public int x;
//    public int y;

//    //public List<Node> Neighbors;
//    public Node Parent; 

//    public float g_Cost;
//    public float fTerrainCost;
//    public bool bWalkable;

//    public Node (int in_x, int in_y)
//    {
//        this.x = in_x;
//        this.y = in_y;
//        this.Parent = null;
//        this.g_Cost = int.MaxValue;
//        this.fTerrainCost = 1;
//        this.bWalkable = true;
//    }
//}

//public class Graph
//{
//    public List<Node> Nodes;
//}

public class Grid 
{

    public int iHeight = 10;
    public int iWidth = 10;

    private float fTileSize;
    private Vector3 v3OriginPosition;

    public Node[,] Nodes;

    public bool bShowDebug = true;

    public Grid(int in_height, int in_width, float in_fTileSize = 10.0f, Vector3 in_v3OriginPosition = default)
    {
        iHeight = in_height;
        iWidth = in_width;

        InitGrid();

        //Con esta variable vamos a crear objetos de tipos textMesh
        this.fTileSize = in_fTileSize;
        this.v3OriginPosition = in_v3OriginPosition;

        if (bShowDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[iHeight, iWidth];

            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    //debugTextArray[y, x] = new TextMesh(x, y);
                }
            }
        }
    }

    public void InitGrid()
    {
        Nodes = new Node[iHeight, iWidth];

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                //Nodes[y, x].x = x;
                //Nodes[y, x].y = y;
                //Nodes[y, x].Parent = null;
                //Nodes[y, x].g_Cost = int.MaxValue;
                //Nodes[y, x].fTerrainCost = 1;
                //Nodes[y,x].Neighbors = new List<Edge>();
                //Node[y,x]

                Nodes[y, x] = new Node(x, y);
            }
        }
    }

    public List<Node> DepthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        

        Node StartNode = GetNode(in_startY,in_startX);
        Node EndNode = GetNode(in_endY, in_endX);

        if(StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid end or start node in DepthFirstSearch");
            return null;
        }

        Stack<Node> OpenList = new Stack<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Push(StartNode);

        while(OpenList.Count > 0)
        {
            Node currentNode = OpenList.Pop();
            Debug.Log("Current Node is " + currentNode.x + ", " + currentNode.y);
            
            if(currentNode == EndNode)
            {
                Debug.Log("Camino Encontrado!");
                return Backtrack(currentNode);

            }
            
            ClosedList.Add(currentNode);

            //if (ClosedList.Contains(currentNode))
            //{
            //    continue;
            //}

            List<Node> currentNeighbors = GetNeighbors(currentNode);
            //foreach(Node neighbor in currentNeighbors)
            //{
            //    if (ClosedList.Contains(neighbor))
            //        continue;

            //    neighbor.Parent = currentNode;
            //    OpenList.Push(neighbor);
            //}

            for(int x = currentNeighbors.Count - 1; x >= 0; x--)
            {
                if(currentNeighbors[x].bWalkable && !ClosedList.Contains(currentNeighbors[x]))
                {
                    currentNeighbors[x].Parent = currentNode;
                    OpenList.Push(currentNeighbors[x]);
                }
            }
            
        }

        Debug.LogError("No path found between start and end.");

        return null;
    }

    public Node GetNode(int x, int y)
    {
        if (x < iWidth && x >= 0 && y < iHeight && y >= 0)
        {
            return Nodes[y, x];
        }

        //Debug.LogError("Invalid coordinates in DepthFirstSearch");
        return null;
    }

    public List <Node> GetNeighbors(Node in_currentNode)
    {
        List<Node> out_Neighbors = new List<Node>();

        int x = in_currentNode.x;
        int y = in_currentNode.y;

        if (GetNode(y + 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y+1,x]);
        }
        if (GetNode(y, x - 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x-1]);
        }

        if (GetNode(y, x + 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x+1]);
        }

        if (GetNode(y - 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y - 1, x]);
        }


        return out_Neighbors;
    }

    public List<Node> Backtrack(Node in_node)
    {
        List <Node> out_Path = new List<Node>();
        Node current = in_node;

        while(current.Parent != null)
        {
            out_Path.Add(current);
            current = current.Parent;
        }

        out_Path.Add(current);
        out_Path.Reverse();

        return out_Path;
    }
    public static TextMesh CreateWorldText(string in_text, Transform in_parent = null,
                                           Vector3 in_localPosition = default,
                                           int in_iFontSize = 32, Color in_color = default,
                                           TextAnchor in_textAnchor = TextAnchor.UpperLeft, TextAlignment in_textAligment = TextAlignment.Left)
    {
        if (in_color == null) in_color = Color.white;

        GameObject goMyObject = new GameObject(in_text, typeof(TextMesh));

        goMyObject.transform.parent = in_parent;
        goMyObject.transform.localPosition = in_localPosition;

        TextMesh myTM = goMyObject.GetComponent<TextMesh>();
        myTM.text = in_text;
        myTM.anchor = in_textAnchor;
        myTM.alignment = in_textAligment;
        myTM.fontSize = in_iFontSize;
        myTM.color = in_color;

        return myTM;
    }
}

