using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credits
{
    class Client
    {
        public static List<Client> clients = new List<Client>();

        public int id;
        public string name;
        public DateTime birthDate;
        public string phone;
        public string passport;

        public override string ToString()
        {
            return $"{name}, {birthDate.ToString("yyyy")} г.р., паспорт {passport}";
        }
    }

    class Employee
    {
        public static List<Employee> employees = new List<Employee>();

        public int id;
        public string name;
        public string position;
        public DateTime birthDate;
        public DateTime hireDate;
        public string phone;
        public string email;

        public override string ToString()
        {
            return $"{position} {name}";
        }
    }

    class Credit
    {
        public static List<Credit> credits = new List<Credit>();

        public int id;
        public Client client;
        public Employee employee;
        public DateTime date;
        public decimal sum;
        public int time;
        public decimal percent;
    }
}
