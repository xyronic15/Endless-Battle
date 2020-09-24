using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // references
    Transform target;
    NavMeshAgent agent;
    public Animator enemyAnim;
    public EnemyStatHandler enemyHandler;
    public GameObject healthDrop;
    public GameManager gameManager;
    public AudioClip getHit;
    public AudioClip swordSwing;
    public AudioClip deathSound;
    public AudioSource enemyAudio;

    // tracking values
    public float lookRadius = 10f;
    public float attackCoolDownCount = 2.0f;
    public bool attackCoolingDown;
    
    public bool isDead;
    public int scoreValue;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyHandler = GetComponent<EnemyStatHandler>();
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyAudio = GetComponent<AudioSource>();
        attackCoolingDown = false;
        agent.speed = enemyHandler.speed;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic for pathfinding
        float distance = Vector3.Distance(target.position, agent.transform.position);

        // Make sure the player is 0 on the z-axis
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (distance <= lookRadius && !isDead)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance && !enemyAnim.GetCurrentAnimatorStateInfo(0).IsTag("Damaged"))
            {
                // Face the target then attack
                FaceTarget();

                Attack();
            }
        }

        // Set moving animation based on movement
        enemyAnim.SetBool("isMoving", (agent.velocity.magnitude > 0));

    }

    void Attack()
    {
        if (!attackCoolingDown)
        {
            enemyAnim.SetTrigger("AttackTrig");
            attackCoolingDown = true;
            agent.velocity = new Vector3(0, 0, 0);
            enemyAudio.PlayOneShot(swordSwing);
        }
        else
        {
            attackCoolDownCount -= Time.deltaTime;
            if(attackCoolDownCount <= 0)
            {
                attackCoolDownCount = 2.0f;
                attackCoolingDown = false;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        // Allow us to see the looking radius of the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void OnTriggerEnter(Collider other)
    {
        Transform attacker = other.transform.root;
        Animator attackerAnim = attacker.GetComponent<Animator>();
        int attackStrength = attacker.GetComponent<PlayerStatHandler>().strength;

        if (other.CompareTag("Hitbox") && attackerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && attacker.CompareTag("Player") && !isDead)
        {
            enemyAudio.PlayOneShot(getHit);
            enemyAnim.SetTrigger("takeDamage");
            enemyHandler.TakeDamage(attackStrength);
            transform.Translate(new Vector3(0, 0, -1), Space.Self);
        }
    }

    public void Death()
    {
        if (!isDead)
        {
            // Set dead to true
            isDead = true;

            // Set animation trigger
            enemyAnim.SetTrigger("isDead");

            // Play death sound
            enemyAudio.PlayOneShot(deathSound);

            // Update the game score
            gameManager.UpdateScore(scoreValue);
        }
    }

    private void OnDestroy()
    {
        if (Random.Range(0, 10) < 5 && GameManager.isGameActive)
        {
            Instantiate(healthDrop, new Vector3(transform.position.x, transform.position.y + 2, 0), healthDrop.transform.rotation);
        }
    }
}
