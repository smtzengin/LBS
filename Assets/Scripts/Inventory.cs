using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<InventoryItem> items;

    public Inventory()
    {
        items = new List<InventoryItem>();
    }

    public InventoryItem GetInventoryItemByBook(Book book)
    {
        return items.Find(item => item.book == book);
    }

    public void AddBookToInventory(Book book)
    {        
        InventoryItem existingItem = items.Find(item => item.book.title == book.title);

        if (existingItem != null)
            existingItem.stack++;
        else
        {            
            InventoryItem newItem = new InventoryItem(book);
            items.Add(newItem);
        }
        
        SaveInventoryToJson();
    }

    public void RemoveBookFromInventory(Book book)
    {
        InventoryItem existingItem = items.Find(item => item.book.title == book.title);
        if (existingItem != null && existingItem.stack > 0)
        {
            existingItem.stack--;
            if (existingItem.stack == 0)
                items.Remove(existingItem);
        }
        else
        {
            Debug.Log("stack fazla geldi.");
        }

        SaveInventoryToJson();
    }

    public void SaveInventoryToJson()
    {
        string json = JsonUtility.ToJson(this);
        System.IO.File.WriteAllText(Application.dataPath + "/inventory.json", json);
    }

    public void LoadInventoryFromJson()
    {
        string path = Application.dataPath + "/inventory.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            Inventory loadedInventory = JsonUtility.FromJson<Inventory>(json);

            if (loadedInventory != null)
                items = loadedInventory.items;
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public Book book;
    public int stack;

    public InventoryItem(Book book)
    {
        this.book = book;
        this.stack = 1;
    }
}