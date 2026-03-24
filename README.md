# Cellar Break

Ez a projekt egy komplex 2D platformer alaprendszer, amely tartalmazza a modern platformer játékok elengedhetetlen funkcióit: precíz irányítást, csáklya-mechanikát, ellenséges AI-t, valamint egy robusztus szintváltó- és mentési rendszert.



## Funkciók

1. Játékos Irányítás és Képességek

Fejlett Mozgás: Input System alapú vízszintes mozgás és ugrás, beépített talajérzékeléssel (GroundCheck).

Csáklya (Grappling Hook): Egérrel irányítható dinamikus kötélmechanika, amely fizika alapú lengést és húzást tesz lehetővé.

Életerő és Sebzés: Szív-alapú HP rendszer. Sebzéskor a játékos hátralökődést (Knockback) szenved el, és rövid ideig sérthetetlenné válik/elveszíti az irányítást.

Időzített Kihívások: Olyan tárgyak és zónák, amelyek felvételekor/belépésekor egy visszaszámláló indul el. Ha az idő lejár, a játékos visszateleportál a kiindulópontra.

2. Világ és Környezet

Interaktív Platformok:

* Mozgó platformok: Két pont között ingáznak, a játékost magukkal viszik.
* Lehulló platformok: Érintésre leesnek, majd megadott idő után újratermelődnek.

Kamerakezelés: Cinemachine alapú szobaváltó rendszer (Trigger alapú kamera prioritás váltás).

Szintváltás (Scene Management): Kapuk és átjárók, amelyek nemcsak jelenetet váltanak, hanem a következő szinten a megfelelő kezdőponthoz (SpawnPoint) rendelik a játékost.

3. Ellenségek (AI)

Patőröző Patkány (Rat): Két pont között mozog, de ha a játékos a látótávolságán (chaseDistance) belülre kerül, üldözőbe veszi.

Lövöldöző Denevér (Bat): Fix pozícióból, időzítve lő lövedékeket a játékos irányába, animált támadással.



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

ItemPickup.cs: Gyűjthető tárgyak kezelése. Interakcióra (E billentyű) működik, és képes kommunikálni a Timer rendszerrel (pl. "időre fuss vissza a tárggyal").

CollectedItemsState.cs: Egy HashSet alapú statikus adattároló, amely számon tartja, mely egyedi tárgyakat vette már fel a játékos (így azok nem termelődnek újra jelenetváltáskor).

Respawn.cs: "Halálos" zónák kezelése (tüskék, szakadékok). Sebzést oszt és azonnal visszahelyezi a játékost a legutóbbi biztonságos pontra.

5. Környezeti Elemek és AI

MovingPlatform.cs: Platform, amely két pont között mozog. Ütközéskor a játékost a saját "gyermekévé" teszi, így elkerülhető a csúszkálás a mozgó felületen.

FallingPlatform.cs: Időzített zuhanó platform. Érintésre statikusból dinamikus állapotba vált, majd megadott idő után visszaáll az eredeti helyére.

RatMovement.cs: Földi ellenség "Patrol" (járőr) és "Chase" (üldözés) állapotokkal. Távolság alapján vált a logikák között.

BatAI.cs: Repülő ellenség, amely fix időközönként támadási animációt indít és lövedékeket (Instantiate) lő a játékos felé.

CameraHandler.cs: Cinemachine prioritás-kezelő. Trigger zónák alapján vált az aktív kamerák között (pl. szobáról szobára haladáskor).



## Technikai megvalósítás

Fizika: Rigidbody2D és Collider2D alapú interakciók.

UI: TextMeshPro és Canvas alapú visszajelzések.

Input: Az új Unity Input System csomagra építve.

Verzió: Unity 2022.3 LTS vagy újabb ajánlott a linearVelocity használata miatt.



## Beállítási Útmutató (Setup)

1. Rétegek (Layers): Hozz létre egy Ground és egy GrappleLayer réteget.
2. Jelenetváltás:

Helyezz el egy SceneSpawnPoint-ot a cél jelenetben, és adj neki egy egyedi nevet (pl. "Level1\_Start").

A SwapScene szkriptben a targetSpawnId mezőbe írd ugyanezt a nevet.

3. Időzítő: A pályán lévő Timer objektumhoz rendeld hozzá a TextMeshProUGUI komponenst a vizuális visszaszámláláshoz.
4. Ellenségek: A denevérnek (BatAI) szüksége van egy lövedék prefabra, a patkánynak (RatMovement) pedig két üres GameObject-re patőrözési pontként.



## Követelmények

Unity: 2022.3 LTS vagy frissebb.

Packages: \* Unity Input System

* Unity Cinemachine
* TextMeshPro

