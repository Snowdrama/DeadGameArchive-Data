#:package Newtonsoft.Json@13.0.4 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;

public struct GameData
{
    public bool active;
    public string name;
    public string type;
    public string genre;
    public DateOnly birth;
    public DateOnly death;
    public string death_reason;
    public string images_path;
    public List<string> images;
    public string[] platforms;
    public string developer;
    public string publisher;
    public string notes;
    public string details;
    public string description;
}

public struct GameList
{
    public List<GameData> games;

    public GameList()
    {
        games = new List<GameData>();
    }
}

public struct GamePage
{
    public int game;
    public List<GameData> games;

    public GamePage()
    {
        games = new List<GameData>();
    }
}


public struct GameImages
{
    public List<string> imagePaths;
    public GameImages()
    {
        imagePaths = new List<string>();
    }
}

public struct GameSearchData
{
    public string name;
    public string file;
}

public struct GameSearch
{
    public List<string> search_list;
    public GameSearch()
    {
        search_list = new List<string>();
    }
}

[RequiresUnreferencedCode("This method calls APIs (e.g. JsonConvert) that may be incompatible with IL trimming.")]
[RequiresDynamicCode("This method may use dynamic code or reflection that is not compatible with AOT/trimming.")]
public class DeadGameArchive
{
    public static void Main(string[] args)
    {
        ProcessDGAPath("Games", "dead-games.json", "dead-games-search.json");
        ProcessDGAPath("DeadBeforeLaunch", "dead-before-launch-games.json", "dead-before-launch-games-search.json");
        ProcessDGAPath("Preserved", "preserved-games.json", "preserved-games-search.json");
        ProcessDGAPath("MMO", "mmo-games.json", "mmo-games-search.json");
        ProcessDGAPath("Abandonware", "abandonware-games.json", "abandonware-games-search.json");
    }

    private static void ProcessDGAPath(string inputFolder, string outputJsonFile, string outputJsonSearchFile)
    {
        string inputFolderPath = $"./{inputFolder}";
        
        //first we extract the games and merge in the image file list
        GameList gameList = new GameList();

        Console.WriteLine("Processing Games");
        var files = Directory.GetFiles(inputFolderPath);

        foreach(var file in files)
        {
            if (file.Contains(".gitignore"))
            {
                continue;
            }

            //add the game data from the file
            var gameData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(file));
            ConsoleEx.Blue($"Processing: {gameData.name}");

            //process the images in the game's image path
            var gameImageFolder = $".{gameData.images_path}";
            //validate the directory exists
            ValidateDirectory(gameImageFolder);

            var imagePaths = Directory.GetFiles(gameImageFolder);
            gameData.images = new List<string>();

            //add each image path to the images array
            foreach(var imagePath in imagePaths)
            {
                var fixedPath = imagePath.Replace("\\", "/");
                ConsoleEx.DarkBlue($"Image: {fixedPath}");
                gameData.images.Add(fixedPath);
            }
            //then finally add the game to the gameList
            gameList.games.Add(gameData);
        }

        //then we sort by death day so the most recent dead games are first
        gameList.games = gameList.games.OrderByDescending(x => x.death).ToList();

        //Now we make the "Search" Json..

        GameSearch searchFile = new GameSearch();
        foreach(var game in gameList.games)
        {
            searchFile.search_list.Add(game.name);
        }

        ValidateDirectory($"./Generated/{inputFolder}");
        File.WriteAllText($"./Generated/{inputFolder}/{outputJsonFile}", JsonConvert.SerializeObject(gameList, Formatting.Indented));

        //then we paginate it into "page" json
        if(gameList.games.Count > 5)
        {
            List<GamePage> pages = new List<GamePage>();
            int pageCount = (int)Math.Ceiling(gameList.games.Count / 5.0d);
            int index = 0;
            for (int i = 0; i < pageCount; i++)
            {
                GamePage page = new GamePage();
                for (int j = 0; j < 5; j++)
                {
                    if(index >= gameList.games.Count)
                    {
                        continue;
                    }
                    page.games.Add(gameList.games[index]);
                    index++;
                }
                pages.Add(page);
            }

            for (int i = 0; i < pages.Count; i++)
            {
                ConsoleEx.DarkGreen($"Creating Page: page-{i}.json");
                ValidateDirectory($"./Generated/{inputFolder}/pages");
                File.WriteAllText($"./Generated/{inputFolder}/pages/page-{i}.json", JsonConvert.SerializeObject(pages[i], Formatting.Indented));
            }
        }

