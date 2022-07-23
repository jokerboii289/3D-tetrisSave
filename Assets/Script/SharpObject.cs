using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpObject : MonoBehaviour
{

    [SerializeField] float speed,rotateSpeed;

    [SerializeField] bool xAxisRotation;
    [SerializeField] bool yAxisRotation;
    [SerializeField] bool zAxisRotation;

    [Header("For Rotation")]
    [SerializeField]GameObject[] rotatingObj;

    [Header("Move Downwards")]
    [SerializeField] GameObject[] downMovingObj;
    // Update is called once per frame
    void Update()
    {       
        if (zAxisRotation)
        {
            foreach(GameObject obj in rotatingObj)
                obj.transform.RotateAround(obj.transform.forward, rotateSpeed * Time.deltaTime);
        }
        else if (xAxisRotation)
        {
            foreach (GameObject obj in rotatingObj)
                obj.transform.RotateAround(obj.transform.right, rotateSpeed * Time.deltaTime);
        }

        foreach (GameObject obj in downMovingObj)
            transform.position += -Vector3.up * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("character"))
        {
            AudioManager.instance.Play("Death");
            Instantiate(GeneralVariable.instance.DeathNPCParticleEffect, new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z), Quaternion.identity);
            collision.gameObject.SetActive(false);
            PauseMenu.instance.LooseCondition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("character"))
        {
            AudioManager.instance.Play("Death");
            Instantiate(GeneralVariable.instance.DeathNPCParticleEffect, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), Quaternion.identity);
            other.gameObject.SetActive(false);
            PauseMenu.instance.LooseCondition();
        }
    }
}
