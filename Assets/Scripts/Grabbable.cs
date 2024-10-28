using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Grabbable : MonoBehaviour
{
    private Transform snapPoint;
    private Transform player;
    [SerializeField]
    private GameObject particles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        PlayerGrabbing.Grabbing += Grabbed;
    }

    public virtual void Update()
    {
        if (snapPoint != null)
        {
            transform.position = snapPoint.position;
            Vector3 target = player.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, target);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 10000f * Time.deltaTime);
        }
    }

    public virtual void Grabbed(GameObject gm, Transform snap, Transform p)
    {
        if(gm == this.gameObject)
        {
            if (snapPoint != null)
            {
                snapPoint = null;
                player = null;
                if (particles != null)
                    particles.SetActive(true);
            }
            else
            {
                snapPoint = snap;
                player = p;
                if(particles != null)
                    particles.SetActive(false);
            }
        }
    }

    void OnDestroy()
    {
        PlayerGrabbing.Grabbing -= Grabbed;
    }
}
