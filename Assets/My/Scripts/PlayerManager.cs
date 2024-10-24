using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;

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

    [SerializeField] private VisualEffect muzzleFlash;
    [SerializeField] private GameObject muzzleLight;


    [Header("IK")]
    [SerializeField]
    private Rig handRig;
    [SerializeField]
    private Rig aimRig;

    private Enemy enemy;

    //임시
    public GameObject pref;
    public Vector3 vec;
    public VisualEffect bomb;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AimCheck();

        //임시
        if(Input.GetKeyDown(KeyCode.C))
        {
            Instantiate(pref, vec, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            bomb.Play();
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

                //enemy = hit.collider.gameObject.GetComponent<Enemy>();
                Transform highestParent = FindHighestParent(hit.collider.gameObject.transform);
                enemy = highestParent.gameObject.GetComponent<Enemy>();
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
                anim.SetBool("Shoot", true);
                muzzleFlash.Play();
                muzzleLight.SetActive(true);
                if(enemy != null)
                {
                    enemy.OnDead();
                }
            }
            else
            {
                anim.SetBool("Shoot", false);
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
}
