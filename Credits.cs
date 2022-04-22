using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credits
{
    static class Credits
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
                Console.WriteLine("1 - Добавить кредит");
                Console.WriteLine("2 - Редактировать кредит");
                Console.WriteLine("3 - Удалить кредит");
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
            string[] columns = new string[] { "ID", "Клиент", "Сотрудник", "Дата оформления", "Сумма кредита", "Срок (мес.)", "Процентная ставка" };
            int[] widths = new int[columns.Length];
            table.Clear();
            for (int i = 0; i < columns.Length; i++)
                widths[i] = columns[i].Length;

            int max;
            foreach (var c in Credit.credits)
            {
                if (c.id.ToString().Length > widths[0])
                    widths[0] = c.id.ToString().Length;
                if (c.client.ToString().Length > widths[1])
                    widths[1] = c.client.ToString().Length;
                if (c.employee.ToString().Length > widths[2])
                    widths[2] = c.employee.ToString().Length;
                max = c.date.ToString("dd.MM.yyyy").Length;
                if (max > widths[3])
                    widths[3] = max;
                if (c.sum.ToString().Length > widths[4])
                    widths[4] = c.sum.ToString().Length;
                if (c.time.ToString().Length > widths[5])
                    widths[5] = c.time.ToString().Length;
                if (c.percent.ToString().Length > widths[6])
                    widths[6] = c.percent.ToString().Length;
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

            foreach (var c in Credit.credits)
            {
                data[0] = (c.id.ToString(), widths[0]);
                data[1] = (c.client.ToString(), widths[1]);
                data[2] = (c.employee.ToString(), widths[2]);
                data[3] = (c.date.ToString("dd.MM.yyyy"), widths[3]);
                data[4] = (c.sum.ToString(), widths[4]);
                data[5] = (c.time.ToString(), widths[5]);
                data[6] = (c.percent.ToString(), widths[6]);
                table.Add(string.Join("", data.Select(f => f.text.PadRight(f.width))));
            }
        }

        public static bool Add()
        {
            string clientStr, employeeStr, dateStr, sumStr, timeStr, percentStr;
            int client = 0;
            int employee = 0;
            DateTime date = new DateTime();
            decimal sum = 0;
            int time = 0;
            decimal percent = 0;
            bool success;

            Console.Clear();
            Clients.table.ForEach(t => Console.WriteLine(t));
            do
            {
                clientStr = Program.ReadLine("Введите ID клиента: ");

                success = int.TryParse(clientStr, out client) &&
                    Client.clients.Any(t => t.id == client);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            Console.Clear();
            Employees.table.ForEach(t => Console.WriteLine(t));
            do
            {
                employeeStr = Program.ReadLine("Введите ID сотрудника: ");

                success = int.TryParse(employeeStr, out employee) &&
                    Employee.employees.Any(t => t.id == employee);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                dateStr = Program.ReadLine("Введите дату оформления кредита: ");

                success = DateTime.TryParse(dateStr, out date);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                sumStr = Program.ReadLine("Введите сумму кредита: ");

                success = decimal.TryParse(sumStr, out sum);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                timeStr = Program.ReadLine("Введите срок кредита: ");

                success = int.TryParse(timeStr, out time);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                percentStr = Program.ReadLine("Введите процентную ставку кредита: ");

                success = decimal.TryParse(percentStr, out percent);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            Console.Clear();
            Console.WriteLine("Клиент: {0}", Client.clients.First(t => t.id == client));
            Console.WriteLine("Сотрудник: {0}", Employee.employees.First(t => t.id == employee));
            Console.WriteLine("Дата офрмления: {0}", date.ToString("dd.MM.yyyy"));
            Console.WriteLine("Сумма: {0}", sum);
            Console.WriteLine("Срок: {0}", time);
            Console.WriteLine("Процентная ставка: {0}", percent);
            Console.WriteLine("Добавить данный кредит?");
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

            Credit.credits.Add(new Credit
            {
                id = Credit.credits.Count > 0 ? Credit.credits.Last().id + 1 : 0,
                client = Client.clients.First(t => t.id == client),
                employee = Employee.employees.First(t => t.id == employee),
                date = date,
                sum = sum,
                time = time,
                percent = percent,
            });

            return true;
        }

        public static bool Edit()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();

            Console.Write("Введите ID кредита, который необходимо отредактировать: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Credit c;
            if (!success || (c = Credit.credits.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Console.Clear();

            string clientStr, employeeStr, dateStr, sumStr, timeStr, percentStr;
            int client = 0;
            int employee = 0;
            DateTime date = new DateTime();
            decimal sum = 0;
            int time = 0;
            decimal percent = 0;

            Console.Clear();
            Clients.table.ForEach(t => Console.WriteLine(t));
            do
            {
                clientStr = Program.ReadLine($"Введите ID клиента ({c.client.id}): ", true);
                if (string.IsNullOrWhiteSpace(clientStr))
                    break;

                success = int.TryParse(clientStr, out client) &&
                    Client.clients.Any(t => t.id == client);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            Console.Clear();
            Employees.table.ForEach(t => Console.WriteLine(t));
            do
            {
                employeeStr = Program.ReadLine($"Введите ID сотрудника ({c.employee.id}): ", true);
                if (string.IsNullOrWhiteSpace(clientStr))
                    break;

                success = int.TryParse(employeeStr, out employee) &&
                    Employee.employees.Any(t => t.id == employee);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                dateStr = Program.ReadLine($"Введите дату оформления кредита ({c.date.ToString("dd.MM.yyyy")}): ", true);
                if (string.IsNullOrWhiteSpace(dateStr))
                    break;

                success = DateTime.TryParse(dateStr, out date);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                sumStr = Program.ReadLine($"Введите сумму кредита ({c.sum}): ", true);
                if (string.IsNullOrWhiteSpace(sumStr))
                    break;

                success = decimal.TryParse(sumStr, out sum);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                timeStr = Program.ReadLine($"Введите срок кредита ({c.time}): ", true);
                if (string.IsNullOrWhiteSpace(timeStr))
                    break;

                success = int.TryParse(timeStr, out time);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            do
            {
                percentStr = Program.ReadLine($"Введите процентную ставку кредита ({c.percent}): ", true);
                if (string.IsNullOrWhiteSpace(percentStr))
                    break;

                success = decimal.TryParse(percentStr, out percent);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);

            Console.Clear();
            Console.WriteLine("Клиент: {0}", string.IsNullOrWhiteSpace(clientStr) ? 
                c.client : Client.clients.First(t => t.id == client));
            Console.WriteLine("Сотрудник: {0}", string.IsNullOrWhiteSpace(employeeStr) ? 
                c.employee : Employee.employees.First(t => t.id == employee));
            Console.WriteLine("Дата офрмления: {0}", string.IsNullOrWhiteSpace(dateStr) ? 
                c.date.ToString("dd.mm.yyyy") : date.ToString("dd.MM.yyyy"));
            Console.WriteLine("Сумма: {0}", string.IsNullOrWhiteSpace(sumStr) ? c.sum : sum);
            Console.WriteLine("Срок: {0}", string.IsNullOrWhiteSpace(timeStr) ? c.time : time);
            Console.WriteLine("Процентная ставка: {0}", string.IsNullOrWhiteSpace(percentStr) ? c.percent : percent);
            Console.WriteLine("Сохранить данный кредит?");
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

            if (!string.IsNullOrWhiteSpace(clientStr))
                c.client = Client.clients.First(t => t.id == client);
            if (!string.IsNullOrWhiteSpace(employeeStr))
                c.employee = Employee.employees.First(t => t.id == employee);
            if (!string.IsNullOrWhiteSpace(dateStr)) c.date = date;
            if (!string.IsNullOrWhiteSpace(sumStr)) c.sum = sum;
            if (!string.IsNullOrWhiteSpace(timeStr)) c.time = time;
            if (!string.IsNullOrWhiteSpace(percentStr)) c.percent = percent;

            return true;
        }

        public static bool Remove()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();
            Console.Write("Введите ID кредита, который необходимо удалить: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Credit c;
            if (!success || (c = Credit.credits.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Credit.credits.Remove(c);
            return true;
        }
    }
}
