using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMana : Singleton<AudioMana>
{
    [SerializeField]
    private ObjectPooler audioSources;
    
    public void PlayAudio(AudioClip info)
    {
        GameObject audio_obj = audioSources.GetObject();
        audio_obj.GetComponent<AudioSource>().clip = info;
        audio_obj.GetComponent<AudioSource>().Play();
        StartCoroutine(StopWhenFinishedPlaying(audio_obj.GetComponent<AudioSource>()));
    }

    private IEnumerator StopWhenFinishedPlaying(AudioSource source)
    {
        while(source.isPlaying)
        {
            yield return null;
        }

        source.gameObject.SetActive(false);
    }
}
