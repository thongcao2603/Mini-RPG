using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{


    [SerializeField] private Stat health;
    [SerializeField] private Stat mana;

    [SerializeField] private float initHealth;
    [SerializeField] private float initMana;
    [SerializeField] private GameObject[] projectilePrefab;

    [SerializeField] private Transform[] exitPoints;
    [SerializeField] private Block[] blocks;

    public Transform MyTarget { get; set; }

    private int exitIndex;
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Block();
        GetInput();
        base.Update();
        // Debug.Log(MyTarget);
    }

    /// <summary>
    /// Listen to player input
    /// </summary>
    private void GetInput()
    {
        // for debug
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
        }


        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 1;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 0;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 2;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 3;
            direction += Vector2.right;
        }
    }

    private IEnumerator Attack(int spellIndex)
    {

        isAttacking = true;
        animator.SetBool("attack", isAttacking);

        // hardcode time cho routine attack 1f
        yield return new WaitForSeconds(1f);

        if (MyTarget != null && InSightLine())
        {
            //có 3 projectilePrefab ứng với 3 button, exitPoint là các gameObject nằm trong player ứng với 4 hướng up, down,left,right
            //khi thay đổi hướng exitPoint sẽ thay đổi theo index
            Spell s = Instantiate(projectilePrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
            //gán target của spell thành target của player để nó bay về phía target
            s.MyTarget = MyTarget;
        }
        // Debug.Log("done cast");
        StopAttack();

    }

    //cast spell theo tham số truyển vào từ listener của button ngoài Canvas
    public void CastSpell(int spellIndex)
    {
        //kiểm tra nếu có target, không tấn công, không di chuyển, raycast không bị block thì mới cho tấn công
        if (MyTarget != null && !isAttacking && !IsMoving() && InSightLine())
        {
            attackRoutine = StartCoroutine(Attack(spellIndex));
        }

    }

    /// <summary>
    /// check hero facing with target
    /// </summary>
    /// <returns></returns>
    private bool InSightLine()
    {
        Vector3 dir = MyTarget.position - transform.position;
        //256 là hardcode int layer của các mảng block chặn tấn công từ phía sau(hàm phía dưới)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(MyTarget.position, transform.position), 256);

        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// khi xoay về phía trước sẽ bật 2 gameobject có collider để che phía sau lưng,
    /// nếu raycast trúng collider block này sẽ chặn raycast
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactive();
        }
        blocks[exitIndex].Active();
    }

}
