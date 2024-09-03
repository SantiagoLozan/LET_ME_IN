using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public class BackgroundElement
    {
        public Renderer renderer; // Renderer del objeto de fondo
        public float speed; // Velocidad del movimiento del fondo
    }

    public List<BackgroundElement> backgroundElements; // Lista de elementos del fondo

    void Update()
    {
        foreach (BackgroundElement element in backgroundElements)
        {
            // Calcula el desplazamiento en el eje X basado en el tiempo y la velocidad
            float offsetX = Time.time * element.speed;

            // Aplica el desplazamiento al material del objeto
            element.renderer.material.mainTextureOffset = new Vector2(offsetX, 0);
        }
    }
}