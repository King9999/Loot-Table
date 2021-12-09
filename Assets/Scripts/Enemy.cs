using UnityEngine;

//enemies consist only of a level and a sprite.    
public class Enemy : MonoBehaviour
{
    public char enemyRank;          //determines which tables are accessible. Ranks from lowest to highest are D, C, B, A, and S.
    public int tableId;             //the enemy's drop table. It's determined by enemy level.
    //public Sprite enemySprite;
    public bool isRare;             //if true, then two rolls occur for an item. Better result is picked.

    //consts
    public int MaxLevel { get; } = 50;
    public int MaxTableId { get; } = 5;

    /* Rank info
     * 
     * D - access table 0
     * C - access tables 0 and 1 (lower chance of accessing table 0)
     * B - access tables 1, 2, and low chance of table 3
     * A - access tables 3, and low chance of 4
     * S - access tables 3 and high chance of 4
     */

}
