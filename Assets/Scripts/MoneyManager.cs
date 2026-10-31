using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingBalance = 100; //starting balance for the player
    [SerializeField] private int betStep = 10; //step amount to increase the bet
    [SerializeField] private TMP_Text balanceText; //balanec after the bet
    [SerializeField] private TMP_Text betText; //amount to bet text



    public int Balance { get; private set; } //to read balance 
    public int Bet {  get; private set; } //to read bet amount
    public bool IsBroke => Balance <= 0; //eval balance <= 0 & returns result to IsBroke
    public bool Locked { get; set; } //lock money changes while isSpinning

    private void Awake()
    {
        ResetMoney(); //reset money after each restart
    }

    public void ResetMoney() //reset money after game restart is requested
    {
        Balance = startingBalance;
        Bet = betStep;
        Locked = false;
        UpdateUI(); //Update UI after reset
    }

    public void IncreaseBet() //increase bet by the step amount 
    {
        if (Locked) return; //if already spinning then locked
        Bet = Mathf.Min(Bet + betStep, Balance); //return smaller val between bet & available balance
        UpdateUI();
    }

    public void DecreaseBet() //decrease bet by the step amount
    {
        if (Locked) return;
        Bet = Mathf.Max(Bet - betStep, betStep); //return maximum of the two val so it doesnt go below betStep
        UpdateUI();
    }

    public bool TakeBet() //confirm the bet & start spinning
    {
        if (Bet <= 0 || Bet > Balance) //check is bet is 0 or bet is greater than player balance
            return false;

        Balance -= Bet; 
        UpdateUI();
        return true;
    }

    public void Win(int amount) //payout winnning amount to player balance
    {
        Balance += amount;

        if (Bet > Balance)
            Bet = Balance;
        if (Bet < betStep && Balance >= betStep)
            Bet = betStep;

        UpdateUI();
    }

    private void UpdateUI() //change balance & bet text
    {
        balanceText.text = "BALANCE: " + Balance;
        betText.text = Bet + "$";
    }

    public void ClosePopup(GameObject window) //used to close win window popup
    {
        window.SetActive(false);
    }

}
