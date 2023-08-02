using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_m7._1 {
    internal class Program {
        static void Main(string[] args) {

            int userChoice = 0;
            bool userInputIsNum;

            Repository rep = new Repository();
            rep.CreateFile();

            while (true) {
                Console.WriteLine("Добро пожаловать в справочник сотрудников v2.0\n");
                Console.WriteLine("Добавить сотрудника - 1");
                Console.WriteLine("Вывести список всех сотрудников - 2");
                Console.WriteLine("Поиск сотрудника по ID - 3");
                Console.WriteLine("Удалить сотрудника из базы - 4");
                Console.WriteLine("Поиск по дате добавления - 5");
                Console.WriteLine("Для выхода из программы - 0");
                Console.Write("Ваш выбор: ");
                userInputIsNum = int.TryParse(Console.ReadLine(), out userChoice);
                Console.WriteLine();

                if (userInputIsNum && userChoice == 1) {
                    Worker newWorker = new Worker();
                    newWorker.creationDate = DateTime.Now;
                    Console.Write("Введите ФИО: ");
                    newWorker.fullName = Console.ReadLine();
                    Console.Write("Введите возраст: ");
                    newWorker.age = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите рост: ");
                    newWorker.height = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Введите дату рождения: ");
                    newWorker.birthdate = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Введите место рождения: ");
                    newWorker.birthplace = Console.ReadLine();

                    rep.AddWorker(newWorker);
                    RefreshProgramWindow("Сотрудник добавлен, нажмите Enter для возврата в меню");
                }

                if (userInputIsNum && userChoice == 2) {
                    Worker[] workersToPrint = rep.GetAllWorkers();
                    Worker[] sortedArray = SortBy(workersToPrint);

                    if (workersToPrint.Length == 0) {
                        RefreshProgramWindow("Файл пуст. Нажмите Enter для возврата в меню");
                    } else {
                        rep.PrintToConsole(sortedArray);
                        RefreshProgramWindow("Нажмите Enter для возврата в меню");
                    }
                }

                if (userInputIsNum && userChoice == 3) {
                    Console.Write("\nВведите ID сотрудника: ");
                    Worker workerToPrint = rep.GetWorkerById(Convert.ToInt32(Console.ReadLine()));
                    if(workerToPrint.id == 0) {
                        RefreshProgramWindow("Сотрудник с данным ID не найден в базе, возможно он был удалён. Нажмите Enter для возврата в меню");
                    }
                    else {
                        rep.PrintToConsole(workerToPrint);
                        RefreshProgramWindow("Нажмите Enter для возврата в меню");
                    }
                }

                if (userInputIsNum && userChoice == 4) {
                    Console.Write("\nВведите ID сотрудника: ");
                    int workerToDelete = Convert.ToInt32(Console.ReadLine());
                    rep.DeleteWorker(workerToDelete);
                    RefreshProgramWindow($"Сотрудник {workerToDelete} удалён из базы, нажмите Enter для возврата в меню");
                }

                if (userInputIsNum && userChoice == 5) {
                    DateTime dateFrom, dateTo;
                    Console.Write("Введите дату начала: ");
                    dateFrom = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Введите конечную дату: ");
                    dateTo = Convert.ToDateTime(Console.ReadLine());

                    Worker[] workersToPrint = rep.GetWorkersBetweenTwoDates(dateFrom, dateTo);
                    Worker[] sortedArray = SortBy(workersToPrint);

                    if (workersToPrint.Length == 0) {
                        RefreshProgramWindow("Файл пуст. Нажмите Enter для возврата в меню");
                    } else {
                        rep.PrintToConsole(sortedArray);
                        RefreshProgramWindow("Нажмите Enter для возврата в меню");
                    }

                    RefreshProgramWindow("Нажмите Enter для возврата в меню");
                }

                if (userInputIsNum && userChoice == 0) {
                    break;
                }

                if (!userInputIsNum || userChoice > 5) {
                    RefreshProgramWindow("Неверная комманда, нажмите Enter для возврата в меню");
                }

            }

            void RefreshProgramWindow(string str) {
                Console.WriteLine("\n" + str);
                Console.ReadKey();
                Console.Clear();
            }

            Worker[] SortBy(Worker[] arrayToSort) {
                int sort = 0;
                Console.WriteLine("Укажите сортировку");
                Console.WriteLine("По дате добавления - 1");
                Console.WriteLine("По ФИО - 2");
                Console.WriteLine("По возрасту - 3");
                Console.WriteLine("По росту - 4");
                Console.WriteLine("По дате рождения - 5");
                Console.WriteLine("По месту рождения - 6");
                Console.WriteLine("По умолчанию - 0");
                Console.Write("Ваш выбор: ");
                sort = int.Parse(Console.ReadLine());

                switch (sort) {
                    case 1:
                        Array.Sort(arrayToSort, (x, y) => x.creationDate.CompareTo(y.creationDate));
                        break;
                    case 2:
                        Array.Sort(arrayToSort, (x, y) => x.fullName.CompareTo(y.fullName));
                        break;
                    case 3:
                        Array.Sort(arrayToSort, (x, y) => x.age.CompareTo(y.age));
                        break;
                    case 4:
                        Array.Sort(arrayToSort, (x, y) => x.height.CompareTo(y.height));
                        break;
                    case 5:
                        Array.Sort(arrayToSort, (x, y) => x.birthdate.CompareTo(y.birthdate));
                        break;
                    case 6:
                        Array.Sort(arrayToSort, (x, y) => x.birthplace.CompareTo(y.birthplace));
                        break;
                    case 0:
                    default:
                        break;
                }
                return arrayToSort;
            }

        }
    }
}
