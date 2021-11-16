
public class Enemy
{
    public int enemyLevel;          //determines which tables are accessible.
    public int tableId;             //the enemy's drop table. It's determined by enemy level.

    //consts
    public int MaxLevel { get; } = 50;
    public int MaxTableId { get; } = 5;

}
