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
    public TMP_Text CoinText;
    public TMP_Text ProgressText;
    public TMP_Text SpeedBoostText;
    public TMP_Text JumpBoostText;
    public TMP_Text ShieldText;

    // References to the new HP UI elements in your prefab.
    public TMP_Text HPText;    // Displays the player's HP (e.g., "100")
    public Image HPBar;        // Filled image that represents the HP bar.

    private int _coinsCollected = 0;
    public int Coins
    {
        get { return _coinsCollected; }
        set
        {
            _coinsCollected = value;
            CoinText.text = "Coins: " + _coinsCollected;
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
            int previousHP = _playerHP;
            _playerHP = value;

            // Update the HP text to match the current HP value.
            if (HPText != null)
            {
                HPText.text = _playerHP.ToString();
            }

            // Update the HP bar fill amount (0 when HP=0, 1 when HP=100).
            if (HPBar != null)
            {
                HPBar.fillAmount = (float)_playerHP / 100f;
            }

            // Check if player HP has reached zero or has decreased.
            if (_playerHP <= 0)
            {
                LossButton.gameObject.SetActive(true);
                UpdateScene("You want another life with that?");
            }
            else if (_playerHP < previousHP)
            {
                ProgressText.text = "Ouch... that's got to hurt.";
            }
            Debug.LogFormat("Health: {0}", _playerHP);
        }
    }

    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f;
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

    void Start()
    {
        // Initialize coins text and HP UI in Start.
        CoinText.text = "Coins: " + _coinsCollected;

        if (HPText != null)
        {
            HPText.text = _playerHP.ToString();
        }
        if (HPBar != null)
        {
            HPBar.fillAmount = (float)_playerHP / 100f;
        }
    }
}
