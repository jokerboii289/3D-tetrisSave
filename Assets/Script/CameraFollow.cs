using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] GameObject npc;
    [SerializeField] GameObject obstacle;
    //CinemachineVirtualCamera vCam;
    Vector3 originalPosition;
    float posY;
    bool start;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        obstacle.SetActive(false);
        start = false;
        posY = transform.position.y;
        npc.SetActive(false);       
             
        StartCoroutine(Delay());
    }

    private void Update()
    {
        if (start)
        {
            var displacement = originalPosition.y - npc.transform.position.y;
            transform.position = new Vector3(transform.position.x, posY - displacement, transform.position.z);
        }
        if (transform.position.y < 1f)
        {
            start = false; //stop if the camera is closer to the ground
        }
    }

   
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.2f);
        npc.SetActive(true);
        yield return new WaitForSeconds(2);

        start = true;
        originalPosition = npc.transform.position;
        obstacle.SetActive(true);       
    }
}
