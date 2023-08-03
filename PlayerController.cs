using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player's Movements")]
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    [Header("Grab Blocks")]
    public Camera cam;
    public Transform lampCenter;
    public float grabbRange = 10f;
    public LayerMask blockLayerMask;
    private World world;
    public Transform rayLookAt;
    public Transform placeBlock;

    [Header("Infos")]
    public WorldData worldData;
    public Vector2 playerPosInChunks;
    public Vector2 _playerPosInChunks;
    public bool inInventory;
    public Helper helper;

    private Animator animator;

    [Header("Toolbar")]
    public int selectedBlockIndex = 1;
    public Toolbar toolbar;

    [Header("HealthBar")]
    public float maxHealth;
    public float health;
    public Slider slider;
    private float oldVelocity;

    void Start () {

        oldVelocity = controller.m_Rigidbody2D.velocity.y;

        animator = this.gameObject.GetComponent<Animator>();
        world = GameObject.Find("World").GetComponent<World>();

        /* Animator : Animations IDs :

            0 : iddle
            1 : walk
            2 : fall
            3 : jump impulse
            4 : crouch
            5 : crouch walking

        */

        animator.SetInteger("AnimationID", 0);

        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;

    }


    void Update()
    {
        // for World.cs
        playerPosInChunks.x = Mathf.FloorToInt(transform.position.x / worldData.chunkSize);
        playerPosInChunks.y = Mathf.FloorToInt(transform.position.y / worldData.chunkSize);
        // end for


        // GET CONTROLS

        if (!inInventory && !helper.inUI)
        {

            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }


            // GRABBING BLOCKS

            if (Input.GetMouseButtonDown(0))
            {

                Vector2 mousePos = Input.mousePosition;
                rayLookAt.position = cam.ScreenToWorldPoint(mousePos);

                //Vector2 lm = new Vector2(rayLookAt.x - lampCenter.position.x, rayLookAt.y - lampCenter.position.y);

                // ces chieurs ils ont inversés Mathf.Atan2(x, y) en Mathf.Atan2(y, x)
                // Mathf.Atan2(lm.y, lm.x) * Mathf.Rad2Deg // obtenir l'angle pour aller de lampCenter à rayLookAt

                lampCenter.LookAt(rayLookAt.position);

                RaycastHit2D hit = Physics2D.Raycast(lampCenter.position, lampCenter.forward, grabbRange, blockLayerMask);
                if (hit)
                {
                    if (Vector2.Distance(lampCenter.position, hit.transform.position) <= grabbRange)
                    {
                        int block = world.DeleteBlock(hit.transform.gameObject);

                        toolbar.addItem(1, block);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && toolbar.itemSlots[toolbar.slotIndex].itemCount >= 1)
            {
                Vector2 placeBlockPos = GetPlaceBlockPositionWithMouse();
                if (placeBlockPos != Vector2.zero)
                {
                    placeBlock.position = placeBlockPos;
                    selectedBlockIndex = toolbar.itemSlots[toolbar.slotIndex].itemID;
                    world.AddBlock(placeBlock.position, world.GetChunkFromVector2(placeBlockPos), selectedBlockIndex);// inventory.selectedBlockItem.blockType);
                    toolbar.deleteItem(1, selectedBlockIndex);
                }
            }
        }
        else
        {
            horizontalMove = 0f;
        }

        //ANIMATION

        if (!controller.m_Grounded)
        {
            animator.SetInteger("AnimationID", 2);
        }
        else
        {
            if (jump)
            {
                animator.SetInteger("AnimationID", 3);
            }
            else
            {
                if (!controller.m_CrouchDisableCollider.enabled) {
                    if (horizontalMove > 0.1f || horizontalMove < -0.1f)
                    {
                        animator.SetInteger("AnimationID", 5);
                    }
                    else
                    {
                        animator.SetInteger("AnimationID", 4);
                    }
                }
                else
                {
                    if (horizontalMove > 0.1f || horizontalMove < -0.1f)
                    {
                        animator.SetInteger("AnimationID", 1);
                    }
                    else
                    {
                        animator.SetInteger("AnimationID", 0);
                    }
                }
            }
        }
    }

    public void TakeDamages(float amount)
    {
        health -= amount;
        slider.value = health;
        Debug.Log(amount);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void LateUpdate () {
        //for World.cs
        _playerPosInChunks.x = Mathf.FloorToInt(transform.position.x / worldData.chunkSize);
        _playerPosInChunks.y = Mathf.FloorToInt(transform.position.y / worldData.chunkSize);
        //end for

    }

    public void CheckFallDamages()
    {
        Debug.Log(oldVelocity);
    }

    Vector2 GetPlaceBlockPositionWithMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        rayLookAt.position = cam.ScreenToWorldPoint(mousePos);
        rayLookAt.position = new Vector2(Mathf.FloorToInt(rayLookAt.position.x + 0.5f), Mathf.FloorToInt(rayLookAt.position.y + 0.5f));

        if (Vector2.Distance(rayLookAt.position, lampCenter.position) < grabbRange) {

            bool fxp = world.FindBlockFromVector2(new Vector2(rayLookAt.position.x + 1f, rayLookAt.position.y));
            bool fxm = world.FindBlockFromVector2(new Vector2(rayLookAt.position.x - 1f, rayLookAt.position.y));
            bool fyp = world.FindBlockFromVector2(new Vector2(rayLookAt.position.x, rayLookAt.position.y + 1f));
            bool fym = world.FindBlockFromVector2(new Vector2(rayLookAt.position.x, rayLookAt.position.y - 1f));
            bool f = world.FindBlockFromVector2(new Vector2(rayLookAt.position.x, rayLookAt.position.y));

            if (!f && (fxp || fxm || fyp || fym))
            {
                return new Vector2(rayLookAt.position.x, rayLookAt.position.y);
            }
        }

        return Vector2.zero;
    }


}
