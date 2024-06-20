using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_GameManager : MonoBehaviour
{
    
     public CharactersManager CharactersManager; 

    void Start()
    {

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
       CharactersManager.AparecerSiguientePersonaje();
    }

}
