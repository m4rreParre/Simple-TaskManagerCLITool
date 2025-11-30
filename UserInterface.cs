using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TaskManagerCLI;

class UserInterface
{
    public static int MenuHandler()
    {
        int selectedIndex = 0;
        ConsoleKey key;
        Console.CursorVisible = false;
        do
        {
            // Clears the console
            Console.Write("\x1b[2J\x1b[H\x1b[2J\x1b[H");
            Program.WriteTasks(Program.tasks, selectedIndex);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            selectedIndex = KeyPressHandler(key, selectedIndex);
            Program.SaveTasks();

        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        // TODO: Are you doing something with this return value?
        return selectedIndex;
    }

    static int KeyPressHandler(ConsoleKey key, int selectedIndex)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = Program.tasks.Count - 1; // wrapping
                    return selectedIndex;
                }
                break;
            case ConsoleKey.DownArrow:
                selectedIndex++;
                if (selectedIndex >= Program.tasks.Count)
                {
                    selectedIndex = 0; // wrapping
                    return selectedIndex;
                }
                break;
            case ConsoleKey.Spacebar:
                Program.ToggleTaskState(Program.tasks[selectedIndex].TaskID);
                break;
        }
        return selectedIndex;
    }

    static void Main(string[] args)
    {
        Program.LoadTasks();
        if (args.Length == 0)
        {
            if (Program.tasks.Count <= 0)
            {
                Console.WriteLine("Ohh noo you have an empty task list:(");
                Console.WriteLine("Try adding some tasks - use <tasks help> to get help");
            }
            else
            {
                MenuHandler();
            }
        }
        else
        {
            string command = args[0].ToLower();
            switch (command)
            {
                case "--help":
                case "--h":
                case "help":
                    Console.WriteLine("  tasks              - Interactive menu");
                    Console.WriteLine("  tasks add <text>   - Add a task");
                    Console.WriteLine("  tasks remove <id>  - Remove a task");
                    Console.WriteLine("  tasks edit <id> <text> - Edit a task");
                    break;
                case "add":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Usage: tasks add <task description>");
                        return;
                    }
                    string taskText = string.Join(" ", args.Skip(1));
                    Program.AddTask(taskText);
                    Program.SaveTasks();
                    break;
                case "remove":
                    if (args.Length < 2 || !int.TryParse(args[1], out int removeId))
                    {
                        Console.WriteLine("Usage: tasks remove <id>");
                        return;
                    }
                    Program.RemoveTask(removeId);
                    Program.SaveTasks();
                    break;
                case "edit":
                    if (args.Length < 3 || !int.TryParse(args[1], out int editId))
                    {
                        Console.WriteLine("Usage: tasks edit <id> <edited task>");
                        return;
                    }
                    string newText = string.Join(" ", args.Skip(2));
                    Program.EditTask(editId, newText);
                    Program.SaveTasks();
                    break;

                default:
                    Console.WriteLine("Write tasks --help or tasks --h to show commands");
                    break;
            }
        }
    }
}