        //finally we write to the Generated folder
        ValidateDirectory($"./Generated/{inputFolder}");
        File.WriteAllText($"./Generated/{inputFolder}/{outputJsonFile}", JsonConvert.SerializeObject(gameList, Formatting.Indented));
    }

    private static void CopyFile(string oldPath, string newPath)
    {
        //validate we can split to the new path
        var (splitPath, _) = PathFileSplitter(newPath);
        ValidateDirectory(splitPath);
        File.Copy(oldPath, newPath, true);
    }

    private static void ValidateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            ConsoleEx.Yellow($"Creating Directory: {path}");
            Directory.CreateDirectory(path);
        }
    }

    private static (string Path, string File) PathFileSplitter(string fullPath)
    {
        var pathParts = fullPath.Replace("\\", "/").Split("/");
        var file = pathParts.Last();
        var path = fullPath.Replace(file, "");
        return (path, file);
    }
}


//  ██████╗ ██████╗ ███╗   ██╗███████╗ ██████╗ ██╗     ███████╗███████╗██╗  ██╗
// ██╔════╝██╔═══██╗████╗  ██║██╔════╝██╔═══██╗██║     ██╔════╝██╔════╝╚██╗██╔╝
// ██║     ██║   ██║██╔██╗ ██║███████╗██║   ██║██║     █████╗  █████╗   ╚███╔╝ 
// ██║     ██║   ██║██║╚██╗██║╚════██║██║   ██║██║     ██╔══╝  ██╔══╝   ██╔██╗ 
// ╚██████╗╚██████╔╝██║ ╚████║███████║╚██████╔╝███████╗███████╗███████╗██╔╝ ██╗
//  ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝

//helper class to write to more easily do colored console stuff.
public static class ConsoleEx
{
    public static void Red(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void DarkRed(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Blue(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Blue;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void DarkBlue(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Green(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void DarkGreen(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Cyan(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Magenta(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Magenta;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Yellow(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void Grey(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.Gray;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }
    public static void DarkGrey(string output, bool writeLine = true)
    {
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        if (writeLine)
        {
            System.Console.WriteLine(output);
        }
        else
        {
            System.Console.Write(output);
        }
        System.Console.ResetColor();
    }

    public static void WriteLine(string output, ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        Console.WriteLine(output);
        System.Console.ResetColor();
    }
    public static void Write(string output, ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        Console.Write(output);
        System.Console.ResetColor();
    }

    public static string? ReadLine(string lineToDisplay = "", ConsoleColor color = ConsoleColor.White)
    {
        System.Console.ForegroundColor = color;
        System.Console.WriteLine(lineToDisplay);
        System.Console.ResetColor();
        return Console.ReadLine();
    }
    public static int ReadInt(string lineToDisplay = "", string errorMessage = "", ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red)
    {

        System.Console.ForegroundColor = color;
        System.Console.WriteLine(lineToDisplay);
        System.Console.ResetColor();
        var line = Console.ReadLine();
        int result;
        while (!int.TryParse(line, out result))
        {
            //then display error message
            System.Console.ForegroundColor = errorColor;
            System.Console.WriteLine(lineToDisplay);
            System.Console.ResetColor();

            line = Console.ReadLine();
        }

        return result;
    }

    private static int waitingAnimIndex;
    private static char[] waitingAnim = new char[] { '|', '\\', '-', '/' };
    private static char waitingCursor = '|';
    public static void DrawWaitingThing(string extra = "")
    {
        var pos = Console.GetCursorPosition();
        waitingAnimIndex = (waitingAnimIndex + 1) % waitingAnim.Length;
        waitingCursor = waitingAnim[waitingAnimIndex];
        Console.SetCursorPosition(20, 0);
        Console.Write($"{waitingCursor} {extra}");
        Console.SetCursorPosition(pos.Left, pos.Top);
    }
}