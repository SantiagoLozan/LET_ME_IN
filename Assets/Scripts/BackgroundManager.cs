using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundElement
    {
        public Renderer renderer;
        public float speed;
    }

    public List<BackgroundElement> backgroundElements;

    void Update()
    {
        foreach (BackgroundElement element in backgroundElements)
        {

            float offsetX = Time.time * element.speed;


            element.renderer.material.mainTextureOffset = new Vector2(offsetX, 0);
        }
    }
}