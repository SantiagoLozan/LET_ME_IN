using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_GameManager : MonoBehaviour
{
    
   public UI_Manager uiManager;
    public CharactersManager charactersManager;
    public string mensajeInicioDia = "";

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
       charactersManager.AparecerSiguientePersonaje();
    }


}