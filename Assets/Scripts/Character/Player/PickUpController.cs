using System.Collections;
using System.Collections.Generic;
using FirstPersonView;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform gunHoldPos;
   
    public float throwForce = 500f; 
    public float pickUpRange = 5f; 
    private float rotationSensitivity = 1f; //rotate sen object held
    private GameObject heldObj; 
    private Rigidbody heldObjRb; 
    private bool canDrop = true;
    private bool isHoldingGun = false;
    private int LayerNumber; 

    private FirstPersonController mouseLookScript;
    float originalvalue;

    [Header("UI Settings")]
    [SerializeField] private Button pickUpButton;
    [SerializeField] private Button throwButton;
    private bool canPickUp = false;
    [SerializeField] private Button shootButton; 
    [SerializeField] private Button reloadButton;
    private bool isShooting = false;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");

        mouseLookScript = player.GetComponent<FirstPersonController>();
        originalvalue = mouseLookScript.sensitivity;

        if (pickUpButton != null)
        {
            pickUpButton.onClick.AddListener(OnPickUpButtonPressed);
            pickUpButton.gameObject.SetActive(false); 
        }
        if (throwButton != null)
        {
            throwButton.onClick.AddListener(OnThrowButtonPressed);
            throwButton.gameObject.SetActive(false); // Ẩn nút ném ban đầu
        }
        if (shootButton != null)
        {
            shootButton.gameObject.SetActive(false);
        }
        if (reloadButton != null)
        {
            reloadButton.onClick.AddListener(OnReloadButtonPressed);
            reloadButton.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        CheckForPickUp();
        if (throwButton != null)
        {
            throwButton.gameObject.SetActive(heldObj != null && canDrop);
        }
        if (shootButton != null)
        {
            shootButton.gameObject.SetActive(isHoldingGun && heldObj != null);
        }
        if (reloadButton != null)
        {
            reloadButton.gameObject.SetActive(isHoldingGun && heldObj != null);
        }
        if (isShooting && isHoldingGun && heldObj != null)
        {
            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null && !gunScript.isReloading)
            {
                gunScript.Shoot(gunScript.GetComponent<Animator>());
            }
        }
        //if (Input.GetKeyDown(KeyCode.E)) 
        //{
        //    if (heldObj == null)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit, pickUpRange))
        //        {
        //            if (hit.transform.gameObject.CompareTag("canPickUp"))
        //            {
        //                PickUpObject(hit.transform.gameObject);
        //            }else if (hit.transform.gameObject.CompareTag("weapon"))
        //            {
        //                PickUpGun(hit.transform.gameObject);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if(canDrop == true)
        //        {
        //            StopClipping(); 
        //            DropObject();
        //        }
        //    }
        //}
        if (heldObj != null) 
        {
            MoveObject(); //keep object position at holdPos
            if(!isHoldingGun) RotateObject();
            if (Input.GetKeyDown(KeyCode.Q) && canDrop == true) 
            {
                StopClipping();
                ThrowObject();
            }

        }
    }

    void CheckForPickUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        canPickUp = false;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.transform.gameObject.CompareTag("canPickUp") || hit.transform.gameObject.CompareTag("weapon"))
            {
                canPickUp = true;
            }
        }

        if (pickUpButton != null)
        {
            pickUpButton.gameObject.SetActive(canPickUp && heldObj == null);
        }
    }
    void OnPickUpButtonPressed()
    {
        if (heldObj == null && canPickUp)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickUpRange))
            {
                if (hit.transform.gameObject.CompareTag("canPickUp"))
                {
                    PickUpObject(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.CompareTag("weapon"))
                {
                    PickUpGun(hit.transform.gameObject);
                }
            }
        }
        else if (heldObj != null && canDrop)
        {
            StopClipping();
            DropObject();
        }
    }
    void OnThrowButtonPressed()
    {
        if (heldObj != null && canDrop)
        {
            StopClipping();
            ThrowObject();
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {
            heldObj = pickUpObj; 
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); 
            heldObjRb.isKinematic = true;
            heldObj.transform.SetParent(holdPos);
            heldObj.layer = LayerNumber;
            isHoldingGun = false;
            //make sure object doesnt collide with player, it can cause weird bugs
             Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void PickUpGun(GameObject pickUpGun)
    {
        if (pickUpGun.GetComponent<Rigidbody>())
        {
            heldObj = pickUpGun;
            heldObjRb = pickUpGun.GetComponent<Rigidbody>();

            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null && gunScript.config != null)
            {
                gunScript.enabled = true;
            }
            
            heldObjRb.isKinematic = true;
            heldObj.transform.SetParent(gunHoldPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            heldObj.transform.localScale = Vector3.one;

            heldObj.layer = LayerNumber;
            isHoldingGun = true;
            //make sure object doesnt collide with player, it can cause weird bugs
            //Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
            //foreach (Collider collider in childColliders)
            //{
            //    if (collider!=null) Physics.IgnoreCollision(collider, player.GetComponent<Collider>(), true);
            //}
            Collider[] childGunColliders = heldObj.GetComponentsInChildren<Collider>();
            foreach (Collider collider in childGunColliders)
            {
                if (collider != null) collider.enabled = false;
            }
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        if (isHoldingGun)
        {
            Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in childColliders)
            {
                if (collider != null) Physics.IgnoreCollision(collider, player.GetComponent<Collider>(), false);
            }
            Gun gunScript = heldObj.GetComponent<Gun>();           
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
            Attack attack = GetComponent<Player>().attack;
            if (attack != null)
            {
                attack.CurrentGun = null;
            }
        }
        else
        {
            if (heldObj.GetComponent<Collider>()) Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        }
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        Collider[] childGunColliders = heldObj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childGunColliders)
        {
            if (collider != null) collider.enabled = true;
        }

        heldObj = null;
        isHoldingGun = false;
    }
    void OnReloadButtonPressed()
    {
        if (isHoldingGun && heldObj != null)
        {
            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null)
            {
                StartCoroutine(gunScript.Reload(gunScript.GetComponent<Animator>()));
            }
        }
    }   
    void MoveObject()
    {       
        heldObj.transform.position = isHoldingGun ? gunHoldPos.transform.position:holdPos.transform.position;
    }
    void RotateObject()
    {
        if (!isHoldingGun)
        {
            if (Input.GetKey(KeyCode.R))
            {
                canDrop = false; //make sure throwing can't occur during rotating

                //disable player being able to look around

                mouseLookScript.sensitivity = 0;

                float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
                float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
                //rotate the object depending on mouse X-Y Axis
                heldObj.transform.Rotate(Vector3.down, XaxisRotation);
                heldObj.transform.Rotate(Vector3.right, YaxisRotation);
            }
            else
            {
                //re-enable player being able to look around
                mouseLookScript.sensitivity = originalvalue;

                canDrop = true;
            }
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        if (isHoldingGun)
        {
            Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider collider in childColliders)
            {
                if (collider != null) Physics.IgnoreCollision(collider, player.GetComponent<Collider>(), false);
            }
            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
            Attack attack = GetComponent<Player>().attack;
            if (attack != null)
            {
                attack.CurrentGun = null;
            }
        }
        else
        {
            if (heldObj.GetComponent<Collider>()) Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        }
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        Collider[] childGunColliders = heldObj.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childGunColliders)
        {
            if (collider != null) collider.enabled = true;
        }

        Vector3 throwDirection = Camera.main.transform.forward;
        heldObjRb.AddForce(throwDirection * throwForce);
     
        heldObj = null;
        isHoldingGun = false;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
