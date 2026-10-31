
using UnityEngine;

public class SlotSymbol : MonoBehaviour
{
    [SerializeField] private Sprite[] symbols; //the 4 symbol sprites
    [SerializeField] private float spacing = 1.5f; //distance between each symbol;
    [SerializeField] private float speed = 12f; //fall down and recycle speed for symbls to give it moving down look



    private SpriteRenderer[] slots; //sprite renderer compoent for each individual slot;
    private int[] ids; //array to store id associated with each sprite

    private bool stopping;

    public bool IsSpinning {  get; private set; } //return spin state
    public int Result {  get; private set; } // for middle row symbols

    private void Awake()
    {
        slots = GetComponentsInChildren<SpriteRenderer>(); //retrieve all SpriteRender component of children of slotSpace
        ids = new int[slots.Length]; //create int array of size of slots

        for(int i=0; i<slots.Length; i++) //set initial position for slots
        {
            float y = (slots.Length / 2 - i) * spacing; //calculate vertical postion of slots
            slots[i].transform.localPosition = new Vector3(0, y, 0); //set children position with coresponding parent position
            SetSymbol(i, Random.Range(0, symbols.Length)); //give each slot random starting symbol 
        }
    }

    private void SetSymbol(int slot, int id)
    {
        ids[slot] = id; 
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
            Transform t = slots[i].transform; //transform of current symbol
            float oldY = t.localPosition.y; //old location of the symbol
            float newY = oldY - move; //calculate where to move

            if (stopping && oldY > 0 && newY <= 0) landedSlot = i; //if stop requested then calculat which symbol to select as landed

            if(newY < bottomLimit) //recycle symbols
            {
                newY += slots.Length * spacing;
                SetSymbol(i, Random.Range(0, symbols.Length));
            }

            t.localPosition = new Vector3(0, newY, 0); //set the recyled symbol to top
        }

        if(landedSlot >= 0) //calulate the aligment shift to reach 0 pos
        {
            float shift = -slots[landedSlot].transform.localPosition.y;
            foreach(var s in slots)
                s.transform.localPosition += new Vector3(0, shift, 0); //move other symbols by the same amount up or down

            Result = ids[landedSlot]; //return id of landed slot
            IsSpinning = false; //stop spinning so update call stops
        }
    }


}
