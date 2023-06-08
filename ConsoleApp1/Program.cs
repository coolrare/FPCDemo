using ConsoleApp1;

var http = new HttpClient();

var client = new swaggerClient("https://localhost:7129", http);

var result = await client.CoursesAllAsync();

foreach (var item in result)
{
    Console.WriteLine(item.Title);
}
