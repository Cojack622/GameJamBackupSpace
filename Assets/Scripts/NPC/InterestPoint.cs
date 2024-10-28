using UnityEngine;

public class InterestPoint
{
    public Transform position;
    public bool occupied;

    public InterestPoint(Transform position)
    {
        this.position = position;
        occupied = false;
    }
}
