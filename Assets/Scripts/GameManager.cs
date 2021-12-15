using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Enemies are defeated, and items have a chance to drop. Sometimes, a rare version of an enemy appears, which
 * allows two chances for an item to drop. The better roll is picked, and if nothing drops, the added weight bonus is
 * tripled. */
public class GameManager : MonoBehaviour
{
    public TableReader reader;
    Enemy enemy;

    [Header("Objects")]
    public Enemy[] enemiesD;    // The letters next to the array names are the enemy ranks. Ranks go from D to S, D is the worst, S is the best.
    public Enemy[] enemiesC;    
    public Enemy[] enemiesB;    
    public Enemy[] enemiesA;    
    public Enemy[] enemiesS;
    public Enemy[] selectedEnemies;     //contains the chosen enemies the player picked.
    public GameObject lootPrefab;     //appears when item is generated.
    GameObject loot;
    public GameObject auraPrefab;     //used to indicate rare enemy
    GameObject aura;

    [Header("UI")]
    public TextMeshProUGUI enemyRankUI;
    public TextMeshProUGUI dropResultUI;    //name of the item that dropped
    public TextMeshProUGUI itemWeightUI;    //shows the weight of the last dropped item.
    public TextMeshProUGUI tableAccessUI;   
    public Button backButton;               //used to go back to rank scene

    [Header("--------------")]
    public AnimationClip slash;
    public Animator slashAnim;

    public float currentTime;
    public float getItemTime = 1;       //time in seconds to generate item.
    public TextMeshProUGUI logText;     //displays drop info.
    const float initPosition = 1;
    bool enemySpawned;
    //bool timeToRoll;                //if true, check if loot generated.

    //coroutine toggles
    bool destroyEnemyCoroutineRunning;              
    bool spawnEnemyCoroutineRunning;
    bool dropLootCoroutineRunning;
    


    // Start is called before the first frame update
    void Start()
    {
        enemySpawned = false;
        //timeToRoll = false;
        destroyEnemyCoroutineRunning = false;
        spawnEnemyCoroutineRunning = false;
        dropLootCoroutineRunning = false;

        //loot setup. It starts invisible until loot is generated.
        loot = Instantiate(lootPrefab, Vector3.zero, Quaternion.identity);
        SpriteRenderer lootSr = loot.GetComponentInChildren<SpriteRenderer>();
        lootSr.enabled = false;

        //aura setup
        aura = Instantiate(auraPrefab, Vector3.zero, Quaternion.identity);
        SpriteRenderer auraSr = aura.GetComponentInChildren<SpriteRenderer>();
        auraSr.enabled = false;

        //UI setup
        enemyRankUI.text = "Enemy Rank: ";      //TODO: complete this once rank screen is completed.
        dropResultUI.text = "--";
        //itemWeightUI.text = "--";
        tableAccessUI.text = "Accessible Tables: ";

        //rank and array setup
        switch(RankManager.instance.selectedRank)
        {
            case RankManager.Rank.D:
                selectedEnemies = enemiesD;
                enemyRankUI.text = "Enemy Rank: D";
                tableAccessUI.text = "Accessible Tables: 0";
                break;

            case RankManager.Rank.C:
                selectedEnemies = enemiesC;
                enemyRankUI.text = "Enemy Rank: C";
                tableAccessUI.text = "Accessible Tables: 0 (low chance), 1";
                break;

            case RankManager.Rank.B:
                selectedEnemies = enemiesB;
                enemyRankUI.text = "Enemy Rank: B";
                tableAccessUI.text = "Accessible Tables: 1, 2, 3 (low chance)";
                break;

            case RankManager.Rank.A:
                selectedEnemies = enemiesA;
                enemyRankUI.text = "Enemy Rank: A";
                tableAccessUI.text = "Accessible Tables: 3, 4 (low chance)";
                break;

            case RankManager.Rank.S:
                selectedEnemies = enemiesS;
                enemyRankUI.text = "Enemy Rank: S";
                tableAccessUI.text = "Accessible Tables: 3 (low chance), 4";
                break;

            default:
                break;
        }
        //selectedEnemies = enemiesS;
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
                StartCoroutine(SpawnEnemy(selectedEnemies));
            }
        }
        else
        {
            if (!destroyEnemyCoroutineRunning)  //if I don't have this condition, the coroutine will execute more than once and cause performance issues.
            {
                destroyEnemyCoroutineRunning = true;
                StartCoroutine(DestroyEnemy());
            }
        }
    }

    #region Coroutines
    IEnumerator SpawnEnemy(Enemy[] enemyGroup)
    {
        yield return new WaitForSeconds(0.1f);
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
            aura.transform.position = enemy.transform.position;
            sr.enabled = true;
        }

        //check enemy rarity
        enemy.CheckEnemyRarity();
        //enemy.isRare = true;
        if (enemy.isRare)
        {
            //give the enemy an "aura"
            SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();
            SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
            auraSr.sprite = sr.sprite;
            auraSr.enabled = true;
        }

        //generate enemy table ID based on its rank
        enemy.GenerateTableID();

        //TODO: prevent the first enemy from moving too far past the target position. This only happens with the first enemy,
        //and has to do with delta time being 0 at the beginning.
        while (enemy.transform.position.x > 0)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x - (10 * Time.deltaTime), enemy.transform.position.y, 0);
            aura.transform.position = enemy.transform.position;
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

        yield return new WaitForSeconds(0.3f);

        //hide the enemy object's sprite and roll for item
        SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
        sr.enabled = false;

        if (enemy.isRare)
        {
            //hide the aura and do a special roll 
            SpriteRenderer auraSr = aura.GetComponent<SpriteRenderer>();
            auraSr.enabled = false;

            reader.GetItem(enemy.tableId, true);
            enemy.isRare = false;   //enemy object is never deleted, so this step must be done.
        }
        else
            reader.GetItem(enemy.tableId);

        //if item is generated, call loot coroutine
        if (reader.itemFound)
        {
            dropResultUI.text = "Found " + reader.itemName;
            //itemWeightUI.text = "Weight: " + (reader.itemWeight + reader.itemWeightBonus).ToString() + " / 1000";
            if (!dropLootCoroutineRunning)
            {
                dropLootCoroutineRunning = true;
                StartCoroutine(DropLoot());
            }
            yield return new WaitForSeconds(1f);    //giving time for loot sprite to complete animation before next enemy is generated.
        }
        else
        {
            dropResultUI.text = "No drop";
            //itemWeightUI.text = "--";
        }

        //update log & change UI size so that scrollbar gets smaller as more text is added.
        logText.text = reader.log;
        logText.rectTransform.sizeDelta = new Vector2(logText.rectTransform.rect.width, logText.rectTransform.rect.height + logText.fontSize);
        enemySpawned = false;
        destroyEnemyCoroutineRunning = false;
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
    #endregion

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("Rank Select");
    }
}
