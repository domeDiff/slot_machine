using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RandomPick : MonoBehaviour
{
    [Header("SymbolSlots")]
    [SerializeField] private SlotSymbol[] slotSpace; //slotSpaces used to hold the slot symbols
    [SerializeField] private float firstStopTime = 1f; //stop duration for the first slotSymbol
    [SerializeField] private float stopDelay = 0.5f; //stop delay for the next 2 slots i.e. 2nd at 1.5 & 3rd at 2.0


    [Header("Lever")] //used to add animation to the slot handle by setting these components true/false
    [SerializeField] private GameObject handleUp; 
    [SerializeField] private GameObject handleDown;

    [Header("Money")]
    [SerializeField] private MoneyManager money; //referenc to MoneyManager to handle bets
    [SerializeField] private int[] tripleMultiplier = { 10, 2, 3, 5 }; //payout multiplier for matching symbols

    [Header("UI")]
    [SerializeField] private TMP_Text messageText; //show result
    [SerializeField] private TMP_Text winText; //win text
     [SerializeField] private GameObject gameOverPanel; //end game panel

    private bool isSpinning; //used to stop from spinning while a spin is already in place

    private void Start()
    {
        Restart(); //restart the slot machine to default setting
    }

    public void Spin()
    {
        if (isSpinning || money.IsBroke) return; //if already spinnig or broke then stop
        if (!money.TakeBet()) return; //return false if bet is invalid otherwise take bet

        StartCoroutine(SpinRoutine());
    }

    public void Restart()
    {
        money.ResetMoney(); //reset player balance to 100
        isSpinning = false; //set slot machine spin state to default
        gameOverPanel.SetActive(false); //remove the game over panel
        messageText.text = "Place Your Bet!"; 
    }

    public void QuitGame() //exit game
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true; //to set machine state in-play
        money.Locked = true; //make placing bet locked while spinning
        Debug.Log("Spinning");
        handleUp.SetActive(false); //swap handle sprite
        handleDown.SetActive(true);

        foreach (SlotSymbol s in slotSpace) s.StartSpin(); //start spinning slots

        yield return new WaitForSeconds(firstStopTime); //wait till it reaches duration of the first slot stop 

        foreach (SlotSymbol s in slotSpace)
        {
            s.StopSpin();
            yield return new WaitForSeconds(stopDelay); //wait till it reaches duration of stopDelay
        }

        while (AnySpinning()) yield return null;

        handleUp.SetActive(true);
        handleDown.SetActive(false);

        int a = slotSpace[0].Result; //result of the first slot
        int b = slotSpace[1].Result; //result of the second slot
        int c = slotSpace[2].Result; // result of the third slot
        int bet = money.Bet; //bet amount
        int payout = 0; 

        if(a == b && b ==c)
        {
            payout = bet * tripleMultiplier[a];
            winText.text = "JACKPOT! You Win " + payout + "$";
        }

        else if(a == b || b ==c || a == c)
        {
            payout = bet;
            messageText.text = "Pair! Bet Returned";
        }

        else
        {
            messageText.text = "You Lose " + bet + "$";
        }

        money.Win(payout);

        if (money.IsBroke)
        {
            messageText.text = "You are Broke!";
            gameOverPanel.SetActive(true);
        }

        money.Locked = false;
        isSpinning = false;
    }

    private bool AnySpinning() 
    { 
        foreach(SlotSymbol s in slotSpace)
        {
            if (s.IsSpinning) return true;
        }
        return false;
    }
}