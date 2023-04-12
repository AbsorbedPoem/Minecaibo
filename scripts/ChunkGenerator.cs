using Godot;
using System;
using System.Collections.Generic;

public class ChunkGenerator : Spatial
{
    // store of all block in chunk (chunkData parameter reference of all chunks)
    public ChunkData[,] terrainData = new ChunkData[10,10];
    // render distance around the render pivos, measured un chunks
    public Vector2 render_pivot;
    public int renderOffset = 5;

    // player
    public KinematicBody p;

    // really useless within dynamic render
    public List<Vector2> loadedChunks = new List<Vector2>();

    // chunk packed scene - a StaticBody node with a MeshInstance child (with Chunk.cs script attached)
    private PackedScene chunk_template;

    public override void _Ready()
    {
        // load resources
        chunk_template = ResourceLoader.Load<PackedScene>("res://Scenes/Chunk.tscn");
        p = GetParent<Spatial>().GetNode<KinematicBody>("steve");

        // start updating chunk around player
        SetPivot(p.Translation);
    }

    public override void _Process(float delta)
    {
        // Dynamic chunk generation? :D
    }

    void SetPivot(Vector3 p){
        render_pivot = new Vector2(Convert.ToInt32(Math.Floor(p.x / 16)),
                                   Convert.ToInt32(Math.Floor(p.z / 16)));

        Godot.Collections.Array children = GetChildren();
        List<Vector2> req = new List<Vector2>();

        // required chunks for this position
        for (int i = Convert.ToInt32(render_pivot.x) - renderOffset; i < render_pivot.x + renderOffset; i++){
            for (int j = Convert.ToInt32(render_pivot.y) - renderOffset; j < render_pivot.y + renderOffset; j++){
                req.Add(new Vector2(i,j));
            }
        }

        // removing chunks out of bounds
        foreach (Vector2 elem in loadedChunks.ToArray()){
            if (!req.Contains(elem)){
                loadedChunks.RemoveAt(loadedChunks.IndexOf(elem));
                GetNode("Chunk[" + elem.x + "," + elem.y + "]").QueueFree();
            }
        }

        List<StaticBody> chunks = new List<StaticBody>();

        // create new chunk objects from template, and putting it into the scene
        foreach (Vector2 elem in req){
            if (!loadedChunks.Contains(elem)){
                loadedChunks.Add(elem);

                StaticBody chunk = chunk_template.Instance<StaticBody>();
                chunk.Name = "Chunk[" + elem.x + "," + elem.y + "]";
                chunk.Translation = new Vector3(elem.x * 16, 0, elem.y * 16);
                chunks.Add(chunk);
                this.AddChild(chunk);
                Chunk instance = chunk.GetChild<Chunk>(0);

                terrainData[Convert.ToInt32(elem.x) + renderOffset,Convert.ToInt32(elem.y) + renderOffset] = instance.chunkData;
            }
        }

        // "tell" to chunks to render
        foreach (StaticBody chunk in chunks){
            chunk.GetChild(0).EmitSignal("RenderMesh");
        }
    }

    public void Destroy(Vector3 search){
        int[] cord = new int[]{Convert.ToInt32(Mathf.Floor(search.x / 16)), Convert.ToInt32(Mathf.Floor(search.z / 16))};
        int[] inner = new int[]{Convert.ToInt32(search.x) - cord[0]*16, Convert.ToInt32(search.y), Convert.ToInt32(search.z) - cord[1]*16};
        
        terrainData[cord[0] + renderOffset, cord[1] + renderOffset].data[inner[0], inner[1], inner[2], 0] = 0;

        GetChunk(new Vector2(cord[0], cord[1])).StartRender();

        if (inner[2] == 15){
            try { GetChunk(new Vector2(cord[0], cord[1]+1)).StartRender();}
            catch {}
        }
        if (inner[2] == 0){
            try { GetChunk(new Vector2(cord[0], cord[1]-1)).StartRender();}
            catch {}
        }

        if (inner[0] == 15){
            try { GetChunk(new Vector2(cord[0]+1, cord[1])).StartRender();}
            catch {}
        }
        if (inner[0] == 0){
            try { GetChunk(new Vector2(cord[0]-1, cord[1])).StartRender();}
            catch {}
        }
    }

    public void Place(Vector3 search, string name){
        int[] cord = new int[]{Convert.ToInt32(Mathf.Floor(search.x / 16)), Convert.ToInt32(Mathf.Floor(search.z / 16))};
        int[] inner = new int[]{Convert.ToInt32(search.x) - cord[0]*16, Convert.ToInt32(search.y), Convert.ToInt32(search.z) - cord[1]*16};

        Chunk chunk = GetChunk(new Vector2(cord[0], cord[1]));
        chunk.SaveBoxel(inner[0],inner[1],inner[2], name);
        chunk.StartRender();

        if (inner[2] == 15){
            try { GetChunk(new Vector2(cord[0], cord[1]+1)).StartRender();}
            catch {}
        }
        if (inner[2] == 0){
            try { GetChunk(new Vector2(cord[0], cord[1]-1)).StartRender();}
            catch {}
        }

        if (inner[0] == 15){
            try { GetChunk(new Vector2(cord[0]+1, cord[1])).StartRender();}
            catch {}
        }
        if (inner[0] == 0){
            try { GetChunk(new Vector2(cord[0]-1, cord[1])).StartRender();}
            catch {}
        }

        
    }

    public Chunk GetChunk(Vector2 name){
        return GetNode("Chunk[" + name.x + "," + name.y + "]").GetChild<Chunk>(0);;
    }
}
