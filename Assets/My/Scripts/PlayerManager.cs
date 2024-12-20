using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator anim;

    [Header("Aim")]
    [SerializeField] private CinemachineVirtualCamera aimCam;
    [SerializeField] private GameObject aimImage;
    [SerializeField] private GameObject aimObj;
    [SerializeField] private float aimObjDis = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private bool isShoot;

    [SerializeField] private VisualEffect muzzleFlash;
    [SerializeField] private GameObject muzzleLight;

    [Header("IK")]
    [SerializeField]
    private Rig handRig;
    [SerializeField]
    private Rig aimRig;

    [Header("Stats")]
    [Range(0, 100)]
    [SerializeField] private int hp;

    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject bomb;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();

        hp = 100;
        isShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();

        if (hp <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("ending");
        }
    }

    private void AimCheck()
    {
        if (input.aim)
        {
            AimControll(true);

            anim.SetLayerWeight(1, 1);

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                //Debug.Log("Name : " + hit.transform.gameObject.name);
                targetPosition = hit.point;
                aimObj.transform.position = hit.point;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Transform highestParent = FindHighestParent(hit.collider.gameObject.transform);
                    enemy = highestParent.gameObject.GetComponent<Enemy>();
                    bomb = null;
                }
                else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Bomb"))
                {
                    bomb = hit.collider.gameObject;
                    enemy = null;
                }
            }
            else
            {
                targetPosition = camTransform.position + (camTransform.forward * aimObjDis);
                aimObj.transform.position = camTransform.position + (camTransform.forward * aimObjDis);
            }

            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);

            SetRigWeight(1);

            if (input.shoot)
            {
                if(!isShoot)
                {
                    anim.SetBool("Shoot", true);
                    muzzleFlash.Play();
                    muzzleLight.SetActive(true);
                    if (enemy != null && bomb == null)
                    {
                        enemy.OnDead();
                    }
                    else if(enemy == null && bomb != null)
                    {
                        VisualEffect effect = bomb.gameObject.GetComponent<VisualEffect>();
                        MeshRenderer renderer = bomb.gameObject.GetComponent<MeshRenderer>();
                        CapsuleCollider collider = bomb.gameObject.GetComponent<CapsuleCollider>();
                        explosionObject explosion = bomb.GetComponent<explosionObject>();

                        collider.enabled = false;
                        renderer.enabled = false;
                        effect.Play();
                        explosion.bomb();
                    }
                    isShoot = true;
                }
                else
                {
                    input.shoot = false;
                }
            }
            else
            {
                anim.SetBool("Shoot", false);
                isShoot = false;
            }
        }
        else
        {
            AimControll(false);
            SetRigWeight(0);
            anim.SetLayerWeight(1, 0);
            anim.SetBool("Shoot", false);
        }
    }

    private void AimControll(bool isCheck)
    {
        aimCam.gameObject.SetActive(isCheck);
        aimImage.SetActive(isCheck);
        controller.isAimMove = isCheck;
    }

    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
        handRig.weight = weight;
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

    public int getHp() { return hp; }

    public void Damaged() { hp -= 15; }
}
