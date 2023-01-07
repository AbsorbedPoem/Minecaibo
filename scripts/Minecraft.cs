using System;
using Godot;
public class Minecraft
{
    public static string[] ids = new string[6]{
        "dirt",
        "grass",
        "cobblestone",
        "oak_wood",
        "oak_leaves",
        "bedrock"
    };

    public static int[][,] textures = {
        new int[3,2] {{2,0}, {2,0}, {2,0}},
        new int[3,2] {{0,0}, {1,0}, {2,0}},
        new int[3,2] {{3,0}, {3,0}, {3,0}},
        new int[3,2] {{5,0}, {4,0}, {5,0}},
        new int[3,2] {{6,0}, {6,0}, {6,0}},
        new int[3,2] {{7,0}, {7,0}, {7,0}}
    };

    public static int GetID(string name){
        return Array.IndexOf<string>(Minecraft.ids, name);
    }
}