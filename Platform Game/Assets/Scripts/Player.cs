using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 8.0f;
    private float maxVelocity = 5.0f;
    private float jumpforce = 250;
    private int jumpCount = 2;
    public Rigidbody2D myRigidBody;
    private Vector3 scale;
    private Animator myAnimator;
    private Collider2D cell;

    public int cherries = 0;

    internal int maxHealth = 200;
    public int currentHealth;
    public HealthBar healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void Update()
    {
      
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void ReduceHealth(int damage)
    {
        if (currentHealth <= maxHealth)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
        

    }

    public void AddLife(int fruit)
    {
        if (currentHealth <= 180)
        {
            currentHealth += fruit;
            healthBar.SetHealth(currentHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            AddLife(20);
            Destroy(collision.gameObject);
            cherries += 1;
        }
        if (collision.tag == "Obstacle")
        {
            ReduceHealth(20);
         
        }
    }

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        scale = this.transform.localScale;
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(jumpCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                myRigidBody.AddForce(Vector2.up * jumpforce);
                jumpCount--;
            }
        }

        Debug.Log(jumpCount);

        Run();
    }

    private void Run()
    {
        float force = 0.0f;
        float velocity = Mathf.Abs(myRigidBody.velocity.x);
        float h = Input.GetAxis("Horizontal");

        if (h > 0)
        {
            if (velocity < maxVelocity)
            {
                force = speed;
                scale.x = 1;
                this.transform.localScale = scale;
            }
        }
        else if (h < 0)
        {
            if (velocity < maxVelocity)
            {
                force = -speed;
                scale.x = -1;
                this.transform.localScale = scale;
            }
        }

        myRigidBody.AddForce(new Vector2(force, 0));
        myAnimator.SetFloat("Speed", Mathf.Abs(h));
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Tilemap")
        {
            jumpCount = 2;
        }
       
    }
  
}
