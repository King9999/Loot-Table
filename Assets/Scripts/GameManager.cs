using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TableReader reader;
    public Enemy enemy;

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > currentTime + getItemTime)
        {
            //get new item
            currentTime = Time.time;
            reader.GetItem(0);
        }
    }
}
