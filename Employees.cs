using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credits
{
    static class Employees
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
                Console.WriteLine("1 - Добавить сотрудника");
                Console.WriteLine("2 - Редактировать сотрудника");
                Console.WriteLine("3 - Удалить сотрудника");
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
            string[] columns = new string[] { "ID", "ФИО", "Должность", "Дата рождения", "Дата найма", "Номер телефона", "Эл. почта" };
            int[] widths = new int[columns.Length];
            table.Clear();
            for (int i = 0; i < columns.Length; i++)
                widths[i] = columns[i].Length;

            int max;
            foreach (var c in Employee.employees)
            {
                if (c.id.ToString().Length > widths[0])
                    widths[0] = c.id.ToString().Length;
                if (c.name.Length > widths[1])
                    widths[1] = c.name.Length;
                if (c.position.Length > widths[2])
                    widths[2] = c.position.Length;
                max = c.birthDate.ToString("dd.MM.yyyy").Length;
                if (max > widths[3])
                    widths[3] = max;
                max = c.hireDate.ToString("dd.MM.yyyy").Length;
                if (max > widths[4])
                    widths[4] = max;
                if (c.phone.Length > widths[5])
                    widths[5] = c.phone.Length;
                if (c.email.Length > widths[6])
                    widths[6] = c.email.Length;
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

            foreach (var c in Employee.employees)
            {
                data[0] = (c.id.ToString(), widths[0]);
                data[1] = (c.name, widths[1]);
                data[2] = (c.position, widths[2]);
                data[3] = (c.birthDate.ToString("dd.MM.yyyy"), widths[3]);
                data[4] = (c.hireDate.ToString("dd.MM.yyyy"), widths[4]);
                data[5] = (c.phone, widths[5]);
                data[6] = (c.email, widths[6]);
                table.Add(string.Join("", data.Select(f => f.text.PadRight(f.width))));
            }
        }

        public static bool Add()
        {
            string name, position, birthStr, hireStr, phone, email;
            DateTime birthDate = new DateTime();
            DateTime hireDate = new DateTime();
            bool success;

            name = Program.ReadLine("Введите ФИО сотрудника: ");
            position = Program.ReadLine("Введите должность сотрудника: ");
            do
            {
                birthStr = Program.ReadLine("Введите дату рождения сотрудника: ");

                success = DateTime.TryParse(birthStr, out birthDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            do
            {
                hireStr = Program.ReadLine("Введите дату найма сотрудника: ");

                success = DateTime.TryParse(hireStr, out hireDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            phone = Program.ReadLine("Введите номер телефона сотрудника: ");
            email = Program.ReadLine("Введите эл. почту сотрудника: ");

            Console.Clear();
            Console.WriteLine("ФИО: {0}", name);
            Console.WriteLine("Должность: {0}", position);
            Console.WriteLine("Дата рождения: {0}", birthDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Дата найма: {0}", hireDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Номер телефона: {0}", phone);
            Console.WriteLine("Эл. почта: {0}", email);
            Console.WriteLine("Добавить данного сотрудника?");
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

            Employee.employees.Add(new Employee
            {
                id = Employee.employees.Count > 0 ? Employee.employees.Last().id + 1 : 0,
                name = name,
                position = position,
                birthDate = birthDate,
                hireDate = hireDate,
                phone = phone,
                email = email,
            });
            return true;
        }

        public static bool Edit()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();

            Console.Write("Введите ID сотрудника, которого необходимо отредактировать: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Employee e;
            if (!success || (e = Employee.employees.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Console.Clear();

            string name, position, birthStr, hireStr, phone, email;
            DateTime birthDate = new DateTime();
            DateTime hireDate = new DateTime();

            name = Program.ReadLine($"Введите ФИО сотрудника ({e.name}): ", true);
            position = Program.ReadLine($"Введите должность сотрудника ({e.position}): ", true);
            do
            {
                birthStr = Program.ReadLine($"Введите дату рождения сотрудника ({e.birthDate.ToString("dd.MM.yyyy")}): ", true);
                if (string.IsNullOrWhiteSpace(birthStr))
                    break;

                success = DateTime.TryParse(birthStr, out birthDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            do
            {
                hireStr = Program.ReadLine($"Введите дату найма сотрудника ({e.hireDate.ToString("dd.MM.yyyy")}): ", true);
                if (string.IsNullOrWhiteSpace(hireStr))
                    break;

                success = DateTime.TryParse(hireStr, out hireDate);
                if (!success)
                    Console.WriteLine("Ошибка ввода");
            } while (!success);
            phone = Program.ReadLine($"Введите номер телефона сотрудника ({e.phone}): ", true);
            email = Program.ReadLine($"Введите эл. почту сотрудника ({e.email}): ", true);

            Console.Clear();
            Console.WriteLine("ФИО: {0}", string.IsNullOrWhiteSpace(name) ? e.name : name);
            Console.WriteLine("Должность: {0}", string.IsNullOrWhiteSpace(position) ? e.position : position);
            Console.WriteLine("Дата рождения: {0}", string.IsNullOrWhiteSpace(birthStr) ? 
                e.birthDate.ToString("dd.MM.yyyy") : birthDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Дата найма: {0}", string.IsNullOrWhiteSpace(hireStr) ? 
                e.hireDate.ToString("dd.MM.yyyy") : hireDate.ToString("dd.MM.yyyy"));
            Console.WriteLine("Номер телефона: {0}", string.IsNullOrWhiteSpace(phone) ? e.phone : phone);
            Console.WriteLine("Эл. почта: {0}", string.IsNullOrWhiteSpace(email) ? e.email : email);
            Console.WriteLine("Сохранить данного сотрудника?");
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

            if (!string.IsNullOrWhiteSpace(name)) e.name = name;
            if (!string.IsNullOrWhiteSpace(position)) e.position = position;
            if (!string.IsNullOrWhiteSpace(birthStr)) e.birthDate = birthDate;
            if (!string.IsNullOrWhiteSpace(hireStr)) e.hireDate = hireDate;
            if (!string.IsNullOrWhiteSpace(phone)) e.phone = phone;
            if (!string.IsNullOrWhiteSpace(email)) e.email = email;

            return true;
        }

        public static bool Remove()
        {
            table.ForEach(t => Console.WriteLine(t));
            Console.WriteLine();
            Console.Write("Введите ID сотрудника, которого необходимо удалить: ");
            int id;
            bool success = int.TryParse(Console.ReadLine(), out id);
            Employee c;
            if (!success || (c = Employee.employees.FirstOrDefault(t => t.id == id)) == null)
                return false;

            Credit.credits.RemoveAll(t => t.employee.id == t.id);
            Employee.employees.Remove(c);
            return true;
        }
    }
}
