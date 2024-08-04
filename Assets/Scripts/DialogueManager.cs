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
    public Button botonRechazo;  // Referencia al botón de "Siguiente"
    public RectTransform panelSiguiente;

    public float velocidadTexto = 0.1f;

    private string[] lineas;
    private List<string> respuestasActuales;

    private bool mostrandoRespuestas = false;
    private bool textoCompleto = false;

    private int indexDialogo;
    private int indexRespuestas;

    public s_GameManager gameManager;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipDialogo();
        }
    }

    public void ComenzarDialogo(string[] dialogos, List<string> respuestas)
    {
        lineas = dialogos;
        respuestasActuales = respuestas;

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

        foreach (char letter in respuestasActuales[indexRespuestas].ToCharArray())
        {
            textoRespuesta.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
            if (textoCompleto) break;
        }

        textoRespuesta.text = respuestasActuales[indexRespuestas]; // Completar el texto

        textoCompleto = false; // Resetear flag

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

        if (indexDialogo >= lineas.Length)
        {
            yield break; // Salir de la coroutine si el índice está fuera de los límites
        }

        foreach (char letter in lineas[indexDialogo].ToCharArray())
        {
            textoDialogo.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
            if (textoCompleto) break;
        }

        textoDialogo.text = lineas[indexDialogo]; // Completar el texto

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
            if (indexRespuestas < respuestasActuales.Count)
            {
                MostrarPanelDialogo();
            }
            else
            {
                panelRespuestas.gameObject.SetActive(false);
                panelDialogo.gameObject.SetActive(false);
                MostrarBotonSiguiente(); // Llamar al método para mostrar el botón de "Siguiente"
            }
        }
    }

    public void PanelDialogoClick()
    {
        if (mostrandoRespuestas && indexRespuestas < respuestasActuales.Count)
        {
            indexDialogo++;
            if (indexDialogo < lineas.Length)
            {
                MostrarPanelRespuestas();
            }
            else
            {
                panelDialogo.gameObject.SetActive(false);
                MostrarBotonSiguiente(); // Llamar al método para mostrar el botón de "Siguiente"
            }
        }
    }

    void MostrarBotonSiguiente()
    {
        botonIngreso.interactable = true;
        botonRechazo.interactable = true;
        panelSiguiente.gameObject.SetActive(true);
    }

    public void SkipDialogo()
    {
        textoCompleto = true;
    }
}

