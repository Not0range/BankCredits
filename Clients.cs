using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credits
{
    static class Clients
    {
        public static List<string> table = new List<string>();

        public static void EnterMenu()
        {
            var changed = false;
            var error = false;
            ConsoleKeyInfo key;

            int prevBuffSize = Console.BufferWidth;
            int max = table.Max(t => t.Length);
            if (max > Console.BufferWidth)
                Console.BufferWidth = max;

            do
            {
                if (changed)
                {
                    FillTable();
                    max = table.Max(t => t.Length);
                    if (max > Console.BufferWidth)
                        Console.BufferWidth = max;
                }

                table.ForEach(t => Console.WriteLine(t));

                Console.WriteLine();
                if (error)
                {
                    Console.WriteLine("Ошибка ввода");
                    error = false;
                }
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1 - Добавить клиента");
                Console.WriteLine("2 - Редактировать клиента");
                Console.WriteLine("3 - Удалить клиента");
                Console.WriteLine("4 - Вернуться в главное меню");
                key = Console.ReadKey(true);
                Console.Clear();
                switch (key.KeyChar)
                {
                    case '1':
                        changed = Add();
                        break;
                    case '2':
                        changed = Edit();
                        break;
                    case '3':
                        changed = Remove();
                        break;
                    default:
                        error = true;
                        break;
                }
                Console.Clear();
            } while (key.KeyChar != '4');
            Console.BufferWidth = prevBuffSize;
        }

        public static void FillTable()
        {
            string[] columns = new string[] { "ID", "ФИО", "Дата рождения", "Номер телефона", "Паспортные аднные" };
            int[] widths = new int[columns.Length];
            table.Clear();
            for (int i = 0; i < columns.Length; i++)
                widths[i] = columns[i].Length;

            int max;
            foreach (var c in Client.clients)
            {
                if (c.id.ToString().Length > widths[0])
                    widths[0] = c.id.ToString().Length;
                if (c.name.Length > widths[1])
                    widths[1] = c.name.Length;
                max = c.birthDate.ToString("dd.MM.yyyy").Length;
                if (max > widths[2])
                    widths[2] = max;
                if (c.phone.Length > widths[3])
                    widths[3] = c.phone.Length;
                if (c.passport.Length > widths[4])
                    widths[4] = c.passport.Length;
            }
            for (int i = 0; i < widths.Length; i++)
                widths[i] += 3;

            int w = widths.Sum() + Environment.NewLine.Length;
            if (w > Console.BufferWidth)
                Console.BufferWidth = widths.Sum() + Environment.NewLine.Length;

            var data = new (string text, int width)[columns.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = (columns[i], widths[i]);
            table.Add(string.Join("", data.Select(f => f.text.PadRight(f.width))));

            foreach (var c in Client.clients)
            {
                data[0] = (c.id.ToString(), widths[0]);
                data[1] = (c.name, widths[1]);
                data[2] = (c.birthDate.ToString("dd.MM.yyyy"), widths[2]);
                data[3] = (c.phone, widths[3]);
                data[4] = (c.passport, widths[4]);
                table.Add(string.Join("", data.Select(f => f.text.PadRight(f.width))));
            }
        }

        public static bool Add()
        {
            string name, birthStr, phone, passport;
            DateTime birthDate = new DateTime();
            bool success;

            name = Program.ReadLine("Введите ФИО клиента: ");
            do
            {
                birthStr = Program.ReadLine("Введите дату рождения клиента: ");

                success = DateTime.TryParse(birthStr, out birthDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            phone = Program.ReadLine("Введите номер телефона клиента: ");
            passport = Program.ReadLine("Введите паспортные данные клиента: ");

            Console.Clear();
            Console.WriteLine("ФИО: {0}", name);
            Console.WriteLine("Дата рождения: {0}", birthDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Номер телефона: {0}", phone);
            Console.WriteLine("Паспортные данные: {0}", passport);
            Console.WriteLine("Добавить данного клиента?");
            Console.WriteLine("1 - Да");
            Console.WriteLine("2 - Нет");
            do
            {
                var k = Console.ReadKey(true);
                if (k.KeyChar == '1')
                    break;
                else if (k.KeyChar == '2')
                    return false;

            } while (true);

            Client.clients.Add(new Client
            {
                id = Client.clients.Count > 0 ? Client.clients.Last().id + 1 : 0,
                name = name,
                birthDate = birthDate,
                phone = phone,
                passport = passport,
            });
            return true;
        }

        public static bool Edit()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();

            Console.Write("Введите ID клиента, которого необходимо отредактировать: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Client c;
            if (!success || (c = Client.clients.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Console.Clear();

            string name, birthStr, phone, passport;
            DateTime birthDate = new DateTime();

            name = Program.ReadLine($"Введите ФИО клиента ({c.name}): ", true);
            do
            {
                birthStr = Program.ReadLine($"Введите дату рождения клиента ({c.birthDate.ToString("dd.MM.yyyy")}): ", true);
                if (string.IsNullOrWhiteSpace(birthStr))
                    break;

                success = DateTime.TryParse(birthStr, out birthDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            phone = Program.ReadLine($"Введите номер телефона клиента ({c.phone}): ", true);
            passport = Program.ReadLine($"Введите паспортные данные клиента ({c.passport}): ", true);

            Console.Clear();
            Console.WriteLine("ФИО: {0}", string.IsNullOrWhiteSpace(name) ? c.name : name);
            Console.WriteLine("Дата рождения: {0}", string.IsNullOrWhiteSpace(birthStr) ? 
                c.birthDate.ToString("dd.MM.yyyy") : birthDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Номер телефона: {0}", string.IsNullOrWhiteSpace(phone) ? c.phone : phone);
            Console.WriteLine("Паспортные данные: {0}", string.IsNullOrWhiteSpace(passport) ? c.passport : passport);
            Console.WriteLine("Сохранить данного клиента?");
            Console.WriteLine("1 - Да");
            Console.WriteLine("2 - Нет");
            do
            {
                var k = Console.ReadKey(true);
                if (k.KeyChar == '1')
                    break;
                else if (k.KeyChar == '2')
                    return false;

            } while (true);

            if (!string.IsNullOrWhiteSpace(name)) c.name = name;
            if (!string.IsNullOrWhiteSpace(birthStr)) c.birthDate = birthDate;
            if (!string.IsNullOrWhiteSpace(phone)) c.phone = phone;
            if (!string.IsNullOrWhiteSpace(passport)) c.passport = passport;

            return true;
        }

        public static bool Remove()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();
            Console.Write("Введите ID клиента, которого необходимо удалить: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Client c;
            if (!success || (c = Client.clients.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Credit.credits.RemoveAll(t => t.client.id == t.id);
            Client.clients.Remove(c);
            return true;
        }
    }
}
