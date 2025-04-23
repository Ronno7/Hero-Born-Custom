using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(AudioSource))]
public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float strafeSpeed = 8f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    [Header("Shooting Settings")]
    public GameObject bullet;
    public float bulletSpeed = 100f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] shootClips;

    [Header("Shield Settings")]
    public GameObject shieldObject;
    public float shieldDuration = 60f;

    [Header("Input Buffer Settings")]
    public float inputBufferTime = 0.1f;

    private Rigidbody _rb;
    private SphereCollider _col;
    private AudioSource _audioSource;
    private GameBehavior _gameManager;

    private float verticalInput;
    private float rotationInput;
    private float strafeInput;

    private float jumpBufferTimer;
    private float shootBufferTimer;

    private bool shieldActive;
    private Coroutine shieldRoutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<SphereCollider>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
    }

    private void Start()
    {
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleShooting();
        if (jumpBufferTimer > 0f) jumpBufferTimer -= Time.fixedDeltaTime;
        if (shootBufferTimer > 0f) shootBufferTimer -= Time.fixedDeltaTime;
    }

    private void HandleInput()
    {
        verticalInput = Input.GetAxis("Vertical") * moveSpeed;
        rotationInput = Input.GetKey(KeyCode.Q) ? -rotateSpeed : (Input.GetKey(KeyCode.E) ? rotateSpeed : 0f);
        strafeInput = Input.GetKey(KeyCode.A) ? -strafeSpeed : (Input.GetKey(KeyCode.D) ? strafeSpeed : 0f);
        if (Input.GetKeyDown(KeyCode.Space)) jumpBufferTimer = inputBufferTime;
        if (Input.GetKeyDown(KeyCode.F)) shootBufferTimer = inputBufferTime;
    }

    private void HandleMovement()
    {
        Quaternion deltaRot = Quaternion.Euler(Vector3.up * rotationInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * deltaRot);
        Vector3 move = (transform.forward * verticalInput + transform.right * strafeInput) * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);
    }

    private void HandleJump()
    {
        if (jumpBufferTimer > 0f && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            jumpBufferTimer = 0f;
        }
    }

    private void HandleShooting()
    {
        if (shootBufferTimer > 0f)
        {
            // Play random shoot sound
            if (shootClips != null && shootClips.Length > 0)
            {
                int idx = Random.Range(0, shootClips.Length);
                _audioSource.PlayOneShot(shootClips[idx]);
            }

            // Instantiate and fire bullet
            GameObject newBullet = Instantiate(bullet, transform.position + transform.forward,
                transform.rotation * Quaternion.Euler(90, 0, 0));
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = transform.forward * bulletSpeed;

            shootBufferTimer = 0f;
        }
    }

    private bool IsGrounded()
    {
        Vector3 bottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        return Physics.CheckCapsule(_col.bounds.center, bottom, distanceToGround, groundLayer,
            QueryTriggerInteraction.Ignore);
    }

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        moveSpeed *= multiplier;
        strafeSpeed *= multiplier;
        _gameManager?.ShowSpeedBoost(true);
        Invoke(nameof(ResetSpeed), duration);
    }

    private void ResetSpeed()
    {
        moveSpeed /= 2f;
        strafeSpeed /= 2f;
        _gameManager?.ShowSpeedBoost(false);
    }

    public void ActivateJumpBoost(float multiplier, float duration)
    {
        jumpVelocity *= multiplier;
        _gameManager?.ShowJumpBoost(true);
        Invoke(nameof(ResetJump), duration);
    }

    private void ResetJump()
    {
        jumpVelocity /= 2f;
        _gameManager?.ShowJumpBoost(false);
    }

    public void ActivateShield()
    {
        if (shieldActive)
            return;

        shieldActive = true;
        if (shieldObject != null)
            shieldObject.SetActive(true);
        _gameManager?.ShowShield(true);
        shieldRoutine = StartCoroutine(ShieldDuration());
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldDuration);
        DeactivateShield();
    }

    public void DeactivateShield()
    {
        shieldActive = false;
        if (shieldObject != null)
            shieldObject.SetActive(false);
        _gameManager?.ShowShield(false);
        if (shieldRoutine != null)
        {
            StopCoroutine(shieldRoutine);
            shieldRoutine = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (shieldActive)
                DeactivateShield();
            else
                _gameManager.HP -= 10;
        }
    }

    public void IncreaseHealth(int amount)
    {
        _gameManager.HP += amount;
    }
}
