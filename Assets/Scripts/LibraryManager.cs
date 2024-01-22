using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{
    public List<Book> books = new List<Book>();
    public Action<Book> OnBookStatusChanged;
    public Action<InventoryItem> OnInventoryChanged;
    public Inventory inventory = new Inventory();
    private void Awake()
    {
        CreateJsonFile();
        LoadBooksFromJson();
        inventory.LoadInventoryFromJson();
    }
    
    //Kitap ekleme işlemlerini gerçekleştirir.
    public void AddNewBook(Book newBook)
    {
        string newBookName = newBook.title;
        string newBookISBN = newBook.ISBN;

        if (!IsBookExists(newBookName, newBookISBN))
        {
            books.Add(newBook);
            SaveBooksToJson();
            Debug.Log("Kitap Başarı ile eklendi!");            
        }
        else
        {            
            Debug.LogWarning("Bu isim veya ISBN numarasına ait bir kitap zaten mevcut!");
        }        
    }

    //Kitap Ödünç Alma işlemlerini gerçekleştirir.
    public void BorrowBook(Book book)
    {
        if(book.totalCopies > 0)
        {
            book.totalCopies--;
            inventory.AddBookToInventory(book);
            SaveBooksToJson();
            OnBookStatusChanged?.Invoke(book);
            UIManager.Instance.OpenInventoryPanel();
            
        }
    }

    //Kitap İade Etme İşlemlerini Gerçekleştirir.
    public void ReturnBook(Book book)
    {        
        inventory.RemoveBookFromInventory(book);
        
        Book existingBook = books.Find(existing => existing.title.Equals(book.title) && existing.ISBN.Equals(book.ISBN));

        if (existingBook != null)
        {
            // Eğer varsa, mevcut kitabın totalCopies değerini artır
            existingBook.totalCopies++;
            SaveBooksToJson(); 
            OnBookStatusChanged?.Invoke(existingBook);
        }
        else
        {
            Debug.LogError("İlgili kitap mevcut kitap listesinde bulunamadı!");
        }

        UIManager.Instance.OpenInventoryPanel();
    }

    //Kitabın ISBN veya ismine göre var olup olmadığını kontrol eden sorgu.
    private bool IsBookExists(string name, string ISBN)
    {
        return books.Exists(book => book.title.Equals(name) || book.ISBN.Equals(ISBN));
    }
    

    //books.json dosyası daha önce oluşturulmadıysa oluşturulur.
    public void CreateJsonFile()
    {
        string path = Application.dataPath + "/books.json";
        if ((System.IO.File.Exists(path))) return;
        else
        {
            System.IO.File.WriteAllText(path, "");
        }
    }

    //veriye yazma işlemini gerçekleştirir.
    public void SaveBooksToJson()
    {
        string json = JsonUtility.ToJson(new BookList(books));
        System.IO.File.WriteAllText(Application.dataPath + "/books.json", json);
    }

    //Mevcut kitapları çekme işlemlerini gerçekleştirir.
    public void LoadBooksFromJson()
    {
        string path = Application.dataPath + "/books.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            BookList bookList = JsonUtility.FromJson<BookList>(json);

            if (bookList != null)
            {
                books = bookList.books;
            }
            PrintBooksToConsole();
        }
    }
    //Mevcut kitapları konsola yazdırmaya yarar.
    public void PrintBooksToConsole()
    {
        if (books != null)
        {
            foreach (Book book in books)
            {
                Debug.Log($"Title : {book.title}" +
                    $"Author : {book.author} " +
                    $"ISBN : {book.ISBN} " +
                    $"TotalCopies : {book.totalCopies}");
            }
        }
    }
}

