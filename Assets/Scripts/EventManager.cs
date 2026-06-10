using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// A játék eseményeit (dialógusok, puzzle állások) tároló adatstruktúra, ami szerializálható a JSON mentéshez.
[System.Serializable]
public class GameEventsConfig
{
    /// A párbeszédeket tároló lista.
    public List<DialogueData> dialogues = new List<DialogueData>();
    /// A sima rejtvények logikáját tároló lista.
    public List<PuzzleData> puzzles = new List<PuzzleData>();
    /// >A kar-alapú rejtvények adatait tároló lista.
    public List<LeverPuzzleGroupData> leverPuzzles = new List<LeverPuzzleGroupData>();
    /// A szintekhez szükséges kulcs(ok) azonosítóinak tömbje.
    public string[] requiredItemIds = new string[] { "Map1_Key", "Map2_Key", "Map3_Key" };
}

///Egy konkrét dialógust definiáló adatstruktúra.
[System.Serializable]
public class DialogueData
{
    /// A dialógus egyedi azonosítója a játékban.
    public string dialogueID;
    /// A dialógushoz tartozó szöveges sorok tömbje.
    public string[] lines;
}

/// Zongora rejtvényt  leíró adatstruktúra.
[System.Serializable]
public class PuzzleData
{
    /// A puzzle egyedi azonosítója.
    public string puzzleID;
    /// A megoldáshoz szükséges minta elemeinek indexei sorrendben.
    public int[] requiredPatternIndices;
}

/// Karos puzzle teljes eseménystruktúrája.
[System.Serializable]
public class LeverPuzzleGroupData
{
    /// >A feladvány egyedi azonosítója.
    public string puzzleID; 
    /// A megfejtéshez kötelezőleg aktív  karok száma.
    public int requiredLeverCount = 5;
    public List<LeverData> levers = new List<LeverData>();
}

/// Egy darab puzzle-kar működését leíró adat.
[System.Serializable]
public class LeverData
{
    /// A kar sorszáma a konkrét feladványon belül (1-től 5-ig).
    public int leverIndex; 
    /// A többi kar indexe, amelyet ez a kar magával mozgat.
    public int[] affectedLevers;
}

/// memóriába töltéséért és JSON fájlba történő mentéséért.
public class EventManager : MonoBehaviour
{
    /// A globális EventManager példány.
    public static EventManager Instance { get; private set; }
    
    /// A JSON-ból felolvasott (vagy a betöltendő) adatok példánya a memóriában.
    public GameEventsConfig EventConfig;
    private string saveFilePath;

