using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SlotSymbol : MonoBehaviour
{
    [SerializeField] private Sprite[] symbols; //the 4 symbol sprites
    [SerializeField] private float spacing = 1.5f; //distance between each symbol;
    [SerializeField] private float speed = 12f; //fall down and recycle speed for symbls to give it moving down look


    private SpriteRenderer[] slots; //sprite renderer compoent for each individual slot;
    private int[] Ids;

    private bool stopping;

    public bool IsSpinning {  get; private set; } //return spin state
    public int Result {  get; private set; } // for middle row symbols

    private void Awake()
    {
        slots = GetComponentsInChildren<SpriteRenderer>();
        Ids = new int[slots.Length];

        for(int i=0; i<slots.Length; i++) //set initial position for slots
        {
            float y = (slots.Length / 2 - i) * spacing;
            slots[i].transform.localPosition = new Vector3(0, y, 0); //positons are +2, +1, 0, -1, -2
            SetSymbol(i, Random.Range(0, symbols.Length)); //give each slot random starting symbol 
        }
    }

    private void SetSymbol(int slot, int id)
    {
        Ids[slot] = id; 
        slots[slot].sprite = symbols[id]; //set slot sprite to selected symbol
    }

    public void StartSpin() //set spin status to start
    {
        IsSpinning = true;
        stopping = false;
    }

    public void StopSpin() //set spin status to stop
    {
        stopping = true;
    }

    private void Update()
    {
        if (!IsSpinning) return;

        float move = speed * Time.deltaTime; //falling speed
        float bottomLimit = -(slots.Length / 2 + 0.5f) * spacing; //recylce line for slots
        int landedSlot = -1; //no slot stopped yet

        for(int i = 0; i < slots.Length; i++)
        {
            Transform t = slots[i].transform;
            float oldY = t.localPosition.y;
            float newY = oldY - move;

            if (stopping && oldY > 0 && newY <= 0) landedSlot = i;

            if(newY < bottomLimit)
            {
                newY += slots.Length * spacing;
                SetSymbol(i, Random.Range(0, symbols.Length));
            }

            t.localPosition = new Vector3(0, newY, 0);
        }

        if(landedSlot >= 0)
        {
            float shift = -slots[landedSlot].transform.localPosition.y;
            foreach(var s in slots)
                s.transform.localPosition += new Vector3(0, shift, 0);

            Result = Ids[landedSlot];
            IsSpinning = false;
        }
    }
}
