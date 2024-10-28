using System;
using UnityEngine;

public class PlayerGrabbing : MonoBehaviour
{
    private bool isGrabbing = false;
    private GameObject target;
    [SerializeField]
    private Transform holdPoint;
    public static event Action<GameObject, Transform, Transform> Grabbing;
    public static event Action<bool> ObjectGrabbed;
    private bool isFloating = false;
    void Start()
    {
        AirLock.AirLockEntered += Floating;
    }

    private void Update()
    {
        if (target != null && !isGrabbing && Input.GetKeyDown(KeyCode.Space))
        {
            isGrabbing = true;
            ObjectGrabbed?.Invoke(isGrabbing);
            Grabbing?.Invoke(target, holdPoint, this.gameObject.transform);
            Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }

        if (target != null && isGrabbing && Input.GetKeyUp(KeyCode.Space) && !isFloating)
        {
            isGrabbing = false;
            ObjectGrabbed?.Invoke(isGrabbing);
            Grabbing?.Invoke(target, holdPoint, this.gameObject.transform);
            Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
    }

    private void Floating(bool isFLoat)
    {
        isFloating = isFLoat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (!isGrabbing && target == null)
            {
                target = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (target == collision.gameObject && !isGrabbing)
            {
                target = null;
            }
        }
    }

    void OnDestroy()
    {
        AirLock.AirLockEntered -= Floating;
    }
}
