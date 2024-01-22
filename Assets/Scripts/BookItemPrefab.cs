using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BookItemPrefab : MonoBehaviour
{
    public Book book;
    public TextMeshProUGUI bookName;
    public TextMeshProUGUI bookISBN;
    public TextMeshProUGUI bookTotalCopies;       
    public LibraryManager _libraryManager;

    private void Awake()
    {
        Debug.Log("BookItemPrefab Awake");
        _libraryManager.OnBookStatusChanged += UpdateBookUI;
    }

    //UI üzerinde güncelleme işlemlerini yapar
    public void UpdateBookUI(Book updatedBook)
    {
        Debug.Log($"UpdateBookUI Called - book: {book.title}, updatedBook: {updatedBook}");

        if (updatedBook == book && IsBookInSearchResults(updatedBook))
        {
            Debug.Log($"Kitap güncellendi - Title: {updatedBook.title}, ISBN: {updatedBook.ISBN}, Total Copies: {updatedBook.totalCopies}");

            book = updatedBook;
            bookName.text = updatedBook.title;
            bookISBN.text = updatedBook.ISBN;
            bookTotalCopies.text = updatedBook.totalCopies.ToString();
        }
    }
    private bool IsBookInSearchResults(Book bookToCheck)
    {       
        return bookToCheck.title.ToLower().Contains(UIManager.Instance.searchInputField.text.ToLower());
    }
    public void SetOnClickAction(Action action)
    {
        GetComponent<Button>().onClick.AddListener(() => action.Invoke());
    }
}
