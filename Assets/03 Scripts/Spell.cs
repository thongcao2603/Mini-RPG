using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rigi;

    public Transform MyTarget { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (MyTarget != null)
        {
            //tính hướng từ transform đến target
            Vector2 dir = MyTarget.position - transform.position;
            rigi.velocity = dir.normalized * speed;
            //tính góc xoay của spell
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // nếu chạm enemy thì hủy object-> test, em chưa biết làm gì tiếp
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            MyTarget = null;
            Destroy(gameObject);
        }
    }



}