    /// A játék indulásakor inicializálja magát és betölti az adatokat a háttértárból.
    /// Gondoskodik róla, hogy csak egyetlen példány éljen belőle (Singleton hitelesítés).
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "gameEvents.json");
            LoadEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Jelenlegi EventConfig állapot fájlba mentése JSON formátumban.
    public void SaveEvents()
    {
        string json = JsonUtility.ToJson(EventConfig, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Events saved to: " + saveFilePath);
    }

    /// A JSON fájlból történő betöltés, és hiányos feladványok alapértelmezett feltöltése.
    public void LoadEvents()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            EventConfig = JsonUtility.FromJson<GameEventsConfig>(json);
        }
        else
        {
            EventConfig = new GameEventsConfig();
        }

        ///gameEvents.json fájl tartalma

        EnsureDialogue("Intro", new string[] { "Végre itt vagyok. Nem hittem volna, hogy valaki ilyen helyen akar zongorát javíttatni.", "Meglepő, hogy ilyen értékes dolgot csak úgy itt hagytak.", "Ideje munkához lássak. Nem akarnék sötétedésig itt maradni egyedül.", "Mik azok ott a földön?", "Valószínűleg a zongora tartozékai lehetnek."});

        EnsureDialogue("END_GAME", new string[] { "Végre kijutottam...", "Ez a rémálom véget ért.", "Soha többé nem megyek a közelébe sem annak a zongorának." });

        EnsureDialogue("MAP1_1", new string[] { "Mi történt? . . . Hova kerültem?", "Csak egy billentyű után nyúltam . . . aztán hirtelen a mélység magával rántott.", "Hol a kivezető út? Egyszerűen köddé vált . . .", "Valami nincs rendben ezzel a hellyel. Bajlós előérzetem van.", "Minél előbb ki kell jussak innen. De csak mélyebbre tudok menni." });

        EnsureDialogue("MAP1_2", new string[] { "Ez nem egy sima fal. Ez egy ajtó.", "Zárva van.", "Lehet később tudom majd csak kinyitni. Vissza kell jöjjek még ide." });

        EnsureDialogue("MAP2_1", new string[] { "Egy új szintre érkeztem. A falak hidegek és nyirkosak.", "Olyan érzésem van minél tovább jutok annál több veszély fenyeget.", "Nem árt ha felkészülök a legrosszabbra.", "A repedezett részek nem tűnnek túl stabilnak, jobb ha nem állok azokon túl sokáig." });

        EnsureDialogue("MAP2_2", new string[] { "Vajon mit csinálhatott ez a kar?", "Vissza kéne menjek, hátha kinyílt a zárt ajtó, amit korábban láttam." });

        EnsureDialogue("MAP3_1", new string[] { "Mi ez a forróság?", "Ez már nem pince.", "Mint egy rég elfeledett hely a ház alatt.", "♪ ♪♪ ♪♪ ♪♪ ♪ ♪♪ ♪♪ ♪♪ ♪", "Egy zongora ... Biztos vagyok benne.", "De a hangja beteg.", "Már közel járok a végéhez ... Érzem." });

        EnsureDialogue("GRAPPLER", new string[] { "Egy kötél . . .", "Talán ezzel át tudok lendülni a tátongó mélységeken, melyek utamat állják.", "De vigyáznom kell! Könnyen alázuhanhatok a mélység veszedelmeibe." });

        EnsureDialogue("GRAPPLER_INST", new string[] { "Használathoz nyomja le az L betűt a hook közelében." });

        EnsureDialogue("KEY_1", new string[] { "Ez . . . egy zongorabillentyű? Ilyen mélyen a ház alatt?", "Mégis mihez kezdhetnék egyetlen billentyűvel ebben a káoszban?", "Lehet meg több is lesz. Tovább kell haladnom." });

        EnsureDialogue("KEY_2", new string[] { "Már a második. Ez nem lehet véletlen. Mintha szándékosan lennének itt elszórva.", "♫ ♫ ♪♪ ♪♪ ♫", "Valaki zenél itt lent? . . . Talán csak a huzat fütyül a folyosókon.", "Jobb ha folytatom az utam." });

        EnsureDialogue("KEY_3", new string[] { "Az utolsó darab. Már közel járok a kiúthoz.", "Ideje pontot tenni ennek a rémálomnak a végére.", "Meg kell találnom a zongorát." });

        EnsureDialogue("PIANO_MISSING_KEYS", new string[] { "Valamelyik billentyű hiányzik.", "Muszáj mindet megtaláljam különben itt ragadok." });

        EnsureDialogue("PIANO_HAS_KEYS", new string[] { "Megvannak a billentyűk.", "Ideje eljátszani a dallamot.", "(A billentyűk sorrendje: Először a fehérek balról jobbra, aztán a feketék ugyanezen a módon. (Összesen 12 db))" });

        EnsureDialogue("MAP1_Riddle", new string[] { "Jegyezd meg mennyi denevért láttál ebben a szobában! Még fontos lesz később!", "A második pedig a szívek száma lesz a teljes szinten!"});

        EnsureDialogue("MAP2_Riddle", new string[] { "A következő hangod az a szám lesz, ahány fáklyát láttál ezen a pályán.", "A negyedik hangot, hogy megtaláld, számold meg a leeső platformokat az utolsó szobában!"});

        EnsureDialogue("MAP3_Riddle", new string[] { "Az ötödik hanghoz a segítség egy égitest, amivel még találkozni fogsz. Hányadik is ő a sorban?", "A végső hang pedig az a szám, ahány zárt ajtót eddig kinyitottál." });



        EnsureLeverPuzzleExists("LeverPuzzle_Scene2", 5, new List<LeverData> {
            new LeverData { leverIndex = 1, affectedLevers = new int[] { 1, 3 } },
            new LeverData { leverIndex = 2, affectedLevers = new int[] { 3, 5 } },
            new LeverData { leverIndex = 3, affectedLevers = new int[] { 1, 3, 5 } },
            new LeverData { leverIndex = 4, affectedLevers = new int[] { 1, 2, 4 } },
            new LeverData { leverIndex = 5, affectedLevers = new int[] { 2, 4, 5 } }
        });

        EnsureLeverPuzzleExists("LeverPuzzle_Scene3", 5, new List<LeverData> {
            new LeverData { leverIndex = 1, affectedLevers = new int[] { 1, 2, 4 } },
            new LeverData { leverIndex = 2, affectedLevers = new int[] { 2, 3 } },
            new LeverData { leverIndex = 3, affectedLevers = new int[] { 1, 3, 5 } },
            new LeverData { leverIndex = 4, affectedLevers = new int[] { 3, 4 } },
            new LeverData { leverIndex = 5, affectedLevers = new int[] { 2, 4, 5 } }
        });

        EnsurePuzzleExists("PianoPuzzle", new int[] { 2, 8, 0, 4, 4, 1});

        SaveEvents();

        CollectedItemsState.InitializeFromConfig(EventConfig.requiredItemIds);
    }

    /// Ellenőrzi, hogy létezik-e egy puzzle  az adott ID-vel. Ha nem, beleteszi a default adatokat. Ha igen, felülírja.
    private void EnsurePuzzleExists(string id, int[] pattern)
    {
        var puzzle = EventConfig.puzzles.FirstOrDefault(p => p.puzzleID == id);
        if (puzzle == null)
        {
            EventConfig.puzzles.Add(new PuzzleData {
                puzzleID = id,
                requiredPatternIndices = pattern
            });
        }
        else
        {
            puzzle.requiredPatternIndices = pattern;
        }
    }

    /// Ellenőrzi, hogy létezik-e egy karos puzzle az adott ID-vel. Ha nem, beleteszi a default adatokat. Ha igen, felülírja.
    private void EnsureLeverPuzzleExists(string id, int count, List<LeverData> defaultLevers)
    {
        var puzzle = EventConfig.leverPuzzles.FirstOrDefault(p => p.puzzleID == id);
        if (puzzle == null)
        {
            EventConfig.leverPuzzles.Add(new LeverPuzzleGroupData {
                puzzleID = id,
                requiredLeverCount = count,
                levers = defaultLevers
            });
        }
        else
        {
            puzzle.requiredLeverCount = count;
            puzzle.levers = defaultLevers;
        }
    }

    /// Visszaadja egy konkrét dialógushoz (ID) tartozó szövegsorokat.
    public string[] GetDialogueLines(string dialogueID)
    {
        var dialogue = EventConfig.dialogues.FirstOrDefault(d => d.dialogueID == dialogueID);
        return dialogue?.lines;
    }

    /// Lekéri az azonosítóhoz tartozó feladvány (Piano) helyes kód-sorrendjét.
    public int[] GetPuzzlePattern(string puzzleID)
    {
        var puzzle = EventConfig.puzzles.FirstOrDefault(p => p.puzzleID == puzzleID);
        return puzzle?.requiredPatternIndices;
    }

    /// Megadja, hogy egy adott rejtvény egy konkrét karja melyik másik karokra van hatással.
    public int[] GetAffectedLevers(string puzzleID, int leverIndex)
    {
        var puzzleGroup = EventConfig.leverPuzzles.FirstOrDefault(l => l.puzzleID == puzzleID);
        if (puzzleGroup != null)
        {
            var leverData = puzzleGroup.levers.FirstOrDefault(l => l.leverIndex == leverIndex);
            if (leverData != null)
            {
                return leverData.affectedLevers;
            }
        }
        return null;
    }

    /// Visszaadja a megadott Puzzle azonosítójához szükséges összes kar számát.
    public int GetRequiredLeverCount(string puzzleID)
    {
        var puzzleGroup = EventConfig.leverPuzzles.FirstOrDefault(l => l.puzzleID == puzzleID);
        return puzzleGroup != null ? puzzleGroup.requiredLeverCount : 5;
    }

    /// Létrehozza, vagy frissíti a dialógust, hogy a szkriptben lévő legyen az érvényes.
    private void EnsureDialogue(string id, string[] lines)
    {
        var diag = EventConfig.dialogues.FirstOrDefault(d => d.dialogueID == id);
        if (diag == null)
        {
            EventConfig.dialogues.Add(new DialogueData { dialogueID = id, lines = lines });
        }
        else
        {
            diag.lines = lines;
        }
    }
}