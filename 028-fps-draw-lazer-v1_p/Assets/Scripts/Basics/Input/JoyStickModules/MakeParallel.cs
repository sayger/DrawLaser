using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeParallel : MonoBehaviour
{
	[SerializeField] private Camera cam;
	
    [SerializeField] float ix;
    [SerializeField] float iy;
    [SerializeField] private bool discardJoystick;
    
    [SerializeField] private Joystick joyStickInput;
    
    [Range(-1,1)]
    [SerializeField] private float FakeInputX=0;
    [Range(-1,1)]
    [SerializeField] private float FakeInputY=0;
    //---------------------------------------------------
    
    public float Speed = 1f; 
    public float RotatingSpeed = 1f; 
    public float AdditionalGravity = 0.5f; 
    public float LandingAccelerationRatio = 0.5f; 
	

    public bool reverse = false; 

    public Rigidbody rb; 
     
    
    public float height; 

    public bool aerial;

	public Quaternion PhysicsRotation; 
    public Quaternion VelocityRotation; 
    public Quaternion InputRotation; 
    public Quaternion ComputedRotation;
    
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void Update()
    {
        variableUpdate();
    }
    //----------------------------------------
    private void adjustments()
    {
        Initialization(); 
    }
    private void variableUpdate()
    {
        ix = joyStickInput.Horizontal+FakeInputX; 
        iy = joyStickInput.Vertical+FakeInputY;
        if (discardJoystick)
        {
	        ix = FakeInputX; 
	        iy = FakeInputY;
        }

        ix = ix > 1 ? 1 : ix < -1 ? -1 : ix;
        iy = iy > 1 ? 1 : iy < -1 ? -1 : iy;
        
        updateCopiedVariables();


        
    }
    private void updateCopiedVariables()
    {
  
        CheckPhysics(); 

        Vector2 direction =  GetDirection();
        SkaterMove(direction); 


    }
    public Vector2 GetDirection()
    {
        return new Vector2(ix,iy); 
    }
    //---------------------------------------
    
    
	void CheckPhysics()
	{
		Ray ray = new Ray(transform.position, -transform.up); 
		RaycastHit hit; 

		if(Physics.Raycast(ray, out hit, 1.05f*height))
		{
			if(aerial)
			{
				VelocityOnLanding(); 
			}
			aerial = false; 
		}
		else
		{
			aerial = true; 
			rb.velocity += Vector3.down*AdditionalGravity; 
		}

	}

	void VelocityOnLanding()
	{
		float magn_vel = rb.velocity.magnitude;
		Vector3 new_vel = rb.velocity; 
		new_vel.y = 0; 
		new_vel = new_vel.normalized*magn_vel;

		rb.velocity +=  LandingAccelerationRatio*new_vel; 

	}


	void SkaterMove(Vector2 inputs)
	{
		
		PhysicsRotation = aerial ? Quaternion.identity : GetPhysicsRotation(); // Rotation according to ground normal 
		VelocityRotation = GetVelocityRot(); 
		InputRotation = Quaternion.identity; 
		ComputedRotation = Quaternion.identity;


		if(inputs.magnitude > 0.1f)
		{
			Vector3 adapted_direction = CamToPlayer(inputs); 
			Vector3 planar_direction = transform.forward; 
			planar_direction.y = 0; 
			InputRotation = Quaternion.FromToRotation(planar_direction, adapted_direction); 

			if(!aerial)
			{
				Vector3 Direction = InputRotation*transform.forward*Speed; 
				rb.AddForce(Direction); 
			}
		} 

		ComputedRotation = PhysicsRotation*VelocityRotation*transform.rotation; 
		transform.rotation = Quaternion.Lerp(transform.rotation, ComputedRotation, RotatingSpeed*Time.deltaTime); 
	}

	
	Quaternion GetVelocityRot()
	{
		Vector3 vel = rb.velocity;
		if(vel.magnitude > 0.2f)
		{
			vel.y = 0; 
			Vector3 dir = transform.forward; 
			dir.y = 0; 
			Quaternion vel_rot = Quaternion.FromToRotation(dir.normalized, vel.normalized); 
			return vel_rot; 
		} 
		else
			return Quaternion.identity; 
	}

	Quaternion GetPhysicsRotation()
	{
		Vector3 target_vec = Vector3.up; 
		Ray ray = new Ray(transform.position, Vector3.down); 
		RaycastHit hit; 

		if(Physics.Raycast(ray, out hit, 1.05f*height))
		{
			target_vec = hit.normal; 
		}

		return Quaternion.FromToRotation(transform.up, target_vec); 
	}

	Vector3 CamToPlayer(Vector2 d)
	{
		Vector3 cam_to_player = transform.position - cam.transform.position; 
		cam_to_player.y = 0; 

		Vector3 cam_to_player_right = Quaternion.AngleAxis(90, Vector3.up)*cam_to_player; 

		Vector3 direction = cam_to_player*d.y + cam_to_player_right*d.x; 
		return direction.normalized; 
	}

	void Initialization()
	{
		// cam = Camera.main; 
		// TargetRotation = transform.rotation; 
		// VelocityRotation = Quaternion.identity; 
		rb = GetComponent<Rigidbody>(); 
		 
		 
		height = GetComponent<Collider>().bounds.size.y/2f; 
	}
    
}
