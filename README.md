# Cellar Break

Ez a projekt egy 2D platformer, amely tartalmazza a modern platformer játékok elengedhetetlen funkcióit: precíz irányítást, csáklya-mechanikát, ellenséges AI-t, valamint egy robusztus szintváltó rendszert.



## Funkciók

1. Játékos Irányítás és Képességek

Mozgás: Input System alapú vízszintes mozgás és ugrás, beépített talajérzékeléssel (GroundCheck).

Csáklya (Grappling Hook): Fix pontokkal működő kötélmechanika, amely fizika alapú lengést és húzást tesz lehetővé.

Életerő és Sebzés: Szív-alapú HP rendszer. Sebzéskor a játékos hátralökődést (Knockback) szenved el, és rövid ideig sérthetetlenné válik.

Időzített Kihívások: Olyan tárgyak és zónák, amelyek felvételekor/belépésekor egy visszaszámláló indul el. Ha az idő lejár vagy a szoba sikerül, a játékos visszateleportál a kiindulópontra.

2. Világ és Környezet

Interaktív Platformok:

* Mozgó platformok: Két pont között ingáznak, a játékost magukkal viszik.
* Lehulló platformok: Érintésre leesnek, majd megadott idő után újratermelődnek.

Kamerakezelés: Cinemachine alapú szobaváltó rendszer (Trigger alapú kamera prioritás váltás).

Szintváltás (Scene Management): Kapuk és átjárók, amelyek nemcsak jelenetet váltanak, hanem a következő szinten a megfelelő kezdőponthoz (SpawnPoint) rendelik a játékost.

3. Ellenségek (AI)

Patőröző Patkány (Rat): Két pont között mozog, de ha a játékos a látótávolságán (chaseDistance) belülre kerül, üldözőbe veszi.

Denevér (Bat): Fix pozícióból, időzítve lő lövedékeket egy adott irányba.

4. Logikai Feladványok és Események

Fény és Sötét Mechanikák: Az első pályán spotlight global light helyett.
Zenei Puzzle (Zongora): Interaktív zongorabillentyűkön alapuló logikai feladvány.
Karos Kapcsolók (Lever Puzzles): Mechanizmusok és ajtók nyitása logikai hálózatokon keresztül.
Dialógus és Történet: Interaktív szövegdobozok a specifikációban foglalt történet közvetítésére.



## Rendszerarchitektúra és Logika

1. Játékos Irányítás (Player Core)

PlayerMovement.cs: A karakter fizikai mozgatásáért felel. Tartalmazza a vízszintes mozgást, ugrást és a Knockback (hátralökés) logikát, amely sérülés esetén rövid időre átveszi az irányítást a Rigidbody felett.

Grappling Hook.cs: Dinamikus csáklya-mechanika. Raycast2D segítségével keres csatlakozási pontot, majd DistanceJoint2D használatával valósítja meg a fizika alapú lengést.

PlayerHealth.cs: A játékos életerő-kezelője. Különlegessége a Runtime mentés: static változók segítségével megőrzi a HP-t jelenetváltáskor is, és Action alapú eseményt vált ki változáskor.

2. Életerő és UI Rendszer

HPController.cs: Feliratkozik a játékos életerő-változásaira, és dinamikusan példányosítja a szív-ikonokat.

HPHeart.cs: Egyedi UI elem, amely egy enum (Empty, Half, Full) alapján váltogatja a szív állapotát megjelenítő sprite-okat.

3. Jelenetkezelés és Spawn Rendszer

SceneSpawnSystem.cs: Egy globális, jelenetek feletti menedzser. Biztosítja, hogy szintváltás után a játékos ne a pálya origójában, hanem a kijelölt kezdőponton bukkanjon fel.

SwapScene.cs: Interaktív átjáró (Trigger), amely elindítja a jelenetváltást és regisztrálja a cél-spawnpoint azonosítóját.

SceneSpawnPoint.cs: Egy egyszerű jelölő objektum a pályán, amelyhez egyedi azonosító (ID) rendelhető.

