using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cs_m7._1 {

    class Repository {

        private string filePath = "db.txt";

        /// <summary>
        /// Creates a db file if doesn't exist
        /// </summary>
        public void CreateFile() {
            if(!File.Exists(this.filePath)) {
                File.Create(this.filePath).Close();
            }
        }

        /// <summary>
        /// Reads the file and returns next available ID
        /// </summary>
        /// <returns></returns>
        public int GetNextId() {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0) {
                return 1;
            } else {
                int lastId = 0;
                foreach(string line in lines) {
                    string[] row = line.Split('#');
                    int id = int.Parse(row[0]);
                    if(id > lastId) {
                        lastId = id;
                    }
                }
                return lastId + 1;
            }
            
        }

        /// <summary>
        /// Reads the file and returns all records as an array
        /// </summary>
        /// <returns></returns>
        public Worker[] GetAllWorkers() {
            string[] lines = File.ReadAllLines(filePath);
            Worker[] workers = new Worker[lines.Length];

            for (int i = 0; i < lines.Length; i++) {
                string[] data = lines[i].Split('#');
                workers[i] = new Worker {
                    id = int.Parse(data[0]),
                    creationDate = DateTime.Parse(data[1]),
                    fullName = data[2],
                    age = int.Parse(data[3]),
                    height = int.Parse(data[4]),
                    birthdate = DateTime.Parse(data[5]),
                    birthplace = data[6]
                };
            }

            return workers;
        }

        /// <summary>
        /// Runs GetAllWorkers and returns an employee by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Worker GetWorkerById(int id) {
            string[] lines = File.ReadAllLines(filePath);
            foreach(string line in lines) {
                string[] row = line.Split('#');
                if (int.Parse(row[0]) == id) {
                    Worker worker = new Worker();
                    worker.id = int.Parse(row[0]);
                    worker.creationDate = DateTime.Parse(row[1]);
                    worker.fullName = row[2];
                    worker.age = int.Parse(row[3]);
                    worker.height = int.Parse(row[4]);
                    worker.birthdate = DateTime.Parse(row[5]);
                    worker.birthplace = row[6];
                    return worker;
                }
            }
            return new Worker();

            //Worker[] workers = GetAllWorkers();
            //return workers[id - 1];
        }

        /// <summary>
        /// Runs GetAllWorkers and copies all the data to the temp array excluding an employee to remove, then writes temp array to the file
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorker(int id) {
            Worker[] workers = GetAllWorkers();
            Worker[] tempWorkers = new Worker[workers.Length - 1];

            int index = 0;
            for (int i = 0; i < workers.Length; i++) {
                if (workers[i].id != id) {
                    tempWorkers[index] = workers[i];
                    index++;
                }
            }

            SaveWorkersToFile(tempWorkers);
        }

        /// <summary>
        /// Write a Worker to the db file
        /// </summary>
        /// <param name="worker"></param>
        public void AddWorker(Worker worker) {
            worker.id = GetNextId();
            worker.creationDate = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            sb.Append(worker.id + "#");
            sb.Append(worker.creationDate.ToString() + "#");
            sb.Append(worker.fullName + "#");
            sb.Append(worker.age.ToString() + "#");
            sb.Append(worker.height.ToString() + "#");
            sb.Append(worker.birthdate.ToString() + "#");
            sb.Append(worker.birthplace);

            using (StreamWriter sw = new StreamWriter(filePath, true)) {
                sw.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Reads the file and returns filtered array
        /// </summary>
        /// <param name="dateFrom">Start date</param>
        /// <param name="dateTo">End date</param>
        /// <returns></returns>
        public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo) {
            Worker[] workers = GetAllWorkers();
            Worker[] tempWorkers = new Worker[0];

            foreach(Worker worker in workers) {
                if(worker.creationDate >= dateFrom && worker.creationDate <= dateTo) {
                    Array.Resize(ref tempWorkers, tempWorkers.Length + 1);
                    tempWorkers[tempWorkers.Length - 1] = worker;
                }
            }
            return tempWorkers;
        }

        /// <summary>
        /// ReWrites the file with the new data
        /// </summary>
        /// <param name="workers"></param>
        private void SaveWorkersToFile(Worker[] workers) {
            if (File.Exists(filePath))
                File.Delete(filePath);

            for (int i = 0; i < workers.Length; i++) {
                StringBuilder sb = new StringBuilder();
                sb.Append(workers[i].id + "#");
                sb.Append(workers[i].creationDate.ToString() + "#");
                sb.Append(workers[i].fullName + "#");
                sb.Append(workers[i].age.ToString() + "#");
                sb.Append(workers[i].height.ToString() + "#");
                sb.Append(workers[i].birthdate.ToString() + "#");
                sb.Append(workers[i].birthplace);

                using (StreamWriter sw = new StreamWriter(filePath, true)) {
                    sw.WriteLine(sb.ToString());
                }
            }
        }

        /// <summary>
        /// Print a Worker[] array to the console
        /// </summary>
        public void PrintToConsole(Worker[] workers) {
            foreach (var worker in workers) {
                Console.WriteLine($"{worker.id,5} " +
                    $"{worker.creationDate.ToShortDateString(),15} " +
                    $"{worker.fullName,30} " +
                    $"{worker.age,7} " +
                    $"{worker.height,7} " +
                    $"{worker.birthdate.ToShortDateString(),15} " +
                    $"{worker.birthplace,15}");
            }
        }

        /// <summary>
        /// Print a Worker to the console
        /// </summary>
        /// <param name="worker"></param>
        public void PrintToConsole(Worker worker) {
            Console.WriteLine($"{worker.id,5} " +
                $"{worker.creationDate.ToShortDateString(),15} " +
                $"{worker.fullName,30} " +
                $"{worker.age,7} " +
                $"{worker.height,7} " +
                $"{worker.birthdate.ToShortDateString(),15} " +
                $"{worker.birthplace,15}");
        }

        /// <summary>
        /// Sort an array by a field
        /// </summary>
        /// <param name="arrayToSort"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public Worker[] SortBy(Worker[] arrayToSort) {
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
