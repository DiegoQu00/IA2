using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NodeQueue
{
    // Coordenadas del nodo
    public int x;
    public int y;

    // Nodos padres
    public NodeQueue Parent;

    // Costo
    public float g_Cost;
    // Costo del terreno
    public float fTerrainCost;
    // Se puede caminar sobre este nodo o no.
    public bool bWalkable; 

    //Constructor Node
    public NodeQueue(int in_x, int in_y) 
    {
        this.x = in_x;
        this.y = in_y;
        this.Parent = null;
        this.g_Cost = int.MaxValue;
        this.fTerrainCost = 1;
        this.bWalkable = true;
    }

    // Funcion ToString para los textos en pantalla
    public override string ToString()
    {
        return x.ToString() + " , " + y.ToString();
    }
}



public class GridQueue : MonoBehaviour
{
    // Altura del grid
    public int iHeight;
    // Anchura del grid
    public int iWidth;

    // Dibujar el grid
    private float fTileSize;
    // Posicion Inicial
    private Vector3 v3OriginPosition;

    // Nodos
    public NodeQueue[,] Nodes;
    // Textos
    public TextMesh[,] debugTextArray;

    // Booleana para info de desarrollador
    public bool bShowDebug = true;
    public GameObject debugGO = null;

    
    public  GridQueue(int in_height, int in_width, float in_fTileSize = 10.0f, Vector3 in_v3OriginPosition = default)
    {
        // Setear el grid
        iHeight = in_height;
        iWidth = in_width;

        // Inicializar Grid
        InitGrid();
        this.fTileSize = in_fTileSize;
        this.v3OriginPosition = in_v3OriginPosition;

        // Info de desarrollo/debuging
        if (bShowDebug)
        {
            debugGO = new GameObject("GridDebugParent");
            debugTextArray = new TextMesh[iHeight, iWidth];

            // Creacion de mapa visual
            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    debugTextArray[y, x] = CreateWorldText(Nodes[y, x].ToString(),
                    debugGO.transform, GetWorldPosition(x, y) + new Vector3(fTileSize * 0.5f, fTileSize * 0.5f),
                    30, Color.white, TextAnchor.MiddleCenter);
                    //debugTextArray[y, x] = new TextMesh(x, y);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }
            }
            Debug.DrawLine(GetWorldPosition(0, iHeight), GetWorldPosition(iWidth, iHeight), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(iWidth, 0), GetWorldPosition(iWidth, iHeight), Color.white, 100f);
        }
    }

    // Inicializacion del grid. 
    public void InitGrid()
    {
        Nodes = new NodeQueue[iHeight, iWidth];

        for (int y = 0; y < iHeight; y++)
        {
            for (int x = 0; x < iWidth; x++)
            {
                Nodes[y, x] = new NodeQueue(x, y);
            }
        }
    }

    // Funcion GetNode
    public NodeQueue GetNode(int x, int y)
    {
        //Checamos si las coordenadas dadas son validas dentro de nuestra cuadricula
        if (x < iWidth && x >= 0 && y < iHeight && y >= 0)
        {
            return Nodes[x, y];
        }
        //Debug.LogError("Invalid coordinates in GetNode");
        return null;
    }

    //Funcion para los Neighbors del los nodos
    public List<NodeQueue> GetNeighbors(NodeQueue in_currentNode)
    {
        List<NodeQueue> out_Neighbors = new List<NodeQueue>();
        //visitamos al nodo de arriba
        int x = in_currentNode.x;
        int y = in_currentNode.y;
        if (GetNode(y + 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y + 1, x]);
        }
        if (GetNode(y, x - 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x - 1]);
        }
        // Checamos a la derecha
        if (GetNode(y, x + 1) != null)
        {
            out_Neighbors.Add(Nodes[y, x + 1]);
        }
        if (GetNode(y - 1, x) != null)
        {
            out_Neighbors.Add(Nodes[y - 1, x]);
        }


        return out_Neighbors;
    }

    //Funcion para hacer Backtrack cuando se haya encontrado el camino.
    public List<NodeQueue> Backtrack(NodeQueue in_node)
    {
        List<NodeQueue> out_Path = new List<NodeQueue>();
        NodeQueue current = in_node;
        while (current.Parent != null)
        {
            out_Path.Add(current);
            current = current.Parent;
        }
        out_Path.Add(current);
        out_Path.Reverse();

        return out_Path;
    }

    // Funcion para enumerar el camino.
    public void EnumeratePath(List<NodeQueue> in_path)
    {
        int iCounter = 0;

        foreach (NodeQueue n in in_path)
        {
            iCounter++;
            debugTextArray[n.y, n.x].text = n.ToString() +
                 Environment.NewLine + "Step: " + iCounter.ToString();
        }
    }

    // Crear los World Texts
    public static TextMesh CreateWorldText(string in_text, Transform in_parent = null,
        Vector3 in_localPosition = default, int in_iFontSize = 32, Color in_color = default,
        TextAnchor in_textAnchor = TextAnchor.UpperLeft, TextAlignment in_textAlignment = TextAlignment.Left)
    {

        GameObject MyObject = new GameObject(in_text, typeof(TextMesh));
        MyObject.transform.parent = in_parent;
        MyObject.transform.localPosition = in_localPosition;

        TextMesh myTM = MyObject.GetComponent<TextMesh>();
        myTM.text = in_text;
        myTM.anchor = in_textAnchor;
        myTM.alignment = in_textAlignment;
        myTM.fontSize = in_iFontSize;
        myTM.color = in_color;


        return myTM;
    }

    // Funcion que nos regresa la posicion en mundo del tile/cuadro especificado por x , y
    public Vector3 GetWorldPosition(int x, int y)
    {
        

        return new Vector3(x, y) * fTileSize + v3OriginPosition;
    }

    // Euclidiana (hasta el momento)
    public int GetDistance(NodeQueue in_a, NodeQueue in_b)
    {
        int x_diff = (in_a.x - in_b.x);
        int y_diff = (in_b.y - in_a.y);
        //Distancia con formula general
        return (int)Mathf.Sqrt(Mathf.Pow(x_diff, 2) + Mathf.Pow(y_diff, 2)); 
    }

    //Funcion BreadthFirstSearch con Queue.
    public List<NodeQueue> BreadthFirstSearch(int in_startX, int in_startY, int in_endX, int in_endY)
    {
        // Nodos iniciales.
        NodeQueue StartNode = GetNode(in_startY, in_startX);
        // Nodos finales.
        NodeQueue EndNode = GetNode(in_endY, in_endX);

        // En caso de falta de coordenadas, mandar error. 
        if (StartNode == null || EndNode == null)
        {
            Debug.LogError("Invalid coordinates in DeepthFirstSearch");
            return null;
        }

        // Creacion de queue de la lista abierta.
        Queue <NodeQueue> OpenList = new Queue<NodeQueue>();
        // Creacion de la lista cerrada. 
        List<NodeQueue> ClosedList = new List<NodeQueue>();

        // Meter el nodo inicial a la lista abierta. 
        OpenList.Enqueue(StartNode);

        
        // Mientas haya nodos, buscara un camino.
        while (OpenList.Count > 0)
        {

            // Obtener el primer nodo de la lista abierta.
            NodeQueue currentNode = OpenList.Dequeue();
            Debug.Log("Current Node is: " + currentNode.x + ", " + currentNode.y);

            // Revisar si se llega al destino.
            if (currentNode == EndNode)
            {
                // Encontramos un camino.
                Debug.Log("Camino encontrado");

                // Construir ese camino. Para eso hacemos backtracking
                List<NodeQueue> path = Backtrack(currentNode);
                EnumeratePath(path);

                return path;
            }

            // Otra posible solución, con caminos pequeños
            if (ClosedList.Contains(currentNode))
            {
                continue;
            }

            ClosedList.Add(currentNode);

            // Vamos a visitar los vecinos de la derecha y arriba
            List<NodeQueue> currentNeighbors = GetNeighbors(currentNode);

            foreach (NodeQueue neighbor in currentNeighbors)
            {
                if (ClosedList.Contains(neighbor))
                    continue;

                // Si no lo contiene, entonces lo agregamos a la lista Abierta
                neighbor.Parent = currentNode;

                // Lo mandamos a llamar para cada vecino
                OpenList.Enqueue(neighbor);
                // Ajustamos la prioridad, para que cada nuevo que entre sea añada al último
                
            }
            // Debug de info a la consola de los nodos en la lista abierta 
            string RemainingNodes = "Nodes in open list are: ";
            foreach (NodeQueue n in OpenList)
                RemainingNodes += "(" + n.x + ", " + n.y + ") - ";
            Debug.Log(RemainingNodes);

        }

        Debug.LogError("No path found between start and end.");

        return null;
    }

}

// Referencia:
// D. (s. f.). Queue Clase (System.Collections.Generic). Microsoft Learn. https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.queue-1?view=net-7.0
