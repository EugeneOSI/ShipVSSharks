using UnityEngine;
using System;
public class ShipTop : MonoBehaviour
{
    public static Action ShipTopDamaged;
    [SerializeField] Collider2D _collider;
    /*void Update()
    {
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Water"))){
            ShipTopDamaged?.Invoke();
        }
        if (_collider.)
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water")){
            ShipTopDamaged?.Invoke();
        }
    }
}
