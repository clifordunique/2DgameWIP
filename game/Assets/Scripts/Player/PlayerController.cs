using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//katana zero walljump: movement disabled, fixed distance jump from wall

public class PlayerController : MonoBehaviour
{
    readonly int playerBaseHealth = 10;
    public HealthSystem health;

    //Levels
    public Level speedLevel;
    public Level rangedLevel;
    public Level attackLevel;
    Level magicLevel;

    public float speed;
    private readonly float arrowForce = 130f;
    private readonly float fireballForce = 250f;  //temp

    //public AudioManager xadada;
    public enum AnimState {IDLE, MOVING, INAIR, FALLING, WALLSLIDING};
    //jump
    [Header("Jumping")]
    public float jumpForce;
    public int extraJumpValue;
    public int extraJump;

    public ParticleSystem boostEffects;
    private readonly float boostedSpeedMod = 1.3f;
    public GameObject boostAnim;

    //groundsensor
    [Header("GroundSensor")]
    public bool grounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask layerGround;
    public LayerMask layerEnemies;

    [Header("EquippedSpells")]
    //public Spells spell1;
    //public Spells spell2;

    private Animator animator;
    private Rigidbody2D body2d;
    private BoxCollider2D box2d;
    //boxcollider seems to cuase the player to get stucked
    private EdgeCollider2D edge2d;
    private CapsuleCollider2D cap2d;

    private float baseSpeed = 5.0f;
    private bool combatIdle = false;
    private bool isDead = false;

    //speed modifier for different speed when in different actions(attack,shooting etc.)
    public float speedMod;

    public readonly float jumpMeleeSpeedModValue = 0.5f;
    public readonly float meleeSpeedModValue = 0.1f;
    public readonly float rangedSpeedModValue = 0.03f;
    public readonly float spellCastSpeedModValue = 0.02f;
    public readonly float defaultSpeedModValue = 1;

    public float fallMultiplier = 2.5f;       //makes falling more natural
    public readonly float speedPerLevel = 0.5f;
    public readonly float arrowForcePerLevel = 50f;


    //state(anim)
    public bool attacking;
    public bool staggered;
    public bool inAction;

    //magic spells
    public GameObject arrowPrefab;
    public GameObject fireballPrefab;
    public GameObject thunderPrefab;
    public GameObject slashPrefab;

    //dust effects
    public ParticleSystem DustEffectsJump;
    public ParticleSystem DustEffectsWalk;
    public float startTimeBetweenTrails = 0.2f;
    private float timeBetweenTrails = 0f;

    public int meleeDamage;
    public int rangedDamage;

    private readonly float meleeHitRange = 0.4f;
    public Transform meleeHitPos;
    private Vector2 finalMeleeHitOffset = new Vector2(0.05f, 0f);
    public float jumpAttackRange;
    public float baseArrowForce;

   
    public Transform jumpAttackPos;

    public bool inCombat;
    public bool boosted;
    //for sliding
    public bool sliding;


    [NonSerialized] public readonly float slideStandUpSpeedMod = 0.5f;
    public float slidingForce;   

    //for walljump/slide
    bool nextToWallInAir;
    bool isTouchingFront;
    public Transform frontCheck;
    public float wallCheckRadius;
    public float wallSlidingSpeed;
    public float wallJumpForce;

    bool wallJumping;

    public Animator camAnim;

    private Vector2 lookdirectionVector2 = new Vector2(1, 0);

    [SerializeField]
    private AnimState animationState = AnimState.IDLE;
    //int x = 0;

    //newattackcombo
    int noOfClicksCombat = 0;
    float lastClickedTime = 0f;

    //time for out of combat idle animation
    float inCombatTimer = 3f;

    //original collider saved for reverting after sliding
    Vector2[] originalColliderPoints;

    Vector2 originalColliderOffset;
    Vector2 originalColliderSize;

    public PlayerSpellsManager spells;
    private enum SpellInput {U, I};
    private SpellInput spellInput;

