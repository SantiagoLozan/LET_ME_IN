using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_GameManager : MonoBehaviour
{
    public UI_Manager uiManager;
    public CharactersManager charactersManager;
    public DialogueManager dialogueManager;

    public string[] mensajesInicioDia; 

    public int sanosIngresados;
    public int enfermosIngresados;
    public int sanosRechazados;
    public int enfermosRechazados;

    public GameObject Musica;
    public AudioSource backgroundMusic;

    private int totalEnfermos;
    public int NivelActual { get; private set; }


    void Start()
    {
       NivelActual = GameData.NivelActual;


          if (uiManager != null && charactersManager != null)
        {
            string mensajeInicio = ObtenerMensajeInicioParaNivel(NivelActual);
            uiManager.MostrarInicioDia(mensajeInicio);
        }
        else
        {
            Debug.LogError("UI_Manager o CharactersManager no están asignados en GameController.");
        }
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
            uiManager.botonSiguienteNivel.gameObject.SetActive(true);
        }
        else if (enfermosIngresados == 1)
        {
            uiManager.mensajeReporte.text = "Más cuidado la próxima vez.";
             uiManager.botonSiguienteNivel.gameObject.SetActive(true);
        }
        else if (enfermosIngresados >= 2)
        {
            uiManager.mensajeReporte.text = "Fuiste retirado del puesto de trabajo.";
        }
    }

    public void OnBotonSiguienteNivel()
    {
        Debug.Log("Botón Siguiente Nivel presionado");
        NivelActual++;
        GameData.NivelActual = NivelActual; // Guardar el nivel actual en la clase GameData
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private string ObtenerMensajeInicioParaNivel(int nivel)
    {
        // Asegurarse de que el índice esté dentro del rango del array
        if (nivel - 1 >= 0 && nivel - 1 < mensajesInicioDia.Length)
        {
            return mensajesInicioDia[nivel - 1];
        }
        else
        {
            return "Mensaje de inicio no definido para este nivel.";
        }
    }
}
