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

    public float velocidadTexto = 0.1f;

    private string[] lineas;
    private List<string> respuestasActuales;

    private bool mostrandoRespuestas = false;
    private bool textoCompleto = false;

    private int indexDialogo;
    private int indexRespuestas;

    public s_GameManager gameManager;
    public AudioManager audioManager;
    public AudioClip[] gibberishClips;
    public AudioClip[] gibberishClips2;

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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.HablarPalabrasEnLoop(AudioManager.instance.gibberishClips);
        }

        foreach (char letter in respuestasActuales[indexRespuestas].ToCharArray())
        {
            textoRespuesta.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
            if (textoCompleto) break;
        }

        textoRespuesta.text = respuestasActuales[indexRespuestas];
        if (AudioManager.instance != null)
        {
            AudioManager.instance.DetenerHablar();
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

        foreach (char letter in lineas[indexDialogo].ToCharArray())
        {
            textoDialogo.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
            if (textoCompleto) break;
        }

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
                MostrarBotonSiguiente();
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
