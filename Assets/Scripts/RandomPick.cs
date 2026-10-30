using System.Collections;
using UnityEngine;

public class RandomPick : MonoBehaviour
{
    [SerializeField] private SlotSymbol[] slotSpace;          // the 3 slots
    [SerializeField] private float firstStopTime = 1f;
    [SerializeField] private float stopDelay = 0.5f;
    [SerializeField] private SpriteRenderer slotHandle;
    [SerializeField] private GameObject handleUp;
    [SerializeField] private GameObject handleDown;

    private bool isSpinning;

    // Hook this to a Button's OnClick
    public void Spin()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        handleUp.SetActive(false);
        handleDown.SetActive(true);

        foreach (SlotSymbol r in slotSpace) r.StartSpin();

        yield return new WaitForSeconds(firstStopTime);

        // ask the reels to stop one after another
        foreach (SlotSymbol r in slotSpace)
        {
            r.StopSpin();
            yield return new WaitForSeconds(stopDelay);
        }

        // wait until every reel has landed
        while (AnySpinning()) yield return null;

        handleUp.SetActive(true);
        handleDown.SetActive(false);

        bool win = slotSpace[0].Result == slotSpace[1].Result &&
                   slotSpace[1].Result == slotSpace[2].Result;

        Debug.Log(win ? "WIN" : "LOSE");
        // show your popup here

        isSpinning = false;
    }

    private bool AnySpinning()
    {
        foreach (SlotSymbol r in slotSpace)
            if (r.IsSpinning) return true;
        return false;
    }
}