using UnityEngine;
using TMPro;
using System.Collections;

/* Enemies are defeated, and items have a chance to drop. Sometimes, a rare version of an enemy appears, which
 * allows two chances for an item to drop. The better roll is picked, and if nothing drops, the added weight bonus is
 * tripled. */
public class GameManager : MonoBehaviour
{
    public TableReader reader;
    public Enemy enemy;
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
    const float initPosition = 1;
    bool enemySpawned;

    // Start is called before the first frame update
    void Start()
    {
        //enemy = Instantiate(enemies1[0], new Vector3(initPosition, 0, 0), Quaternion.identity);
        //SpriteRenderer sr = enemies1[0].GetComponentInChildren<SpriteRenderer>();
        //sr.color = new Color(1, 1, 1, 0.2f);
        enemySpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy appears and shifts to the centre of the screen. Use coroutine
        if (!enemySpawned)
        {
            StartCoroutine(SpawnEnemy(enemies1));
        }
        else
        {
            StartCoroutine(DestroyEnemy());
            /*if (Time.time > currentTime + getItemTime)
            {
                //get new item
                slashAnim.Play("Slash");        //TODO: set up a coroutine for this animation so that it plays first before anything else happens
                currentTime = Time.time;
                reader.GetItem(3);

                //update log & change UI size so that scrollbar gets smaller as more text is added.
                logText.text = reader.log;
                logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
            }*/
        }
    }

    IEnumerator SpawnEnemy(Enemy[] enemyGroup)
    {
        if (enemy == null)
        {
            enemy = Instantiate(enemyGroup[0], new Vector3(initPosition, 0, 0), Quaternion.identity);
        }
        else
        {
            //reset its position
            enemy.transform.position = new Vector3(initPosition, 0, 0);

            //change the sprite of the existing enemy object
            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
            sr.enabled = true;           
        }

        while (enemy.transform.position.x > 0)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x - 0.05f, enemy.transform.position.y, 0);
            yield return null;
        }

        enemySpawned = true;
    }

    //removes on screen enemy and then performs a check for loot
    IEnumerator DestroyEnemy()
    {
        slashAnim.Play("Slash");

        yield return new WaitForSeconds(0.5f);

        //reset animation?

        //hide the enemy object's sprite and roll for item
        SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;
        //if (enemy != null)
            //Destroy(enemy.gameObject);

        reader.GetItem(3);

        //update log & change UI size so that scrollbar gets smaller as more text is added.
        logText.text = reader.log;
        logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
        enemySpawned = false;
        //yield return null;
    }
}
