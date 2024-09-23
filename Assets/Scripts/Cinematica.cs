using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cinematica : MonoBehaviour
{
    public Image imagen; // Referencia al componente Image
    public TextMeshProUGUI texto;   // Referencia al componente Text
    public Sprite[] imagenes; // Array de sprites para las imágenes
    public string[] textos;   // Array de textos para mostrar
    public AudioClip[] audios; // Array de clips de audio
    public float duracionPorImagen = 5f; // Duración de cada imagen en segundos
    public float velocidadDeTipeo = 0.05f; // Velocidad de tipeo del texto
    private AudioSource audioSource; // Referencia al componente AudioSource

    private void Start()
    {
        // Obtén el componente AudioSource adjunto al GameObject
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un componente AudioSource en el GameObject.");
            return;
        }

        StartCoroutine(ReproducirCinematica());
    }

    private IEnumerator ReproducirCinematica()
    {
        // Asegúrate de que los arrays de imágenes, textos y audios tengan la misma longitud
        if (imagenes.Length != textos.Length || imagenes.Length != audios.Length)
        {
            Debug.LogError("El número de imágenes, textos y audios no coincide.");
            yield break;
        }

        for (int i = 0; i < imagenes.Length; i++)
        {
            // Cambia la imagen
            imagen.sprite = imagenes[i];

            // Reproduce el audio correspondiente
            audioSource.clip = audios[i];
            audioSource.Play();

            // Tipea el texto letra por letra
            yield return StartCoroutine(TipearTexto(textos[i]));

            // Espera la duración de la imagen
            yield return new WaitForSeconds(duracionPorImagen);
        }
    }

    private IEnumerator TipearTexto(string textoCompleto)
    {
        texto.text = ""; // Limpia el texto antes de empezar
        foreach (char letra in textoCompleto.ToCharArray())
        {
            texto.text += letra; // Añade una letra al texto
            yield return new WaitForSeconds(velocidadDeTipeo); // Espera antes de añadir la siguiente letra
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
