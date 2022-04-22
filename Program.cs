using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credits
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Оформление банковских кредитов";

            bool success = true;
            success &= ReadClients();
            success &= ReadEmployees();
            success &= ReadCredits();

            if (!success)
            {
                Console.WriteLine("При чтении файла были обнаружены некорректные записи.");
                Console.WriteLine("Они не были добавлены в таблицы и будут удалены");
                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
                Console.ReadKey(true);
            }
            Clients.FillTable();
            Employees.FillTable();
            Credits.FillTable();

            ConsoleKeyInfo key;
            var error = false;

            do
            {
                if (!error)
                {
                    Console.Write(new string('*', Console.BufferWidth));
                    Console.Write(new string('*', Console.BufferWidth));
                    Console.WriteLine("Добро пожаловать в систему оформления банковских кредитов");
                    Console.Write(new string('*', Console.BufferWidth));
                }
                else
                {
                    Console.WriteLine("Ошибка ввода");
                    error = false;
                }

                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - Работа с клиентами");
                Console.WriteLine("2 - Работа с сотрудниками");
                Console.WriteLine("3 - Работа с кредитами");
                Console.WriteLine("4 - Завершение работы");
                key = Console.ReadKey(true);
                Console.Clear();
                switch (key.KeyChar)
                {
                    case '1':
                        Clients.EnterMenu();
                        break;
                    case '2':
                        Employees.EnterMenu();
                        break;
                    case '3':
                        Credits.EnterMenu();
                        break;
                    default:
                        error = true;
                        break;
                }
            } while (key.KeyChar != '4');
            SaveAll();
        }

        static bool ReadClients()
        {
            bool error = false;
            int id;
            string[] data;
            var reader = new StreamReader(new FileStream("clients.txt",
                FileMode.OpenOrCreate, FileAccess.ReadWrite));

            while (!reader.EndOfStream)
            {
                try
                {
                    data = reader.ReadLine().Split('\t');
                    id = int.Parse(data[0]);
                    if (Client.clients.Any(t => t.id == id))
                        throw new ArgumentException();

                    Client.clients.Add(new Client
                    {
                        id = id,
                        name = data[1],
                        birthDate = DateTime.Parse(data[2]),
                        phone = data[3],
                        passport = data[4],
                    });
                }
                catch
                {
                    error = true;
                }
            }
            reader.Close();
            return !error;
        }

        static bool ReadEmployees()
        {
            bool error = false;
            int id;
            string[] data;
            var reader = new StreamReader(new FileStream("employees.txt",
                FileMode.OpenOrCreate, FileAccess.ReadWrite));
            while (!reader.EndOfStream)
            {
                try
                {
                    data = reader.ReadLine().Split('\t');
                    id = int.Parse(data[0]);
                    if (Employee.employees.Any(t => t.id == id))
                        throw new ArgumentException();

                    Employee.employees.Add(new Employee
                    {
                        id = id,
                        name = data[1],
                        position = data[2],
                        birthDate = DateTime.Parse(data[3]),
                        hireDate = DateTime.Parse(data[4]),
                        phone = data[5],
                        email = data[6],
                    });
                }
                catch
                {
                    error = true;
                }
            }
            reader.Close();
            return !error;
        }

        static bool ReadCredits()
        {
            bool error = false;
            int id;
            string[] data;
            var reader = new StreamReader(new FileStream("credits.txt",
                FileMode.OpenOrCreate, FileAccess.ReadWrite));
            while (!reader.EndOfStream)
            {
                try
                {
                    data = reader.ReadLine().Split('\t');
                    id = int.Parse(data[0]);
                    if (Credit.credits.Any(t => t.id == id))
                        throw new ArgumentException();


                    Credit.credits.Add(new Credit
                    {
                        id = id,
                        client = Client.clients.First(t => t.id == int.Parse(data[1])),
                        employee = Employee.employees.First(t => t.id == int.Parse(data[2])),
                        date = DateTime.Parse(data[3]),
                        sum = decimal.Parse(data[4]),
                        time = int.Parse(data[5]),
                        percent = decimal.Parse(data[6]),
                    });
                }
                catch
                {
                    error = true;
                }
            }
            reader.Close();
            return !error;
        }

        static void SaveAll()
        {
            StreamWriter writer;
            writer = new StreamWriter("clients.txt");
            foreach (var c in Client.clients)
                writer.WriteLine(string.Join("\t", c.id, c.name, c.birthDate.ToString("dd.MM.yyyy"),
                    c.phone, c.passport));
            writer.Close();

            writer = new StreamWriter("employees.txt");
            foreach (var e in Employee.employees)
                writer.WriteLine(string.Join("\t", e.id, e.name, e.position,
                    e.birthDate.ToString("dd.MM.yy"), e.hireDate.ToString("dd.MM.yy"), e.phone, e.email));
            writer.Close();

            writer = new StreamWriter("credits.txt");
            foreach (var c in Credit.credits)
                writer.WriteLine(string.Join("\t", c.id, c.client.id, c.employee.id,
                    c.date.ToString("dd.MM.yy"), c.sum, c.time, c.percent));
            writer.Close();
        }

        public static string ReadLine(string text, bool allowEmpty = false)
        {
            bool success;
            string str;
            do
            {
                Console.Write(text);
                str = Console.ReadLine().Trim();
                success = !string.IsNullOrWhiteSpace(str) || allowEmpty;
                if (!success)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string('\0', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            } while (!success);
            return str;
        }
    }
}
