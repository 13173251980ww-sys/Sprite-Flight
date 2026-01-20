using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float minScale=0.5f;
    public float maxScale=1.5f;
    public float minSpeed=50.0f;
    public float maxSpeed=150.0f;
    public float SpinSpeed=10f;
    public GameObject BoundcyEffect;
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        
        float randomScale=Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomScale, randomScale, 1);
        
        float randomSpeed=Random.Range(minSpeed, maxSpeed)/randomScale;
        
        Vector2 randomDirection = Random.insideUnitCircle;
        
        float randomTporque = Random.Range(-SpinSpeed, SpinSpeed);
        rb.AddTorque(randomTporque);
        rb.AddForce(randomDirection * randomSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject boundcy=Instantiate(BoundcyEffect, transform.position, transform.rotation);
        
        Destroy(boundcy,0.5f);
    }
}
