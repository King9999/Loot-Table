

[System.Serializable]
public class Table
{
    public float tableId;                //table identification. Each enemy encountered will drop an item from a table with the given ID.
    public Item[] tableItems;      //list of items contained in the table.
}
