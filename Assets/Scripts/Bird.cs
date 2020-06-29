using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    public float UpForce;                   //Upward force of the "flap".
    public ParticleSystem JumpParticleSystem;


    private bool IsDead = false;            //Has the player collided with a wall?
    private Animator anim;                  //Reference to the Animator component.
    private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.

    void Start()
    {
        //Get reference to the Animator component attached to this GameObject.
        anim = GetComponent<Animator>();
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsDead == false && GameManager.Instance.PlayerCanMove)
        {
            //Checking Y+ position to avoid flying out of view
            if (Input.GetMouseButtonDown(0) && transform.position.y < 3.5)
            {
                //...tell the animator about it and then...
                anim.SetTrigger("Flap");
                //...zero out the birds current y velocity before...
                rb2d.velocity = Vector2.zero;
                //	new Vector2(rb2d.velocity.x, 0);
                //..giving the bird some upward force.
                JumpParticleSystem.Play();
                rb2d.AddForce(new Vector2(0, UpForce));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Column"))
        {
            //Bird triggered a column score trigger

            GameManager.Instance.BirdScored();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Zero out the bird's velocity
        rb2d.velocity = Vector2.zero;
        // If the bird collides with something set it to dead...
        IsDead = true;
        //...tell the Animator about it...
        anim.SetTrigger("Die");
        //...and tell the game control about it.
        GameManager.Instance.BirdDied();


    }
}
