using UnityEngine;
using System.Collections.Generic;
using System;
public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;


    //Fucked up clown version of a singleton
    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;
    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callBack, Grid grid)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack, grid);
        instance.pathRequestQueue.Enqueue(newRequest);

        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;

            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.grid);
        }

    }

    public void FinishedProcessingPath(Vector2[] path, bool success)
    {
        if (path.Length > 0)
        {
            currentPathRequest.callBack(path, success);
        }

        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callBack;
        public Grid grid;

        public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback, Grid _grid)
        {
            pathStart = _start;
            pathEnd = _end; 
            callBack = _callback;    
            grid = _grid;   
        }
    }
}
