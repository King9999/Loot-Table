using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Table
{
    float tableWeight;      //weight category. Lower value = more valuable items. Data is from JSON file.
    Item[] tableItems;      //list of items contained in the table.
}
