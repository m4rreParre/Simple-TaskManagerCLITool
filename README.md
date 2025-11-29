# Easy-TaskManagerCLI
ITS NOT A OS TASK MANAGER ITS AN ACCTUAL TASK MANAGER FOR YOUR PERSONAL TASKS!
Despite the name, this is a to-do list manager rather than a Windows task manager - it manages your personal tasks and to-do items in json form. NOT SYSTEM PROCESSES
A simple and efficient command-line task manager for Windows. Manage your tasks directly from the terminal with an interactive menu or quick CLI commands.

Simple is key!
## Features

- Interactive menu interface with keyboard navigation
- Add, remove, and edit tasks from the command line
- Toggle task completion status (with SPACEBAR)
- Persistent storage in JSON format
- Data stored in AppData for proper Windows integration
- Quick command shortcuts for power users

## What It Does

TaskManagerCLI is a lightweight task management tool that runs entirely in your terminal. It maintains a list of tasks with unique IDs, descriptions, and completion status. All data is automatically saved to `%appdata%\Simple-TaskManagerCLITool\tasks.json`, ensuring your tasks persist between sessions. You can easily transfer your `tasks.json` however you want as long as the naming remains the same: `tasks.json`.

The tool offers two modes of operation:
1. **Interactive Mode**: Launch the menu-driven interface to browse and manage tasks using arrow keys
2. **Command Mode**: Execute quick actions directly from the command line

## Installation

### Requirements

- Windows 10 or later
- .NET 9.0 Runtime or SDK
- Powershell :))

### Installation Steps

1. Clone this repository or download the release
2. Navigate to the project directory
3. Run the installer script:

```powershell
powershell -ExecutionPolicy Bypass -File .\install.ps1
```

The installer will:
- Build the project in Release mode
- Copy files to `%localappdata%\Programs\TaskManagerCLI`
- Rename the executable to `tasks.exe`
- Add the program to your PATH environment variable

4. Restart your terminal
5. Type `tasks` to start using the tool

Alternatively just right click the `install.ps1` and click "Run with PowerShell" to install
### Uninstallation

To remove TaskManagerCLI:

```powershell
powershell -ExecutionPolicy Bypass -File .\install.ps1 -Uninstall
```

## Usage

### Interactive Mode

Launch the interactive menu by running:

```powershell
tasks
```

**Keyboard Controls:**
- Up/Down Arrow Keys: Navigate through tasks
- Spacebar: Toggle task completion status
- Enter: Exit the interactive menu

### Command Line Mode

**View Help:**
```powershell
tasks help
tasks --help
tasks --h
```

**Add a Task:**
```powershell
tasks add "Buy groceries"
tasks add "Finish project documentation"
```

**Remove a Task:**
```powershell
tasks remove 1
```

**Edit a Task:**
```powershell
tasks edit 1 "Buy groceries and cook dinner"
```

## Data Storage

All tasks are stored in a JSON file located at:
```
%appdata%\Simple-TaskManagerCLITool\tasks.json
```

The data structure includes:
- Task ID (auto-incremented)
- Task description
- Completion status (true/false)

## Development

### Building from Source

```powershell
dotnet build
```

### Publishing

```powershell
dotnet publish -c Release -r win-x64 --self-contained false -o publish
```

### Project Structure

- `Program.cs`: Core task management logic and data persistence
- `UserInterface.cs`: Interactive menu and command-line argument handling
- `install.ps1`: Automated installation and uninstallation script

## Technical Details

- **Language**: C#
- **Framework**: .NET 9.0
- **JSON Serialization**: Newtonsoft.Json
- **Storage**: Local AppData directory
- **Platform**: Windows (win-x64)

## License

This project is open source and available for personal and commercial use.

## Contributing

Contributions are welcome. Please fork the repository and submit pull requests for any improvements or bug fixes. This is practice for me and im still learning, so don't hesitate to point out flaws !
