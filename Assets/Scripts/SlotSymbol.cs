using UnityEngine;

public class SlotSymbol : MonoBehaviour
{
    [SerializeField] private Sprite[] symbols;      // the 4 symbol sprites
    [SerializeField] private float spacing = 1.5f;  // distance between symbols
    [SerializeField] private float speed = 12f;

    private SpriteRenderer[] slots;
    private int[] ids;
    private bool stopping;

    public bool IsSpinning { get; private set; }
    public int Result { get; private set; }         // symbol in the middle row

    private void Awake()
    {
        slots = GetComponentsInChildren<SpriteRenderer>();
        ids = new int[slots.Length];

        // stack them: top to bottom (+2, +1, 0, -1, -2) x spacing
        for (int i = 0; i < slots.Length; i++)
        {
            float y = (slots.Length / 2 - i) * spacing;
            slots[i].transform.localPosition = new Vector3(0, y, 0);
            SetSymbol(i, Random.Range(0, symbols.Length));
        }
    }

    private void SetSymbol(int slot, int id)
    {
        ids[slot] = id;
        slots[slot].sprite = symbols[id];
    }

    public void StartSpin()
    {
        IsSpinning = true;
        stopping = false;
    }

    public void StopSpin()
    {
        stopping = true;
    }

    private void Update()
    {
        if (!IsSpinning) return;

        float move = speed * Time.deltaTime;
        float bottomLimit = -(slots.Length / 2 + 0.5f) * spacing;
        int landedSlot = -1;

        for (int i = 0; i < slots.Length; i++)
        {
            Transform t = slots[i].transform;
            float oldY = t.localPosition.y;
            float newY = oldY - move;

            // this symbol just crossed the middle while we want to stop
            if (stopping && oldY > 0 && newY <= 0) landedSlot = i;

            // fell off the bottom -> back to the top with a new random symbol
            if (newY < bottomLimit)
            {
                newY += slots.Length * spacing;
                SetSymbol(i, Random.Range(0, symbols.Length));
            }

            t.localPosition = new Vector3(0, newY, 0);
        }

        // snap everything so the symbols line up perfectly, then stop
        if (landedSlot >= 0)
        {
            float shift = -slots[landedSlot].transform.localPosition.y;
            foreach (var s in slots)
                s.transform.localPosition += new Vector3(0, shift, 0);

            Result = ids[landedSlot];
            IsSpinning = false;
        }
    }
}
