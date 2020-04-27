using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text hpDisplay;
    public int maxHP = 10;

    private int currentHP;
    public int CurrentHP
    {
        get { return currentHP; }
        private set
        {
            currentHP = value;
            hpDisplay.text = "HP:" + currentHP.ToString() + "/" + maxHP.ToString();
        }
    }

    public void Init()
    {
        CurrentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        CurrentHP -= dmg;
        if (CurrentHP <= 0)
        {
            Player script = GetComponent<Player>();
            script.SetPlayerState(Player.PlayerState.Die);
        }
    }
}
