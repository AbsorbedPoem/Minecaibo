using Godot;
using System;
using System.Diagnostics;

public class Chunk : MeshInstance
{
    // This signal starts the mesh rendering
    [Signal]
    delegate void RenderMesh();

    ChunkGenerator parent_generator;

    // data store for all blocks in the chunk
    public ChunkData chunkData = new ChunkData();
    // spatial indexed chunk location : 1 chunk = 1 unit
    public Vector3 cordinates;

    // parameters for Perlin Noise's generation
    private float difussion = 0.0006f; //less value -> flatter terrain
    private float offset = 0.002f;
    private int minheight = 30;
    private int terrain_amplitude = 30;

    private SurfaceTool st = new SurfaceTool();

    public override void _Ready()
    {
        parent_generator = GetTree().Root.GetNode<ChunkGenerator>("Spatial/Chunks");
        Connect("RenderMesh", this, "StartRender");

        // chunk cordinates
        cordinates = new Vector3(this.GetParent<StaticBody>().Translation.x / 16, 0, this.GetParent<StaticBody>().Translation.z / 16);

        // resources - material and block_textures
        SpatialMaterial mat = ResourceLoader.Load<SpatialMaterial>("Materials/cubes_default.tres");
        Image img = new Image();
        img.Load("Textures/textures.png");

        // put image into material (via script because i needed to entablish flags directly)
        ImageTexture img_texture = new ImageTexture();
        img_texture.CreateFromImage(img, 1);
        mat.AlbedoTexture = img_texture;

        // Perlin Noise function instance
        Perlin perlin = new Perlin();

        // Generate chunk terrain information
        for (int x = 0; x < ChunkData.width; x++){
            for (int y = 0; y < ChunkData.max_heigth; y++){
                for (int z = 0; z < ChunkData.width; z++){

                    // first perlin noise output
                    double per = perlin.OctavePerlin((x+cordinates.x*16 + 1000000)*difussion + offset,
                                                      1 + offset,
                                                     (z+cordinates.z*16 + 1000000)*difussion + offset,
                                                      8, 2) * terrain_amplitude + minheight;

                    // absolute cordinates
                    int _x = x + Convert.ToInt32(cordinates.x) * 16;
                    int _z = z + Convert.ToInt32(cordinates.z) * 16;

                    // grass, dirt, cobblestone and bedrock generation
                    if (y <= per){
                        if (y <= per && y > per-1)        SaveBoxel(x,y,z,"grass");
                        else if (y <= per-1 && y > per-4) SaveBoxel(x,y,z,"dirt");
                        else if (y <= per-4 && y > 1)     SaveBoxel(x,y,z,"cobblestone");
                        else if (y <= 1)                  SaveBoxel(x,y,z,"bedrock");
                    }
                    else{
                        // not solid air
                        chunkData.data[x,y,z,0] = 0;
                    }
                }
            }
        }
    }

    public void StartRender(){
        st.Begin(Mesh.PrimitiveType.Triangles);
        ChunkGenerator parent = GetTree().Root.GetNode("Spatial").GetNode<ChunkGenerator>("Chunks");
        
        // reference to chunk in front, back, left and rigth
        ChunkData f,b,r,l;

        try {f = parent.terrainData[Convert.ToInt32(cordinates.x) + parent.renderOffset, Convert.ToInt32(cordinates.z+1) + parent.renderOffset];}
        catch (IndexOutOfRangeException) {f = null;}
        try {b = parent.terrainData[Convert.ToInt32(cordinates.x) + parent.renderOffset, Convert.ToInt32(cordinates.z-1) + parent.renderOffset];}
        catch (IndexOutOfRangeException) {b = null;}
        try {r = parent.terrainData[Convert.ToInt32(cordinates.x+1) + parent.renderOffset, Convert.ToInt32(cordinates.z) + parent.renderOffset];}
        catch (IndexOutOfRangeException) {r = null;}
        try {l = parent.terrainData[Convert.ToInt32(cordinates.x-1) + parent.renderOffset, Convert.ToInt32(cordinates.z) + parent.renderOffset];}
        catch (IndexOutOfRangeException) {l = null;}
        

        for (int x = 0; x < chunkData.data.GetLength(0); x++){
            for (int y = 0; y < chunkData.data.GetLength(1); y++){
                for (int z = 0; z < chunkData.data.GetLength(2); z++){
                    if (chunkData.data[x,y,z,0] == 1){         
                        // render xyz-boxel if solid
                        GDBoxel.GenerateFaces(st, new Vector3(x,y,z) , chunkData, Minecraft.textures[chunkData.data[x,y,z,1]]);
                        
                        // generate side face if it's in chunk's edge
                        if (z == 15 && f != null) GDBoxel.GenerateFrontFace(st, new Vector3(x,y,z) , chunkData, Minecraft.textures[chunkData.data[x,y,z,1]], f.data[x,y,0,0]);
                        if (z == 0 && b != null)  GDBoxel.GenerateBackFace(st, new Vector3(x,y,z) , chunkData, Minecraft.textures[chunkData.data[x,y,z,1]], b.data[x,y,15,0]);
                        if (x == 15 && r != null) GDBoxel.GenerateRightFace(st, new Vector3(x,y,z) , chunkData, Minecraft.textures[chunkData.data[x,y,z,1]], r.data[0,y,z,0]);
                        if (x == 0 && l != null)  GDBoxel.GenerateLeftFace(st, new Vector3(x,y,z) , chunkData, Minecraft.textures[chunkData.data[x,y,z,1]], l.data[15,y,z,0]);
                    }
                }
            }
        }

        // upload mesh
        st.GenerateNormals();
        this.Mesh = st.Commit();

        // delete previus collider if it exists
        if (GetChildCount() != 0) GetChild(0).QueueFree();

        // create a StaticBody child with a ConcavePolygonShape collision shape
        this.CreateTrimeshCollision();
    }

    public void SaveBoxel(int x, int y, int z, string block_name){
        if (block_name == "air"){
            chunkData.data[x,y,z,0] = 0;
            chunkData.data[x,y,z,1] = 0;
        }
        else {
            chunkData.data[x,y,z,0] = 1; // 1 for solid
            chunkData.data[x,y,z,1] = Minecraft.GetID(block_name);
        }
    }
}
