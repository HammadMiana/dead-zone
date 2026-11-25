using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller;
    public Animator animator;
    public Camera playerCamera;

    [Header("Movement (Joystick Only)")]
    public float walkSpeed = 3f;
    public float gravity = -9.8f;
    Vector3 velocity;

    [Header("Touch Rotation")]
    public float touchRotationSpeed = 0.2f;

    [Header("Health")]
    public ControlledHealthSystem health_down;
    bool isDead = false;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    float nextFireTime;
    bool isShooting;

    [Header("Reload System")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    bool isReloading = false;
    public AudioClip reloadSound;
    public Text ammoText;

    [Header("Mobile Joystick")]
    public RectTransform joystickBG;
    public RectTransform joystickHandle;
    public float joystickRadius = 100f;
    Vector2 joystickInput;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Effects")]
    public GameObject muzzleSparkPrefab;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isDead) return;

        if (!isReloading)
        {
            HandleMovement();
            HandleTouchRotation();
            HandleShooting();
        }

        UpdateAmmoUI();
        if (health_down != null && health_down.totalHealthZero)
            Die();
    }
    void UpdateAmmoUI()
    {
        if (ammoText)
        {
            if (isReloading)
                ammoText.text = "Ammo : Reloading...";
            else
                ammoText.text = "Ammo : " + currentAmmo + " / " + magazineSize;
        }
    }
    // ================= MOVEMENT =================
    void HandleMovement()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir =
            camForward * joystickInput.y +
            camRight * joystickInput.x;

        if (moveDir.magnitude > 0.1f)
        {
            controller.Move(moveDir * walkSpeed * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        ApplyGravity();
        animator.SetBool("isIdle", !isShooting && moveDir.magnitude <= 0.1f);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ================= TOUCH ROTATION =================
    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.position.x > Screen.width * 0.5f &&
                    touch.phase == TouchPhase.Moved &&
                    !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    float deltaX = touch.deltaPosition.x;
                    transform.Rotate(Vector3.up, deltaX * touchRotationSpeed);
                }
            }
        }
    }

    // ================= SHOOTING =================
    void HandleShooting()
    {
        animator.SetBool("isShooting", isShooting);

        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
            return;
        }

        if (isShooting && Time.time >= nextFireTime)
        {
            ShootBullet();
            currentAmmo--;
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootBullet()
    {
        if (currentAmmo <= 0) return;
        if (!bulletPrefab || !firePoint || !playerCamera) return;

        Vector3 shootDir = playerCamera.transform.forward;

        if (muzzleSparkPrefab)
        {
            GameObject spark = Instantiate(
                muzzleSparkPrefab,
                firePoint.position,
                Quaternion.LookRotation(shootDir)
            );
            spark.transform.parent = firePoint;
            Destroy(spark, 0.15f);
        }

        if (audioSource && shootSound)
            audioSource.PlayOneShot(shootSound);

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(shootDir)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (!rb) rb = bullet.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.velocity = shootDir * bulletSpeed;

        Destroy(bullet, 5f);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        isShooting = false;

        animator.SetTrigger("isReloading");

        if (audioSource && reloadSound)
            audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;
    }

    // ================= JOYSTICK =================
    public void JoystickDrag(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG,
            ped.position,
            ped.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, joystickRadius);
        joystickHandle.anchoredPosition = pos;
        joystickInput = pos.normalized;
    }

    public void JoystickRelease()
    {
        joystickInput = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    // ================= UI BUTTONS =================
    public void ShootDown()
    {
        if (!isReloading)
            isShooting = true;
    }

    public void ShootUp()
    {
        isShooting = false;
    }

    public void ReloadButton()
    {
        if (!isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    // ================= DEATH =================
    void Die()
    {
        animator.SetBool("isDead", true);
        isDead = true;
    }
}
