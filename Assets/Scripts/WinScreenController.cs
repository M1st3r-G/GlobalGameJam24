using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreenController : MonoBehaviour
{
    //ComponentReferences
    [SerializeField] private Image[] winnerImages;
    private CanvasGroup group;
    //Params
    //Temps
    //Public

    private void Awake() {
        group = GetComponent<CanvasGroup>();
    }

    public void SetScreen(List<PlayerInput> players) {
        group.blocksRaycasts = group.interactable = true;        
        group.alpha = 1f;
        players.Reverse();
        for (int i = 0; i < players.Count; i++)
        {
            PlayerInput p = players[i];
            winnerImages[i].gameObject.SetActive(true);
            winnerImages[i].sprite = p.GetComponent<PlayerController>().GetCharacter().WinSprite;
        }
    }
}