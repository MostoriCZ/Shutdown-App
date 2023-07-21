using System;
using System.Reflection.Metadata;
using System.Threading;
using Spectre.Console;
using WindowsInput;


class Diamond
{
    public static void Main()
    {
        string casikPure = "";
        Home:
        Console.Clear();
        string decisionlog = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Vyber z následujících: [/]")
                .PageSize(10)
                .MoreChoicesText("[red]Vyber launcher šipkami[/]") // Zde je odstraněna mezičárka za [red]
                .AddChoices("[purple]Chci zadat čas kdy chci počítač vypnout[/]", "[purple]Chci vybrat z mnou dříve zadaných časů[/]")
        );
        if (decisionlog == "[purple]Chci zadat čas kdy chci počítač vypnout[/]")
        {
            Console.WriteLine("zadej čas v minutách: ");
            casikPure = Console.ReadLine();
            try
            {
                int number = int.Parse(casikPure);
                Console.WriteLine($"Zadané číslo bude převedeno na sekundy, aby mu Windows rozuměl: {number} min.");

                int casikosetren = (number * 60);
                Console.WriteLine("Číslo bylo převedeno na sekundy: " + casikosetren + " s.");

                string saveDecisionLog = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Vyber z následujících: [/]")
                    .PageSize(10)
                    .MoreChoicesText("[red]Vyber launcher šipkami[/]")
                    .AddChoices("[purple]Chci uložit čas a pokračovat k vypnutí[/]", "[purple]Chci [red]POUZE[/] pokračovat k vypnutí[/]")
                    );

                if (saveDecisionLog == "[purple]Chci uložit čas a pokračovat k vypnutí[/]")
                {
                    if (File.Exists(casikPure + " minut" + ".txt"))  /*zkontroluje, jestli už nejsou údaje v paměti a když ne zapíše je k danému launcheru*/
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("PRO TENTO ČASOVÝ ÚDAJ UŽ MÁŠ ULOŽENO... ty maniaku!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Bude uložen následující čas: " + casikPure + " min.");
                        Console.ResetColor();
                        File.WriteAllText(casikPure + " minut" + ".txt", "shutdown -s -t " + casikosetren);
                        
                        Thread.Sleep(1000);
                        Console.WriteLine("Zahajuji sekvenci vypínání");
                        Thread.Sleep(1500);

                        InputSimulator simulator = new InputSimulator();
                        simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LWIN, WindowsInput.Native.VirtualKeyCode.VK_R);

                        // Počkejte chvíli, než se dialog Spustit stačí otevřít
                        Thread.Sleep(500); // Chvíli počkej (500 milisekund = 0,5 sekundy)

                        // Vlož text "shutdown -s -t x" do dialogu Spustit a potvrď Enterem
                        string command = "shutdown -s -t " + casikosetren; //doplní do commandu už převedený čas na sekundy, aby mu Win rozuměl
                        simulator.Keyboard.TextEntry(command);
                        simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                    }
                }
                if (saveDecisionLog == "[purple]Chci [red]POUZE[/] pokračovat k vypnutí[/]")
                {
                    InputSimulator simulator = new InputSimulator();
                    simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.LWIN, WindowsInput.Native.VirtualKeyCode.VK_R);

                    // Počkejte chvíli, než se dialog Spustit stačí otevřít
                    Thread.Sleep(500); // Chvíli počkej (500 milisekund = 0,5 sekundy)

                    // Vlož text "shutdown -s -t x" do dialogu Spustit a potvrď Enterem
                    string command = "shutdown -s -t " + casikosetren; //doplní do commandu už převedený čas na sekundy, aby mu Win rozuměl
                    simulator.Keyboard.TextEntry(command);
                    simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                }
                 
                
                
                

                Console.ReadKey();
                goto Home;
            }
            catch (FormatException)
            {
                Console.WriteLine("Zadaná hodnota není platné celé číslo minut.");
                goto Home;
            }
           
            
        }
        if (decisionlog == "[purple]Chci vybrat z mnou dříve zadaných časů[/]")
        {
            try /*ošetřuje, jestli ještě nebyla data zapsána*/
            {
                string directoryPath = "C:\\Users\\jakub\\source\\repos\\Shutdown App\\Shutdown App\\bin\\Debug\\net6.0";
                string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");
                foreach (string txtFile in txtFiles)
                {
                    string obsahSouboru = File.ReadAllText(txtFile);
                    Console.WriteLine($"Obsah souboru {Path.GetFileName(txtFile)}:\n{obsahSouboru}\n");
                 
                }
                    string deleteOrBackLog = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Vyber z následujících: [/]")
                    .PageSize(40)
                    .MoreChoicesText("")
                    .AddChoices("[red]Smazat[/]", "[purple]Zpět[/]")
                    );
                if (deleteOrBackLog == "[red]Smazat[/]")
                {
                    Console.WriteLine("Napiš časovou jednotku, kterou si přeješ smazat");
                    string mankindDecision = Console.ReadLine();
                    string smazuTeJakLolko = mankindDecision + " minut" + ".txt";
                    File.Delete(smazuTeJakLolko);
                    goto Home;
                }
                if (deleteOrBackLog == "[purple]Zpět[/]")
                {
                    goto Home;
                }
            }
            catch (FileNotFoundException)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("-NEZADÁNO-");
                Console.ResetColor();
                Console.ReadKey();
                goto Home;
            }
        }
        
    }
}
