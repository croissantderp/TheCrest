using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class enemySpawner : MonoBehaviour
{
    //note gameobjects
    public GameObject Note;

    //inputs
    public string title;
    public float bpm;
    public int beatsPerMeasure;
    public int beatsPerNote;
    public string[] note = { };
    public float[] length = { };
    public float[] duration = { };
    public AudioClip audioClip;

    public AudioSource source;

    public float extraDelay;

    //dictionary that stores string to vector 3 info
    Dictionary<string, int> noted = new Dictionary<string, int>()
    {
        {"c", 1},
        {"c#", 2},
        {"d", 3},
        {"d#", 4},
        {"e", 5},
        {"f", 6},
        {"f#", 7},
        {"g", 8},
        {"g#", 9},
        {"a", 10},
        {"a#", 11},
        {"b", 12},
        {"c2", 13},
        {"c#2", 14},
        {"d2", 15},
        {"d#2", 16},
        {"e2", 17},
        {"f2", 18},
        {"f#2", 19},
        {"g2", 20},
        {"g#2", 21},
        {"a2", 22},
        {"a#2", 23},
        {"b2", 24},
    };

    public float currentTempo;

    //what place the data is passing out to
    int placeValue = 0;

    //timer
    double timeRemaining = 1;
    double totalTime;
    double maxTime;

    //total time remaining
    double totalTimeRemaining = 0;
    double totalTimeRemaining2 = 0;

    //the smallest note value
    float smallest = 1;

    //a counter for the timer
    int counter = 0;

    //the delayed beat
    bool delayedStart;
    public float delayValue;
    [HideInInspector]
    public bool delayedBeat;

    //duration of note until next note spawns (short for duration)
    public float dure;

    //the number of the beat
    int beatNum = 0;

    //the current note
    string currentNote;

    float angle;

    //length of the arrays that control the notes
    int[] musicLengths = new int[3];

    //if game has started
    [HideInInspector]
    public bool started = false;
    [HideInInspector]
    public bool started2 = false;

    //if the game is finished
    public bool finished = false;

    //various scripts
    public beam beam;
    public battleHealth bh;
    public quartz quartz;

    //the object pooler
    objectPooler objectPooler;

    //player
    public GameObject player;

    //statistics of the game
    public int totalNotes = 0;
    public int shotNotes = 0;
    public int hitNotes = 0;
    public int totalDestroyed = 0;

    int noteNum = 0;

    //if player is recharging
    [HideInInspector]
    public bool recharging;

    [HideInInspector]
    public bool bladeWait;

    //how much time (in seconds) has passed since the song started
    double dsptimesong;

    // Start is called before the first frame update
    void Awake()
    {
        objectPooler = objectPooler.Instance;

        notes st = FindObjectOfType<notes>();
        title = st.title;
        bpm = st.bpm;
        string time = st.time;
        note = st.note;
        length = st.length;
        duration = st.duration;
        audioClip = st.audioClip;
        extraDelay = st.audioDelay;

        //parses the time signature
        string[] time2 = time.Split('/');
        beatsPerMeasure = int.Parse(time2[0]);
        beatsPerNote = int.Parse(time2[1]);

        source.clip = audioClip;
        source.Stop();

        angle = 180 / (float)(noted.Count + 1);
        
        //multiplying the lengths by the beats per note
        for (int i = 0; i < length.Length; i++)
        {
            length[i] *= beatsPerNote;
        }

        //doing the same to the duration
        for (int i = 0; i < duration.Length; i++)
        {
            duration[i] *= beatsPerNote;

            //finding the smallest non 0 duration
            if (duration[i] < smallest && duration[i] != 0 && !Regex.IsMatch(duration[i].ToString(), @"(3|6){4,}"))
            {
                smallest = duration[i] / 3;
            }
        }

        //converts bpm to seconds and divides smallest as well
        totalTime = 60 / (bpm / smallest);
        timeRemaining = totalTime;

        //sets the array length variables to the array length
        musicLengths[0] = note.Length;
        musicLengths[1] = length.Length;
        musicLengths[2] = duration.Length;

        int b = musicLengths[0];
        foreach (int a in musicLengths)
        {
            if (a != b)
            {
                Debug.Log("make sure there are the same number of notes, note lengths and durations!");
                Debug.Log(a + " | " + b);
            }
            b = a;
        }

        //the regular bpm to seconds without smallest division
        maxTime = 60 / bpm;

        //sets the time values to max (without dividing by smallest)
        quartz.SetMaxTime((float)maxTime);

        currentTempo = bpm;
    }

    // Update is called once per frame
    void Update()
    {
        //duplicates if started
        if (started2)
        {
            //duplicates the first note
            duplicate();

            started2 = false;

            //starts delay
            StartCoroutine(delay());

            //resets duration
            dure = duration[placeValue - 1];
        }

        //timer
        if (started)
        {
            if (timeRemaining <= 0)
            {
                //updates the remaining duration of the note
                dure -= smallest;

                //adds to the counter
                counter += 1;

                //adds the total time to the extra counter for the metronome
                totalTimeRemaining2 += totalTime;

                //when the counter reaches the number of smallest notes that fit in a single beat
                if (counter >= (1 / smallest))
                {
                    if (!bladeWait)
                    {
                        beam.beat = true;
                        bh.beat = true;

                        recharging = false;
                    }
                    //starts particle scripts once delay is over
                    if (delayedStart)
                    {
                        delayedBeat = true;
                    }

                    beatNum++;

                    totalTimeRemaining2 = 0;

                    counter = 0;
                }

                ChangeTempo();

                //resets timer
                timeRemaining = totalTime;
            }
            else
            {
                timeRemaining -= AudioSettings.dspTime - dsptimesong;

                dsptimesong = AudioSettings.dspTime;

                delayedBeat = false;

                //calculatesd the total time left for the metronome
                totalTimeRemaining = maxTime - (totalTimeRemaining2 + (totalTime - timeRemaining));
            }

            //updates quartz script if recharging
            quartz.SetTime(recharging ? (float)totalTimeRemaining : 0);
        }
       
        //spawns in the next object if place value is less than length of the arrays
        if (placeValue < musicLengths[0])
        {
            //if the note duration is up, spawns the next note.
            if (dure <= 0 && started)
            {
                duplicate();
            }
        }
        else
        {
            finished = true;
        }
    }

    //changes tempo
    void ChangeTempo()
    {
        if (!finished)
        {
            if (note[placeValue].Contains("tempo"))
            {
                int temp = int.Parse(note[placeValue].Remove(0, 6));

                StartCoroutine(DelayedChange(temp));

            }
        }
    }

    //applies another delay before the tempo change
    IEnumerator DelayedChange(int temp)
    {
        //the regular bpm to seconds without smallest division
        maxTime = 60 / temp;

        //converts bpm to seconds and divides smallest as well
        totalTime = 60 / (temp / smallest);
        timeRemaining = totalTime;

        //beam.infinite = true;

        bladeWait = true;

        StartCoroutine(tempTimer());

        //waits for amount of time based on distance formula
        yield return new WaitForSeconds(delayValue / (bpm / 20));

        currentTempo = temp;

        bladeWait = false;

        //beam.infinite = false;
    }

    IEnumerator tempTimer()
    {
        //waits for amount of time based on distance formula
        yield return new WaitForSeconds(60 / currentTempo);

        beam.beat = true;
        bh.beat = true;

        recharging = false;

        if (bladeWait)
        {
            StartCoroutine(tempTimer());
        }
    }

    //a delayer to account for the time the note takes to travel to the player
    IEnumerator delay()
    {
        //waits for amount of time based on distance formula
        yield return new WaitForSeconds(delayValue / (bpm / 20));

        //starts after delay
        delayedStart = true;

        //waits for amount of time based on distance formula
        yield return new WaitForSeconds(extraDelay);

        source.Play();
    }

    void duplicate()
    {
        if (note[placeValue] == "rest" || note[placeValue].Contains("tempo"))
        {
            //sets duration to new note
            dure = duration[placeValue];

            //increases place value to match current data being passed out.
            placeValue++;

            return;
        }
        else
        {
            //a float value of the length that can be used
            float tempLength = length[placeValue];

            //clones the enemy based on the duration
            for (int i = 0; i < length[placeValue]; i++)
            {
                Note = objectPooler.Instance.SpawnFromPool("notes", player.transform.position, i, noted[note[placeValue]] * angle);
                
                //finds enemyscript
                enemyController Enemy = Note.GetComponent<enemyController>();

                Enemy.noteNum = noteNum;

                //passes out data to enemy
                Enemy.speed = bpm / 20;

                //if the length is greater than 1
                Enemy.length = (tempLength > 1 ? 1 : tempLength);

                //subtracts one from the temp length every iteration of the loop
                tempLength -= 1;

                //continues passing out data
                Enemy.move = true;
                Enemy.destroyable = true;

                //adds one note to the total count
                totalNotes++;

                noteNum++;
            }
        }

        //sets duration to new note
        dure = duration[placeValue];

        //increases place value to match current data being passed out.
        placeValue++;
    }
}
