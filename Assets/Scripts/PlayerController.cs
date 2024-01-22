using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;

    public float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    private Animator anim;
    public bool isLibraryTrigger;

    public CinemachineFreeLook freeLookCam;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude > .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir * speed * Time.deltaTime);

            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }


        if (isLibraryTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("E'ye basıyorum");
                UIManager.Instance.OpenMainPanel();
                UIManager.Instance.statusText.text = "";
                freeLookCam.gameObject.SetActive(false);
            }
        }
        else
        {
            UIManager.Instance.CloseAllPanels();
            freeLookCam.gameObject.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LibraryTrigger"))
        {
            UIManager.Instance.statusText.text = "KÜTÜPHANEYİ AÇMAK İÇİN \"E\" YE BASINIZ";
            UIManager.Instance.statusText.gameObject.SetActive(true);
            isLibraryTrigger = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LibraryTrigger"))
        {
            UIManager.Instance.statusText.text = "";
            UIManager.Instance.statusText.gameObject.SetActive(false);
            UIManager.Instance.ClearContents();
            isLibraryTrigger = false;

        }
    }

}

