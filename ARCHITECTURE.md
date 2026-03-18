# Cellar Break Dokumentáció

## Architektúra

Ez az ábra a játékunk logikai felépítését és a C# scriptek kapcsolatát mutatja be a Unity-n belül.

```mermaid
graph TD
    subgraph Unity_Project [Unity 2D Project Structure]
        
        subgraph Scenes_Layer [Scenes and Maps]
            Menu[Menu Scene]
            Maps[Map 1, 2, 3]
        end

        subgraph Script_Logic [C# Scripts and Logic]
            Player_Ctrl[Player - Movement and GrapplingHook]
            Health_Sys[Health - PlayerHealth and HPController]
            Enemy_Logic[Enemies - BatAI and RatMovement]
            Env_Interactions[Environment - Platforms and Items]
            System_Scripts[Systems - Spawning and Scenes]
        end

        subgraph Asset_Folders [Assets and Sprites]
            Sprites[Sprites - Characters and Tiles]
            Anims[Animations]
            Prefabs[Prefabs]
        end
    end

    %% Kapcsolatok
    System_Scripts --> Scenes_Layer
    Player_Ctrl --> Env_Interactions
    Health_Sys --- Player_Ctrl
    Enemy_Logic --> Health_Sys
    Asset_Folders -.-> Scenes_Layer

    style Script_Logic fill:#f5f5f5,stroke:#333,stroke-width:2px
    style System_Scripts fill:#d1e7ff,stroke:#004a99 
   ```