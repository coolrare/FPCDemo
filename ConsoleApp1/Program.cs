using ConsoleApp1;
using System.Diagnostics;

var http = new HttpClient();

var client = new swaggerClient("https://localhost:7129", http);

var result = await client.GetCourseAllAsync();

foreach (var item in result)
{
    Console.WriteLine(item.Title);
}

Console.WriteLine("-------------------------------------");

var one = await client.GetCourseByIdAsync(1);

Console.WriteLine(one.Title);