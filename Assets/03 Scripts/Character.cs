using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// player movement speed
    /// </summary>
    [SerializeField] protected float speed;

    protected Animator animator;
    protected Rigidbody2D rigi;
    protected bool isAttacking = false;
    protected Coroutine attackRoutine;

    /// <summary>
    /// player move direction
    /// </summary>
    protected Vector2 direction;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
    {
        HandleLayers();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }

    public bool IsMoving()
    {
        return direction.x != 0 || direction.y != 0;
    }

    // chuyển layer animation, hiện đang có 3 layer IdleLayer, WalkLayer, AttackLayer
    private void HandleLayers()
    {
        if (IsMoving())
        {
            ActiveLayer("WalkLayer");
            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);
            //fix bug animation đã dừng nhưng routine attack vẫn còn chạy.
            StopAttack();
        }
        else if (isAttacking)
        {
            ActiveLayer("AttackLayer");
        }
        else
        {
            ActiveLayer("IdleLayer");
        }
    }

    /// <summary>
    /// move player
    /// </summary>
    public void Move()
    {
        rigi.velocity = direction.normalized * speed;
    }

    //set tất cả weight về 0, active theo layer name
    public void ActiveLayer(string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);

    }

    protected void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("attack", isAttacking);
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
}
