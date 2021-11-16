using UnityEngine;

/* this reader gets text data from a JSON file, which contains the loot table. Here's how it works:
 * 1. When it's time to generate an item, we check table ID. The available table IDs are based on enemy level.
 * 2. Once the ID is figured out, we generate a random number to find out which item is acquired, if any.
 * 3. If no item is generated, we apply a bonus that increases the item weight by 1 to make it easier to acquire an item next time.
 * 4. If an item was acquired, the bonus is reset to 0. 
 
The project uses a weight system to determine drop rates. The maximum weight is 1000, and each item in the loot table is given
an individual weight value. The tables themselves also have weight. The actual drop rate of an item is determined by
dividing its weight value by the maximum weight. E.g. an item with a weight of 10 has a drop rate of 10/1000 = 0.01, or 1%.
The total item weight in each table should add up to the maximum weight.
 
 */

public class TableReader : MonoBehaviour
{
    public TextAsset tableFile;
    public float itemWeightBonus;      //applied to itemWeight whenever an item isn't rolled.
    public string log;                 //record of all the rolls and items acquired.

    //consts
    public float MaxWeight { get; } = 1000;

    // Start is called before the first frame update
    void Start()
    {
        Tables lootTables = JsonUtility.FromJson<Tables>(tableFile.text);
        //Item item = JsonUtility.FromJson<Item>(tableFile.text);
        //Debug.Log("Name: " + item.itemName + " Weight: " + item.itemWeight);
        //Debug.Log("Name: " + tables[1].tableItems[0]. + " Weight: " + item.itemWeight);

        foreach(Table table in lootTables.tables)
        {
            Debug.Log("Name: " + table.tableItems[1].itemName + " Weight: " + table.tableItems[1].itemWeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
