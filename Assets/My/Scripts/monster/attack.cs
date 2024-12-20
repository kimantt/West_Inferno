using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    [SerializeField] private bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void trunOnAttack() { canAttack = true; }
    public void trunOffAttack() { canAttack = false; }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Player" && canAttack)
        {
            canAttack = false;
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            player.Damaged();
        }
    }
}
