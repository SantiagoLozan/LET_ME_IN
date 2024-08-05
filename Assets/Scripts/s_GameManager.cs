using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_GameManager : MonoBehaviour
{
    public UI_Manager uiManager;
    public CharactersManager charactersManager;
    public DialogueManager dialogueManager;

    public string mensajeInicioDia = "";

    public int sanosIngresados;
    public int enfermosIngresados;
    public int sanosRechazados;
    public int enfermosRechazados;

    public GameObject Musica;
    public AudioSource backgroundMusic;

    void Start()
    {
        if (uiManager != null && charactersManager != null)
        {
            uiManager.MostrarInicioDia(mensajeInicioDia);
        }
        else
        {
            Debug.LogError("UI_Manager o CharactersManager no están asignados en GameController.");
        }
    }

    void Update()
    {
        // Actualizaciones de frame si es necesario
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NextCharacter()
    {
        charactersManager.AparecerSiguientePersonaje();
    }

    public void OnBotonIngresoClick()
    {
        VerificarEstadoPersonaje(true); 
        charactersManager.MoverPersonajeAlPunto(charactersManager.exitPoint.position);
    }

    public void OnBotonRechazoClick()
    {
        VerificarEstadoPersonaje(false);
        charactersManager.MoverPersonajeAlPunto(charactersManager.spawnPoint.position);
    }

    void VerificarEstadoPersonaje(bool esIngreso)
    {
        Character personajeActual = charactersManager.GetCharacter(charactersManager.CurrentCharacterIndex);
        if (personajeActual != null)
        {
            if (esIngreso)
            {
                if (personajeActual.estado == CharacterState.Sano)
                {
                    Debug.Log("¡Elección correcta! Personaje sano ingresado.");
                    sanosIngresados++;
                }
                else if (personajeActual.estado == CharacterState.Enfermo)
                {
                    Debug.Log("¡Elección incorrecta! Personaje enfermo ingresado.");
                    enfermosIngresados++;
                }
            }
            else
            {
                if (personajeActual.estado == CharacterState.Sano)
                {
                    Debug.Log("¡Elección incorrecta! Personaje sano rechazado.");
                    sanosRechazados++;
                }
                else if (personajeActual.estado == CharacterState.Enfermo)
                {
                    Debug.Log("¡Elección correcta! Personaje enfermo rechazado.");
                    enfermosRechazados++;
                }
            }
        }

        // Avanzar al siguiente personaje
        NextCharacter();
    }

    public void MostrarPanelReporte()
    {
        uiManager.ActualizarPanelReporte(sanosIngresados, enfermosIngresados, sanosRechazados, enfermosRechazados);
    }

    public void MostrarMensaje()
    {
        // Mostrar mensaje según la cantidad de enfermos ingresados
        if (enfermosIngresados == 0)
        {
            uiManager.mensajeReporte.text = "¡Buen trabajo!";
        }
        else if (enfermosIngresados == 1)
        {
            uiManager.mensajeReporte.text = "Más cuidado la próxima vez.";
        }
        else if (enfermosIngresados >= 2)
        {
            uiManager.mensajeReporte.text = "Fuiste retirado del puesto de trabajo.";
        }
    }
}
