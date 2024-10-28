using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float baseMoveSpeed = 8f;
    [SerializeField]
    private float grabMoveSpeed = 5f;
    private float speed = 5f;
    [SerializeField]
    private float rotationSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;
    private bool animPlaying = false;
    [SerializeField]
    private Camera cam;

    private bool isFloating = false;
    private bool isTouchingWall = false;
    private Vector2 spaceVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AirLock.AirLockEntered += Floating;
        PlayerGrabbing.ObjectGrabbed += Grabbing;
        speed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, moveInput);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            if (!animPlaying && !isFloating)
            {
                anim.Play("WalkCycle", 0, 0.0f);
                animPlaying = true;
            }
            else if(isFloating && animPlaying)
            {
                anim.Play("Idle", 0, 0.0f);
            }
        }
        else
        {
            anim.Play("Idle", 0, 0.0f);
            animPlaying = false;
        }

        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void FixedUpdate()
    {
        if (!isFloating || (isFloating && isTouchingWall))
        {
            rb.linearVelocity = moveInput.normalized * speed;
        }
        else
        {
            rb.linearVelocity = spaceVelocity.normalized * speed * .68f;
        }
    }

    private void Floating(bool isFLoat)
    {
        isFloating = isFLoat;
        isTouchingWall = false;
        spaceVelocity = rb.linearVelocity;
    }

    private void Grabbing(bool grabbing)
    {
        if (grabbing)
        {
            speed = grabMoveSpeed;
        }
        else
        {
            speed = baseMoveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTouchingWall && isFloating && collision.gameObject.layer == 7)
        {
            isTouchingWall = true;
            spaceVelocity = Vector2.zero;
        }

        if (collision.gameObject.CompareTag("Coworker"))
        {
            //End game
            print("Collided with coworker");
            SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isTouchingWall && isFloating && collision.gameObject.layer == 7)
        {
            isTouchingWall = false;
            spaceVelocity = rb.linearVelocity;
        }
    }

    void OnDestroy()
    {
        AirLock.AirLockEntered -= Floating;
        PlayerGrabbing.ObjectGrabbed -= Grabbing;
    }
}
