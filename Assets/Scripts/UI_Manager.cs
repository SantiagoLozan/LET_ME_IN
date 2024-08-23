using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class UI_Manager : MonoBehaviour
{

    public RectTransform panelInicioDia;
    public TextMeshProUGUI textoInicioDia;
    public float velocidadTexto = 0.1f;
    public float duracionPanel = 1.0f;

    public RectTransform panelReporte;
    public TextMeshProUGUI mensajeReporte;
    public TextMeshProUGUI reporteText;

    public Button botonSiguienteNivel;

    public event Action PanelInicioDesactivado;

    public DialogueManager dialogueManager;
    public s_GameManager gameManager;

    public AudioSource audioTecleo;

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
        botonSiguienteNivel.gameObject.SetActive(false);

        panelInicioDia.gameObject.SetActive(true);
        int diaActual = gameManager.NivelActual;
        string titulo = $"Día {diaActual}\n\n";
        StartCoroutine(MostrarPanelInicioDiaCoroutine(titulo + mensaje));
    }


    private IEnumerator MostrarPanelInicioDiaCoroutine(string mensaje)
    {
        textoInicioDia.text = "";

        audioTecleo.Play();

        foreach (char letter in mensaje)
        {

            //adelantar texto con la tecla espacio
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textoInicioDia.text = mensaje;
                break;
            }

            textoInicioDia.text += letter;
            yield return new WaitForSeconds(velocidadTexto);

        }

        audioTecleo.Stop();

        yield return new WaitForSeconds(duracionPanel);

        panelInicioDia.gameObject.SetActive(false);


        //invoca el evento cuando el panel se desactive
        PanelInicioDesactivado?.Invoke();
    }


    public void ActualizarPanelReporte(int sanosIngresados, int enfermosIngresados, int sanosRechazados, int enfermosRechazados)
    {
        panelReporte.gameObject.SetActive(true);
        // botonSiguienteNivel.gameObject.SetActive(true);

        int diaActual = gameManager.NivelActual;
        string tituloReporte = $"Reporte Día {diaActual}\n\n";
        reporteText.text = $"{tituloReporte}" +
                        $"Sanos ingresados: {sanosIngresados}\n" +
                        $"Enfermos ingresados: {enfermosIngresados}\n" +
                        $"Sanos rechazados: {sanosRechazados}\n" +
                        $"Enfermos rechazados: {enfermosRechazados}";

        gameManager.MostrarMensaje();
    }

}
