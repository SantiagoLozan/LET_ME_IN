using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textoDialogo;
    public RectTransform panelDialogo;

    public RectTransform panelRespuestas;
    public TextMeshProUGUI textoRespuesta;

    public Button botonIngreso;
    public Button botonRechazo;
    public RectTransform panelSiguiente;

    public float velocidadTexto = 0.05f;

    private string[] lineas;
    private List<string> respuestasActuales;

    private bool mostrandoRespuestas = false;
    private bool textoCompleto = false;
    private bool esAgresivo;

    private int indexDialogo;
    private int indexRespuestas;

    public s_GameManager gameManager;
    public AudioManager audioManager;
    public AudioClip[] gibberishClips;
    public AudioClip[] gibberishClips2;

    public AggressiveNPCs aggressiveNPCs;
    public CheckCondition checkCondition;

    public bool medicoUsado = false;

    public float intervaloCursor = 0.5f; // Intervalo para el titileo del cursor
    private bool cursorVisible = true;
    private float tiempoUltimaActualizacion; // Para el control del tiempo del cursor


    void Start()
    {
        /* var imageIngreso = botonIngreso.GetComponent<Image>();
           var imageRechazo = botonRechazo.GetComponent<Image>();

           if (imageIngreso != null) {
               imageIngreso.alphaHitTestMinimumThreshold = 0.1f;
           }

           if (imageRechazo != null) {
               imageRechazo.alphaHitTestMinimumThreshold = 0.1f;
           }*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipDialogo();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaltarTodosLosDialogos();
        }
    }

    void SaltarTodosLosDialogos()
    {
        StopAllCoroutines();
         panelDialogo.gameObject.SetActive(false);
        panelRespuestas.gameObject.SetActive(false);

         if (esAgresivo)
    {
        aggressiveNPCs.MostrarComportamientoAgresivo();
    }
    else
    {
        MostrarBotonSiguiente();
    }

       

       
    }

    public void ComenzarDialogo(string[] dialogos, List<string> respuestas, bool esAgresivo)
    {
        lineas = dialogos;
        respuestasActuales = respuestas;
        this.esAgresivo = esAgresivo;

        indexDialogo = 0;
        indexRespuestas = 0;

        MostrarPanelDialogo();
    }

    void MostrarPanelRespuestas()
    {
        panelRespuestas.gameObject.SetActive(true);
        panelDialogo.gameObject.SetActive(false);
        StartCoroutine(EscribirRespuestas());
    }

    IEnumerator EscribirRespuestas()
    {
        textoRespuesta.text = "";
        if (AudioManager.instance != null)
        {
            AudioManager.instance.HablarPalabrasEnLoop(AudioManager.instance.gibberishClips);
        }

        tiempoUltimaActualizacion = Time.time; // Inicializar el tiempo del cursor

        while (textoRespuesta.text.Length < respuestasActuales[indexRespuestas].Length)
        {
            if (textoCompleto) break;

            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Construye el texto actual con el cursor
            string textoParcial = respuestasActuales[indexRespuestas].Substring(0, textoRespuesta.text.Length);
            if (cursorVisible)
            {
                textoParcial += "_";
            }
            textoRespuesta.text = textoParcial;

            yield return new WaitForSeconds(velocidadTexto);
        }

        // Asegúrate de que el texto final se muestre correctamente sin el cursor
        textoRespuesta.text = respuestasActuales[indexRespuestas];
        if (AudioManager.instance != null)
        {
            AudioManager.instance.DetenerHablar();
        }

        while (true)
        {
            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Construir el texto actual con el cursor titilante
            string textoConCursorTitilante = respuestasActuales[indexRespuestas];
            if (cursorVisible)
            {
                textoConCursorTitilante += "_";
            }
            textoRespuesta.text = textoConCursorTitilante;

            yield return null; // Espera hasta el siguiente frame

            // Salir del bucle cuando se haga clic
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
        }



        textoCompleto = false;

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        PanelRespuestasClick();
    }


    bool EstaDentroDelPanel(Vector2 posicionClic, RectTransform panel)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, posicionClic, Camera.main, out localPoint);

        localPoint.x += panel.rect.width * panel.pivot.x;
        localPoint.y += panel.rect.height * panel.pivot.y;

        return panel.rect.Contains(localPoint);
    }

    void MostrarPanelDialogo()
    {
        mostrandoRespuestas = false;
        panelDialogo.gameObject.SetActive(true);
        panelRespuestas.gameObject.SetActive(false);
        ComenzarEscritura();
    }

    void ComenzarEscritura()
    {
        StartCoroutine(EscribirLinea());
    }

    IEnumerator EscribirLinea()
    {
        textoDialogo.text = string.Empty;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.HablarPalabrasEnLoop(gibberishClips);
        }

        tiempoUltimaActualizacion = Time.time; // Inicializar el tiempo del cursor

        while (textoDialogo.text.Length < lineas[indexDialogo].Length)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textoDialogo.text = lineas[indexDialogo];
                break;
            }

            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Construye el texto actual con el cursor
            string textoParcial = lineas[indexDialogo].Substring(0, textoDialogo.text.Length);
            if (cursorVisible)
            {
                textoParcial += "_";
            }
            textoDialogo.text = textoParcial;

            yield return new WaitForSeconds(velocidadTexto);
        }

        while (true)
        {
            if (Time.time - tiempoUltimaActualizacion >= intervaloCursor)
            {
                cursorVisible = !cursorVisible;
                tiempoUltimaActualizacion = Time.time;
            }

            // Construir el texto actual con el cursor titilante
            string textoConCursorTitilante = lineas[indexDialogo];
            if (cursorVisible)
            {
                textoConCursorTitilante += "_";
            }
            textoDialogo.text = textoConCursorTitilante;

            yield return null; // Espera hasta el siguiente frame

            // Salir del bucle cuando se haga clic
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
        }



        // Asegúrate de que el texto final se muestre correctamente sin el cursor
        textoDialogo.text = lineas[indexDialogo];
        if (AudioManager.instance != null)
        {
            AudioManager.instance.DetenerHablar();
        }

        textoCompleto = false;

        mostrandoRespuestas = true;

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        PanelDialogoClick();
    }

    public void MostrarRespuestas(List<string> respuestas)
    {
        respuestasActuales = respuestas;
        MostrarPanelRespuestas();
    }

    public void PanelRespuestasClick()
    {
        if (mostrandoRespuestas)
        {
            mostrandoRespuestas = false;
            indexRespuestas++;
            MostrarPanelDialogo();
        }
    }

    public void PanelDialogoClick()
    {
        if (mostrandoRespuestas && indexRespuestas < respuestasActuales.Count)
        {
            indexDialogo++;
            MostrarPanelRespuestas();
        }
        else
        {
            if (indexDialogo == lineas.Length - 1)
            {
                panelDialogo.gameObject.SetActive(false);
                //MostrarBotonSiguiente();
                if (esAgresivo)
                {
                    // Llama al método que maneja el comportamiento agresivo
                    //  MostrarComportamientoAgresivo();
                    aggressiveNPCs.MostrarComportamientoAgresivo();
                }
                else
                {
                    MostrarBotonSiguiente();
                }
            }
        }
    }

    void MostrarBotonSiguiente()
    {
        botonIngreso.interactable = true;
        botonRechazo.interactable = true;
        panelSiguiente.gameObject.SetActive(true);

        if (!medicoUsado)
        {
            checkCondition.botonMedico.interactable = true;
        }

    }

    public void SkipDialogo()
    {
        textoCompleto = true;
    }

}
