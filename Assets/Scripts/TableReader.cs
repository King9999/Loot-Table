using UnityEngine;

/* this reader gets text data from a JSON file, which contains the loot table. Here's how it works:
 * 1. When it's time to generate an item, we check table ID. The available table IDs are based on enemy level.
 * 2. Once the ID is figured out, we generate a random number to find out which item is acquired, if any.
 * 3. If no item is generated, we apply a bonus that increases the item weight by 1 to make it easier to acquire an item next time.
 * 4. If an item was acquired, the bonus is reset to 0. 
 
The project uses a weight system to determine drop rates. The maximum weight is 1000, and each item in the loot table is given
an individual weight value. The actual drop rate of an item is determined by
dividing its weight value by the maximum weight. E.g. an item with a weight of 10 has a drop rate of 10/1000 = 0.01, or 1%.
The total item weight in each table consists of all of the droppable items. Any values outside of the total means no items will drop.
 
 */

public class TableReader : MonoBehaviour
{
    public TextAsset tableFile;
    public float itemWeightBonus;      //applied to itemWeight whenever an item isn't rolled.
    public string log;                 //record of all the rolls and items acquired.
    Tables lootTables;

    //consts
    public float MaxWeight { get; } = 1000;

    // Start is called before the first frame update
    void Start()
    {
        lootTables = JsonUtility.FromJson<Tables>(tableFile.text);
        itemWeightBonus = 0;
        //Item item = JsonUtility.FromJson<Item>(tableFile.text);
        //Debug.Log("Name: " + item.itemName + " Weight: " + item.itemWeight);
        //Debug.Log("Name: " + tables[1].tableItems[0]. + " Weight: " + item.itemWeight);

        /*foreach(Table table in lootTables.tables)
        {
            Debug.Log("Name: " + table.tableItems[1].itemName + " Weight: " + table.tableItems[1].itemWeight);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //roll for an item from the given table ID.
    public void GetItem(int tableId)
    {
        //get a random number
        float randNum = Random.Range(0f, 1f);
        Debug.Log("Rolled " + randNum);

        //compare number against weight of each item in table
        int i = lootTables.tables[tableId].tableItems.Length - 1;
        bool itemFound = false;
        //string itemName = "";
        while (!itemFound && i >= 0)
        {
            float dropChance = (lootTables.tables[tableId].tableItems[i].itemWeight + itemWeightBonus) / MaxWeight;     //drop chance = base drop rate + item weight bonus.
            string itemName = lootTables.tables[tableId].tableItems[i].itemName;
            if (randNum <= dropChance)
            {
                //we found the item, generate it
                float itemWeight = lootTables.tables[tableId].tableItems[i].itemWeight;
                Debug.Log("Enemy dropped " + itemName + ", Weight: " + itemWeight + ", Weight Bonus: " + itemWeightBonus + ", Drop Rate: " + dropChance);
                log += "Enemy dropped " + itemName + ", Weight: " + itemWeight + ", Weight Bonus: " + itemWeightBonus + ", Drop Rate: " + dropChance + "\n";
                itemFound = true;
            }
            else
            {
                //Debug.Log("Failed to drop " + itemName + ", moving to next item");
                i--;
            }
        }

        //if no item was found, then grant a bonus.
        if (i <= 0)
        {
            itemWeightBonus++;
            Debug.Log("No drop, adding a weight bonus. Current bonus is " + itemWeightBonus);
            log += "No drop, weight bonus added. Current bonus is " + itemWeightBonus + "\n";
        }
        else
        {
            itemWeightBonus = 0;
        }
       
    }
}
