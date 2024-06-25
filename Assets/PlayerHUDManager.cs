using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    public TMP_Text healthText;

    void FixedUpdate()
    {
        healthText.text=""+playerStats._currentHealth;
    }
}
