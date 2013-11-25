using System;
using System.Data.Entity;
using System.Linq;

namespace GenericsCovarianceContravariance
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<EmployeeDb>());

            using (var employeeRepository = new SqlRepository<Employee>(new EmployeeDb()))
            {
                AddEmployees(employeeRepository);
                AddManagers(employeeRepository);
                CountEmployees(employeeRepository);
                QueryEmployees(employeeRepository);
                DumpPeople(employeeRepository);
            }

            Console.ReadKey();
        }

        private static void AddManagers(IWriteOnlyRepository<Manager> employeeRepository)
        {
            var manager = new Manager { Name = "Larry" };
            employeeRepository.Add(manager);
            employeeRepository.Commit();
        }

        private static void DumpPeople(IReadOnlyRepository<Person> repository)
        {
            var persons = repository.FindAll();
            foreach (var person in persons)
            {
                Console.WriteLine(person.Name);
            }
        }

        private static void QueryEmployees(IRepository<Employee> employeeRepository)
        {
            var employee = employeeRepository.Find(1);
            Console.WriteLine(employee.Name);
        }

        private static void CountEmployees(IRepository<Employee> employeeRepository)
        {
            Console.WriteLine(employeeRepository.FindAll().Count());
        }

        private static void AddEmployees(IRepository<Employee> employeeRepository)
        {
            employeeRepository.Add(new Employee { Name = "Steve"});
            employeeRepository.Add(new Employee { Name = "Bill" });
            employeeRepository.Commit();
        }
    }
}
