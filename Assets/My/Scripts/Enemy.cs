using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material mtrlOrg;
    [SerializeField] private Material mtrlPhase;
    [SerializeField] private Material mtrlDissolve;
    [SerializeField] private float fadeTime = 2f;

    private float phaseTargetValue = 1.5f;
    private float phaseCurrentValue = 0f;
    float phaseIncrementRate;

    private float dissolveTargetValue = 0f;
    private float dissolveCurrentValue = 1f;
    float dissolveIncrementRate;

    private Animator anim;
    private byte state; // 0:phase, 1:idle, 2:dissolve

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        state = 0;
        phaseIncrementRate = phaseTargetValue / fadeTime;
        dissolveIncrementRate = dissolveCurrentValue / fadeTime;
        _renderer.material = mtrlPhase;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 0)
        {
            DoPhase();
        }
        else if(state == 1)
        {
            
        }
        else if(state == 2)
        {
            DoDissolve();
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
            state = 1;
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
        anim.SetBool("isDie", true);
    }

    private void changeState2()
    {
        state = 2;
        _renderer.material = mtrlDissolve;
    }
}
