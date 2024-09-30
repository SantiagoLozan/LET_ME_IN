using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class VoiceRecognitionManager : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords;

    public s_GameManager gameManager; 

    void Start()
    {
        
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("si", OnPermitir);
        keywords.Add("no", OnDenegar);

        
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action action;
        if (keywords.TryGetValue(args.text, out action))
        {
            action.Invoke();
        }
    }

    void OnPermitir()
    {
        gameManager.OnBotonIngresoClick(); 
    }

    void OnDenegar()
    {
        gameManager.OnBotonRechazoClick(); 
    }

    public void ActivarReconocimientoVoz()
    {
        if (keywordRecognizer != null && !keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start();
            Debug.Log("Reconocimiento de voz activado.");
        }
    }

    public void DesactivarReconocimientoVoz()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            Debug.Log("Reconocimiento de voz desactivado.");
        }
    }

    void OnDisable()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.OnPhraseRecognized -= OnKeywordsRecognized;
            keywordRecognizer.Stop();
        }
    }
}