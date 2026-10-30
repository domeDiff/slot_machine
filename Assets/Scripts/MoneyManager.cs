using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingBalance = 100; //starting balance for the player
    [SerializeField] private int betStep = 10; //step amount to increase the bet
    [SerializeField] private TMP_Text balanceText; //balanec after the bet
    [SerializeField] private TMP_Text betText;

    public int Balance { get; private set; }
    public int Bet {  get; private set; }
    public bool IsBroke => Balance <= 0;
    public bool Locked { get; set; } //lock money changes while isSpinning

    private void Awake()
    {
        ResetMoney(); //reset money after each restart
    }

    public void ResetMoney() //
    {
        Balance = startingBalance;
        Bet = betStep;
        Locked = false;
        UpdateUI(); //Update UI after reset
    }

    public void IncreaseBet()
    {
        if (Locked) return; //if already spinning then locked
        Bet = Mathf.Min(Bet + betStep, Balance);
        UpdateUI();
    }

    public void DecreaseBet()
    {
        if (Locked) return;
        Bet = Mathf.Max(Bet - betStep, betStep);
        UpdateUI();
    }

    public bool TakeBet()
    {
        if (Bet <= 0 || Bet > Balance)
            return false;

        Balance -= Bet;
        UpdateUI();
        return true;
    }

    public void Win(int amount)
    {
        Balance += amount;

        if (Bet > Balance)
            Bet = Balance;
        if (Bet < betStep && Balance >= betStep)
            Bet = betStep;

        UpdateUI();
    }

    private void UpdateUI()
    {
        balanceText.text = "BALANCE: " + Balance;
        betText.text = Bet + " $";
    }


}
