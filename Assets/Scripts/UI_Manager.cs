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

public RectTransform indicaciones1; 
public RectTransform indicaciones2;  
public RectTransform indicaciones3; 

public float duracionIndicaciones = 2.5f;

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

        panelInicioDiaCoroutine = StartCoroutine(MostrarPanelInicioDiaCoroutine(titulo + mensaje));
         
    PanelInicioDesactivado += MostrarPanelIndicaciones;
}

public void MostrarPanelIndicaciones()
{
    
    if (gameManager.NivelActual == 1)
    {
        StartCoroutine(MostrarPanelIndicacionesCoroutine());
        PanelInicioDesactivado -= MostrarPanelIndicaciones;
    }
}

    public IEnumerator MostrarPanelInicioDiaCoroutine(string mensaje)
    {
        textoInicioDia.text = "";
        string mensajeConCursor = mensaje + "_";

        audioTecleo.Play();
        tiempoUltimaActualizacion = Time.time;

      
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

       
        while (true)
        {
            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

         
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

        PanelInicioDesactivado?.Invoke();
    }



    public void CerrarPanelInicioDia()
    {
        if (panelInicioDiaCoroutine != null)
        {
            StopCoroutine(panelInicioDiaCoroutine);
            panelInicioDiaCoroutine = null;
        }

        audioTecleo.Stop();
        panelInicioDia.gameObject.SetActive(false);

        PanelInicioDesactivado?.Invoke();
    }


private IEnumerator MostrarPanelIndicacionesCoroutine()
{
    yield return StartCoroutine(MostrarIndicacionesSecuencia());
    PanelInicioDesactivado -= MostrarPanelIndicaciones;
    yield break;
}


private IEnumerator MostrarIndicacionesSecuencia()
{
    indicaciones1.gameObject.SetActive(true);
    yield return new WaitForSeconds(duracionIndicaciones);
    indicaciones1.gameObject.SetActive(false);
   
    indicaciones2.gameObject.SetActive(true);
    yield return new WaitForSeconds(duracionIndicaciones);
    indicaciones2.gameObject.SetActive(false);

    indicaciones3.gameObject.SetActive(true);
    yield return new WaitForSeconds(duracionIndicaciones);
    indicaciones3.gameObject.SetActive(false);
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