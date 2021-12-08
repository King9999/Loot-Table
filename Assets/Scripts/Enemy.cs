using UnityEngine;

//enemies consist only of a level and a sprite.    
public class Enemy : MonoBehaviour
{
    public char enemyRank;          //determines which tables are accessible.
    public int tableId;             //the enemy's drop table. It's determined by enemy level.
    //public Sprite enemySprite;
    public bool isRare;             //if true, then two rolls occur for an item. Better result is picked.

    //consts
    public int MaxLevel { get; } = 50;
    public int MaxTableId { get; } = 5;

}
