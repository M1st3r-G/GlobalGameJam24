using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreenController : MonoBehaviour
{
    //ComponentReferences
    [SerializeField] private Image[] winnerImages;
    private CanvasGroup group;
    private VideoPlayer vid;
    //Params
    //Temps
    //Public

    private void Awake() {
        group = GetComponent<CanvasGroup>();
        vid = GetComponentInChildren<VideoPlayer>();
    }

    public void SetScreen(List<PlayerInput> players) {
        group.blocksRaycasts = group.interactable = true;        
        group.alpha = 1f;
        vid.Play();
        players.Reverse();
        for (int i = 0; i < players.Count; i++)
        {
            PlayerInput p = players[i];
            winnerImages[i].gameObject.SetActive(true);
            winnerImages[i].sprite = p.GetComponent<PlayerController>().GetCharacter().WinSprite;
        }
    }
}