using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material mtrlOrg;
    [SerializeField] private Material mtrlPhase;
    [SerializeField] private Material mtrlDissolve;
    [SerializeField] private float fadeTime = 2f;
    private bool isPhased;

    private float phaseTargetValue = 1.5f;
    private float phaseCurrentValue = 0f;
    float phaseIncrementRate;

    private float dissolveTargetValue = 0f;
    [SerializeField] private float dissolveCurrentValue = 1f;
    float dissolveIncrementRate;

    [SerializeField] private SpawnManager spawnManager;
    private Animator anim;
    [SerializeField] private byte state; // 0:phase, 1:idle, 2:run, 3:dissolve
    private bool isAttack;
    private bool isDead;

    private Transform enemyTransform;
    [SerializeField] private Transform playerTransform;
    private NavMeshAgent Nav;

    [SerializeField] private CapsuleCollider coll1;
    [SerializeField] private CapsuleCollider coll2;
    [SerializeField] private CapsuleCollider coll3;
    [SerializeField] private CapsuleCollider coll4;
    [SerializeField] private CapsuleCollider coll5;
    [SerializeField] private CapsuleCollider coll6;
    [SerializeField] private CapsuleCollider coll7;
    [SerializeField] private CapsuleCollider coll8;
    [SerializeField] private attack attack;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("GameManager").GetComponent<SpawnManager>();
        Nav = GetComponent<NavMeshAgent>();
        enemyTransform = gameObject.transform;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Nav.destination = playerTransform.position;
        Nav.isStopped = true;

        anim = GetComponent<Animator>();
        state = 0;
        phaseIncrementRate = phaseTargetValue / fadeTime;
        dissolveIncrementRate = dissolveCurrentValue / fadeTime;
        _renderer.material = mtrlPhase;
        isPhased = false;
        isAttack = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 3)
        {
            DoDissolve();
        }

        if (isDead)
            return;

        checkState();

        if (state == 0)
        {
            DoPhase();
        }
        else if (state == 1)
        {

        }
        else if(state == 2)
        {
            Nav.destination = playerTransform.position;
            Nav.isStopped = false;
            anim.SetBool("isMoving", true);
        }
    }

    private void checkState()
    {
        float dist = Vector3.Distance(playerTransform.position, enemyTransform.position);

        if(isPhased && dist <= 2.7)
        {
            state = 1;
            isAttack = true;
            changeColl(true);
            Nav.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            Nav.isStopped = true;

            anim.SetTrigger("Attack1");
        }
        else if (isPhased && !isAttack && dist > 2.7)
        {
            state = 2;
            changeColl(false);
        }
    }

    private void DoPhase()
    {
        if (phaseCurrentValue < phaseTargetValue)
        {
            phaseCurrentValue += phaseIncrementRate * Time.deltaTime;
            _renderer.material.SetFloat("_SplitValue", phaseCurrentValue);
        }
        else
        {
            state = 2;
            isPhased = true;
            _renderer.material = mtrlOrg;
        }
    }

    private void DoDissolve()
    {
        if(dissolveCurrentValue > dissolveTargetValue)
        {
            dissolveCurrentValue -= dissolveIncrementRate * Time.deltaTime;
            _renderer.material.SetFloat("_SplitValue", dissolveCurrentValue);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDead()
    {
        isDead = true;
        Nav.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        Nav.isStopped = true;
        anim.SetBool("isDie", true);
        spawnManager.RemoveMonsterFromList(gameObject);
    }

    private void changeState2()
    {
        state = 3;
        _renderer.material = mtrlDissolve;
    }

    private void changeColl(bool x)
    {
        coll1.isTrigger = x;
        coll2.isTrigger = x;
        coll3.isTrigger = x;
        coll4.isTrigger = x;
        coll5.isTrigger = x;
        //coll6.isTrigger = x;
        coll7.isTrigger = x;
        coll8.isTrigger = x;
    }

    public void stopAttack() { isAttack = false; }

    public void trunOnAttack() { attack.trunOnAttack(); }
    public void trunOffAttack() { attack.trunOffAttack(); }
}
