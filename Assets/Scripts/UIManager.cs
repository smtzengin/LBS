using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject AddBookPanel;
    [SerializeField] private GameObject ListBooksPanel;
    [SerializeField] private GameObject BorrowAndReturnBookPanel;

    [Header("ADD BOOK")]
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField ISBNInputField;
    [SerializeField] private TMP_InputField totalCopiesInput;

    [Header("ListOfBooks")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject BookItemPrefab;

    [Header("Managers")]
    [SerializeField] private LibraryManager libraryManager;

    [SerializeField] private BorrowAndReturnBook borrowAndReturnBook;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }

    //UI üzerinden aldığı verileri işleyerek ekleme işlemini gerçekleştirir.
    public void AddNewBookFromUI()
    {
        Book newBook = new Book
        {
            title = titleInputField.text,
            author = authorInputField.text,
            ISBN = ISBNInputField.text,
            totalCopies = int.Parse(totalCopiesInput.text),
        };

        libraryManager.AddNewBook(newBook);
        ClearUIFields();
    }     

    //Ekleme işleminden sonra input fieldleri temizler.
    public void ClearUIFields()
    {
        titleInputField.text = string.Empty;
        authorInputField.text = string.Empty;
        ISBNInputField.text = string.Empty;
        totalCopiesInput.text = string.Empty;
    }

    public void ListOfBooksToUI()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Book book in libraryManager.books)
        {
            GameObject bookItem = Instantiate(BookItemPrefab,content.transform);
            BookItemPrefab bookItemScript = bookItem.GetComponent<BookItemPrefab>();
            bookItemScript._libraryManager = libraryManager;
            bookItemScript.book = book;
            bookItemScript.bookName.text = book.title;
            bookItemScript.bookISBN.text = book.ISBN;
            bookItemScript.bookTotalCopies.text = book.totalCopies.ToString();
        }

        Debug.Log("Bütün Kitaplar Listelendi!");
    }    

    public void OpenBorrowAndReturnPanel(Book book)
    {
        BorrowAndReturnBookPanel.SetActive(true);
        borrowAndReturnBook.book = book;
    }

    public void OpenAddBookPanel()
    {
        AddBookPanel.SetActive(true);
        CloseAllPanels();
    }

    public void OpenListOfBooksPanel()
    {
        ListOfBooksToUI();
        CloseAllPanels();
        ListBooksPanel.SetActive(true);
    }    

    public void CloseAllPanels()
    {
        MainPanel.SetActive(false);
        AddBookPanel.SetActive(false);
        ListBooksPanel.SetActive(false);
        BorrowAndReturnBookPanel.SetActive(false);
    }
}
