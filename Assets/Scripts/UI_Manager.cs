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
    public float intervaloCursor = 0.5f;

    public RectTransform panelReporte;
    public RectTransform panelPerdiste;
    public TextMeshProUGUI mensajeReporte;
    public TextMeshProUGUI reporteText;

    public RectTransform indicaciones; 
    public float duracionIndicaciones = 3.0f;

    public Button botonSiguienteNivel;

    public event Action PanelInicioDesactivado;

    public DialogueManager dialogueManager;
    public s_GameManager gameManager;

    public AudioSource audioTecleo;

    private Coroutine panelInicioDiaCoroutine; 
    private bool cursorVisible = true;
    private float tiempoUltimaActualizacion;

    public void MostrarInicioDia(string mensaje)
    {
        dialogueManager.panelDialogo.gameObject.SetActive(false);
        dialogueManager.panelRespuestas.gameObject.SetActive(false);

        dialogueManager.botonIngreso.interactable = false;
        dialogueManager.botonRechazo.interactable = false;

        panelReporte.gameObject.SetActive(false);
        panelPerdiste.gameObject.SetActive(false);
        botonSiguienteNivel.gameObject.SetActive(false);

        panelInicioDia.gameObject.SetActive(true);
        int diaActual = gameManager.NivelActual;
        string titulo = $"Día {diaActual}\n\n";

        // Iniciar la corrutina y guardar su referencia
        panelInicioDiaCoroutine = StartCoroutine(MostrarPanelInicioDiaCoroutine(titulo + mensaje));
    }

    private IEnumerator MostrarPanelInicioDiaCoroutine(string mensaje)
    {
        textoInicioDia.text = "";
        string mensajeConCursor = mensaje + "_";

        audioTecleo.Play();
        tiempoUltimaActualizacion = Time.time;

        // Mostrar el texto con efecto de escritura
        while (textoInicioDia.text.Length < mensaje.Length)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textoInicioDia.text = mensaje;
                break;
            }

            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Construye el texto actual con el cursor
            string textoParcial = mensaje.Substring(0, textoInicioDia.text.Length);
            if (cursorVisible)
            {
                textoParcial += "_";
            }
            textoInicioDia.text = textoParcial;

            yield return new WaitForSeconds(velocidadTexto);
        }


        textoInicioDia.text = mensaje;
        audioTecleo.Stop();

        // mantiene el cursor titilante al final del texto
        while (true)
        {
            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Muestra el cursor titilante
            string textoConCursorTitilante = mensaje;
            if (cursorVisible)
            {
                textoConCursorTitilante += "_";
            }
            textoInicioDia.text = textoConCursorTitilante;

            yield return null;
        }


        yield return new WaitForSeconds(duracionPanel);
        panelInicioDia.gameObject.SetActive(false);

        // Invoca el evento cuando el panel se desactive
        PanelInicioDesactivado?.Invoke();
    }



    public void CerrarPanelInicioDia()
    {
        // Detener la corrutina si está en ejecución
        if (panelInicioDiaCoroutine != null)
        {
            StopCoroutine(panelInicioDiaCoroutine);
            panelInicioDiaCoroutine = null;
        }

        // Detener el sonido y desactivar el panel
        audioTecleo.Stop();
        panelInicioDia.gameObject.SetActive(false);

        // Invocar el evento
        PanelInicioDesactivado?.Invoke();
    }

    public void ActualizarPanelReporte(int sanosIngresados, int enfermosIngresados, int sanosRechazados, int enfermosRechazados)
    {
        panelReporte.gameObject.SetActive(true);
        // botonSiguienteNivel.gameObject.SetActive(true);

        int diaActual = gameManager.NivelActual;
        string tituloReporte = $"Reporte Día {diaActual}\n";
        reporteText.text = $"{tituloReporte}" +
                        $"Sanos ingresados: {sanosIngresados}\n" +
                        $"Enfermos ingresados: {enfermosIngresados}\n" +
                        $"Sanos rechazados: {sanosRechazados}\n" +
                        $"Enfermos rechazados: {enfermosRechazados}";
        gameManager.MostrarMensaje();
    }

    public void PanelReporte()
    {
        panelPerdiste.gameObject.SetActive(true);
    }
}