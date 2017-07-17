using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMana : Singleton<AudioMana>
{
    public void PlayChoppoing()
    {
        GetComponents<AudioSource>()[0].Play();
    }

    public void PlayPickUp()
    {
        GetComponents<AudioSource>()[1].Play();
    }

    public void PlayRip()
    {
        GetComponents<AudioSource>()[2].Play();
    }

    public void PlayWind()
    {
        GetComponents<AudioSource>()[3].Play();
    }
}
