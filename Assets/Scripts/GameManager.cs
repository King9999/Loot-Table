using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TableReader reader;
    public Enemy enemy;
    public Sprite playerSprite;

    public Enemy[] enemies1;    //level 1 to 10

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.
    public TextMeshProUGUI logText;     //displays drop info.

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemies1[0], new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Show animation of player defeating an enemy

        //once they're defeated, roll for an item

      

        

        if (Time.time > currentTime + getItemTime)
        {
            //get new item
            currentTime = Time.time;
            reader.GetItem(3);

            //update log & change UI size so that scrollbar gets smaller as more text is added.
            logText.text = reader.log;
            logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
        }
    }
}
