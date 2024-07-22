using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Sano,
    Enfermo
}

[System.Serializable]
public class Character
{
    public string nombre;
    public CharacterState estado;
    public List<string> dialogos;
    public List<string> respuestas;
}

public class CharactersManager : MonoBehaviour
{
    public GameObject[] personajesPrefabs;
    public Transform spawnPoint;
    public Transform centerPoint;
    public Transform exitPoint;
    public List<Character> characters;
    public DialogueManager dialogueManager;
    public s_GameManager gameManager;
    public UI_Manager uiManager;

    private List<GameObject> personajesEnPantalla = new List<GameObject>();
    private int index = 0;
    public float tiempoDeEspera = 4.0f;
    public float moveDuration = 4.0f; // Duración del movimiento de deslizamiento
    public float bounceHeight = 0.2f; // Altura del rebote
    public float bounceSpeed = 2.0f; // Velocidad del rebote

    void Start()
    {
        if (uiManager != null)
        {
            // suscribirse al evento OnPanelInicioDiaDesactivado
            uiManager.PanelInicioDesactivado += AparecerSiguientePersonaje;
        }
        else
        {
            Debug.LogError("UI_Manager no está asignado en CharactersManager.");
        }
    }

    void OnDestroy()
    {
        if (uiManager != null)
        {
            // desuscribirse del evento para evitar posibles errores
            uiManager.PanelInicioDesactivado -= AparecerSiguientePersonaje;
        }
    }

    public void AparecerSiguientePersonaje()
    {
        StartCoroutine(AparecerConRetraso());
    }

    private IEnumerator AparecerConRetraso()
    {
        LimpiarPersonajes();

        dialogueManager.botonIngreso.interactable = false;
        dialogueManager.botonRechazo.interactable = false;

        yield return new WaitForSeconds(tiempoDeEspera);

        if (index < personajesPrefabs.Length)
        {
            GameObject nuevoPersonaje = Instantiate(personajesPrefabs[index], spawnPoint.position, Quaternion.identity);
            personajesEnPantalla.Add(nuevoPersonaje);
            StartCoroutine(MoverPersonajeAlCentro(nuevoPersonaje, centerPoint.position, index));
            index++;
        }
        else
        {
            Debug.Log("Todos los personajes han sido instanciados.");
            gameManager.MostrarPanelReporte();
        }
    }

    private IEnumerator MoverPersonajeAlCentro(GameObject personaje, Vector3 destino, int characterIndex)
    {
        Vector3 inicio = personaje.transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < moveDuration)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / moveDuration);

            // Movimiento de deslizamiento
            personaje.transform.position = Vector3.Lerp(inicio, destino, t);

            // Movimiento de rebote (simular caminar)
            float offsetY = Mathf.Sin(tiempoTranscurrido * bounceSpeed) * bounceHeight;
            personaje.transform.position = new Vector3(personaje.transform.position.x, destino.y + offsetY, personaje.transform.position.z);

            yield return null;
        }

        // Asegurarse de que el personaje esté exactamente en el destino final
        personaje.transform.position = destino;

        // Mostrar el diálogo del personaje
        MostrarDialogoPersonaje(characterIndex);
    }


    public void LimpiarPersonajes()
    {
        StartCoroutine(MoverPersonajesFueraDePantalla());
    }


    private IEnumerator MoverPersonajesFueraDePantalla()
    {
        List<Coroutine> salidas = new List<Coroutine>();

        foreach (GameObject personaje in personajesEnPantalla)
        {
            salidas.Add(StartCoroutine(MoverPersonajeFueraDePantalla(personaje, exitPoint.position)));
        }

        // Esperar a que todos los personajes se hayan movido fuera de la pantalla
        foreach (Coroutine salida in salidas)
        {
            yield return salida;
        }

        // Limpiar personajes después de moverlos fuera de la pantalla
        foreach (GameObject personaje in personajesEnPantalla)
        {
            Destroy(personaje);
        }
        personajesEnPantalla.Clear();
    }


    private IEnumerator MoverPersonajeFueraDePantalla(GameObject personaje, Vector3 destino)
    {
        Vector3 inicio = personaje.transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < moveDuration)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / moveDuration);

            // Movimiento de deslizamiento hacia la derecha
            personaje.transform.position = Vector3.Lerp(inicio, destino, t);

            // Movimiento de rebote (simular caminar)
            float offsetY = Mathf.Sin(tiempoTranscurrido * bounceSpeed) * bounceHeight;
            personaje.transform.position = new Vector3(personaje.transform.position.x, destino.y + offsetY, personaje.transform.position.z);

            yield return null;
        }

        // Asegurarse de que el personaje esté exactamente en el destino final
        personaje.transform.position = destino;
    }

    public void MostrarDialogoPersonaje(int characterIndex)
    {
        if (characterIndex < characters.Count)
        {
            Character character = characters[characterIndex];
            string[] dialogos = character.dialogos.ToArray();
            dialogueManager.ComenzarDialogo(dialogos, character.respuestas);
        }
        else
        {
            Debug.Log("Índice de personaje fuera de rango");
        }
    }

    public void MostrarRespuestas(List<string> respuestas)
    {
        dialogueManager.MostrarRespuestas(respuestas);
    }


    public Character GetCharacter(int index)
    {
        if (index >= 0 && index < characters.Count)
        {
            return characters[index];
        }
        Debug.LogError("Índice de personaje fuera de rango");
        return null;
    }

    public int CurrentCharacterIndex
    {
        get { return index - 1; }  
    }
}