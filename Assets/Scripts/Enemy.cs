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

    public void GenerateTableID()
    {
        float randNum;          //used to check odds of accessing certain tables

        switch(enemyRank)
        {
            case 'D':
                tableId = 0;
                break;

            case 'C':
                randNum = Random.Range(0f, 1f);
                if (randNum <= 0.25f)    //25% chance of accessing table 0
                    tableId = 0;
                else
                    tableId = 1;
                break;

            case 'B':
                randNum = Random.Range(0f, 1f);
                if (randNum <= 0.25f)
                    tableId = 3;
                else
                    tableId = Random.Range(1, 3);   //access either table 1 or 2
                break;

            case 'A':
                randNum = Random.Range(0f, 1f);
                if (randNum <= 0.25f)
                    tableId = 4;
                else
                    tableId = 3;
                break;

            case 'S':
                randNum = Random.Range(0f, 1f);
                if (randNum <= 0.25f)
                    tableId = 3;
                else
                    tableId = 4;
                break;

            default:
                break;
        }
    }

}
