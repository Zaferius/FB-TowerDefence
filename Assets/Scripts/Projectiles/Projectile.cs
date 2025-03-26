using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    public float speed = 10f;

    public void SetTarget(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (!(Vector3.Distance(transform.position, target.position) < 0.2f)) return;
        
        var enemy = target.GetComponent<IHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}