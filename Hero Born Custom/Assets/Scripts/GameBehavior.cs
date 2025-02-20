using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    private int _playerHP = 100;
    public int MaxCoins = 5;
    public Button WinButton;
    public Button LossButton;
    public TMP_Text HealthText;
    public TMP_Text CoinText;
    public TMP_Text ProgressText;
    public TMP_Text SpeedBoostText;
    public TMP_Text JumpBoostText;
    public TMP_Text ShieldText;

    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f;
    }

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
                WinButton.gameObject.SetActive(true);
                UpdateScene("You've found all the coins!");
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
            int previousHP = _playerHP;  // store the old HP
            _playerHP = value;
            HealthText.text = "Health: " + _playerHP;
            if (_playerHP <= 0)
            {
                LossButton.gameObject.SetActive(true);
                UpdateScene("You want another life with that?");
            }
            else if (_playerHP < previousHP)
            {
                ProgressText.text = "Ouch... that's got to hurt.";
            }
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }

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

    public void ShowShield(bool isActive)
    {
        if (ShieldText != null)
        {
            ShieldText.gameObject.SetActive(isActive);
        }
    }
}