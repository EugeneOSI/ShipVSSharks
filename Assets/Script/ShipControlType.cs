using UnityEngine;

    public enum ShipControl {
        withMovement = 0,
        withoutMovement = 1,
        tinyWings = 2,
    }
public class ShipControlType : MonoBehaviour
{
     [field: SerializeField] public ShipControl currentControlType {get; private set;}
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


