using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public static class ShuffleClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
} 

public class PointOfInterest : MonoBehaviour
{
    public enum PointOfInterestType
    {
        DESKS,
        MEETINGROOM,
        BATHROOM,
        WATERS,
        BREAKROOM
    }
    public static int interestTypeCount = 5;
    public PointOfInterestType Type;    

    Queue<InterestPoint> freePoints;
    List<InterestPoint> occupiedPoints;

    

    public void Start()
    {

        freePoints = new Queue<InterestPoint>();

        Transform[] random = GetComponentsInChildren<Transform>();
        random.Shuffle();
        foreach (Transform t in random)
        {
            freePoints.Enqueue(new InterestPoint(t));
        }

        occupiedPoints = new List<InterestPoint>();
    }

    public InterestPoint? getPoint()
    {
        InterestPoint point = null;
        if (freePoints.Count > 0)
        {
            point = freePoints.Dequeue();
            occupiedPoints.Add(point);
        }

        return point;
    }

    public void LeavePoint(Transform pointPosition)
    {
        int i = 0;
        while (i < occupiedPoints.Count && occupiedPoints[i].position != pointPosition)
        {
            i++;
        }


        freePoints.Enqueue(occupiedPoints[i]);
        occupiedPoints.RemoveAt(i);
    }



}
