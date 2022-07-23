using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    public float Intensity=1f;
    public float mass=1f;
    public float stiffness=1f;
    public float damping=.75f;
    private Mesh originalMesh,meshClone;
    private MeshRenderer renderer;

    private JellyVertex[] jv;
    private Vector3[] vertexArray;
    // Start is called before the first frame update
    void Start()
    {
        originalMesh = GetComponent<MeshFilter>().sharedMesh;
        meshClone = Instantiate(originalMesh);
        GetComponent<MeshFilter>().sharedMesh = meshClone;
        renderer = GetComponent<MeshRenderer>();

        jv = new JellyVertex[meshClone.vertices.Length];
        for(int i=0;i<meshClone.vertices.Length;i++)//array 
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(meshClone.vertices[i]));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vertexArray = originalMesh.vertices;
        for(int i=0;i<jv.Length;i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].ID]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].position);
            vertexArray[jv[i].ID] = Vector3.Lerp(vertexArray[jv[i].ID], target, intensity); //quadaractic 
           
        }
        meshClone.vertices = vertexArray; //assigning shared clone mesh
    }

    public class JellyVertex
    {
        public int ID;
        public Vector3 position; 
        public Vector3 velocity,force;
        
        public JellyVertex(int _id,Vector3 _pos)
        {
            ID = _id;
            position = _pos;
        }

        public void Shake(Vector3 target,float m,float s,float d)//vertical displacement of vertices
        {
            force = (target - position) * s;
            velocity = (velocity + force / m) * d;
            position += velocity;
            if ((velocity + force + force / m).magnitude < .001f)
                position = target;
        }
    }
}
