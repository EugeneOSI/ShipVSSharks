using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D _RB;
    [SerializeField] float _acceleration = 10f;
    public float _destroyTime = 3f;
    void Awake()
    {
        _RB = GetComponent<Rigidbody2D>();
        _RB.AddForce(transform.right * _acceleration, ForceMode2D.Impulse);
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime(){
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shark")||other.gameObject.CompareTag("Aim"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Water")) Destroy(gameObject);
    }
}
