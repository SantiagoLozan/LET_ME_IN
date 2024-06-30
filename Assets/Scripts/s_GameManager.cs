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
        //
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void NextCharacter()
    {
        // dialogueManager.botonIngreso.interactable = false;
        //  dialogueManager.botonRechazo.interactable = false;
        charactersManager.AparecerSiguientePersonaje();

    }

 public void MostrarPanelReporte()
    {
        
        uiManager.ActualizarPanelReporte(sanosIngresados, enfermosIngresados);
    }


    public void OnBotonIngresoClick()
    {
        VerificarEstadoPersonaje(CharacterState.Sano);
    }

    public void OnBotonRechazoClick()
    {
        VerificarEstadoPersonaje(CharacterState.Enfermo);
    }



    void VerificarEstadoPersonaje(CharacterState estadoSeleccionado)
    {
        Character personajeActual = charactersManager.GetCharacter(charactersManager.CurrentCharacterIndex);
        if (personajeActual != null)
        {
            if (personajeActual.estado == estadoSeleccionado)
            {
                Debug.Log("¡Elección correcta!");

                if (estadoSeleccionado == CharacterState.Sano)
                {
                    sanosIngresados++;
                }
                else if (estadoSeleccionado == CharacterState.Enfermo)
                {
                    enfermosIngresados++;
                }
            }
            else
            {
                Debug.Log("Elección incorrecta.");
                // Aquí puedes añadir lógica para lo que sucede cuando la elección es incorrecta
            }
        }

        // Avanzar al siguiente personaje
        NextCharacter();
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

