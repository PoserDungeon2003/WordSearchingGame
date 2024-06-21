### Word Searching Game Installation Guide (v2)

## Setup API Project

*Note: This project use SQL Server Database. Refer to this [link](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for installation*

### Step 1: Install .NET 8 SDK and Runtime

To install .NET 8 SDK and runtime, follow the instructions below based on your operating system:

#### For Windows:

1. **Download the installer:**
   Visit the [official .NET download page](https://dotnet.microsoft.com/download/dotnet/8.0) and download the installer for .NET 8 SDK.

2. **Run the installer:**
   Execute the installer and follow the on-screen instructions to complete the installation.

3. **Verify the installation:**
   Open a new terminal or command prompt and type the following command to verify the installation:
   ```sh
   dotnet --version
   ```
   You should see the installed version of .NET SDK.

#### For macOS:

1. **Download the installer:**
   Visit the [official .NET download page](https://dotnet.microsoft.com/download/dotnet/8.0) and download the installer for macOS.

2. **Run the installer:**
   Open the downloaded .pkg file and follow the instructions to install .NET SDK.

3. **Verify the installation:**
   Open a new terminal and type the following command to verify the installation:
   ```sh
   dotnet --version
   ```
   You should see the installed version of .NET SDK.

#### For Linux:

1. **Install dependencies:**
   Open a terminal and run the following command to install required dependencies:
   ```sh
   sudo apt-get update
   sudo apt-get install -y wget apt-transport-https
   ```

2. **Add Microsoft package signing key:**
   ```sh
   wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   ```

3. **Install the SDK:**
   ```sh
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0
   ```

4. **Verify the installation:**
   ```sh
   dotnet --version
   ```
   You should see the installed version of .NET SDK.

### Step 2: Clone the Repository

Open terminal/cmd and type:
```sh
git clone https://github.com/thinhnt27/WordSearchingGameAPI.git
```

### Step 3: Open Project in Visual Studio 2022

Open the project in Visual Studio 2022.

### Step 4: Configure Connection Strings

Update the `ConnectionStrings` in `appsettings.json` to match your SQL Server configuration.

### Step 5: Run executable file

#### For Windows
```sh
./start.bat
```
#### For MacOS/Linux
```sh
./start.sh
```

*Note: If step 5 fails, delete the `Migrations` folder in the project, then rerun step 5*

### And you are good to go

---

## Setup Unity Project

### Step 1: Clone the Repository

Open terminal/cmd and type:
```sh
git clone https://github.com/PoserDungeon2003/WordSearchingGame.git
```
After cloning the project, change the directory to the project:
```sh
cd WordSearchingGame
```

### IMPORTANT: Google OAuth

**You must do this before open the Unity Editor**

Because this game has Google Login, the Google OAuth keys have been ignored from this project for security reasons. Download the Credentials zip file, extract, copy folder `Credentials` and paste it to `Assets` folder. The path should look like this `WordSearchingGame/Assets/Credentials`.

### Step 2: Install Unity Hub

Install Unity Hub (any version) and the correct Unity Editor version (current version: 2022.3.27f1). [Download Unity Editor here](https://unity.com/releases/editor/whats-new/2022.3.27#installs).

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
change from step 3 API to  cd WordSearchingGameAPI/WordSearchingGameAPI/ 
then for windows run ./start.bat, for linux/macos run chmod +x start.sh first then /.start.sh	
