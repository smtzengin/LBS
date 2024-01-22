using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPrefab : MonoBehaviour
{
    public InventoryItem inventoryItem;
    public Book book;
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI bookISBN;
    public TextMeshProUGUI count;
    public LibraryManager libraryManager;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OpenBorrowAndReturnPanel);
    }

    public void UpdateInventoryItemUI(InventoryItem updatedItem)
    {
        if (updatedItem == inventoryItem)
        {
            inventoryItem = updatedItem;
            book = inventoryItem.book;
            bookName.text = updatedItem.book.title;
            bookISBN.text = updatedItem.book.ISBN;
            count.text = updatedItem.stack.ToString();
        }
    }
    public void OpenBorrowAndReturnPanel()
    {
        UIManager.Instance.OpenGiveBackBookPanel(inventoryItem.book);
    }
}
