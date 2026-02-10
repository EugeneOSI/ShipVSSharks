using UnityEngine;

public class WaterWave : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1*speed * Time.deltaTime, 0, 0);
    }
}
