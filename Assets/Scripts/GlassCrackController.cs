using System.Collections;
using UnityEngine;

public class GlassCrackController : MonoBehaviour
{
    public GameObject[] crackPrefabs;
    public float delayBetweenCracks = 1.0f;

    //private int currentCrackIndex = 0;

    void Start()
    {

        foreach (GameObject crack in crackPrefabs)
        {
            crack.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void StartCracking()
    {
        Debug.Log("StartCracking llamado");
        gameObject.SetActive(true);
        StartCoroutine(ShowCracksOneByOne());
    }

    private IEnumerator ShowCracksOneByOne()
    {
        foreach (GameObject crack in crackPrefabs)
        {
            if (crack != null)
            {
                crack.SetActive(true);
                yield return new WaitForSeconds(delayBetweenCracks);
            }
        }
    }
}