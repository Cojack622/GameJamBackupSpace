using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    [SerializeField]
    public GameObject WinScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            WinScreen.SetActive(true);
        }
    }
}
