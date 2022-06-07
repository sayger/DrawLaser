using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class doorGuard : MonoBehaviour
{
    [SerializeField] private bool notASectorDoor=false;
    
    [SerializeField] private int belongToSector = 1;
    [SerializeField] private bool itsStartsOpen=false;
    [SerializeField] private float openingTime =3f;
    [SerializeField] private float closingTime =3f;
    [SerializeField] private bool visibleWhileOpened=true;
    [SerializeField] private bool visibleWhileClosed=true;
    [SerializeField] private bool visibleWhileMovement=true;
    
    [SerializeField] private Vector3 offsetsForTheOtherPosition=new Vector3();
    [SerializeField] private bool useElasticMove=false;
    [SerializeField] private bool allowCommendStacking=false;
    
    
    [SerializeField] private bool itsOpening=false;
    [SerializeField] private bool itsClosing=false;

    [SerializeField] private bool itsOpen=false;
    [SerializeField] private bool itsClosed=false;
    
    private Vector3 theFirstPosition=new Vector3();
    
    [SerializeField] private  Vector3[] openAndClosed = new Vector3[2];

    private Renderer _renderer;
    
    
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        if (!notASectorDoor)
        {
            addToSector();
        }
        
        makeAdjustments();
        
    }
    

    private void makeAdjustments()
    {
        
         
        theFirstPosition = transform.position;
        Vector3 OtherPosition = new Vector3(theFirstPosition.x + offsetsForTheOtherPosition.x,
            theFirstPosition.y + offsetsForTheOtherPosition.y, theFirstPosition.z + offsetsForTheOtherPosition.z);
        
        if (itsStartsOpen)
        {
            itsOpen=true;
            
            openAndClosed[0] = theFirstPosition;
            openAndClosed[1] = OtherPosition;
            
            _renderer.enabled = visibleWhileOpened;
        }
        else
        {
            itsClosed = true;
            
            openAndClosed[1] = theFirstPosition;
            openAndClosed[0] = OtherPosition;
            
            _renderer.enabled = visibleWhileClosed;
        }


    }

    private void addToSector()
    {
      //  GameManager.Instance.addMeToTheSectorDoor(belongToSector,this.gameObject);
        gateManager.Instance.addMeToTheSectorDoor(belongToSector,this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateIt()
    {

        if (itsOpen)
        {
            closeIt();
        }
        else
        {
            openIt();
        }


    }

    public void openIt()
    {
        
        if (itsOpening)
        {
            if(allowCommendStacking)
                Invoke(nameof(openIt),(openingTime+0.1f));
            return;
        }
        if (itsClosing)
        {
            if(allowCommendStacking)
                Invoke(nameof(openIt),(closingTime+0.1f));
            return;
        }

        if (useElasticMove)
        {
             
            itsOpening = true;
            if (!_renderer.enabled&&visibleWhileMovement)
            {
                _renderer.enabled = visibleWhileOpened;
            }
            transform.DOMove(openAndClosed[0], openingTime).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                itsOpen = true;
                _renderer.enabled = visibleWhileOpened;
                itsClosed = false;
                itsOpening = false;
                
            });
        
        }
        else
        {
            itsOpening = true;
            if (!_renderer.enabled&&visibleWhileMovement)
            {
                _renderer.enabled = visibleWhileOpened;
            }
            transform.DOMove(openAndClosed[0],openingTime).OnComplete(() =>
            {
                itsOpen = true;
                _renderer.enabled = visibleWhileOpened;
                itsClosed = false;
                itsOpening = false;
            });
        }


    }

    public void closeIt()
    {
        if (itsOpening)
        {
            if(allowCommendStacking)
                Invoke(nameof(closeIt),(openingTime+0.1f));
            return;
        }
        if (itsClosing)
        {
            if(allowCommendStacking)
                Invoke(nameof(closeIt),(closingTime+0.1f));
            return;
        }

        if (useElasticMove)
        {
            itsClosing = true;
            if (!_renderer.enabled&&visibleWhileMovement)
            {
                _renderer.enabled = visibleWhileClosed;
            }
            transform.DOMove(openAndClosed[1], closingTime).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                itsClosed = true;
                _renderer.enabled = visibleWhileClosed;
                itsOpen = false;
                itsClosing = false;
            });
        
        }
        else
        {
            itsClosing = true;
            if (!_renderer.enabled&&visibleWhileMovement)
            {
                _renderer.enabled = visibleWhileClosed;
            }
            transform.DOMove(openAndClosed[1],closingTime).OnComplete(() =>
            {
                itsClosed = true;
                _renderer.enabled = visibleWhileClosed;
                itsOpen = false;
                itsClosing = false;
            });
        }



    }
}
