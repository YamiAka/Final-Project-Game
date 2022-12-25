using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public GameObject winHud;
    public GameObject diedHud;
    public GameObject gameHud;
    private bool isDead;

    

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10);
        }

        if (currentHealth <= 0)
        {
            diedHud.SetActive(true);
            gameHud.SetActive(false);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.gameObject.tag == "WhatIsAcid")
        {
            TakeDamage(100);
        }

        if (col.transform.gameObject.tag == "Victory")
        {
            winHud.SetActive(true);
            gameHud.SetActive(false);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
