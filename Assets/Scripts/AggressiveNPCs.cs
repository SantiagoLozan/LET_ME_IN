using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AggressiveNPCs : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public s_GameManager gameManager; 
    public CharactersManager charactersManager;
    public GameObject panelPerdiste; 

    private float tiempoRestante;
    private bool temporizadorActivo = false;
    public GameObject PanelSeguridad;
    private Coroutine toggleCoroutine;
    public AudioSource audioSeguridad;
    public Button botonSeguridad;

    void Start()
    {
        botonSeguridad.interactable = false;
    }
    void Update()
    {
        if (temporizadorActivo)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                temporizadorActivo = false;
                ActualizarTextoTemporizador();
                FinTemporizador();
            }
            else
            {
                ActualizarTextoTemporizador();
            }
        }
    }

    public void MostrarComportamientoAgresivo()
    {
        Debug.Log("¡El personaje está actuando de manera agresiva!");
        botonSeguridad.interactable = true;
        StartTimer(5); 
        Peligro();
    }

    void StartTimer(float tiempo)
    {
        tiempoRestante = tiempo;
        temporizadorActivo = true;
        ActualizarTextoTemporizador();
    }

    void ActualizarTextoTemporizador()
    {
        if (timerText != null)
        {
            timerText.text = $"{tiempoRestante:F1}"; 
        }
    }

    void FinTemporizador()
    {
        DetenerPeligro();
        panelPerdiste.SetActive(true);
    }

    public void Peligro()
    {
        if (!PanelSeguridad.activeInHierarchy)
        {
            if (toggleCoroutine == null)
            {
                toggleCoroutine = StartCoroutine(TogglePanel());
            }
        }
    }

    IEnumerator TogglePanel()
    {
        while (true)
        {
            PanelSeguridad.SetActive(true);
            audioSeguridad.Play();

            yield return new WaitForSeconds(1f);

            PanelSeguridad.SetActive(false);

            yield return new WaitForSeconds(1f);
        }
    }

    public void DetenerPeligro()
    {
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
            PanelSeguridad.SetActive(false); 
            audioSeguridad.Stop(); 
        }
    }

    public void LlamarSeguridad()
    {
        DetenerPeligro();

        if (temporizadorActivo)
        {
            temporizadorActivo = false;
            timerText.gameObject.SetActive(false); 
        }
        charactersManager.AparecerSiguientePersonaje();
    }
}
