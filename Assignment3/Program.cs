using System;
using System.IO;
using System.Net;
using System.Net.Sockets; 
using System.Collections.Generic;
using Assignment1;
using System.Text.Json;


namespace Assignment3
{
    class Program
    {
        public static List<Book> _books = new List<Book>()
        {
            new Book{Title="book1", Author="me", PageNumber=512, ISBN13="123A" },
            new Book{Title="book2", Author="you", PageNumber=520, ISBN13="123B" },
            new Book{Title="book3", Author="he", PageNumber=564, ISBN13="123C" }
        };
        static void Main(string[] args)
        {
            

            TcpListener listener = new TcpListener(IPAddress.Loopback, 7);//
            listener.Start();//
            Console.WriteLine("Server ready");

            while (true) // to continue receiving clients
            {
                TcpClient socket = listener.AcceptTcpClient();//Until client comes in, the code doesn't continue
                Console.WriteLine("Incoming client");

                NetworkStream ns = socket.GetStream();
                //For more presise use, our reader and writer
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns);

                
                while(true)
                {
                    string message;
                    var json = JsonSerializer.Serialize(_books);
                    message = reader.ReadLine();
                    if (message == "GetAll")
                    {
                        writer.WriteLine(json);
                    }
                    if (message=="Get")
                    {
                        message = reader.ReadLine();
                        var myItem = _books.Find(item => item.ISBN13 == message);
                        var mz = JsonSerializer.Serialize(myItem);
                        writer.WriteLine(mz);
                    }
                    if (message=="Save")
                    {
                        message = reader.ReadLine();
                        var mz = JsonSerializer.Deserialize<Book>(message);
                        _books.Add(mz);
                    }
                    writer.Flush();
                }
                socket.Close();

            }
            
        }

    }
}
