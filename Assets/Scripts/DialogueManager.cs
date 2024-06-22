using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textoDialogo;
    public RectTransform panelDialogo;

    public RectTransform panelRespuestas;
    public TextMeshProUGUI textoRespuesta;

    public float velocidadTexto = 0.1f;

    private string[] lineas;
    private List<string> respuestasActuales;

    private bool mostrandoRespuestas = false;

    private int indexDialogo;
    private int indexRespuestas;

    void Start()
    {

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
        }

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && EstaDentroDelPanel(Input.mousePosition, panelRespuestas));
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
        panelRespuestas.gameObject.SetActive(false);
        panelDialogo.gameObject.SetActive(true);

        ComenzarEscritura();
    }

    void ComenzarEscritura()
    {
        StartCoroutine(EscribirLinea());
    }

    IEnumerator EscribirLinea()
    {
        textoDialogo.text = string.Empty;
        foreach (char letter in lineas[indexDialogo].ToCharArray())
        {
            textoDialogo.text += letter;
            yield return new WaitForSeconds(velocidadTexto);
        }

        mostrandoRespuestas = true;

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && EstaDentroDelPanel(Input.mousePosition, panelDialogo));
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
        if (mostrandoRespuestas && indexRespuestas < respuestasActuales.Count )
        {
            indexDialogo++;
            MostrarPanelRespuestas();
        }
        else
         {
            if (indexDialogo == lineas.Length - 1)
            {
                panelDialogo.gameObject.SetActive(false);
            }
          }
    }
}


