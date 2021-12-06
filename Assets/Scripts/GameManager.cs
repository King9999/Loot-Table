using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TableReader reader;
    public Enemy enemy;

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.
    public TextMeshProUGUI logText;     //displays drop info.

    // Start is called before the first frame update
    void Start()
    {
        //have a window to display log information. Window should have a scrollbar.
        
    }

    // Update is called once per frame
    void Update()
    {
        //Show animation of player defeating an enemy

        //once they're defeated, roll for an item

        //update log
        logText.text = reader.log;
        if (Time.time > currentTime + getItemTime)
        {
            //get new item
            currentTime = Time.time;
            reader.GetItem(0);
        }
    }
}
