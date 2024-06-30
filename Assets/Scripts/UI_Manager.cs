using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class UI_Manager : MonoBehaviour
{
    public RectTransform panelInicioDia;
    public TextMeshProUGUI textoInicioDia;
    public float velocidadTexto = 0.1f;
    public float duracionPanel = 1.0f;

    public RectTransform panelReporte; // Panel que se mostrará al final
    public TextMeshProUGUI mensajeReporte;

    public TextMeshProUGUI sanosText;
    public TextMeshProUGUI enfermosText;

    public event Action PanelInicioDesactivado;

    public DialogueManager dialogueManager;
    public s_GameManager gameManager;

    void Start()
    {

    }

    public void MostrarInicioDia(string mensaje)
    {
        dialogueManager.panelDialogo.gameObject.SetActive(false);
        dialogueManager.panelRespuestas.gameObject.SetActive(false);

        dialogueManager.botonIngreso.interactable = false;
        dialogueManager.botonRechazo.interactable = false;

        panelReporte.gameObject.SetActive(false);

        panelInicioDia.gameObject.SetActive(true);
        StartCoroutine(MostrarPanelInicioDiaCoroutine(mensaje));
    }


    private IEnumerator MostrarPanelInicioDiaCoroutine(string mensaje)
    {
        textoInicioDia.text = "";

        foreach (char letter in mensaje)
        {
            textoInicioDia.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
        }

        yield return new WaitForSeconds(duracionPanel);

        panelInicioDia.gameObject.SetActive(false);

        //invoca el evento cuando el panel se desactive
        PanelInicioDesactivado?.Invoke();
    }


    public void ActualizarPanelReporte(int sanos, int enfermos)
    {
        panelReporte.gameObject.SetActive(true);
        sanosText.text = "Sanos ingresados: " + sanos;
        enfermosText.text = "Enfermos ingresados: " + enfermos;

gameManager.MostrarMensaje();
    }
}
