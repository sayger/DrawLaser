using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    #region Singleton class: Magnet
    public static PlayerMagnet Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] private float magnetForce;
    List<Rigidbody> affectedRigidbodies = new List<Rigidbody>();
    List<Rigidbody> allPossibleRigidBody = new List<Rigidbody>();
    [SerializeField] private float detectionDistance;
    [SerializeField] private float centerMoveForwardBy=1f;
    [SerializeField] private float centerInPileRadius=1f;
    [SerializeField] private float centerPileRadius=1f;
    
    
    private Transform magnetismCenter;
    
    void Start()
    {
        magnetismCenter = transform;
        affectedRigidbodies.Clear();
        allPossibleRigidBody.Clear();
        allPossibleRigidBody = takeRigidBodys(findLayeredObjects("cubes"));
        Debug.Log("all posible object count is  : "+allPossibleRigidBody.Count);
    }

    private void FixedUpdate()
    {
//        Debug.Log("affected object count is  : "+affectedRigidbodies.Count);
        
        removeCollected(affectedRigidbodies,5);

        pull(magnetismCenter, affectedRigidbodies, magnetForce);

        

        addCloseOnes();

    }

    private void addCloseOnes()
    {
        foreach (var e in allPossibleRigidBody)
        {
            Vector3 cubePosition = e.gameObject.transform.position;
            float dis= Vector3.Distance (transform.position, cubePosition);

            if (dis<detectionDistance&& !affectedRigidbodies.Contains(e))
            {
                affectedRigidbodies.Add(e);
            }

        }
    }
    private void removeCollected(List<Rigidbody> effected, int layerNo)
    {
        List<Rigidbody> subject = new List<Rigidbody>(effected);

        foreach (var e in subject)
        {
            if (e.gameObject.layer==layerNo)
            {
                Debug.Log(" NUMBER OF EFFECTED : "+affectedRigidbodies.Count);
                affectedRigidbodies.Remove(e);
                allPossibleRigidBody.Remove(e);
                Debug.Log(" NUMBER OF EFFECTED must be mınus one : "+affectedRigidbodies.Count);
            }
        }
        
    }
    
    private void pull(Transform center, List<Rigidbody> effected, float power)
    {
        
        foreach (Rigidbody rb in effected)
        {  
            var forwardDifferance = center.forward;
            var rightDifferance = center.right;
            
            var cubePosition = rb.position;

            var centerPos = center.position;
            
            var desiredCenter = centerPos + (forwardDifferance * centerMoveForwardBy);

            
            var centerInRightLimit = desiredCenter + (rightDifferance * centerInPileRadius);
            var centerInLeftLimit = desiredCenter + (rightDifferance * (-1 * centerInPileRadius));
            var centerInForwardLimit = desiredCenter + (forwardDifferance * centerInPileRadius);
            var centerInBackwardLimit = desiredCenter + (forwardDifferance * (-1 * centerInPileRadius));
            
            var centerRightLimit = desiredCenter + (rightDifferance * centerPileRadius);
            var centerLeftLimit = desiredCenter + (rightDifferance * (-1 * centerPileRadius));
            var centerForwardLimit = desiredCenter + (forwardDifferance * centerPileRadius);
            var centerBackwardLimit = desiredCenter + (forwardDifferance * (-1 * centerPileRadius));
            
            Vector3 target = new Vector3(desiredCenter.x,cubePosition.y,desiredCenter.z);

            bool outInnerCircle = !checkIfItsIn(  cubePosition, centerInRightLimit, centerInLeftLimit,centerInForwardLimit,centerInBackwardLimit );
            bool inOuterCircle=checkIfItsIn(  cubePosition, centerRightLimit, centerLeftLimit,centerForwardLimit,centerBackwardLimit );

            if (outInnerCircle&&inOuterCircle)
            {
                rb.AddForce((target-cubePosition) * (power * Time.fixedDeltaTime));
            }
            
            
        }
    }

    private bool checkIfItsIn(Vector3 subject, Vector3 border1, Vector3 border2, Vector3 border3, Vector3 border4)
    {
         

        List<Vector3> borders=new List<Vector3>();
        borders.Add(border1);
        borders.Add(border2);
        borders.Add(border3);
        borders.Add(border4);

        float biggestX = findMost('x', borders);
        float biggestZ = findMost('z', borders);
        float smallestX = findLeast('x', borders);
        float smallestZ = findLeast('z', borders);

        if (subject.x>biggestX||subject.z>biggestZ||subject.x<smallestX||subject.z<smallestZ)
        {
            return false;
        }

        return true;

    }
    private float findMost(char A,List<Vector3> input)
    {
        float result=0;

        foreach (var e in input)
        {
            switch (A)
            {
                case 'x':case 'X':
                {
                    if (e.x > result)
                        result = e.x;
                    break;
                }
                case 'y':case 'Y':
                {
                    if (e.y > result)
                        result = e.y;
                    break;
                }
                case 'z':case 'Z':
                {
                    if (e.z > result)
                        result = e.z;
                    break;
                }
            }
        }

        return result;
    }
    
    private float findLeast(char A,List<Vector3> input)
    {
        float result=0;

        foreach (var e in input)
        {
            switch (A)
            {
                case 'x':case 'X':
                {
                    if (e.x < result)
                        result = e.x;
                    break;
                }
                case 'y':case 'Y':
                {
                    if (e.y < result)
                        result = e.y;
                    break;
                }
                case 'z':case 'Z':
                {
                    if (e.z < result)
                        result = e.z;
                    break;
                }
            }
        }

        return result;
    }
    

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible") && other.gameObject.layer != 5)
        {
            AddMagnetField(other.attachedRigidbody);
        }
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            RemoveFromMagnetField(other.attachedRigidbody);
        }
    }*/
    
    public void AddMagnetField(Rigidbody rb)
    {
        affectedRigidbodies.Add(rb);
    }

    public void RemoveFromMagnetField(Rigidbody rb)
    {
        affectedRigidbodies.Remove(rb);
    }

    private List<Rigidbody> takeRigidBodys(List<GameObject> collectableObjects)
    {
        List <Rigidbody> rigidBodys = new List <Rigidbody>() ;
        
        foreach (var e in collectableObjects)
        {
          rigidBodys.Add(e.GetComponent<Rigidbody>());
        }

        return rigidBodys;
    }
    
    public List< GameObject>   findLayeredObjects(string layer) // finding object first name if not tag if not layer
    {
        
        GameObject[] allAbjects = findAllObjects();
        List <GameObject> matchedObjects = new List <GameObject>();

        foreach (var e in allAbjects)
        {
            if(LayerMask.LayerToName(e.layer)==layer) // to change layer to name
                matchedObjects.Add(e);
        }
        
        
        return  matchedObjects ;
    }
    public GameObject []  findAllObjects() // finding object first name if not tag if not layer
    {
        return GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        
    }
}
