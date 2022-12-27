using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour, PooledObject
{
    //sudo "this" object
    public GameObject thas;

    //the collider on the note
    public Collider2D cr;

    //speed of bullets
    [HideInInspector]
    public float speed = 1;

    //length of the note
    [HideInInspector]
    public float length = 0;

    //target of bullets
    public Transform target;

    //if the note is destroyed
    bool destroy;

    //if the note is able to move or be destroyed
    public bool move;
    public bool destroyable;

    //getting components
    public battleHealth battleHealth;
    public beam beam;
    public Animator anim;
    public enemySpawner enemy;

    Vector3 offset;

    public float smoothing;

    //the components for highlighting the note
    public MeshRenderer render;
    public Material notNext;
    public Material next;

    //the number of the note
    [HideInInspector]
    public int noteNum;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        //enables collider
        cr.enabled = true;

        render.material = notNext;

        //animated spawning of note
        anim.SetTrigger("spawn");

        //makes note look at target

        //gets the direction to look in
        Vector3 direction = target.position - transform.position;

        //sets transform.up to that direction
        transform.up = direction;

        offset = transform.position - target.position;

        destroy = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if (!destroy && move)
        {
            //adjusts offset to move
            offset += (transform.up * speed * Time.deltaTime);

            //lerps object
            transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        }

        //if the player has destroyed all previous notes
        if (enemy.totalDestroyed == noteNum)
        {
            render.material = next;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if the note collides with the ground
        if (collider.gameObject.layer == 11)
        {
            StartCoroutine(destroy2());
            destroy = true;
        }

        if (!destroy)
        {
            switch (collider.name)
            {
                case "collider":
                    //adds one to the number of destroyed notes
                    enemy.hitNotes++;

                    //does damage to the player
                    battleHealth.damage(20);

                    //destroys this note
                    StartCoroutine(destroy2());
                    destroy = true;
                    break;

                case "highlighter":
                    battleHealth.highlight();
                    break;

                case "burst":
                    enemy.hitNotes++;

                    StartCoroutine(destroy2());
                    destroy = true;
                    break;

                case "blade":
                    //adds one to the number of destroyed notes
                    enemy.shotNotes++;

                    //subtracts this note's length from the blade duration
                    beam.dureRemain -= length;
                    StartCoroutine(destroy2());
                    destroy = true;
                    break;

            }
            //if the note collides with the player
            if (collider.name == "collider")
            {
            }


            if (collider.name == "burst")
            {
            }

            //if the note enters the blade
            if (collider.name == "blade")
            {
            }
        }
    }

    //destroy function
    public IEnumerator destroy2()
    {
        if (destroyable)
        {
            //disables collider upon death
            cr.enabled = false;

            //plays death animation
            anim.SetTrigger("death");

            enemy.totalDestroyed++;

            yield return new WaitForSeconds(0.25f);

            thas.SetActive(false);
        }        
    }
}