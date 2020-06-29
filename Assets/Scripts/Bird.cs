using UnityEngine;

public class Bird : MonoBehaviour
{
    public float UpForce;                   
    public ParticleSystem JumpParticleSystem;


    private bool IsDead = false;            
    private Animator anim;                  
    private Rigidbody2D rb2d;               

    void Start()
    {
        anim = GetComponent<Animator>();
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
