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
    public List<Character> characters;
    public DialogueManager dialogueManager;
    public UI_Manager uiManager;

    private List<GameObject> personajesEnPantalla = new List<GameObject>();
    private int index = 0;

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
        LimpiarPersonajes();

        if (index < personajesPrefabs.Length)
        {
            GameObject nuevoPersonaje = Instantiate(personajesPrefabs[index], spawnPoint.position, Quaternion.identity);
            personajesEnPantalla.Add(nuevoPersonaje);
            MostrarDialogoPersonaje(index);
            index++;
        }
        else
        {
            Debug.Log("Todos los personajes han sido instanciados.");
        }
    }

    public void LimpiarPersonajes()
    {
        foreach (GameObject personaje in personajesEnPantalla)
        {
            Destroy(personaje);
        }
        personajesEnPantalla.Clear();
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
}