4. Játékmechanikák és Akadályok

Timer.cs: Visszaszámláló rendszer. Ha az idő lejár, a játékost automatikusan visszateleportálja a megadott biztonságos pontra.

ItemPickup.cs: Gyűjthető tárgyak kezelése. Interakcióra (E billentyű) működik, és képes kommunikálni a Timer rendszerrel.

CollectedItemsState.cs: Egy HashSet alapú statikus adattároló, amely számon tartja, mely egyedi tárgyakat vette már fel a játékos (így azok nem termelődnek újra jelenetváltáskor).


5. Környezeti Elemek és AI

MovingPlatform.cs: Platform, amely két pont között mozog. Ütközéskor a játékost a saját "gyermekévé" teszi, így elkerülhető a csúszkálás a mozgó felületen.

FallingPlatform.cs: Időzített zuhanó platform. Érintésre statikusból dinamikus állapotba vált, majd megadott idő után visszaáll az eredeti helyére.

RatMovement.cs: Földi ellenség "Patrol" (járőr) és "Chase" (üldözés) állapotokkal. Távolság alapján vált a logikák között.

BatAI.cs: Ellenség, ami bizonyos időközönként lövedékeket lő adott irányba.

BatProjectile.cs: A denevérek által kilőtt lövedékek fizikájáért és sebzéséért felelős szkript.

CameraHandler.cs: Cinemachine prioritás-kezelő. Trigger zónák alapján vált az aktív kamerák között.

6. Feladványok és Interakciók

LeverPuzzleSwitch.cs: Logikai karok, amik reagálnak a játékos interakciójára.
LightManager.cs & LightSwitch.cs: Dinamikus világítás-kezelés és a világítás kapcsolása szobánként/zónánként.
PianoInteract.cs & PianoPuzzle.cs: Sorrendiségre épülő feladványmechanika.

7. Felhasználói Felület és Történetmesélés (UI)

MainMenu.cs & PauseMenu.cs: A játék alapvető C# menüvezérlői (Játék indítása, Szüneteltetés, Kilépés).
DeathPanelController.cs: Halál képernyő kezelése, menübe lépés vagy újraéledés funkcióival.
Dialogue.cs: Történetmesélést és a specifikáció szöveges részeit biztosító UI szövegíró modul.

8. Központi Állapotkezelés

GameState.cs & GameStateResetter.cs: Globális játékállapot (pl. begyűjtött kulcsok, feladványok státusza) mentése és kezelése halál vagy újraindítás esetén.
EventManager.cs: Általános "Event Bus" eseménykezelő hálózat a független objektumok kommunikációjához.



## Technikai megvalósítás

Fizika: Rigidbody2D és Collider2D alapú interakciók.

UI: TextMeshPro és Canvas alapú visszajelzések.

Input: Az új Unity Input System csomagra építve.

Verzió: Unity 2022.3 LTS vagy újabb ajánlott a linearVelocity használata miatt.


## Követelmények

Unity: 2022.3 LTS vagy frissebb.

Packages: \* Unity Input System

* Unity Cinemachine
* TextMeshPro

## Pályák és Assetek elérése (Labirintus Dokumentáció)

A specifikációban tárgyalt pályák (labirintusok) manuálisan kerültek felépítésre a Unity Editorban. A játékban ezek a pályák elérhetőek és játszhatóak, a projekt fájlrendszerében pedig a következő útvonalakon találhatóak meg (Unity Scene fájlok formájában):

- **Főmenü:** `Assets/Scenes/Menu.unity`
- **1. Pálya:** `Assets/Scenes/Map 1.unity`
- **2. Pálya:** `Assets/Scenes/Map 2.unity`
- **3. Pálya:** `Assets/Scenes/Map 3.unity`

A pályákat felépítő vizuális és interaktív elemek (prefabok, sprite-ok, stb.) az `Assets/Prefabs/` és `Assets/Sprites/` mappákban találhatóak.
Jelenleg még néhány asset unity asset storeból letöltött és placeholderként működik.


