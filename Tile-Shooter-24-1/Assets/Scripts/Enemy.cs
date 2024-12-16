using UnityEngine;

public class Enemy : MonoBehaviour
{
    public LayerMask groundLayer;

    public float wallDistance;
    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    Transform castPoint;

    public Transform player;
    private bool facingRight = true;
    public GameObject bulletPrefab;
    private GameObject Player;

    public int maxAmmo = 10;
    public int currentAmmo;

    public bool isAutomatic = true;
    public float fireInterval = 0.5f;
    public float fireCooldown;
    public float force;

    public float pitchRange = 0.1f;
    AudioSource audioSource;


    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CanSeePlayer(agroRange))
        {
            ChasePlayer();
        }
        else
        {
            IdleMove();
        }

    }

    void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {

            rb.velocity = new Vector2(0, 0);
            transform.localScale = new Vector2(1, 1);
;
        }
        else if(transform.position.x > player.position.x)
        {

            rb.velocity = new Vector2(0, 0);
            transform.localScale = new Vector2(-1, 1);

        }


        if (isAutomatic && CanSeePlayer(agroRange) && fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = fireInterval;
        }

        fireCooldown -= Time.deltaTime;
    }

    bool CanSeePlayer(float distance)
    {
        bool val = false;


        float direction = facingRight ? 1f : -1f;


        Vector2 endPos = castPoint.position + new Vector3(direction * distance, 0, 0);


        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);
        }
        return val;
    }



    void IdleMove()
    {

        RaycastHit2D hit = Physics2D.Raycast(castPoint.position, Vector2.down, wallDistance, groundLayer);


        if (hit.collider == null)
        {
            facingRight = !facingRight; 
            moveSpeed = -moveSpeed; 
            Flip();
        }

        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }


    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }


    public void Shoot()
    {
        float direction = facingRight ? 1f : -1f;
        Vector3 spawnPosition = transform.position + new Vector3(direction * 0.5f, 1, 0); 
        GameObject obj = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D bulletRb = obj.GetComponent<Rigidbody2D>();
        bulletRb.velocity = new Vector2(direction * 5f, 0); 
        audioSource.pitch = Random.Range(1 - pitchRange, 1 + pitchRange);
        audioSource.PlayOneShot(audioSource.clip);
    }
}