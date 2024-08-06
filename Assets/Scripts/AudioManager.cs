using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource AS;
    public bool estaHablando = false;

    public AudioClip[] gibberishClips;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        AS = GetComponent<AudioSource>();
        if (AS == null)
        {
            AS = gameObject.AddComponent<AudioSource>();
        }
        Debug.Log("Número de clips cargados: " + gibberishClips.Length);
    }

    public void HablarPalabrasEnLoop(AudioClip[] gibberishClips)
    {
        if (gibberishClips == null || gibberishClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de audio disponibles para reproducir.");
            return;
        }

        Debug.Log("Iniciando reproducción de clips de audio.");
        StartCoroutine(HablarPalabrasEnLoopRoutine(gibberishClips));
    }

    IEnumerator HablarPalabrasEnLoopRoutine(AudioClip[] gibberishClips)
    {
        estaHablando = true;
        Debug.Log("Comienza la rutina de reproducción en loop.");

        while (estaHablando)
        {
            AS.Stop();
            int randomIndex = Random.Range(0, gibberishClips.Length);
            AS.clip = gibberishClips[randomIndex];
            Debug.Log("Reproduciendo clip: " + gibberishClips[randomIndex].name);
            AS.Play();

            while (AS.isPlaying)
            {
                yield return null;
            }
        }

        Debug.Log("Terminó la rutina de reproducción en loop.");
    }

    public void DetenerHablar()
    {
        estaHablando = false;
        AS.Stop();
    }
}
