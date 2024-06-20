using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI textoDialogo;
    public RectTransform panelDialogo;

    public string[] lineas;

    public float velocidadTexto = 0.1f;

    int index;


    void Start()
    {
        textoDialogo.text = string.Empty;
        ComenzarDialogo();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && EstaDentroDelPanel(Input.mousePosition))
        {
            if (textoDialogo.text == lineas[index])
            {
                ProximaLinea();
            }
            else
            {
                StopAllCoroutines();
                textoDialogo.text = lineas[index];
            }
        }
    }
    
    bool EstaDentroDelPanel(Vector2 posicionClic)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelDialogo, posicionClic, Camera.main, out localPoint);


        localPoint.x += panelDialogo.rect.width * panelDialogo.pivot.x;
        localPoint.y += panelDialogo.rect.height * panelDialogo.pivot.y;

        return panelDialogo.rect.Contains(localPoint);
    }

    public void ComenzarDialogo()
    {
        index = 0;
        StartCoroutine(EscribirLinea());
    }

    IEnumerator EscribirLinea()
    {

        foreach (char letter in lineas[index].ToCharArray())
        {

            textoDialogo.text += letter;

            yield return new WaitForSeconds(velocidadTexto);
        }
    }

    public void ProximaLinea()
    {
        if (index < lineas.Length - 1)
        {
            index++;
            textoDialogo.text = string.Empty;
            StartCoroutine(EscribirLinea());
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

}


