using System;
using System.Collections.Generic;
using System.IO;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace PandaCuteOS
{
    public class Command
    {
        public string Name { get; }
        public Action<string[]> Execute { get; }

        public Command(string name, Action<string[]> execute)
        {
            Name = name;
            Execute = execute;
        }
    }

    public class CommandManager
    {
        private readonly Dictionary<string, Command> _commands;

        public CommandManager()
        {
            _commands = new Dictionary<string, Command>();
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            RegisterCommand("cd", ChangeDirectory);
            RegisterCommand("del", DeleteFile);
        }

        private void RegisterCommand(string name, Action<string[]> execute)
        {
            _commands.Add(name, new Command(name, execute));
        }

        private void ChangeDirectory(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: cd [directory]");
                return;
            }

            string path = args[1];
            Directory.SetCurrentDirectory(path);
            Console.WriteLine($"Changed directory to: {path}");
        }

        private void DeleteFile(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: del [file]");
                return;
            }

            string file = args[1];
            if (File.Exists(file))
            {
                File.Delete(file);
                Console.WriteLine($"Deleted file: {file}");
            }
            else
            {
                Console.WriteLine($"File not found: {file}");
            }
        }

        public void ExecuteCommand(string input)
        {
            string[] parts = input.Split(' ');
            if (parts.Length == 0)
            {
                return;
            }

            string commandName = parts[0];
            if (_commands.TryGetValue(commandName, out Command command))
            {
                command.Execute(parts);
            }
            else
            {
                Console.WriteLine($"Command not found: {commandName}");
            }
        }

        public void Help()
        {
            Console.WriteLine("Available commands:");
            foreach (var command in _commands.Values)
            {
                Console.WriteLine($"- {command.Name}");
            }
        }
    }

    public class Kernel : Cosmos.System.Kernel
    {
        private CommandManager _commandManager;

        private const string OsName = "PandaCute OS";
        private const string OsVersion = "1.0.0";
        private const string Copyright = "Copyright 2024 by PT Media Pengembangan Teknologi Indonesia Jaya";
        private const string DevDate = "Developed date 13 June 2024";

        protected override void BeforeRun()
        {
            DisplayAsciiArt();
            Console.WriteLine("Welcome to Panda OS ^-^");
            Console.WriteLine("We have 2 variant models of OS:");
            Console.WriteLine("1. PandaCute OS");
            Console.WriteLine("2. PandaKiller OS");
            Console.WriteLine("You've chosen PandaCute OS");
            Console.WriteLine("PandaCute OS supports the Python programming language");
            Console.WriteLine("Type 'help' for details commands.");
            _commandManager = new CommandManager();
            SetOSName();
            SetOSVersion();
            SetOSCopyright();
            SetOSDevDate();
        }

        private void DisplayAsciiArt()
        {
            try
            {
                string path = @"C:\Folder Download Sementara\Data Perusahaan Gue\Project Teknologi Perusahaan\Panda-OS\Panda-Cute\PandaCute-OS\PandaCute-OS\ASCII_ART\pandacuteos.txt";
                if (File.Exists(path))
                {
                    string asciiArt = File.ReadAllText(path);
                    Console.WriteLine(asciiArt);
                }
                else
                {
                    Console.WriteLine("ASCII art file not found at: " + path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading ASCII art file: " + ex.Message);
            }
        }

        private void SetOSName()
        {
            Console.WriteLine($"Operating System: {OsName}");
        }

        private void SetOSVersion()
        {
            Console.WriteLine($"Version: {OsVersion}");
        }

        private void SetOSCopyright()
        {
            Console.WriteLine($"{Copyright}");
        }

        private void SetOSDevDate()
        {
            Console.WriteLine($"{DevDate}");
        }

        protected override void Run()
        {
            Console.WriteLine("Before using, please input your name for kernel access!");
            Console.Write("Your name: ");
            var user = Console.ReadLine();
            Console.WriteLine($"Hello, {user}!");
            Console.WriteLine("Thank you for using PandaCute OS Beta Version 1 ^-^");
            Console.WriteLine("For more information, please visit the Facebook page of PT Media Pengembangan Teknologi Indonesia Jaya");
            Console.WriteLine("And visit our GitHub Organization: https://github.com/Tmd-Group-ID");
            Console.Write(">");
            string input = Console.ReadLine();
            _commandManager.ExecuteCommand(input);
        }
    }
}
