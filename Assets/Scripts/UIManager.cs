using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject AddBookPanel;
    [SerializeField] private GameObject ListBooksPanel;
    [SerializeField] private GameObject BorrowAndReturnBookPanel;
    [SerializeField] private GameObject inventoryPanel;

    [Header("ADD BOOK")]
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField ISBNInputField;
    [SerializeField] private TMP_InputField totalCopiesInput;

    [Header("ListOfBooks")]
    [SerializeField] private GameObject listOfBooksContent;
    [SerializeField] private GameObject BookItemPrefab;
    [SerializeField] private List<GameObject> bookItemPrefabs = new List<GameObject>();

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryContent;
    [SerializeField] private GameObject inventoryItemPrefab;

    [Header("Managers")]
    [SerializeField] private LibraryManager libraryManager;

    [Header("BorrowAndReturnBook")]
    [SerializeField] private BorrowAndReturnBook borrowAndReturnBook;
    [SerializeField] private Button borrowBookBtn;
    [SerializeField] private Button giveBackBtn;

    public TMP_InputField searchInputField;
    public TextMeshProUGUI statusText;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(Instance.gameObject);
        }
        searchInputField.onValueChanged.AddListener(SearchBooks);

    }
    
    private void SearchBooks(string searchTerm)
    {
        UIManager.Instance.SearchBooksByName(searchTerm);
    }

    public void SearchBooksByName(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();

        foreach (GameObject bookItem in bookItemPrefabs)
        {            
            if (bookItem == null) continue;            
            BookItemPrefab bookItemScript = bookItem.GetComponent<BookItemPrefab>();            
            if (bookItemScript == null || bookItemScript.book == null) continue;            

            Book book = bookItemScript.book;

            bool isMatch = string.IsNullOrEmpty(searchTerm) || book.title.ToLower().Contains(searchTerm);

            // Eğer obje hala aktifse veya deaktifse ve aranan sonuca uymuyorsa deaktif et
            if (bookItem.activeSelf != isMatch) bookItem.SetActive(isMatch);
            
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
        
        foreach (Transform child in listOfBooksContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Book book in libraryManager.books)
        {
            GameObject bookItem = Instantiate(BookItemPrefab, listOfBooksContent.transform);
            bookItem.SetActive(false); // Başlangıçta hepsini gizle
            BookItemPrefab bookItemScript = bookItem.GetComponent<BookItemPrefab>();
            bookItemScript._libraryManager = libraryManager;
            bookItemScript.book = book;
            bookItemScript.bookName.text = book.title;
            bookItemScript.bookISBN.text = book.ISBN;
            bookItemScript.bookTotalCopies.text = book.totalCopies.ToString();
            bookItemScript.SetOnClickAction(() => UIManager.Instance.OpenBorrowBookPanel(book));

            bookItemPrefabs.Add(bookItem);
        }

        Debug.Log("Bütün Kitaplar Listelendi!");
    }

    public void ListOfBooksToInventory()
    {
        
        foreach (Transform child in inventoryContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryItem inventoryItem in libraryManager.inventory.items)
        {
            GameObject inventoryItemObject = Instantiate(inventoryItemPrefab, inventoryContent.transform);
            InventoryItemPrefab inventoryItemScript = inventoryItemObject.GetComponent<InventoryItemPrefab>();
            inventoryItemScript.inventoryItem = inventoryItem;
            inventoryItemScript.bookName.text = inventoryItem.book.title;
            inventoryItemScript.bookISBN.text = inventoryItem.book.ISBN;
            inventoryItemScript.count.text = inventoryItem.stack.ToString();
            inventoryItemScript.libraryManager = libraryManager;
        }
        
    }

    public void ClearContents()
    {
        foreach (Transform child in listOfBooksContent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in inventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OpenBorrowBookPanel(Book book)
    {
        BorrowAndReturnBookPanel.SetActive(true);
        borrowBookBtn.gameObject.SetActive(true);
        giveBackBtn.gameObject.SetActive(false);
        borrowAndReturnBook.book = book;
    }

    public void OpenGiveBackBookPanel(Book book)
    {
        BorrowAndReturnBookPanel.SetActive(true);
        borrowBookBtn.gameObject.SetActive(false);
        giveBackBtn.gameObject.SetActive(true);
        borrowAndReturnBook.book = book;
    }

    public void OpenMainPanel()
    {
        MainPanel.SetActive(true);
    }
    
    public void OpenInventoryPanel()
    {
        inventoryPanel.SetActive(true);
        ListOfBooksToInventory();
    }

    public void OpenAddBookPanel()
    {        
        CloseAllPanels();
        AddBookPanel.SetActive(true);
    }

    public void OpenListOfBooksPanel()
    {
        ListOfBooksToUI();        
        CloseAllPanels();
        ListOfBooksToInventory();
        SearchBooks(string.Empty);
        searchInputField.text = string.Empty;
        ListBooksPanel.SetActive(true);
        inventoryPanel.SetActive(true);
    }        

    public void CloseAllPanels()
    {
        MainPanel.SetActive(false);
        AddBookPanel.SetActive(false);
        ListBooksPanel.SetActive(false);
        BorrowAndReturnBookPanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }
}
