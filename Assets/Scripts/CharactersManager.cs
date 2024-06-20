using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public GameObject[] personajesPrefabs;
    public Transform spawnPoint;

    private List<GameObject> personajesEnPantalla = new List<GameObject>();

    private int index = 0;

    void Start()
    {

        AparecerSiguientePersonaje();
    }

    public void AparecerSiguientePersonaje()
    {

        LimpiarPersonajes();

        if (index < personajesPrefabs.Length)
        {
            GameObject nuevoPersonaje = Instantiate(personajesPrefabs[index], spawnPoint.position, Quaternion.identity);
            personajesEnPantalla.Add(nuevoPersonaje);

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

        // index = 0;
    }
}