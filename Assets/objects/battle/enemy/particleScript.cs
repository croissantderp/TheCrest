using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class particleScript : MonoBehaviour
{
    //particle system components
    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    //other components
    public battleHealth bh;
    public Transform player;
    public Transform highlighter;
    public enemySpawner es;

    //this transform
    public Transform tm;

    //if the particle system is not transformed
    bool noTransform;
    bool noRotate;

    //components for transform
    float timeElapsed;
    Vector3 StartPos;

    public bool[] Play = { };
    public float[] PlayDures = { };

    //the positions to move to
    public Vector3[] transform2 = { };
    public float[] transformDures = { };

    //the rotations
    public float[] rotate2 = { };
    public float[] rotationDures = { };
    public bool[] lookAt = { };

    //the current values the arrays are on
    int current;
    int current1;
    int current2;

    //the duration until the next note
    float dureRemain;
    float dureRemain1;
    float dureRemain2;

    //controls beat
    bool beat;
    bool canBeat;

    [HideInInspector]
    public float bpm;

    //when the particles hits the player
    void OnParticleTrigger()
    {
        //adds to a list (strangely required for some reason)
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        //if the number of particles that hits the player is not 0
        for (int i = 0; i < numEnter; i++)
        {
            if (Vector3.Distance(enter[i].position + enter[i].totalVelocity * 0.1f, highlighter.position) <= 0.5f && bh.mainCollider.enabled)
            {
                //does damage
                bh.damage(10);
            }
            bh.highlight();
        }
    }


    void Start()
    {
        //checks lengths of arrays
        if (transform2.Length != transformDures.Length)
        {
            UnityEngine.Debug.Log("make sure transform arrays have same number of values");
        }

        if (rotate2.Length != rotationDures.Length || rotate2.Length != lookAt.Length)
        {
            UnityEngine.Debug.Log("make sure rotation arrays have same number of values");
        }

        if (Play.Length != PlayDures.Length)
        {
            UnityEngine.Debug.Log("make sure play arrays have same number of values");
        }

        if (transform2.Length <= 0)
        {
            noTransform = true;
        }

        if (rotate2.Length <= 0)
        {
            noRotate = true;
        }

        if (Play.Length <= 0)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }

        transformDures = transformDures.Select(a => a * es.beatsPerNote).ToArray();
        rotationDures =   rotationDures.Select(a => a * es.beatsPerNote).ToArray();
        PlayDures =           PlayDures.Select(a => a * es.beatsPerNote).ToArray();

        if (!noTransform)
        {
            dureRemain1 = transformDures[current1];
        }

        if (!noRotate)
        {
            dureRemain2 = rotationDures[current2];
        }

        dureRemain = PlayDures[current];

        //sets bpm
        bpm = es.bpm;
    }

    void FixedUpdate()
    {
        //transforms the particle system over the duration
        if (!noTransform)
        {
            tm.position = Vector3.Lerp(StartPos, transform2[current1], timeElapsed / (transformDures[current1] / (bpm/60)));
            timeElapsed += Time.deltaTime;
        }

        if (!noRotate)
        {
            //rotates the particle system
            tm.Rotate(0, 0, rotate2[current2]);
            if (lookAt[current2])
            {
                //gets the direction to look in
                Vector3 direction = player.position - tm.position;

                //sets transform.up to that direction
                tm.up = direction;
            }
        }
    }

    void Update()
    {
        //very bad singleton
        if (es.delayedBeat && canBeat)
        {
            beat = true;
            canBeat = false;
        }
        else if (!es.delayedBeat)
        {
            canBeat = true;
        }

        //counts only when it needs to
        if (current1 < transform2.Length - 1 || current2 < rotate2.Length - 1 || current < Play.Length - 1)
        {
            //subtracts from dure during beat
            if (es.started && beat)
            {
                dureRemain--;
                dureRemain1--;
                dureRemain2--;


                beat = false;
            }
        }

        //changes the current value when dure is up
        if (current < Play.Length - 1 && dureRemain <= 0)
        {
            current++;
            dureRemain = PlayDures[current];

            if (Play[current])
            {
                ps.Stop();
                ps.Play();
            }
        }

        //changes the current value when dure is up
        if (current1 < transform2.Length - 1 && dureRemain1 <= 0)
        {
            current1++;
            dureRemain1 = transformDures[current1];

            //resets transforms
            timeElapsed = 0;
            StartPos = tm.position;
        }

        //changes the current value when dure is up
        if (current2 < rotate2.Length - 1 && dureRemain2 <= 0)
        {
            current2++;
            dureRemain2 = rotationDures[current2];
        }
    }
}
