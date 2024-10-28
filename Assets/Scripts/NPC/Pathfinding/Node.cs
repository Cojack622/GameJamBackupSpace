using UnityEngine;
using UnityEngine.Rendering;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector2 worldPos;
    public int gridX;
    public int gridY;


    public int gCost;
    public int hCost;
    public Node parent;
    public int movementPenalty;
    int heapIndex;

    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        this.gridX = _gridX;
        this.gridY = _gridY;

        
    }

    public int fCost {  get { return gCost + hCost; } }

    public int HeapIndex {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}