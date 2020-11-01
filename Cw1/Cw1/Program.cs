using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response;

            try
            {
                if (args.Length == 0) throw (new ArgumentNullException("Nie przekazano parametru"));

                try
                {
                    response = await httpClient.GetAsync(args[0]);
                }catch(HttpRequestException hre)
                {
                    throw (new ArgumentException("Niepoprawny adres www"));
                }
                
                if (response.IsSuccessStatusCode)
                {
                    var html = await response.Content.ReadAsStringAsync();
                    var regex = new Regex("[a-z0-9]+@[a-z.]+");
                    var matches = regex.Matches(html);

                    if (matches.Count > 0)
                    {
                        List<string> emails = new List<string>();
                        foreach (var i in matches)
                        {
                            if (!emails.Contains(i.ToString()))
                            {
                                emails.Add(i.ToString());
                            }
                        }
                        emails.ForEach(Console.WriteLine);
                    }
                    else
                    {
                        Console.WriteLine("Nie znaleziono adresów email");
                    }
                }
                else
                {
                    Console.WriteLine("Błąd w czasie pobierania strony");
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException: {0} ", ane.Message);
            }
            catch(ArgumentException ae)
            {
                Console.WriteLine("ArgumentException: {0} ", ae.Message);
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
