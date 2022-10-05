using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpForce;
    public Vector2 throwForce;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public bool isDead { get; set; }

    public PlayerController nearBody;
    public PlayerController grabbedBody;
    private bool facingRight;
    private bool isGrounded;
    private List<Collider2D> groundContacts;

    private Rigidbody2D rb;
    private Collider2D myCol;
    private Animator anim;

    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        myCol = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        facingRight = true;
        groundContacts = new List<Collider2D>();
    }

    void Start ()
    {
        moveSpeed = LevelController.instance.playerMoveSpeed;
        jumpForce = LevelController.instance.playerJumpForce;
        transform.localScale = new Vector3(transform.localScale.x * LevelController.instance.playerSize,
                                           transform.localScale.y * LevelController.instance.playerSize,
                                           transform.localScale.z);
    }

    void Update ()
    {
        if (Input.GetButton("Horizontal"))
        {
            float moveH = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveH * moveSpeed, rb.velocity.y);
            if ((moveH < 0 && facingRight) || (moveH > 0 && !facingRight))
                Flip();
        }

        anim.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        anim.SetBool("grabbing", grabbedBody != null ? true : false);
        anim.SetBool("grounded", isGrounded);
        anim.SetBool("onTopOfBody", IsOnTopOfBody());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || IsOnTopOfBody())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && GameController.instance.CurrentLives > 1)
        {
            Kill();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (grabbedBody == null)
                GrabBody();
            else
                StartCoroutine(ReleaseBody());
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (grabbedBody != null)
                ThrowBody();
        }

        if (grabbedBody != null)
        {
            grabbedBody.rb.mass = 1;
            grabbedBody.transform.position = new Vector3(transform.position.x,
                                                             transform.position.y + myCol.bounds.size.y + 0.2f,
                                                             transform.position.z);
            if (facingRight && !grabbedBody.facingRight)
                grabbedBody.Flip();
            else if (!facingRight && grabbedBody.facingRight)
                grabbedBody.Flip();
        }
    }

    void Flip ()
    {
        facingRight = !facingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }

    bool IsOnTopOfBody ()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, playerLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void Kill ()
    {
        if (!isDead)
        {
            isDead = true;
            this.enabled = false;
            rb.mass = 1000;
            anim.SetTrigger("dead");
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x * LevelController.instance.playerSize,
                                               transform.localScale.y * LevelController.instance.playerSize,
                                               transform.localScale.z);
            GameController.instance.CurrentLives--;
            GameController.instance.SpawnNewPlayer();
        }
    }

    void GrabBody ()
    {
        if (nearBody != null && grabbedBody == null)
        {
            grabbedBody = nearBody;
        }
    }

    IEnumerator ReleaseBody ()
    {
        if (nearBody != null && grabbedBody != null)
        {
            grabbedBody.rb.mass = 1000;
            PlayerController gb = grabbedBody;
            Physics2D.IgnoreCollision(grabbedBody.GetComponent<Collider2D>(), myCol);
            grabbedBody = null;
            yield return new WaitForSeconds(0.3f);
            Physics2D.IgnoreCollision(gb.GetComponent<Collider2D>(), myCol, false);
        }
    }

    void ThrowBody ()
    {
        if (nearBody != null && grabbedBody != null)
        {
            grabbedBody.rb.mass = 1000;
            if (facingRight)
                grabbedBody.rb.velocity = new Vector2(throwForce.x + Input.GetAxisRaw("Horizontal") * 20, 
                                                      throwForce.y + Input.GetAxisRaw("Vertical") * 20);
            else
                grabbedBody.rb.velocity = new Vector2(-throwForce.x + Input.GetAxisRaw("Horizontal") * 20, 
                                                       throwForce.y + Input.GetAxisRaw("Vertical") * 20);
            grabbedBody = null;
            anim.SetTrigger("throw");
        }
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            isGrounded = true;
            groundContacts.Add(col.collider);
        }
    }

    void OnCollisionExit2D (Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            groundContacts.Remove(col.collider);
            if (groundContacts.Count == 0)
                isGrounded = false;
        }
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "NearCollider" && grabbedBody == null)
        {
            PlayerController body = col.GetComponentInParent<PlayerController>();
            if (body != null)
                nearBody = body;
        }
    }

    void OnTriggerExit2D (Collider2D col)
    {
        if (col.tag == "NearCollider" && grabbedBody == null)
        {
            nearBody = null;
        }
    }
}
