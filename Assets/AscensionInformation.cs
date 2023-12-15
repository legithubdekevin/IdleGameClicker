using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AscensionInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private GameObject ascensionInformationText;

    public void Start() {
        ascensionInformationText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ascensionInformationText.gameObject.SetActive(true);   
    }

    public void OnPointerExit(PointerEventData eventData) {
        ascensionInformationText.gameObject.SetActive(false);
    }

}
