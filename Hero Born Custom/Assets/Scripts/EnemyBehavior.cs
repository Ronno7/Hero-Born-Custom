using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource))]
public class EnemyBehavior : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] hitClips;

    public Transform Player;
    public Transform PatrolRoute;
    public List<Transform> Locations;

    private int _lives = 4;
    private int _locationIndex = 0;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;
    private List<Transform> orbitCubes = new List<Transform>();

    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy Down.");
            }
        }
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        Player = GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
        InitializeOrbitCubes();
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
            Locations.Add(child);
    }

    void InitializeOrbitCubes()
    {
        foreach (Transform child in transform)
            if (child.name != "Cube")
                orbitCubes.Add(child);
    }

    void Update()
    {
        if (_agent.remainingDistance < 0.2f && !_agent.pathPending)
            MoveToNextPatrolLocation();
    }

    void MoveToNextPatrolLocation()
    {
        if (Locations.Count == 0)
            return;
        _agent.destination = Locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % Locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _agent.destination = Player.position;
            Debug.Log("Player detected - attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player out of range, resume patrol");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            // Play random hit sound
            if (hitClips != null && hitClips.Length > 0)
            {
                int idx = Random.Range(0, hitClips.Length);
                _audioSource.PlayOneShot(hitClips[idx]);
            }

            // Decrease health.
            EnemyLives -= 1;
            Debug.Log("Critical Hit!");

            // Remove orbiting cube if available.
            if (orbitCubes.Count > 0)
            {
                Transform cubeToRemove = orbitCubes[0];
                orbitCubes.RemoveAt(0);
                Destroy(cubeToRemove.gameObject);
            }
        }
    }
}
