using UnityEngine;

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
                anim.SetTrigger("Flap");
                rb2d.velocity = Vector2.zero;
                JumpParticleSystem.Play();
                rb2d.AddForce(new Vector2(0, UpForce));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Column") && IsDead == false)
        {
            //Bird triggered a column score trigger
            GameManager.Instance.BirdScored();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //if bird detects collision with anything physic ( ground or columns ) dies.
        if (IsDead == false)
        {
            rb2d.velocity = Vector2.zero;
            IsDead = true;
            anim.SetTrigger("Die");
            GameManager.Instance.BirdDied();
        }



    }
}
