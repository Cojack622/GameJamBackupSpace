using System;
using UnityEngine;

public class AirLock : MonoBehaviour
{
    public static event Action<bool> AirLockEntered;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AirLockEntered?.Invoke(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AirLockEntered?.Invoke(false);
        }
    }
}