    private float cooldown1;
    private float cooldown2;
    // Use this for initialization
    void Start()
    {        
        
        sliding = false;
        wallJumping = false;
        attacking = false;
        staggered = false;

        rangedLevel = new Level();
        attackLevel = new Level();
        magicLevel = new Level();
        speedLevel = new Level();



        speedMod = defaultSpeedModValue;
        extraJump = 0;

        originalColliderPoints = GetComponent<EdgeCollider2D>().points;
        

        Debug.Log(extraJump);

        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        edge2d = GetComponent<EdgeCollider2D>();
        cap2d = GetComponent<CapsuleCollider2D>();
        health = new HealthSystem(playerBaseHealth);

        spells = GetComponent<PlayerSpellsManager>();

        originalColliderOffset = cap2d.offset;
        originalColliderSize = cap2d.size;

        speed = baseSpeed;
        //arrowForce = baseArrowForce;
        //speedLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, layerGround);
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.3f,0.05f), 0, layerGround);
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, wallCheckRadius, layerGround);
        animator.SetBool("Grounded", grounded);
        //speed = baseSpeed + speedLevel * speedPerLevel;
        
        if (health.GetHealth() <= 0 && isDead != true)
        {
           Die();
        }

        //Things that disable looking around and moving
        inAction = staggered || sliding || wallJumping;

        //Debug.Log(grounded);
        //Debug.Log(XSpeedCheck());

        lookdirectionVector2 = new Vector2(transform.localScale.x, 0);

        if (staggered)
        {
            StopMovement();
        }

        // -- Handle input and movement --

        //float horizontalInput = UnityService.GetAxis("Horizontal");
        float horizontalInput;
        if (GetComponent<PopUpMovesMenu>().paused)
        {
            horizontalInput = 0;
        }
        else 
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        Debug.Log("grounded" + grounded);
        Debug.Log(isTouchingFront);
        if (isTouchingFront == true && grounded == false)
        {
            nextToWallInAir = true;
            //body2d.velocity = new Vector2 (body2d.velocity.x, 0);
        }
        else
        {
            nextToWallInAir = false;
        }
        
        if (nextToWallInAir && ! attacking )  //!attacking to avoid bug from attacking == true when wallsliding
        {
            //cannot fall faster than wallsldingspeed, without the clamp, it would just fall with gravity
            body2d.velocity = new Vector2(body2d.velocity.x, Mathf.Clamp(body2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            

            //no extra jump when wallslide start
            //temp
        }


        if (!isDead )
        {
            //state for loop animations.
            if ((Mathf.Abs(horizontalInput) > 0) && grounded)
            {
                animationState = AnimState.MOVING;
                if (!attacking)
                {
                    DustEffectsWhileWalking();
                }
                //Moving
            }

            else if ((body2d.velocity.y > 0) && !grounded && !nextToWallInAir)
            {
                animationState = AnimState.INAIR;
            }
            //falling
            else if ((body2d.velocity.y <= 0) && !grounded && !nextToWallInAir)
            {
                animationState = AnimState.FALLING;
            }
            else if (nextToWallInAir && !attacking) //!attacking to prevent bug caused by jump attack going into wallsliding
            {
                //Wallsliding spite are flipped, no change in lookdirectionVector2 needed
                animationState = AnimState.WALLSLIDING;
                Debug.Log("AYYY 2");
            }
            //falling

            //Idle
            else
            {
                animationState = (int)AnimState.IDLE;
            }
            animator.SetInteger("AnimState", (int)animationState);
            //Debug.Log(x);
        }
        
        if (body2d.velocity.y < 0)
        {
            body2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (wallJumping)
        {
            //tbc wont be affected by input
            body2d.velocity = new Vector2(body2d.velocity.x, body2d.velocity.y);

        }
        if (!inAction)
        {
            body2d.velocity = new Vector2(HorizontalVelocity(horizontalInput), body2d.velocity.y);
            if (!attacking)             //!inAction and !attacking
            {
                SetLookDirection(horizontalInput);
                
            }
        }

        //if (Time.time - lastClickedTime > maxComboTime)
        //{
        //    noOfClicksCombat = 0;
        //}

        //Attack
        if (!GetComponent<PopUpMovesMenu>().paused)         //if not paused
        {
            if (Input.GetKeyDown("j") && grounded)
            {
                lastClickedTime = Time.time;
                noOfClicksCombat++;
                if (noOfClicksCombat == 1)
                {
                    animator.SetBool("Attack1", true);
                }
                noOfClicksCombat = Mathf.Clamp(noOfClicksCombat, 0, 3);
            }
            else if (Input.GetKeyDown("j") && !grounded)      //distance from ground with raycast
            {
                RaycastHit2D hit = Physics2D.Raycast(body2d.position, Vector2.down, 1.2f, layerGround);
                if (hit.collider == null)
                {
                    animator.SetTrigger("JumpAttack");
                }
         
            }
            if (!attacking && !inAction)
            {
                //else in case two inputs at the same time, unlikely though)
                if (Input.GetKeyDown("w"))
                {
                    Jump();
                }
                else if (Input.GetKeyDown("s") && horizontalInput != 0 && grounded)
                {
                    Slide();
                }
                else if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K)) && grounded)
                {
                    StartRangedAttack();
                }
                else if (Input.GetKeyDown("u"))
                {
                    spellInput = SpellInput.U;
                    StartCastSpell();
                }
                else if (Input.GetKeyDown("i"))
                {
                    spellInput = SpellInput.I;
                    StartCastSpell();
                }

                //Jump

            }
        }
    }

    private void StartCastSpell()
    {
        animator.SetTrigger("CastSpell");
    }

    public void CastSpell()
    {
        if (spellInput == SpellInput.U)
        {
            switch (spells.currentSpell[0])
            {
                case (int)PlayerSpellsManager.MagicSpells.FIRESTRIKE:
                    MagicIconU.Instance.cooldown += 3f;
                    LaunchFireball();
                    break;

                case (int)PlayerSpellsManager.MagicSpells.THUNDERSTRIKE:
                    ThunderStrike();
                    break;

                case (int)PlayerSpellsManager.MagicSpells.BOOSTSELF:
                    BoostSelf();
                    break;
            }
        }
        else if (spellInput == SpellInput.I)
        {
            switch (spells.currentSpell[1])
            {
                case (int)PlayerSpellsManager.MagicSpells.FIRESTRIKE:
                    LaunchFireball();
                    break;

                case (int)PlayerSpellsManager.MagicSpells.THUNDERSTRIKE:
                    ThunderStrike();
                    break;

                case (int)PlayerSpellsManager.MagicSpells.BOOSTSELF:
                    BoostSelf();
                    break;
            }
        }

    }


    private void BoostDownJumpAttack()
    {
        body2d.AddForce(Vector2.down * 800f);
    }

    public void FinalJumpAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(transform.position + new Vector3(-0.02f, -0.3f), new Vector2(1.55f, 0.5f), 0);
        MeleeDamage(enemiesToDamage);
        KnockBackEnemy(enemiesToDamage, 250f);
    }
    //body2d.velocity = new Vector2(horizontalInput * speed * speedMod, body2d.velocity.y);
   
    private void UltPrep()
    {
        animator.SetTrigger("ultPrep");
    }

    private void Slide()
    {
        animator.SetTrigger("Slide");
        //body2d.AddForce(lookdirectionVector2 * slidingForce);
        
        //so that it scales with current speed, prevents player from spamming slide
        body2d.AddForce(lookdirectionVector2 * slidingForce * Mathf.Abs(body2d.velocity.x));
    }
    public void ChangeColliderForSlide()
    {

        //used in animator behavior script
        cap2d.offset = new Vector2(0f, -0.38f);
        cap2d.size = new Vector2(0.3f, 0.3f);
        
        


        //Vector2[] colliderPoints;
        //colliderPoints = edge2d.points;

        //colliderPoints[0] = new Vector2(-0.2f, -0.1f);
        //colliderPoints[1] = new Vector2(-0.2f, -0.55f);
        //colliderPoints[2] = new Vector2(0.15f, -0.55f);
        //colliderPoints[3] = new Vector2(0.15f, -0.1f);
        //colliderPoints[4] = new Vector2(-0.2f, -0.1f);

        //edge2d.points = colliderPoints;
    }
    public void RevertCollider()
    {
        //edge2d.points = originalColliderPoints;

        cap2d.size = originalColliderSize;
        cap2d.offset = originalColliderOffset;
    }

    public void Punch()
    {
        animator.SetTrigger("FlyingPunch");
        //force tbc
        StartCoroutine(Punching());
        
    }
    public IEnumerator Punching()
    {
        yield return new WaitForSeconds(0.35f);

        //body2d.AddForce(lookdirectionVector2 * 3000);
        body2d.AddForce(lookdirectionVector2*100);

        yield return null;
    }

    //Magic attack
    public void LaunchFireball()
    {       
        Fireball fireball = Instantiate(fireballPrefab, body2d.position + new Vector2(GetLookDirection() * 0.5f, -0.13f), Quaternion.identity).GetComponent<Fireball>();

        fireball.transform.localScale = transform.localScale;
        fireball.damage = 1;    //temp

        fireball.GetComponent<Rigidbody2D>().AddForce(new Vector2(lookdirectionVector2.x * 500f,0));
    }
    public void ThunderStrike()
    {
        RaycastHit2D hit = Physics2D.Raycast(body2d.position, lookdirectionVector2, 200f, layerEnemies);
        if (hit.collider != null)
        {
            ThunderStrike thunderStrike = Instantiate(thunderPrefab, new Vector2(hit.collider.GetComponent<Rigidbody2D>().position.x - 0.35f, -0.1f),Quaternion.identity).GetComponent<ThunderStrike>();
            thunderStrike.damage = 5;   //temp
            Debug.Log("ThunderStrike Appears");
        }
    }

    public void BoostSelf()
    {
        boosted = true;
        boostEffects.gameObject.SetActive(true);
        Instantiate(boostAnim, body2d.position, Quaternion.identity);
        CancelInvoke("BoostDone");
        Invoke("BoostDone", 5f);
    }
    void BoostDone()
    {
        boostEffects.gameObject.SetActive(false);
        boosted = false;
    }
    //Ranged attack
    public void LaunchArrow()
    {
        
        Arrow arrow = Instantiate(arrowPrefab, body2d.position + new Vector2 (GetLookDirection() * 0.5f, -0.1f), Quaternion.identity).GetComponent<Arrow>();

        arrow.transform.localScale = transform.localScale;
        arrow.damage = rangedDamage;

        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(lookdirectionVector2.x * arrowForce, arrowForce/15));

        PlayRandomRangedAttackAudio();

    }

    //taking damage
    public void TakeDamage(int damage)
    {
        health.Damage(damage);
        UIHealth.Instance.SetValue(health.GetHealthPercentage());
        if (!isDead)
        {
            animator.SetTrigger("Hurt");
            PlayAudio("PlayerHurtAudio");
        }
    }

    //attack moves
    //public void StartMeleeAttack()
    //{
    //    lastClickedTime = Time.time;

    //        if (BasicHitNumber == 1)
    //            {
    //                BasicHitNumber = 2;
    //            }
    //        else if (BasicHitNumber == 2)
    //        {
    //            BasicHitNumber = 1;
    //        }
        
    //    animator.SetTrigger("BasicHit" + BasicHitNumber.ToString());

    //}

    public void StartRangedAttack()
    {
            animator.SetTrigger("Shoot");
    }

    public void Pickup(GameObject item)
    {
        if (item.CompareTag("Boots"))
        {
            speedLevel.LevelUp();
            Destroy(item);
        }
        else if (item.CompareTag("Scroll"))
        {
            magicLevel.LevelUp();
            Destroy(item);
        }
        else if (item.CompareTag("Sword"))
        {
            attackLevel.LevelUp();
            Destroy(item);
        }
        else if (item.CompareTag("Bow"))
        {
            rangedLevel.LevelUp();
            Destroy(item);
        }
        else
        {
            return;
        }
        PlayAudio("PlayerLevelUpAudio");
    }
    
    public void Jump()
    {
        if (grounded && !nextToWallInAir)
        {
            CreateDustEffectsJump();           

            animator.SetTrigger("Jump");
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            ResetExtraJump();
        } 
else if (nextToWallInAir)
        {
            WallJump();
            //body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            //body2d.AddForce(-lookdirectionVector2 * slidingForce);
        }
        else if (!grounded && extraJump > 0 && body2d.velocity.y < 2)
        {
            //vertical velocity less than 2 so that spammign it wont look weird, arbituary
            animator.SetTrigger("Jump");
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            extraJump--;
        }
    }

    private void CreateDustEffectsJump()
    {
        Instantiate(DustEffectsJump, groundCheck.position + new Vector3(0f, 0.05f), Quaternion.Euler(-90f,0f,0f));
    }
    private void CreateDustEffectsWalk()
    {
        Instantiate(DustEffectsWalk, groundCheck.position + new Vector3(0f, 0.05f), Quaternion.Euler(-90f, 0f, 0f));
    }
    private void DustEffectsWhileWalking()
    {
        if (timeBetweenTrails <= 0)
        {
            CreateDustEffectsWalk();
            timeBetweenTrails = startTimeBetweenTrails;
        }
        else
        {
            timeBetweenTrails -= Time.deltaTime;
        }

    }

    void WallJump()
    {
        wallJumping = true;
        Invoke("SetWallJumpingToFalse", 0.25f);  //tbc viarble        
        animator.SetTrigger("WallJump");
        body2d.velocity = new Vector2(-lookdirectionVector2.x * wallJumpForce, jumpForce/1.4f);
        SetLookDirection(-lookdirectionVector2.x);

        DisableExtraJump();
    }
    public void ChangeSpeed(float speedChange)
    {
        speed += speedChange;
    }

    //item pickup
    void OnTriggerEnter2D(Collider2D other)
    {
        Pickup(other.gameObject);
    }

    public void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
    //    public void MeleeCombo(int i)
    //{
    //    if (noOfClicksCombat >= i + 1)
    //    {
    //        animator.SetBool("Attack" + i.ToString(), false);
    //        //so that the 3rd attack wont come even if the player click 3 times during the first attack, which would feel unnatural
    //        noOfClicksCombat = i + 1;
    //    }
    //    else
    //    {
    //        for (int j = 1 ; j < i ; j++) 
    //        {
    //            animator.SetBool("Attack" + j.ToString(), false);
    //            noOfClicksCombat = 0;
    //        }
            
    //        animator.SetBool("InCombat", true);
    //        CancelInvoke("OutOfCombat");
    //        Invoke("OutOfCombat", inCombatTimer);
    //    }
    //}
    public void MeleeCombo(int i)
    {
        if (noOfClicksCombat >= i + 1)
        {
            animator.SetBool("Attack" + (i + 1).ToString(), true);
            //so that the 3rd attack wont come even if the player click 3 times during the first attack, which would feel unnatural
            noOfClicksCombat = i + 1;
        }
        else
        {
            for (int j = 1; j < i; j++)
            {
                animator.SetBool("Attack" + j.ToString(), false);
                noOfClicksCombat = 0;
            }
            animator.SetBool("InCombat", true);
            CancelInvoke("OutOfCombat");
            Invoke("OutOfCombat", inCombatTimer);
        }
    }
    public void MeleeCombo1()
    {
        if (noOfClicksCombat >= 2)
        {
            animator.SetBool("Attack2", true);           
            //so that the 3rd attack wont come even if the player click 3 times during the first attack, which would feel unnatural
            noOfClicksCombat = 2;
        }
        else
        {
            animator.SetBool("Attack1", false);
            noOfClicksCombat = 0;
            animator.SetBool("InCombat", true);
            CancelInvoke("OutOfCombat");
            Invoke("OutOfCombat", inCombatTimer);

        }
    }
    public void MeleeCombo2()
    {
        if (noOfClicksCombat >= 3)
        {
            animator.SetBool("Attack3", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            noOfClicksCombat = 0;
            animator.SetBool("InCombat", true);
            CancelInvoke("OutOfCombat");
            Invoke("OutOfCombat", inCombatTimer);

        }
    }
    public void MeleeCombo3()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);

        noOfClicksCombat = 0;
        animator.SetBool("InCombat", true);
        CancelInvoke("OutOfCombat");
        Invoke("OutOfCombat", inCombatTimer);
    }
    public void OutOfCombat()
    {
        animator.SetBool("InCombat",false);
    }


    public void MeleeHit1()
    {
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(meleeHitPos.position, meleeHitRange, layerEnemies);
        MeleeDamage(enemiesToDamage);
        PlayAudio("BasicHit1Audio");
    }


    public void MeleeHit2()
    {
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(meleeHitPos.position, meleeHitRange, layerEnemies);
        MeleeDamage(enemiesToDamage);
        PlayAudio("BasicHit2Audio");
    }

    public void MeleeHit3()
    {
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(meleeHitPos.position + (Vector3)finalMeleeHitOffset, meleeHitRange, layerEnemies);
        MeleeDamage(enemiesToDamage);
        KnockBackEnemy(enemiesToDamage, 200f);
        PlayAudio("BasicHit3Audio");
    }

    public void JumpHit()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(meleeHitPos.position, meleeHitRange, layerEnemies);
        MeleeDamage(enemiesToDamage);
        PlayAudio("BasicHit1Audio");
    }

    //As Melee attacks should hit all enemies in the area and uses overlap functions, Collider2D[] is used for detecting the enemies 
    private void MeleeDamage(Collider2D[] enemiesToDamage)
    {
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if ((enemiesToDamage[i].GetComponent<BasicEnemy>() != null) && (enemiesToDamage[i].GetComponent<BasicEnemy>().health.GetHealth() > 0))
            {
                enemiesToDamage[i].GetComponent<BasicEnemy>().TakeDamage(meleeDamage);

                GameObject slash = Instantiate(slashPrefab, enemiesToDamage[i].GetComponent<Rigidbody2D>().position, Quaternion.identity);
                slash.transform.localScale *= enemiesToDamage[i].GetComponent<BasicEnemy>().enemySize;

                PlayRandomMeleeOnHitAudio();
            }
        }
    }
    private void KnockBackEnemy(Collider2D[] enemiesToDamage, float force)
    {
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if ((enemiesToDamage[i].GetComponent<BasicEnemy>() != null) && (enemiesToDamage[i].GetComponent<BasicEnemy>().health.GetHealth() > 0)) //katana
            {
                if (transform.position.x > enemiesToDamage[i].transform.position.x)
                {
                    enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
                }
                else if (transform.position.x <= enemiesToDamage[i].transform.position.x)
                {
                    enemiesToDamage[i].GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
                }
                camAnim.SetTrigger("Shake");
            }

        }
    }
    
    private void DisableExtraJump()
    {
        extraJump = 0;
        Debug.Log("ExtraJump Disabled");
    }
    private void ResetExtraJump()
    {
        extraJump = extraJumpValue;
        Debug.Log("ExtraJump Reset");
    }
    public void DamageEnemyInFront(float range, Transform pos)
    {
        //tbc
        //camAnim.SetTrigger("Shake");
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(pos.position, range, layerEnemies);
        MeleeDamage(enemiesHit);
    }

    public void SetLookDirection(float horizontalInput)
    {
            if (horizontalInput > 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                //transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
    }
    public float HorizontalVelocity(float horizontalInput)
    {
        if (boosted)
        {
            return horizontalInput* speed* speedMod* boostedSpeedMod;
        }
        return horizontalInput * speed * speedMod;
    }
    public void Die()
    {
        PlayAudio("PlayerDieAudio");
        isDead = true;
        animator.SetTrigger("Die");
        
    }
    void PlayAudio(string audioName)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(audioName);
        }
    }
    void StopMovement()
    {
        body2d.velocity = new Vector2(0, 0);
    }

    public float GetLookDirection()
    {
        return transform.localScale.x;
    }
    public void ResetMeleeAttack()
    {
        //For when an attack is interupted
        attacking = false;
        noOfClicksCombat = 0;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
    }

    private void PlayRandomAudioFromList(string[] listOfAudios)
    {
        string audioClip = listOfAudios[UnityEngine.Random.Range(0, listOfAudios.Length)];      //(0,10) return a range of (0,9)
        PlayAudio(audioClip);
        Debug.Log("Audio Played: " + audioClip);
    }

    private void PlayRandomRangedAttackAudio()
    {
        string[] listOfAudios = { "ShootArrowAudio1", "ShootArrowAudio2" };
        PlayRandomAudioFromList(listOfAudios);
    }

    private void PlayRandomMeleeOnHitAudio()
    {
        string[] listOfAudios = { "MeleeOnHitAudio1", "MeleeOnHitAudio2", "MeleeOnHitAudio3" };
        PlayRandomAudioFromList(listOfAudios);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector2(0.3f,0.05f));
        //Gizmos.DrawCube(groundCheck.position, new Vector2(0.12f, 0.12f));
        //Gizmos.DrawWireSphere(meleeHit1Pos.position, meleeHit1Range);


        //overlap red
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(meleeHit2Pos.position, meleeHit2Range);

        Gizmos.color = Color.green;
        
        //Gizmos.DrawWireSphere(meleeHit3Pos.position, meleeHit3Range);

    }
}
