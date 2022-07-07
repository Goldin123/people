using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class Program
    {
        public class Results
        {
            public string count { get; set; }
            public string next { get; set; }
            public string previous { get; set; }

            public List<Person> results { get; set; }
        }
        public class Person
        {
            public string name { get; set; }
            public string height { get; set; }
            public string mass { get; set; }
            public string hair_color { get; set; }
            public List<string> films { get; set; }

        }

        public class film
        {
            public string Url { get; set; }
        }

        public class vehicles
        {
            public string Rul { get; set; }
        }
        static void Main(string[] args)
        {
            string name = string.Empty;
            string urlParameters = "";
            string URL = "https://swapi.dev/api/people";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                var people = JsonConvert.DeserializeObject<Results>(dataObjects);

                foreach (var person in people.results)
                {

                    foreach(var film in person.films) 
                    {
                        var has = people.results.Where(a => a.films.Contains(film));

                        if (has.Any())
                        {
                            if(!name.Contains(person.name))
                                name = name + "; " + person.name;
                        }
                    }
                    
                }
                
                Console.WriteLine(name);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }
    }
}
