using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue 
{
    private List<Node> nodes = new List<Node>();

    public List<Node> Nodes
    {
        get { return nodes; }
    }
    public int Count
    {
        get { return nodes.Count; }
    }

    public void Add(Node in_node)
    {
        nodes.Add(in_node);
    }

    //Meter un elemento en cualquier lugar (inicio, medio o final)
    public void Insert(int in_iPriority, Node in_node)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            //Camibar el f_Cost por g_Cost estámos rompiendo los otros algoritmos, pero A* es más importante
            if (nodes[i].f_Cost > in_node.f_Cost)
            {
                nodes.Insert(i, in_node);
                return;
            }
            else if (nodes[i].f_Cost == in_node.f_Cost &&
                     nodes[i].h_Cost > in_node.h_Cost)
            {
                //Este es el caso en que tienen el mismo f_cost pero el node a insertar tiene menor h_cost
                //https://youtu.be/i0x5fj4PqP4
                nodes.Insert(i, in_node);
                return;
            }
        }
        //Si nunca encontró a alguien con mayor costo que él, entonces in_node es el de mayor costo
        //y debe ir hasta atrás de la lista de prioridad
        nodes.Add(in_node);
    }


    public void Remove(Node in_node)
    {
        nodes.Remove(in_node);
    }

    public Node Dequeue()
    {
        Node out_node = nodes[0];
        nodes.RemoveAt(0);
        return out_node;
    }

    public Node GetAt(int i)
    {
        return nodes[i];
    }

    // void RemoveAt(int in_index)
    //
        //nodes.RemoveAt(in_index)
    //}

    public bool Contains(Node in_node)
    {
        return nodes.Contains(in_node);
    }
}
