using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Grid : MonoBehaviour
{
    
    public LayerMask unwalkableMask;
    public LayerMask enemyMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;



    float nodeDiamter;
    int gridSizeX, gridSizeY;

    float timer = 0f;
    public float timeWait;


    public bool toggleGrid;
    private void Awake()
    {
        nodeDiamter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiamter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiamter);

        CreateGrid();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeWait)
        {
            CreateGrid();   
            timer = 0f;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiamter + nodeRadius) + Vector2.up * (y*nodeDiamter + nodeRadius);
                
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                if (Physics2D.OverlapCircle(worldPoint, nodeRadius, enemyMask))
                {
                    grid[x, y].movementPenalty = 2;
                }
               
            }

        }
    }

    public int maxSize { get
        {
            return gridSizeX * gridSizeY;
        } 
    }

    public void UpdateNode(int x, int y, bool newValue)
    {
        grid[x,y].walkable = newValue;
    }
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    Node neighbor = grid[checkX, checkY];
                    neighbors.Add(neighbor);

                    if (!neighbor.walkable)
                    {
                        node.movementPenalty += 2;
                    }
                }
            }
        }
        return neighbors;
    }
    public Node NodeFromWorldPoint(Vector2 worldPos)
    {
        //float gridLeft = (transform.position.x - gridWorldSize.x / 2);
        //float gridTop = (transform.position.y + gridWorldSize.y / 2);
        Vector2 localPos = new Vector2(transform.position.x - worldPos.x, transform.position.y - worldPos.y); 

        float percentX = (localPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (localPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if(grid != null && toggleGrid)
        {

            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector2.one * (nodeDiamter-0.1f));
            }
        }
    }
}

