using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    private int _playerHP = 100;
    public int MaxCoins = 3;
    public Button WinButton;

    public TMP_Text HealthText;
    public TMP_Text CoinText;
    public TMP_Text ProgressText;
    public TMP_Text SpeedBoostText;
    public TMP_Text JumpBoostText;


    void Start()
    {
        CoinText.text += _coinsCollected;
        HealthText.text += _playerHP;
    }

    private int _coinsCollected = 0;
    public int Coins
    {
        get { return _coinsCollected; }
        set
        {
            _coinsCollected = value;
            CoinText.text = "Coins: " + Coins;
            if (_coinsCollected >= MaxCoins)
            {
                ProgressText.text = "You've found all the coins!";
                WinButton.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                ProgressText.text = "Coin found, only " + (MaxCoins - _coinsCollected) + " more to go!";
            }
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public int HP
    {
        get { return _playerHP; }
        set
        {
            _playerHP = value;
            HealthText.text = "Health: " + HP;
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }

    // Method to show/hide Boost Text
    public void ShowSpeedBoost(bool isActive)
    {
        if (SpeedBoostText != null)
        {
            SpeedBoostText.gameObject.SetActive(isActive);
        }
    }
    public void ShowJumpBoost(bool isActive)
    {
        if (JumpBoostText != null)
        {
            JumpBoostText.gameObject.SetActive(isActive);
        }
    }

}