using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool musicOn = true;
    public AudioSource audioSource;
    public GameObject volumeButton;
    public Sprite volumeOn, volumeOff;
    void Start()
    {
        DontDestroyOnLoad(this);
        if (volumeButton != null)
        {
            volumeButton.GetComponent<Button>().onClick.AddListener(() =>
            {

                musicOn = !musicOn;
                if (musicOn)
                {
                    audioSource.mute = false;
                    volumeButton.GetComponent<Image>().sprite = volumeOn;
                }
                else
                {
                    audioSource.mute = true;

                    volumeButton.GetComponent<Image>().sprite = volumeOff;
                }

            });
        }

    }




}
