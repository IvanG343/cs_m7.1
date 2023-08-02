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
        /// <returns>ID for the next record</returns>
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
        /// <returns>An array of Worker struct</returns>
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
        /// <param name="id">ID of the target worker</param>
        /// <returns>Worker</returns>
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
        /// Runs GetAllWorkers and copies all the data to the temp array excluding a worker to remove, then writes temp array to the file
        /// </summary>
        /// <param name="id">ID of the worker to remove</param>
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
        /// <returns>A filtered array of Worker struct</returns>
        public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo) {
            Worker[] workers = GetAllWorkers();
            Worker[] tempWorkers = new Worker[0];

            foreach(Worker worker in workers) {
                if(worker.creationDate >= dateFrom && worker.creationDate <= dateTo.AddDays(1)) {
                    Array.Resize(ref tempWorkers, tempWorkers.Length + 1);
                    tempWorkers[tempWorkers.Length - 1] = worker;
                }
            }
            return tempWorkers;
        }

        /// <summary>
        /// ReWrites the file with the new data
        /// </summary>
        /// <param name="workers">Array of struct Worker</param>
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

    }

}
