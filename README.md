# Word Searching Game

## Setup API Project

### Step 1: Clone the Repository

Open terminal/cmd and type:
```sh
git clone https://github.com/thinhnt27/WordSearchingGameAPI.git
```

### Step 2: Install .NET 8
Open Visual Studio Installer and install .NET 8 (if it was not installed before).

### Step 3: Open Project in Visual Studio 2022
Open the project in Visual Studio 2022.

### Step 4: Configure Connection Strings
Update the ConnectionStrings in `appsettings.json` to match your SQL Server configuration.

### Step 5: Apply Migrations
Open Package Manager Console (Nuget Package/ Ctrl + `).
- If the project contains a folder named `Migrations`, type:
  ```sh
  Update-Database
  ```
- If there is no `Migrations` folder, type:
  ```sh
  Add-Migration init
  Update-Database
  ```

*Note: If step 5 fails, delete the `Migrations` folder in the project, then type:*
```sh
Add-Migration init
Update-Database
```

### Step 6: Run the Project
Run the project.

## Setup Unity Project

### Step 1: Clone the Repository
Open terminal/cmd and type:
```sh
git clone https://github.com/PoserDungeon2003/WordSearchingGame.git
```
*Note: Because the project is in development, you need to checkout to the dev branch.*

After cloning the project, change the directory to the project:
```sh
cd WordSearchingGame
```
Then checkout to the dev branch:
```sh
git checkout dev
```

### Step 2: Install Unity Hub
Install Unity Hub (any version) and the correct Unity Editor version (current version: 2022.3.27f1).
[Download Unity Editor here](https://unity.com/releases/editor/whats-new/2022.3.27#installs).

### Step 3: Launch Unity Editor
After installing the Unity Editor, launch it. The editor should look like this:

### Step 4: Open Game Scene
In the project tab, navigate to the `Scenes` folder and open `GameScene`.

### Step 5: Start the Game
Click the Play button to start the game.

Now you can start playing!

---

## Copyright
This project is for educational purposes only. All credits go to this author: [YouTube Playlist](https://youtube.com/playlist?list=PLJLLSehgFnspMBk7VaLI18Digsj2xuMhT&si=RTvReRSj2z98C7Ar).
