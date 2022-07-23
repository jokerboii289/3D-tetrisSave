using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
   
    public bool instantiateNPC;
    RaycastHit hit;
    private void Start()
    {
        instantiateNPC = true;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.up, out hit, 2f))
        {
            if (!hit.collider.CompareTag("cube"))//when cube is not up
            {
                instantiateNPC = true;
            }
            else if (!hit.transform.root.CompareTag(transform.root.tag))//when same same color cube is not up
            {
                instantiateNPC = false;
            }
            else if (hit.transform.root.CompareTag(transform.root.tag))// when same color of same cube is on top
            {
                RaycastVertical(hit.transform);
            }
        }
        else
        {
            instantiateNPC = true; //raycast doesnt hit anything
        }
    }

    void RaycastVertical(Transform cubeTransform)// raycast till no cube is above the box or different color cube is found out;
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cubeTransform.position, cubeTransform.up, out hitInfo, 2f))
        {
            if (hitInfo.transform.root.CompareTag(transform.root.tag))//denotes the color of cube by its root
            {
                RaycastVertical(hitInfo.transform);
            }
            else if (hitInfo.transform.CompareTag("cube") && !hitInfo.transform.root.CompareTag(transform.root.tag))// hits different cube
            {
                instantiateNPC = false;
            }
            if (!hitInfo.collider.CompareTag("cube"))//when cube is not up
            {
                instantiateNPC = true;
            }
        }
        else
        {
            instantiateNPC = true;  //if rayacast doesnt hit anything
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("SharpObject"))
        {
            PauseMenu.instance.LooseCondition();
        }
    }
    
    
}
