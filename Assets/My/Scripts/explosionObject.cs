using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionObject : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        radius = 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    public void bomb()
    {
        colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider col in colliders)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Transform highestParent = FindHighestParent(col.gameObject.transform);
                Enemy enemy = highestParent.gameObject.GetComponent<Enemy>();
                enemy.OnDead();
            }
        }
    }

    Transform FindHighestParent(Transform child)
    {
        Transform current = child;

        // 부모가 없을 때까지 반복
        while (current.parent != null)
        {
            current = current.parent;
        }

        return current; // 가장 높은 부모 반환
    }
}
