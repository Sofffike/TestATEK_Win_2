using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestWinApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add("http://localhost:8080/");
            server.Start();

            var context =  server.GetContext();
            var request = context.Request; 
            var response = context.Response;

            var i = request.QueryString["i"];
            var j = request.QueryString["j"];

            var responseString = $"Результат: {Calculate(int.Parse(i), int.Parse(j))}";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            // Добавляем заголовки CORS
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
                output.FlushAsync();
            }

            Console.WriteLine("Запрос обработан");

            server.Stop();
            server.Close();
        }

        static int Calculate(int i, int j)
        {
            return i + j;
        }
    }
}
