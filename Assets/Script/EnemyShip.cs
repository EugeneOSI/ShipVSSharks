using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    private float speed;
    public float health;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = gameManager.currentSpeed;
        transform.position += new Vector3(-1*speed * Time.deltaTime, 0, 0);
        if (transform.position.x < -gameManager.xLimit||health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CannonBall"))
        {
            health--;
        }
    }
}
