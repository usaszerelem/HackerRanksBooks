using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HackerRankBooks
{
    class MainClass
    {
        private static readonly string urlPageNum = "https://jsonmock.hackerrank.com/api/articles?author={auth}&page={pageNum}";
        private static readonly string urlAllBooks = "https://jsonmock.hackerrank.com/api/articles?page={pageNum}";

        private static HttpClient client = new HttpClient();

        private enum MenuChoice
        {
            NotInitialized,
            Terminate,
            ListAllBooks,
            RetrieveByAuthor
        }

        public static async Task Main(string[] args)
        {
            MenuChoice choice = MenuChoice.NotInitialized;

            while (choice != MenuChoice.Terminate)
            {
                choice = DisplayMenu();

                switch (choice)
                {
                    case MenuChoice.ListAllBooks:
                        await ListAllBooks();
                        break;

                    case MenuChoice.RetrieveByAuthor:
                        await ListBooksByAuthor();
                        break;
                }
            }
        }

        private static MenuChoice DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("HackerRank Books by Author\n");
            Console.WriteLine("0.- Terminate");
            Console.WriteLine("1.- List all books");
            Console.WriteLine("2.- Books by author");

            MenuChoice choice = MenuChoice.NotInitialized;

            do
            {
                switch (Console.ReadKey().KeyChar)
                {
                    case '0':
                        choice = MenuChoice.Terminate;
                        break;

                    case '1':
                        choice = MenuChoice.ListAllBooks;
                        break;

                    case '2':
                        choice = MenuChoice.RetrieveByAuthor;
                        break;

                    default:
                        Console.Beep();
                        break;
                }
            } while (choice == MenuChoice.NotInitialized);

            Console.WriteLine();

            return (choice);
        }

        private static async Task ListAllBooks()
        {
            try
            {
                Console.WriteLine($"Retrieving all books");
                List<Book> bookList = await GetAuthors(string.Empty);
                ListBooks(bookList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadKey();
        }

        private static async Task ListBooksByAuthor()
        {
            try
            {
                Console.Write("Author: ");
                string author = Console.ReadLine();
                Console.WriteLine($"Retrieving all books for author {author}\n");

                List<Book> bookList = await GetAuthors(author);

                if (bookList.Count == 0)
                {
                    Console.WriteLine("Could not find any books written by: " + author);
                }
                else
                {
                    Console.WriteLine(string.Format("Found {0} books written by {1}", bookList.Count, author));

                    ListBooks(bookList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press a key to continue...");
            Console.ReadLine();
        }

        private static void ListBooks(List<Book> bookList)
        {
            int bookIdx = 0;

            do
            {
                Console.WriteLine($"\n{bookIdx + 1} ---------------------");
                Console.WriteLine("Title: " + bookList[bookIdx].Title);
                Console.WriteLine("Url: " + bookList[bookIdx].Url);
                Console.WriteLine("Author: " + bookList[bookIdx].Author);
                Console.WriteLine("Comments: " + bookList[bookIdx].Comments);
                Console.WriteLine("Story ID: " + bookList[bookIdx].StoryID);
                Console.WriteLine("Story Title: " + bookList[bookIdx].StoryTitle);
                Console.WriteLine("Story URL: " + bookList[bookIdx].StoryUrl);
                Console.WriteLine("Parent ID: " + bookList[bookIdx].ParentID);
                Console.WriteLine("Created At: " + bookList[bookIdx].CreatedAt);

            } while (++bookIdx < bookList.Count);
        }

        private static async Task<List<Book>> GetAuthors(string auth)
        {
            List<Book> bookList = new List<Book>();
            int pageNum = 0;
            BookRequest bookRequest;
            string url;

            do
            {
                if (auth.Length > 0)
                {
                    url = urlPageNum
                             .Replace("{auth}", auth)
                             .Replace("{pageNum}", pageNum.ToString());
                }
                else
                {
                    url = urlAllBooks.Replace("{pageNum}", pageNum.ToString());
                }

                HttpResponseMessage result = await client.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string strBody = await result.Content.ReadAsStringAsync();

                    bookRequest = JsonConvert.DeserializeObject<BookRequest>(strBody);
                    bookList.AddRange(bookRequest.Books);
                    pageNum++;
                }
                else
                {
                    StringBuilder sb = new StringBuilder("Error in HTTP call:\n");
                    sb.Append($"Url: {url}\n");
                    sb.Append($"Http Status: {result.StatusCode}\n");
                    sb.Append($"Message: {result.ReasonPhrase}\n");

                    throw new Exception(sb.ToString());
                }


            } while (pageNum < bookRequest.TotalPages);

            return (bookList);
        }
    }
}
