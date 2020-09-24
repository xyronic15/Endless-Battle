using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Input variables
    public float horizontalInput;
    public float verticalInput;

    // Animator, rigidbody, colliders for player
    public Animator playerAnim;
    public Rigidbody playerRb;
    public CapsuleCollider bodyCollider;

    // indicator for attacking
    public bool canAttack;
    public int attackNo;
    public bool tempInvincible;

    // jump force, physics and indicators
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround;
    public float jumpTime;
    public float jumpCounter;
    public bool isJumping;
    // Stats handler
    public PlayerStatHandler playerHandler;
    public bool isDead;
    public GameManager gameManager;
    // Sound handlers
    public AudioClip swordSwing;
    public AudioClip getHit;
    public AudioClip jumpSound;
    public AudioClip deadSound;
    public AudioClip rollSound;
    public AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerHandler = GetComponent<PlayerStatHandler>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        bodyCollider = GetComponent<CapsuleCollider>();
        
    }

    // Method called every level loaded by the game manager in order to eep the stat improvements of the player
    public void OnLevelLoaded()
    {
        Debug.Log("Controller: New Level loaded");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        canAttack = true;
        attackNo = 0;
        tempInvincible = false;
        isDead = false;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && GameManager.isGameActive)
        {

            // Check if the player has fallen down
            if (transform.position.y < -50)
            {
                gameManager.GameOver();
            }

            // Take the horizontal and vertical inputs
            horizontalInput = Input.GetAxis("Horizontal");
            // verticalInput = Input.GetAxis("Vertical");

            // Make sure the player is 0 on the z-axis
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            // transform.rotation = Quaternion.Euler(0, 90, 0);

            // Constantly set isOnGround trigger
            playerAnim.SetBool("isOnGround", isOnGround);

            // Constantly set jumping trigger
            playerAnim.SetBool("JumpTrig", isJumping);
            
            // Moving controls method
            Moving();

            // Call the combat controls
            Combat();
        }
        
    }

    void Combat()
    {
        // Constantly update the AttackNo trigger
        playerAnim.SetInteger("AttackNo", attackNo);

        if (Input.GetKeyDown(KeyCode.O) & canAttack & isOnGround)
        {
            attackNo++;
            playerAudio.PlayOneShot(swordSwing);
        }

        // Set control call for rolling backward
        if (Input.GetKeyDown(KeyCode.P) & isOnGround)
        {
            // Call the roll animation
            playerAnim.SetTrigger("DodgeTrig");
            // Set attackno to 0 so wont continue attack after roll
            attackNo = 0;
            // Play roll clip
            playerAudio.PlayOneShot(rollSound);
        }

        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Roll Backward"))
        {
            tempInvincible = true;
        }
        else
        {
            tempInvincible = false;
        }
    }

    void Moving()
    {
        if (attackNo == 0)
        {
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Landing") & !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Roll Backward"))
            {
                // Translate based on the horizontal input
                transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * playerHandler.speed, Space.World);
                // transform.rotation = Quaternion.Euler(0, 90 * horizontalInput, 0);
            }
            
            // Rotate based on the direction you're going
            if (horizontalInput > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                // Set running animation only if on ground
                if (isOnGround)
                {
                    playerAnim.SetBool("Moving", true);
                }
            }
            else if (horizontalInput < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 270, 0);
                // Set running animation only if on ground
                if (isOnGround)
                {
                    playerAnim.SetBool("Moving", true);
                }
            }
            else
            {
                playerAnim.SetBool("Moving", false);
            }

            if (Input.GetKeyDown(KeyCode.W) & isOnGround)
            {
                // Jump controls
                playerRb.velocity = Vector3.up * jumpForce;
                isJumping = true;
                jumpCounter = jumpTime;
                playerAudio.PlayOneShot(jumpSound);          
            }

            // Set the jump so that the height is based on how long the button is held
            if (Input.GetKey(KeyCode.W) & isJumping)
            {
                if(jumpCounter > 0)
                {
                    playerRb.velocity = Vector3.up * jumpForce;
                    jumpCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
            }
            
        }
    }

    public void CheckingCombo()
    {
        // At the end of every animation check this function to see how many presses were made
        // Set canAttack to false so that the payer is limited to pressing during the animations
        canAttack = false;

        if (attackNo == 1 & playerAnim.GetCurrentAnimatorStateInfo(0).IsName("First Attack"))
        { // check if only one attack has been made then changle back to idle or movement anim
            canAttack = true;
            attackNo = 0;
        }
        else if (attackNo >= 2 & playerAnim.GetCurrentAnimatorStateInfo(0).IsName("First Attack"))
        { //check if two or more attack has been made then keep going
            canAttack = true;
        }
        else if (attackNo == 2 & playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Second Attack"))
        { //if only two attacks made then stop and return to idle or movement
            canAttack = true;
            attackNo = 0;
        }
        else if (attackNo >= 3 & playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Second Attack"))
        { //if the third attack happens send back to moving or idle no matter what
            canAttack = true;
        }
        else if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Third Attack"))
        { // Once the third attack happens send back to moving or idle no matter what
            canAttack = true;
            attackNo = 0;
            // Debug.Log("working");
        }
        else
        {
            canAttack = true;
            attackNo = 0;
        }
    }

    // Check if the player is standing on ground
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    // If the player isn't touching the ground then it is falling
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hitbox"))
        {
            Transform attacker = other.transform.root;
            Animator attackerAnim = attacker.GetComponent<Animator>();
            int attackStrength = attacker.GetComponent<EnemyStatHandler>().strength;
            if (attackerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && attacker.CompareTag("Enemy") && !isDead && !tempInvincible && !playerHandler.isInvincible)
            {
                playerAudio.PlayOneShot(getHit);
                playerAnim.SetTrigger("takeDamage");
                playerHandler.TakeDamage(attackStrength);
                transform.Translate(new Vector3(0, 0, -1), Space.Self);
            }
            
        }
    }

    public IEnumerator Death()
    {
        if (!isDead)
        {
            // Set dead to true
            isDead = true;

            // Set animation trigger
            playerAnim.SetTrigger("isDead");

            // Play death noise
            playerAudio.PlayOneShot(deadSound);

            // Wait a few seconds
            yield return new WaitForSeconds(3);

            // Call game over method
            gameManager.GameOver();
        }
    }

    private void OnDestroy()
    {
        if (DontDestroy.instance == this.GetComponent<DontDestroy>())
        {
            Physics.gravity /= gravityModifier;
        }
    }
}