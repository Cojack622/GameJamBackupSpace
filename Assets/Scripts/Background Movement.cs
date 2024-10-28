using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    private Vector2 target;
    private int targetNum = 0;
    private float speed = 1f;
    void Start()
    {
        
    }

    private void Update()
    {
        if ((Vector2) transform.position == target)
        {
            switch (targetNum)
            {
                case 0:
                    target = (Vector2) cam.transform.position;
                    targetNum++;
                    break;
                default:
                    target = (Vector2) cam.transform.position + Random.insideUnitCircle.normalized * 5f;
                    targetNum = 0;
                    break;
            }
            speed = Random.Range(0.1f, 0.2f);
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * speed);
    }
}
