using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Book
{    
    public string title;
    public string author;
    public string ISBN;
    public int totalCopies;
}

[System.Serializable]
public class BookList
{
    public List<Book> books;

    public BookList(List<Book> books)
    {
        this.books = books;
    }
}