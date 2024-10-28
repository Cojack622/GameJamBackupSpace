using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform[] points;
    Queue<Transform> freePoints;
    void Start()
    {
        
        points = GetComponentsInChildren<Transform>();
        freePoints = new Queue<Transform>(points);
        //First node should be the bad one?
        freePoints.Dequeue();
    }

    
    //public Transform? GetPoint() {
    //    Transform point = null;
    //    foreach(PointOfInterest p in points)
    //    {
    //        if(p.Type == type)
    //        {
    //            point = p;
    //            break;
    //        }
    //    }
    //    return point;
    //}

    public Transform GetRandomPoint()
    {
        Transform point = freePoints.Dequeue();
        freePoints.Enqueue(point);
        return point;
    }
}
