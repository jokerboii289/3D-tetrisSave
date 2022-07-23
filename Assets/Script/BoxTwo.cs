using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BoxTwo : MonoBehaviour
{
    public delegate void Jump();
    public static event Jump StartJump;

    private bool canDestroy;
    public bool check;

    public bool npcBox;


    private void Start()
    {
        npcBox = false;
        check = false;
    }

    private void OnMouseDown()
    {
        PauseMenu.instance.LooseCheck();
        CheckTheSideCubes();      
    }

  
    void Effect(Transform obj)
    {
        AudioManager.instance.Play("CubeBlast"); //cube sound
        if (transform.childCount <= 0)
        {
            if (obj.tag == "blue")
                Instantiate(GeneralVariable.instance.BlueParticle, transform.position, Quaternion.identity);
            else if (obj.tag == "red")
                Instantiate(GeneralVariable.instance.redParticle, transform.position, Quaternion.identity);
            else if (obj.tag == "green")
                Instantiate(GeneralVariable.instance.greenParticle, transform.position, Quaternion.identity);
            else if (obj.tag == "yellow")
                Instantiate(GeneralVariable.instance.yellowParticle, transform.position, Quaternion.identity);
        }
    }

    public void CheckTheSideCubes()
    {   
        List<GameObject> listOfObj = new List<GameObject>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, .4f))  //for right
        {            
            AddToList(hit.transform.gameObject);
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, .4f))  //for left
        {
            AddToList(hit.transform.gameObject);
        }
        if (Physics.Raycast(transform.position, transform.up, out hit, .4f))  //for top
        {
            AddToList(hit.transform.gameObject);
        }
        if (Physics.Raycast(transform.position, -transform.up, out hit, .4f))  //for down
        {
            AddToList(hit.transform.gameObject);
        }

        if(listOfObj.Count<1)//checking if Adjacentgameobject none or have some cubes
        {
            canDestroy = false;
        }
        else if(listOfObj.Count>=1)
        {
            canDestroy = true;  //make to true to destroy cubes

            //testing bool check
            foreach (GameObject obj in listOfObj)
            {               
                if (!obj.GetComponent<BoxTwo>().check)
                {                  
                    obj.GetComponent<BoxTwo>().check = true;
                    obj.GetComponent<BoxTwo>().CheckTheSideCubes();
                }
                else
                {
                    //Debug.Log("already checked");                 
                }
            }         
        }

        if (canDestroy)// destroy cube
        {
            if(StartJump!=null)
                StartJump();
            var root = transform.root;
            Effect(transform.root);
        }

        void AddToList(GameObject obj)
        {
            if(obj.CompareTag("cube") && obj.transform.root.CompareTag(transform.root.tag))//check its a cube and belongs to same tag group
                listOfObj.Add(obj);        
        }

        foreach (GameObject x in listOfObj) //deactivate all objects
        {
            //check for npcbox
            if(x.transform.childCount>0 && x.transform.GetChild(0).CompareTag("insidebox") && !x.GetComponent<BoxTwo>().npcBox && x.GetComponent<SpawnNPC>().instantiateNPC)
            {
                var obj= GetTypeOfNpc(x.transform.GetChild(0).transform.GetChild(0).gameObject);
                if(obj!=null)
                Instantiate(obj, x.transform.position, Quaternion.Euler(0, 180, 0));  //spawn npc

                Instantiate(GeneralVariable.instance.particleSpawn, x.transform.position, Quaternion.identity);//particle  
                x.GetComponent<BoxTwo>().npcBox = true;
            }

            x.SetActive(false);

            CheckBomb();
        }      
    }

    GameObject GetTypeOfNpc(GameObject x)
    {
        GameObject temp = null;
        var npcTag = x.transform.GetChild(0).tag;
        print( x.transform.GetChild(0).name);
        foreach(GameObject obj in GeneralVariable.instance.NPCCollecttion)
        {
            if(obj.transform.GetChild(0).CompareTag(npcTag))
            {
                temp= obj;
            }
        }
        return temp;
    }
     
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("SharpObject"))
        {
            Effect(transform.root);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SharpObject"))
        {
            Effect(transform.root);
            gameObject.SetActive(false);
        }
    }


    void CheckBomb()
    {
        if (transform.childCount > 0 && transform.GetChild(0).gameObject.CompareTag("bomb"))
        {
            print("Destroy");
            GameObject[] count = GameObject.FindGameObjectsWithTag("manager");
            if (count.Length > 0)
            {
                AudioManager.instance.Play("grenade");
                PauseMenu.instance.LooseCondition();
                
                Instantiate(GeneralVariable.instance.bombparticleEffect, transform.position + new Vector3(0, 0, -.6f), Quaternion.identity);
            }
        }
    }

}
