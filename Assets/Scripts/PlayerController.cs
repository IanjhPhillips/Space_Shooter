using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	private static PlayerController playerController;

	private GameObject ship;
	private Rigidbody2D rb;

	private Vector2 velocity;
	private float maxSpeed;
	public float acceleration;
	public float drag;
	private float turnSpeed;
	private float rotation;
	private Vector2 moveInput;
	private Vector3 mousePos;
	private Vector2 lookDirection;

	private int health;

	public float proximityToMouseMovementThreshold;

	void Awake()
	{

		if (playerController!=null && playerController!=this)
		{
			Destroy(this.gameObject);
		}

		playerController = this;

		ship = this.gameObject;
		rb = ship.GetComponent<Rigidbody2D>();
	}

    // Start is called before the first frame update
    void Start()
    {

    	proximityToMouseMovementThreshold = 0.4f;

        maxSpeed = 5f;
        drag = 0.5f;
        acceleration = 1f;

        health = 10;

        velocity = new Vector2(0f, 0f);
        moveInput = new Vector2 (0f, 0f);

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2 (mousePos.x-transform.position.x, mousePos.y-transform.position.y);
        transform.up = lookDirection;

    }

    //Fixed update for physics movement
    void FixedUpdate()
    {

    	moveInput = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    	//so you cant move if youre right on the mouse (to prevent spinning bug), you must be a certain distance from the mouse while inputing to acclerate
        if ((moveInput.x != 0 || moveInput.y != 0) && lookDirection.magnitude >= proximityToMouseMovementThreshold) 
    		Accelerate();
    	else if (velocity.magnitude != 0)
    		Drag(); 

    	//Vector2 moveTarget = new Vector2(transform.position.x, transform.position.y) + velocity*Time.fixedDeltaTime;
    	//rb.MovePosition(moveTarget);
    	rb.velocity = velocity;

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = new Vector2 (mousePos.x-transform.position.x, mousePos.y-transform.position.y);
        transform.up = lookDirection;

    }

    //mutators
    public PlayerController getPlayerController() {return playerController;}

    //class functions

    //Caps velocity and only allows acceleration if below max or if input is not in the same direction
    private void Accelerate() 
    {
    	
    	Vector2 direction = transform.right*moveInput.x + transform.up*moveInput.y;
    	direction.Normalize();

    	if (velocity.magnitude < maxSpeed || direction != velocity.normalized)
    	{
    		velocity += direction*acceleration;
    	}

    	if (velocity.magnitude >= maxSpeed)
    	{
    		velocity = velocity.normalized*maxSpeed;
    	}
    }

    //Constant drag opposite the velocity, will not go below zero
    private void Drag()
    {
    	print ("dragging");
    	if (drag >= velocity.magnitude)
    		velocity = new Vector2 (0f, 0f);
    	else
    		velocity -= velocity.normalized*drag;
    }

}
