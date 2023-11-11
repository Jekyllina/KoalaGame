using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float health = 3;
    private float maxHealth;

    [Header("Skins")]
    [SerializeField] private SkinManager skinManager;
    public SkinManager SkinManager => skinManager;
    [SerializeField] private string baseSkinID = "BaseSkin";

    public static UnityAction OnGetDamage;

    private void Start()
    {
        maxHealth = health;
        skinManager.SetSkin(baseSkinID);
    }

    #region Health
    public void GetDamage(float dmg)
    {
        health -= 1;

        if (health <= 0)
        {
            health = 0;
            GameOver();
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Heal(int heal)
    {
        health += 1;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private void GameOver()
    {
        Debug.Log("You're Dead");
    }
    #endregion
}
