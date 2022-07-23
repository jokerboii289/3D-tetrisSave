using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterNPC : MonoBehaviour
{
    [HideInInspector]
    public static CharacterNPC instance;
    Animator animator;
  
    [SerializeField] float runSpeed;

    [Header("Groundcheck")]
    bool grounded;
    [SerializeField] Transform raycastPoint;

   
    private Transform restPoint;

    // Start is called before the first frame update
    void Start()
    {
        restPoint = GameObject.FindGameObjectWithTag("restpoint").transform;
        BoxTwo.StartJump += Jump;
        grounded = false;
        instance = this;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(raycastPoint.position,-transform.up,out hit,2f))
        {
            if (hit.transform.CompareTag("Ground") && hit.distance < 0.1f)
            {
                StartCoroutine(DelayRun());                          
            }
            else if(hit.transform.CompareTag("cube") && hit.distance < 0.1f)
            {
                StartCoroutine(DelayIdleState());
            }        
        }


        if (grounded && (new Vector3(restPoint.position.x, transform.position.y, restPoint.position.z) - transform.position).magnitude>.1f)// run towards restPoint
        {           
            var direction = (new Vector3(restPoint.position.x,transform.position.y,restPoint.position.z) - transform.position).normalized;
            transform.forward = direction;
            transform.position += direction * runSpeed *Time.deltaTime;
        }
        else if(grounded && (new Vector3(restPoint.position.x, transform.position.y, restPoint.position.z) - transform.position).magnitude <= .1f)
        {
            animator.SetBool("run", false);
        }

        //raycast to see box above head
        RaycastHit hhit;
        if (Physics.Raycast(raycastPoint.position+new Vector3(0,1,0), transform.up, out hhit, 2f))
        {
            if(hhit.transform.CompareTag("cube"))
                gameObject.SetActive(false);
        }
    }

    IEnumerator DelayRun()
    {
        animator.SetBool("run", true);
        yield return new WaitForSeconds(1.5f);
        grounded = true;
    }
   
    public void Jump()//make it jump after the distance is greater tham .01f
    {
        animator.SetBool("Jump", true);
    } 

    IEnumerator DelayIdleState()
    {
        yield return new WaitForSeconds(.4f);
        GoIdleState();
    }

       
    public void GoIdleState()
    {
        animator.SetBool("Jump", false);        
    }   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("restpoint"))
        {
            print("reached");
            PauseMenu.instance.CountNoCharacterReached();
        }
    }

    private void OnDestroy()
    {
        BoxTwo.StartJump -= Jump;
    }
}
