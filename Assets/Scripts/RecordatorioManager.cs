using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordatorioManager : MonoBehaviour
{
    public RectTransform panelRecordatorio;

    public void AbrirRecordatorio()
    {
        panelRecordatorio.gameObject.SetActive(true);
    }

    public void CerrarRecordatorio()
    {
        panelRecordatorio.gameObject.SetActive(false);
    }
}
