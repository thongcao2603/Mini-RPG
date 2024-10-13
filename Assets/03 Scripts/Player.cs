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
        yield return new WaitForSeconds(1f);
        if (MyTarget != null && InSightLine())
        {
            Spell s = Instantiate(projectilePrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
            s.MyTarget = MyTarget;
        }
        // Debug.Log("done cast");
        StopAttack();

    }

    public void CastSpell(int spellIndex)
    {
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(MyTarget.position, transform.position), 256);

        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// add block behind hero -> cannot attack 
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
