using UnityEngine;
using System.Collections.Generic;
public class SpawnManager : MonoBehaviour
{
    public List<GameObject> patterns;
    private GameObject patternInstance;
    public float xLimit;
    public float yLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPattern();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPattern(){
        patternInstance = Instantiate(patterns[Random.Range(0, patterns.Count)]);
        patternInstance.transform.position = new Vector3(xLimit, patternInstance.transform.position.y, 0);
    }
}
