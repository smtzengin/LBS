using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorrowAndReturnBook : MonoBehaviour
{
    [SerializeField] private LibraryManager libraryManager;
    public Book book;

    public void SetBook(Book selectedBook)
    {
        book = selectedBook;
    }


    public void BorrowBook()
    {
        if (book != null)
        {
            libraryManager.BorrowBook(book);
        }
    }

    public void GiveBackBook()
    {
        if (book != null)
        {
            libraryManager.ReturnBook(book);
        }
    }
}
