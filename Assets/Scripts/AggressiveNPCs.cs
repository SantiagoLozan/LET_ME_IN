using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AggressiveNPCs : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public s_GameManager gameManager;
    public CharactersManager charactersManager;
    public GameObject panelPerdiste;
    public GameObject seguridadPrefab;
    public Transform spawnPointSeguridad;
    public Transform targetPoint;

    private GameObject seguridadInstance;

    private float tiempoRestante;
    private bool temporizadorActivo = false;
    public GameObject PanelSeguridad;
    public GameObject PanelTimer;
    private Coroutine toggleCoroutine;
    public AudioSource audioSeguridad;
    public AudioSource pasosSeguridad;
    public AudioSource escobaSeguridad;
    public AudioSource golpe;

    public GameObject filtroVidrio;

    public Button botonSeguridad;

    public AudioSource sonidoBoton;

    void Start()
    {
        audioSeguridad.Stop();
        pasosSeguridad.Stop();
        escobaSeguridad.Stop();
        botonSeguridad.interactable = false;
    }

    void Update()
    {
        if (temporizadorActivo)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                temporizadorActivo = false;
                ActualizarTextoTemporizador();
                FinTemporizador();
            }
            else
            {
                ActualizarTextoTemporizador();
            }
        }
    }

    public void MostrarComportamientoAgresivo()
    {
        Debug.Log("¡El personaje está actuando de manera agresiva!");
        botonSeguridad.interactable = true;
        StartTimer(5);
        Peligro();
    }

    void StartTimer(float tiempo)
    {
        PanelTimer.SetActive(true);
        tiempoRestante = tiempo;
        temporizadorActivo = true;
        ActualizarTextoTemporizador();
    }

    void ActualizarTextoTemporizador()
    {
        if (timerText != null)
        {
            timerText.text = $"{tiempoRestante:F1}";
        }
    }

    void FinTemporizador()
    {
        DetenerPeligro();
        panelPerdiste.SetActive(true);
    }

    public void Peligro()
    {
        if (!PanelSeguridad.activeInHierarchy)
        {
            if (toggleCoroutine == null)
            {
                toggleCoroutine = StartCoroutine(TogglePanel());
            }

            GameObject personajeAgresivoActual = charactersManager.GetCharacterGameObject();
            if (personajeAgresivoActual != null)
            {
                NPCAnimationController animController = personajeAgresivoActual.GetComponent<NPCAnimationController>();
                animController?.StartDangerAnimation();
            }


            if (filtroVidrio != null)
            {
                filtroVidrio.SetActive(true);


                GlassCrackController crackController = filtroVidrio.GetComponent<GlassCrackController>();
                if (crackController != null)
                {
                    Debug.Log("GlassCrackController encontrado y StartCracking será llamado.");
                    crackController.StartCracking();
                }
                else
                {
                    Debug.LogError("GlassCrackController no se encontró en FiltroVidrio.");
                }
            }
            else
            {
                Debug.LogError("FiltroVidrio no está asignado en el inspector.");
            }
        }
    }
    IEnumerator TogglePanel()
    {
        while (true)
        {
            PanelSeguridad.SetActive(true);
            audioSeguridad.Play();

            yield return new WaitForSeconds(0.5f);

            PanelSeguridad.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DetenerPeligro()
    {
        if (toggleCoroutine != null)
        {
            StopCoroutine(toggleCoroutine);
            toggleCoroutine = null;
            PanelSeguridad.SetActive(false);
            PanelTimer.SetActive(false);
            audioSeguridad.Stop();
        }


        GameObject personajeAgresivoActual = charactersManager.GetCharacterGameObject();
        if (personajeAgresivoActual != null)
        {
            NPCAnimationController animController = personajeAgresivoActual.GetComponent<NPCAnimationController>();
            animController?.StopDangerAnimation();
        }
    }

    public void LlamarSeguridad()
    {
        sonidoBoton.Play();
        DetenerPeligro();
        botonSeguridad.interactable = false;
        if (temporizadorActivo)
        {
            temporizadorActivo = false;
            timerText.gameObject.SetActive(false);
        }


        StartCoroutine(EsperarAntesDeLlamarSeguridad(3f));
    }

    IEnumerator EsperarAntesDeLlamarSeguridad(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(gameManager.AbrirPuerta(10f));


        seguridadInstance = Instantiate(seguridadPrefab, spawnPointSeguridad.position, Quaternion.identity);

        pasosSeguridad.Play();
        escobaSeguridad.Play();


        GameObject personajeAgresivoActual = charactersManager.GetCharacterGameObject();

        if (personajeAgresivoActual != null)
        {

            yield return new WaitForSeconds(1f);


            StartCoroutine(EmpujarPersonajeAggressivo(personajeAgresivoActual.transform));
        }
    }

    IEnumerator EmpujarPersonajeAggressivo(Transform personajeAggressivo)
    {
        float distanciaSeguridadYAgresivo = 0.1f;
        float velocidadMovimiento = 2f;
        float velocidadSeguridad = 3.5f;


        Vector3 puntoFueraDePantalla = new Vector3(targetPoint.position.x - 1f, personajeAggressivo.position.y, personajeAggressivo.position.z);

        while (Vector3.Distance(personajeAggressivo.position, puntoFueraDePantalla) > 0.1f)
        {

            personajeAggressivo.position = Vector3.MoveTowards(personajeAggressivo.position, puntoFueraDePantalla, Time.deltaTime * velocidadMovimiento);


            if (seguridadInstance != null)
            {
                Vector3 posicionSeguridad = personajeAggressivo.position - new Vector3(distanciaSeguridadYAgresivo, 0, 0);
                seguridadInstance.transform.position = Vector3.MoveTowards(seguridadInstance.transform.position, posicionSeguridad, Time.deltaTime * velocidadSeguridad);
            }

            yield return null;
        }


        if (personajeAggressivo != null)
        {
            Destroy(personajeAggressivo.gameObject);
        }


        if (seguridadInstance != null)
        {
            Destroy(seguridadInstance);
        }

        pasosSeguridad.Stop();
        escobaSeguridad.Stop();
        golpe.Play();

        yield return new WaitForSeconds(1f);
        charactersManager.AparecerSiguientePersonaje();
    }
}
