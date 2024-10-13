using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int health;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=)
    }

    private void TakeDamage(){
        this.health-=10;
    }
}
