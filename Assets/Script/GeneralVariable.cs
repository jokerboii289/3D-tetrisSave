using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralVariable : MonoBehaviour
{
    
    public static GeneralVariable instance;

    public GameObject BlueParticle;
    public GameObject redParticle;
    public GameObject greenParticle;
    public GameObject yellowParticle;

    public GameObject DeathNPCParticleEffect;

    [Header("NPC")]//character
    public GameObject NPC;
    public GameObject[] NPCCollecttion
;   public GameObject particleSpawn;
    //public GameObject jumpSmoke;

    [Header("BOMB")]
    public GameObject bombparticleEffect;
    public bool dontSpawnBomb;

    private void Start()
    {
        instance = this;
    } 
}
