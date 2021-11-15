

[System.Serializable]
public class Table
{
    public float tableWeight;      //weight category. Lower value = more valuable items. Data is from JSON file.
    public Item[] tableItems;      //list of items contained in the table.
}
