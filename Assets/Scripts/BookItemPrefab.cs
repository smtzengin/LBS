using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookItemPrefab : MonoBehaviour
{
    public Book book;
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI bookISBN;
    public TextMeshProUGUI bookTotalCopies;       
    public LibraryManager _libraryManager;

    private void Awake()
    {
        _libraryManager.OnBookStatusChanged += UpdateBookUI;
        

        GetComponent<Button>().onClick.AddListener(OpenBorrowAndReturnPanel);
    }

    //UI üzerinde güncelleme işlemlerini yapar
    public void UpdateBookUI(Book updatedBook)
    {
        if (updatedBook == book)
        {
            Debug.Log($"Kitap güncellendi - Title: {updatedBook.title}, ISBN: {updatedBook.ISBN}, Total Copies: {updatedBook.totalCopies}");

            book = updatedBook;
            bookName.text = updatedBook.title;
            bookISBN.text = updatedBook.ISBN;
            bookTotalCopies.text = updatedBook.totalCopies.ToString();
        }
    }

    public void OpenBorrowAndReturnPanel()
    {
        UIManager.Instance.OpenBorrowBookPanel(book);
    }
}
