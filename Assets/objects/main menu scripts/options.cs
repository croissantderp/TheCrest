using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class options : MonoBehaviour
{
    //referencing an animator
    public Animator Atransition;

    public TMP_Dropdown resolutionDropdown;

    //the time LoadLevel waits
    public float transitionTime = 0.5f;

    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public float volume2;

    public int fullscreen2;

    public int fog2;

    public Toggle fl;

    public Toggle fg;

    public Slider vm;

    // Start is called before the first frame update
    void Start()
    {
        //retrieves and applys options
        SetVolume(PlayerPrefs.GetFloat("volume", -80));
        vm.value = PlayerPrefs.GetFloat("volume", -80);

        if (PlayerPrefs.GetInt("fullscreen", 0) == 0)
        {
            SetFullscreen(false);
            fl.isOn = false;
        }
        else
        {
            SetFullscreen(true);
            fl.isOn = true;
        }

        if (PlayerPrefs.GetInt("fog", 0) == 0)
        {
            FogOn(false);
            fg.isOn = false;
        }
        else
        {
            FogOn(true);
            fg.isOn = true;
        }

        //finds and sets resolutions
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //skips delay
        Atransition.SetBool("start_new", true);
    }

    //if fog is toggled
    public void FogOn(bool fog)
    {
        data data = FindObjectOfType<data>();
        data.disableFog = fog;

        //converts bool to int
        fog2 = (fog ? 1 : 0);
    }

    //goes back to main page
    public void Back()
    {
        StartCoroutine(load());
        save();
    }

    //sets volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        volume2 = volume;
    }

    //sets fullscreen
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;

        //converts bool to int
        fullscreen2 = (fullscreen ? 1 : 0);
    }

    //sets resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //loads
    IEnumerator load()
    {
        //starts animation
        Atransition.SetTrigger("start");

        Atransition.SetBool("start_new", false);

        //waits, then continues rest of code
        yield return new WaitForSeconds(transitionTime);

        //loads next scene
        SceneManager.LoadScene("start menu normal");
    }

    //saves options
    void save()
    {
        PlayerPrefs.SetFloat("volume", volume2);
        PlayerPrefs.SetInt("fullscreen", fullscreen2);
        PlayerPrefs.SetInt("fog", fog2);

        PlayerPrefs.Save();
    }
}
