# Cellar Break Dokumentáció

## Architektúra

Ez az ábra a játékunk logikai felépítését és a C# scriptek kapcsolatát mutatja be a Unity-n belül.

```mermaid
graph TD
    subgraph Unity_Project [Unity 2D Project Structure]
        direction TB
        
        subgraph Scenes_Layer [Scenes / Maps]
            Menu[Menu Scene]
            Maps[Map 1, 2, 3]
        end

        subgraph Script_Logic [C# Scripts / Logic]
            direction TR
            Player_Ctrl[Player: Movement, GrapplingHook]
            Health_Sys[Health: PlayerHealth, HPController, HPHeart]
            Enemy_Logic[Enemies: BatAI, RatMovement, EnemyDamage]
            Env_Interactions[Environment: MovingPlatform, FallingPlatform, ItemPickup]
            System_Scripts[Systems: SceneSpawnSystem, SwapScene, CollectedItemsState]
        end

        subgraph Asset_Folders [Assets & Sprites]
            Sprites[Sprites: Characters, Tiles, Platforms]
            Anims[Animations]
            Prefabs[Prefabs]
        end
    end

    %% Kapcsolatok ábrázolása
    System_Scripts -->|Kezeli| Scenes_Layer
    Player_Ctrl -->|Interakció| Env_Interactions
    Health_Sys --- Player_Ctrl
    Enemy_Logic -->|Sebzi| Health_Sys
    Asset_Folders -.->|Megjelenítés| Scenes_Layer

    style Script_Logic fill:#f5f5f5,stroke:#333,stroke-width:2px
    style System_Scripts fill:#d1e7ff,stroke:#004a99 
   ```