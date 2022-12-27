using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class battleHealth : MonoBehaviour
{
    //main collider
    public Collider2D mainCollider;

    public SpriteRenderer highlighter;

    //health
    public int health = 5;
    string health2;

    //beats until heal
    public int bph;

    public float bpm;

    //animator component
    public Animator animator;

    //textmesh components
    public TextMeshProUGUI tmh;

    //pause script
    public pause ps;

    //if it is the beat
    public bool beat;

    //counter for healing
    int counter = 0;

    //damage burst
    public Collider2D damageCollider;
    public ParticleSystem pas;

    // Start is called before the first frame update
    void Start()
    {
        //sets health ui
        health2 = health.ToString();
        tmh.text = health2;

        enemySpawner es = FindObjectOfType<enemySpawner>();
        bph = es.beatsPerMeasure;
        bpm = es.bpm;

        var main = pas.main;
        main.duration = bph * (60 / bpm);
    }

    // Update is called once per frame
    void Update()
    {
        //if player is out of health
        if (health <= 0)
        {
            gameOver();
        }

        //adds to counter every beat and if health is under 100
        if (beat && health < 100)
        {
            counter += 1;

            beat = false;
        }

        //if the counter is filled
        if (counter >= bph)
        {
            //adds and updates health
            health += 1;

            health2 = health.ToString();
            tmh.text = health2;

            counter = 0;
        }
    }

    //takes damage
    public void damage(int damage)
    {
        health = health - damage;

        if (health < 0)
        {
            health = 0;
        }

        //if the player is hit
        if (damage == 20)
        {
            StartCoroutine(damageBurst());
        }

        health2 = health.ToString();
        tmh.text = health2;

        //disabled main collider
        mainCollider.enabled = false;
        animator.SetBool("active", false);

        //delays reactivation of collider
        StartCoroutine(damageDelay());
    }

    public void highlight()
    {
        StartCoroutine(highlight2());
    }

    IEnumerator highlight2()
    {
        highlighter.color = new Color(255, 129, 129, 255);
        yield return new WaitForSeconds(1f);
        highlighter.color = Color.clear;

    }

    IEnumerator damageDelay()
    {
        //converts bph and bpm into seconds
        yield return new WaitForSeconds(bph * (60 / bpm));

        mainCollider.enabled = true;
        animator.SetBool("active", true);
    }

    //activates a round collider
    IEnumerator damageBurst()
    {
        pas.Play();

        damageCollider.enabled = true;

        yield return new WaitForSeconds(bph * (60 / bpm));

        damageCollider.enabled = false;

        pas.Stop();
    }

    void gameOver()
    {
        ps.lose();
    }
}
