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

    [Header("Objects")]
    public Enemy[] enemies1;    //level 1 to 10
    public Enemy[] enemies2;    //level 11 to 20
    public Enemy[] enemies3;    //level 21 to 30
    public Enemy[] enemies4;    //level 31 to 40
    public Enemy[] enemies5;    //level 41 to 50
    public GameObject lootPrefab;     //appears when item is generated.
    GameObject loot;


    public AnimationClip slash;
    public Animator slashAnim;

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.
    public TextMeshProUGUI logText;     //displays drop info.
    const float initPosition = 1;
    bool enemySpawned;
    bool timeToRoll;                //if true, check if loot generated.

    //coroutine toggles
    bool coroutineRunning;              //controls DestroyEnemy
    bool spawnEnemyCoroutineRunning;
    bool dropLootCoroutineRunning;
    


    // Start is called before the first frame update
    void Start()
    {
        //enemy = Instantiate(enemies1[0], new Vector3(initPosition, 0, 0), Quaternion.identity);
        //SpriteRenderer sr = enemies1[0].GetComponentInChildren<SpriteRenderer>();
        //sr.color = new Color(1, 1, 1, 0.2f);
        enemySpawned = false;
        timeToRoll = false;
        coroutineRunning = false;
        spawnEnemyCoroutineRunning = false;
        dropLootCoroutineRunning = false;

        //loot setup. It starts invisible until loot is generated.
        loot = Instantiate(lootPrefab, Vector3.zero, Quaternion.identity);
        SpriteRenderer lootSr = loot.GetComponentInChildren<SpriteRenderer>();
        lootSr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy appears and shifts to the centre of the screen. Use coroutine
        if (!enemySpawned)
        {
            if (!spawnEnemyCoroutineRunning)
            {
                spawnEnemyCoroutineRunning = true;
                StartCoroutine(SpawnEnemy(enemies1));
            }
        }
        else
        {
            if (!coroutineRunning)  //if I don't have this condition, the coroutine will execute more than once and cause performance issues.
            {
                coroutineRunning = true;
                StartCoroutine(DestroyEnemy());
            }

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

        /*if (timeToRoll)
        {
            //read the table based on the enemy that was defeated
            reader.GetItem(3);

            //update log & change UI size so that scrollbar gets smaller as more text is added.
            logText.text = reader.log;
            logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);

            timeToRoll = false;
        }*/
    }

    IEnumerator SpawnEnemy(Enemy[] enemyGroup)
    {
        int enemySpriteIndex = Random.Range(0, enemyGroup.Length);
        if (enemy == null)
        {
            enemy = Instantiate(enemyGroup[enemySpriteIndex], new Vector3(initPosition, 0, 0), Quaternion.identity);
        }
        else
        {
            //change the sprite of the existing enemy object
            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer enemyGroupSr = enemyGroup[enemySpriteIndex].GetComponentInChildren<SpriteRenderer>();
            sr.sprite = enemyGroupSr.sprite;
            

            //reset its position
            enemy.transform.position = new Vector3(initPosition, 0, 0);
            sr.enabled = true;
        }

        //generate enemy table ID based on its rank
        enemy.GenerateTableID();

        //TODO: prevent the first enemy from moving too far past the target position. This only happens with the first enemy,
        //and has to do with delta time being 0 at the beginning.
        while (enemy.transform.position.x > 0)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x - (10 * Time.deltaTime), enemy.transform.position.y, 0);
            yield return null;
        }

        enemySpawned = true;
        spawnEnemyCoroutineRunning = false;
    }

    //removes on screen enemy and then performs a check for loot
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(0.5f);  //added a slight delay before animation plays so that the enemy can "live" for a bit
        slashAnim.Play("Slash");    
        //slash.Play();

        yield return new WaitForSeconds(0.3f);

        //reset animation?

        //hide the enemy object's sprite and roll for item
        SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;
        //yield return new WaitForSeconds(0.5f);
       
        reader.GetItem(enemy.tableId);

        //if item is generated, call loot coroutine
        if (reader.ItemFound)
        {
            if (!dropLootCoroutineRunning)
            {
                dropLootCoroutineRunning = true;
                StartCoroutine(DropLoot());
            }
            yield return new WaitForSeconds(1f);

        }

        //update log & change UI size so that scrollbar gets smaller as more text is added.
        logText.text = reader.log;
        logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
        enemySpawned = false;
        //timeToRoll = true;
        coroutineRunning = false;
    }

    IEnumerator DropLoot()
    {
        //show loot object. 
        loot.transform.position = Vector3.zero;     //reset position
        SpriteRenderer lootSr = loot.GetComponentInChildren<SpriteRenderer>();
        lootSr.enabled = true;

        //loot animates by moving up for a duration before hiding again.
        float duration = 1;
        float currentTime = Time.time;
        while (Time.time < currentTime + duration)
        {
            loot.transform.position = new Vector3(loot.transform.position.x, loot.transform.position.y + (2 * Time.deltaTime), 0);
            yield return null;
        }

        //loot disappears until next time.
        lootSr.enabled = false;
        dropLootCoroutineRunning = false;
    }
}
