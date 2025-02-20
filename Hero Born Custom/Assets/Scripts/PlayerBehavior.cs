using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;
    public float StrafeSpeed = 8f;
    public float JumpVelocity = 5f;
    public float DistanceToGround = 0.1f;
    public LayerMask GroundLayer;
    public GameObject Bullet;
    public float BulletSpeed = 100f;
    public GameObject shieldObject;

    private float _vInput;
    private float _hInput;
    private float _strafeInput;
    private Rigidbody _rb;
    private bool _isJumping;
    private SphereCollider _col;
    private bool _isShooting;
    private GameBehavior _gameManager;
    private bool shieldActive = false;
    private Coroutine shieldRoutine;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<SphereCollider>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();

        // Ensure the shield is disabled initially.
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        // Q and E for strafing
        _strafeInput = 0f;
        if (Input.GetKey(KeyCode.Q)) _strafeInput = -StrafeSpeed; // Strafe left
        if (Input.GetKey(KeyCode.E)) _strafeInput = StrafeSpeed;  // Strafe right

        _isJumping |= Input.GetKeyDown(KeyCode.Space);
        _isShooting |= Input.GetKeyDown(KeyCode.F);
    }

    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        // Apply movement (forward/backward & strafing)
        Vector3 moveDirection = (this.transform.forward * _vInput + this.transform.right * _strafeInput) * Time.fixedDeltaTime;
        _rb.MovePosition(this.transform.position + moveDirection);

        // Apply rotation
        _rb.MoveRotation(_rb.rotation * angleRot);

        // Ensure jumping only happens when grounded
        if (_isJumping && IsGrounded())
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }
        _isJumping = false; // Reset after checking

        // Shooting logic
        if (_isShooting)
        {
            GameObject newBullet = Instantiate(Bullet, this.transform.position + this.transform.forward, this.transform.rotation * Quaternion.Euler(90, 0, 0));
            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();
            BulletRB.velocity = this.transform.forward * BulletSpeed;
        }
        _isShooting = false;
    }

    private bool IsGrounded()
    {
        Vector3 sphereBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        return Physics.CheckCapsule(_col.bounds.center, sphereBottom, DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);
    }

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        MoveSpeed *= multiplier;
        StrafeSpeed *= multiplier;

        _gameManager?.ShowSpeedBoost(true); // Show Speed Boost text if gameManager is not null

        Debug.Log("Speed Boost Active!");
        Invoke("ResetSpeed", duration);
    }

    private void ResetSpeed()
    {
        MoveSpeed /= 2f;
        StrafeSpeed /= 2f;

        _gameManager?.ShowSpeedBoost(false); // Hide Speed Boost text if gameManager is not null

        Debug.Log("Speed Boost Ended.");
    }

    public void ActivateJumpBoost(float multiplier, float duration)
    {
        JumpVelocity *= multiplier;

        _gameManager?.ShowJumpBoost(true); // Show Jump Boost text if gameManager is not null

        Debug.Log("Jump Boost Active!");
        Invoke("ResetJump", duration);
    }

    private void ResetJump()
    {
        JumpVelocity /= 2f;

        _gameManager?.ShowJumpBoost(false); // Hide Jump Boost text if gameManager is not null

        Debug.Log("Jump Boost Ended.");
    }

    public void ActivateShield()
    {
        if (shieldActive) return;

        shieldActive = true;
        if (shieldObject != null)
            shieldObject.SetActive(true);

        _gameManager?.ShowShield(true);  // Show Shield text

        shieldRoutine = StartCoroutine(ShieldDuration());
        Debug.Log("Shield Activated!");
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(60f);
        DeactivateShield();
    }

    public void DeactivateShield()
    {
        shieldActive = false;
        if (shieldObject != null)
            shieldObject.SetActive(false);

        _gameManager?.ShowShield(false); // Hide Shield text

        if (shieldRoutine != null)
        {
            StopCoroutine(shieldRoutine);
            shieldRoutine = null;
        }
        Debug.Log("Shield Deactivated!");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (shieldActive)
            {
                // Shield takes the hit and is consumed.
                DeactivateShield();
                Debug.Log("Shield absorbed enemy hit!");
            }
            else
            {
                _gameManager.HP -= 10;
            }
        }
    }

    public void IncreaseHealth(int amount)
    {
        _gameManager.HP += amount;
        Debug.Log("Player health increased by " + amount);
    }
}
