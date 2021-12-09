using UnityEngine;
using TMPro;

/* Enemies are defeated, and items have a chance to drop. Sometimes, a rare version of an enemy appears, which
 * allows two chances for an item to drop. The better roll is picked, and if nothing drops, the added weight bonus is
 * tripled. */
public class GameManager : MonoBehaviour
{
    public TableReader reader;
    //public Enemy enemy;
    //public Sprite playerSprite;

    public Enemy[] enemies1;    //level 1 to 10
    public Enemy[] enemies2;    //level 11 to 20
    public Enemy[] enemies3;    //level 21 to 30
    public Enemy[] enemies4;    //level 31 to 40
    public Enemy[] enemies5;    //level 41 to 50

    public AnimationClip slash;
    public Animator slashAnim;

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.
    public TextMeshProUGUI logText;     //displays drop info.

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemies1[0], new Vector3(0, 0, 0), Quaternion.identity);
        //SpriteRenderer sr = enemies1[0].GetComponentInChildren<SpriteRenderer>();
        //sr.color = new Color(1, 1, 1, 0.2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Show animation of player defeating an enemy

        //once they're defeated, roll for an item

        



        if (Time.time > currentTime + getItemTime)
        {
            //get new item
            slashAnim.Play("Slash");        //TODO: set up a coroutine for this animation so that it plays first before anything else happens
            currentTime = Time.time;
            reader.GetItem(3);

            //update log & change UI size so that scrollbar gets smaller as more text is added.
            logText.text = reader.log;
            logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
        }
    }
}
