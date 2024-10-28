using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    public Grid grid;

    public void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        //this.grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector2 startPos, Vector2 endPos, Grid grid)
    {
        this.grid = grid;
        StartCoroutine(FindPath(startPos, endPos));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {

        
        Vector2[] wayPoints = new Vector2[0];
        bool pathFound = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            //print("Finding path from" + startPos.ToString() + " to " + targetPos.ToString());
            Heap<Node> openSet = new Heap<Node>(grid.maxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            while (openSet.Count > 0)
            {

                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathFound = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    int newMoveCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.movementPenalty;
                    if (newMoveCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);

                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }


            }
        }
        yield return null;
        if (pathFound)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathFound);
    }

    Vector2[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        
    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            //Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            //if(directionNew != directionOld)
            //{
                
                
            //}
            waypoints.Add(path[i].worldPos);
            //directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //return dstX + dstY;
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
