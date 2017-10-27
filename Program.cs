using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace Sprawdzarka
{
    public class Program
    {
        
        public static DateTime[] przerwy = {
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,9,45,0),
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,11,30,0),
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,13,15,0),
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,15,15,0),
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,17,0,0),
            new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,18,45,0)
        };
        public static int minutDoPrzerwy = 0;
        public static string numerZadania = "";

        public static void Main(string[] args)
        {
            DateTime now = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                if (przerwy[i] > now)
                {
                    int a1 = przerwy[i].Hour * 60;
                    a1 += przerwy[i].Minute;
                    int a2 = now.Hour * 60;
                    a2 += now.Minute;
                    minutDoPrzerwy = a1 - a2;
                    break;
                }
            }
            Start();
            
        }

        public static async Task test()
        {
            
            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 30000;
            await Task.Delay((60 - DateTime.Now.Second) * 1000);
            timer.Start();
            await Task.Delay(-1);

        }
        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("Podaj ilosc zrobionych zadan");
            int iloscWykonanychZadan = Convert.ToInt32(Console.ReadLine());
            for (int increment = 0; increment < iloscWykonanychZadan; increment++)
            {
                Console.WriteLine("podaj numer zadania");
                numerZadania = Console.ReadLine();
                Console.WriteLine("Podaj rozwiazanie");
                string rozwiazanie = Console.ReadLine().ToLower();
                Komenda(rozwiazanie);
            }
            Program.test().GetAwaiter().GetResult();
        }
        public static bool Komenda(string rozwiazanie)
        {
            if(rozwiazanie.IndexOf("from") > rozwiazanie.IndexOf("select"))
            {
                if(rozwiazanie.Contains("where") && rozwiazanie.Contains("order by"))
                {
                    if(rozwiazanie.IndexOf("where") > rozwiazanie.IndexOf("from") && rozwiazanie.IndexOf("order by") > rozwiazanie.IndexOf("where"))
                    {
                        if (!Directory.Exists("odpowiedzi")) Directory.CreateDirectory("odpowiedzi");
                        File.WriteAllText(@"odpowiedzi/" + numerZadania + ".txt", rozwiazanie);
                        Console.WriteLine("Ok");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("NOk");
                        return false;
                    }
                }else if(rozwiazanie.Contains("where"))
                {
                    if(rozwiazanie.IndexOf("where") > rozwiazanie.IndexOf("from"))
                    {
                        if (!Directory.Exists("odpowiedzi")) Directory.CreateDirectory("odpowiedzi");
                        File.WriteAllText(@"odpowiedzi/" + numerZadania + ".txt", rozwiazanie);
                        Console.WriteLine("Ok");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("NOk");
                        return false;
                    }
                }
                else if(rozwiazanie.Contains("order by"))
                {
                    if(rozwiazanie.IndexOf("order by") > rozwiazanie.IndexOf("from"))
                    {
                        if (!Directory.Exists("odpowiedzi")) Directory.CreateDirectory("odpowiedzi");
                        File.WriteAllText(@"odpowiedzi/" + numerZadania + ".txt", rozwiazanie);
                        Console.WriteLine("Ok");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("NOk");
                        return false;
                    }
                }
                else
                    {
                    Console.WriteLine("Ok");
                    return false;
                    }
            }
            else
            {
                Console.WriteLine("NOk");
                return false;
            }
        }
        private static int counter = 0;

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (counter % 2 == 0 && (int)DateTime.Now.DayOfWeek < 6)
            {
                Console.WriteLine(String.Format("Zostało {0} minut do przerwy", minutDoPrzerwy));
                minutDoPrzerwy--;
            }
            counter++;
            string[] files;
            files = Directory.GetFiles("odpowiedzi/", "?.txt");
            Array.Sort(files);
            foreach (string odpowiedzi in files)
            {
                File.AppendAllText("odp.txt", File.ReadAllText(odpowiedzi));
            }
        }
    }
}