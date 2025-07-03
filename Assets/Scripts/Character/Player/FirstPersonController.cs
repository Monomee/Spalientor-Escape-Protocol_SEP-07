using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FirstPersonView
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("JoyStick Setting")]
        public FloatingJoystick joystick;

        [Header("Camera Setting")]
        public Camera playerCamera;
        public Sprite crosshairSprite;
        private Image crosshairImg;
        private Vector3 cameraStandPosition;
        private Vector3 cameraCrouchPosition;

        [Header("Movement Parameters")]
        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float crouchSpeed = 2f;
        public float jumpPower = 5f;
        public float sensitivity = 100f;
        public float lookXLimit = 90f;

        [Header("Head Bob Settings")]
        public float bobFrequency = 5f; 
        public float bobAmplitude = 0.1f; 
        private float bobTimer = 0f;
        private Vector3 cameraBasePosition;

        [Header("Crouch Settings")]
        private float standingHeight;
        private float crouchingHeight;

        private Rigidbody rb;
        private CapsuleCollider capsule;
        private float rotationX = 0f;
        private bool isCrouching = false;
        private bool isRunning = false;

        [Header("ForMobile Settings")]
        // Bi?n m?i cho ði?u khi?n c?m ?ng
        private Vector2 touchDeltaPosition;
        private bool isTouchingRightSide = false;
        [SerializeField] private Button jumpButton; 
        [SerializeField] private Button runButton; 
        [SerializeField] private Button crouchButton;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
            rb.freezeRotation = true;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            // Ð?m b?o con tr? không khóa trên mobile
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;

            standingHeight = capsule.height > 0 ? capsule.height : 1.5f;
            crouchingHeight = standingHeight / 2f;

            playerCamera.transform.localPosition = new Vector3(0, capsule.height / 2f - 0.1f, -0.45f);
            cameraStandPosition = playerCamera.transform.localPosition;
            float crouchDelta = 0.3f;
            cameraCrouchPosition = cameraStandPosition - new Vector3(0, crouchDelta, 0);
            cameraBasePosition = playerCamera.transform.localPosition;

            // G?n s? ki?n cho các nút mobile
            if (jumpButton != null) jumpButton.onClick.AddListener(OnJumpButtonPressed);
            if (runButton != null) runButton.onClick.AddListener(ToggleRun);
            if (crouchButton != null) crouchButton.onClick.AddListener(ToggleCrouch);

            SetCrossHair();
        }

        void Update()
        {
            RotateCameraMobile();

            bool wasCrouching = isCrouching;
            isCrouching = Input.GetKey(KeyCode.LeftControl);
            if (isCrouching != wasCrouching)
            {
                ToggleCrouch();
            }

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            UpdateHeadBob();
        }

        void FixedUpdate()
        {
            //Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            //Vector3 move = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            Vector3 move = transform.forward * joystick.Vertical + transform.right * joystick.Horizontal;
            move.y = 0;
            float moveMagnitude = move.magnitude;
            move.Normalize();

            //float speed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
            float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
            rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
        }

        void RotateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(0, mouseX, 0);
        }

        void RotateCameraMobile()
        {
            // X? l? c?m ?ng trên mobile
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    // Ch? x? l? ch?m ? n?a ph?i màn h?nh (ho?c ngoài vùng joystick)
                    if (touch.position.x > Screen.width / 2f)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            isTouchingRightSide = true;
                        }
                        else if (touch.phase == TouchPhase.Moved && isTouchingRightSide)
                        {
                            // L?y delta position c?a ngón tay
                            touchDeltaPosition = touch.deltaPosition;

                            // Tính toán góc xoay
                            float mouseX = touchDeltaPosition.x * sensitivity * Time.deltaTime;
                            float mouseY = touchDeltaPosition.y * sensitivity * Time.deltaTime;

                            // C?p nh?t rotationX (góc nh?n lên/xu?ng)
                            rotationX -= mouseY;
                            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

                            // Xoay camera theo tr?c Y (lên/xu?ng)
                            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

                            // Xoay nhân v?t theo tr?c X (trái/ph?i)
                            transform.Rotate(0, mouseX, 0);
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            isTouchingRightSide = false;
                        }
                    }
                }
            }
        }
        void ToggleCrouch()
        {
            capsule.height = isCrouching ? crouchingHeight : standingHeight; 
            playerCamera.transform.localPosition = isCrouching ? cameraCrouchPosition : cameraStandPosition;
            cameraBasePosition = playerCamera.transform.localPosition; 
        }

        void OnJumpButtonPressed()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }

        void ToggleRun()
        {
            isRunning = !isRunning;
        }
        void UpdateHeadBob()
        {
            if (!IsGrounded() || isCrouching) 
            {
                bobTimer = 0f;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraBasePosition, Time.deltaTime * 10f);
                return;
            }

            Vector3 moveInput = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            if (moveInput.magnitude > 0.1f) 
            {
                //float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
                float speed = isRunning ? runSpeed : walkSpeed;
                bobTimer += Time.deltaTime * bobFrequency * (speed / walkSpeed); 
                float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
                playerCamera.transform.localPosition = cameraBasePosition + new Vector3(0, bobOffset, 0);
            }
            else
            {
                bobTimer = 0f;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraBasePosition, Time.deltaTime * 10f);
            }
        }

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, capsule.bounds.extents.y + 0.1f);
        }

        void SetCrossHair()
        {
            Canvas canvas = playerCamera.gameObject.GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                canvas = new GameObject("AutoCrosshair").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.pixelPerfect = true;
                canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvas.transform.SetParent(playerCamera.transform);
                canvas.transform.position = Vector3.zero;
            }

            crosshairImg = new GameObject("Crosshair").AddComponent<Image>();
            crosshairImg.sprite = crosshairSprite;
            crosshairImg.rectTransform.sizeDelta = new Vector2(25, 25);
            crosshairImg.transform.SetParent(canvas.transform);
            crosshairImg.transform.position = Vector3.zero;
            crosshairImg.raycastTarget = false;
        }
    }
}