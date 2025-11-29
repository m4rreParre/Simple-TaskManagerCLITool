using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TaskManagerCLI;

public class Program
{
    public static List<Task> tasks = new List<Task>();

    public class Task
    {
        public static void IdChange(int newId)
        {
            nextID = newId;
        }

        private static int nextID = 1;
        public int TaskID { get; private set; }
        public string WhatToDo { get; set; }
        public bool Finished { get; set; }

        public Task(string whatToDo)
        {
            TaskID = nextID;
            nextID++;
            WhatToDo = whatToDo;
        }

        public void ToggleTaskState()
        {
            if (Finished == true)
            {
                Finished = false;
            }
            else
            {
                Finished = true;
            }
        }
    }

    public static void EditTask(int ID, string whatToDo)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].TaskID == ID)
            {
                tasks[i].WhatToDo = whatToDo;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task was changed!");
                Console.ResetColor();
            }
        }
    }

    public static void ToggleTaskState(int ID)
    {
        foreach (Task task in tasks)
        {
            if (task.TaskID == ID)
            {
                task.ToggleTaskState();
            }
        }
    }

    public static void AddTask(string whatToDo)
    {
        tasks.Add(new Task(whatToDo));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Task was added!");
        Console.ResetColor();
    }

    public static void RemoveTask(int ID)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].TaskID == ID)
            {
                tasks.RemoveAt(i);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Task was Removed!");
                Console.ResetColor();
                return;
            }
        }
    }

    public static void SaveTasks()
    {
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        string filePath = "tasks.json";
        File.WriteAllText(filePath, json);
    }

    public static void LoadTasks()
    {
        string filePath = "tasks.json";

        if (!File.Exists(filePath))
        {
            // Create empty tasks list and save it
            tasks = new List<Task>();
            SaveTasks();
            return;
        }

        string json = File.ReadAllText(filePath);
        tasks = JsonConvert.DeserializeObject<List<Task>>(json);
        int currentID = LoadID();
        Task.IdChange(currentID);
    }

    static int LoadID()
    {
        int id = 0;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].TaskID > id)
            {
                id = tasks[i].TaskID;
            }
        }
        return id + 1;
    }

    public static void WriteTasks(List<Task> tasks, int selectedIndex)
    {
        string tasksString = "";

        for (int i = 0; i < tasks.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                if (tasks[i].Finished)
                {
                    Console.WriteLine($"[x] {tasks[i].TaskID}: {tasks[i].WhatToDo}\n\r");
                }
                else
                {
                    Console.WriteLine($"[ ] {tasks[i].TaskID}: {tasks[i].WhatToDo}\n\r");
                }
                Console.ResetColor();
            }
            else
            {
                if (tasks[i].Finished)
                {
                    Console.WriteLine($"[x] {tasks[i].TaskID}: {tasks[i].WhatToDo}");
                }
                else
                {
                    Console.WriteLine($"[ ] {tasks[i].TaskID}: {tasks[i].WhatToDo}");
                }
            }
        }
    }
}
