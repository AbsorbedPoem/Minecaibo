using Godot;
using System;

public class GDBoxel
{
    public static void GenerateFaces(SurfaceTool st, Vector3 index, ChunkData currentChunk, int[,] faces){

        Vector3 FTL = new Vector3(index.x    , index.y + 1, index.z + 1);   // Front-top-left
        Vector3 FTR = new Vector3(index.x + 1, index.y + 1, index.z + 1);   // Front-top-right
        Vector3 FBL = new Vector3(index.x    , index.y    , index.z + 1);   // Front-bottom-left
        Vector3 FBR = new Vector3(index.x + 1, index.y    , index.z + 1);   // Front-bottom-right
        Vector3 BBR = new Vector3(index.x + 1, index.y    , index.z    );   // Back-bottom-right
        Vector3 BTR = new Vector3(index.x + 1, index.y + 1, index.z    );   // Back-top-right
        Vector3 BTL = new Vector3(index.x    , index.y + 1, index.z    );   // Back-top-left
        Vector3 BBL = new Vector3(index.x    , index.y    , index.z    );   // Back-bottom-left

        int x = Convert.ToInt32(index.x);
        int y = Convert.ToInt32(index.y);
        int z = Convert.ToInt32(index.z);

        try{ if (currentChunk.data[x,y,z+1,0] == 0){
            addTri(st,FTL,FTR,FBL, faces[1,0], faces[1,1]); //front
            addTri(st,FBR,FBL,FTR, faces[1,0], faces[1,1], true);
            }
        } catch (System.IndexOutOfRangeException) {}

        try{ if (currentChunk.data[x,y,z-1,0] == 0){
            addTri(st,BTR,BTL,BBR, faces[1,0], faces[1,1]); //back
            addTri(st,BBL,BBR,BTL, faces[1,0], faces[1,1], true);
            }
        } catch (System.IndexOutOfRangeException) {}

        try{ if (currentChunk.data[x-1,y,z,0] == 0){
            addTri(st,BTL,FTL,BBL, faces[1,0], faces[1,1]); //left
            addTri(st,FBL,BBL,FTL, faces[1,0], faces[1,1], true);
            }
        } catch (System.IndexOutOfRangeException) {}

        try{ if (currentChunk.data[x+1,y,z,0] == 0){
            addTri(st,FTR,BTR,FBR, faces[1,0], faces[1,1]); //right
            addTri(st,BBR,FBR,BTR, faces[1,0], faces[1,1], true);
            }
        } catch (System.IndexOutOfRangeException) {}

        try{ if (currentChunk.data[x,y+1,z,0] == 0){
            addTri(st,FTL,BTL,FTR, faces[0,0], faces[0,1]); //top
            addTri(st,BTR,FTR,BTL, faces[0,0], faces[0,1], true);
            }
        } catch (System.IndexOutOfRangeException) {
            addTri(st,FTL,BTL,FTR, faces[0,0], faces[0,1]);
            addTri(st,BTR,FTR,BTL, faces[0,0], faces[0,1], true);}

        try{ if (currentChunk.data[x,y-1,z,0] == 0){
            addTri(st,BBR,BBL,FBR, faces[2,0], faces[2,1]); //bottom
            addTri(st,FBL,FBR,BBL, faces[2,0], faces[2,1], true);}
        } catch (System.IndexOutOfRangeException) {
            addTri(st,BBR,BBL,FBR, faces[2,0], faces[2,1]);
            addTri(st,FBL,FBR,BBL, faces[2,0], faces[2,1], true);
        }
    }

    public static void addTri(SurfaceTool ST, Vector3 p1, Vector3 p2, Vector3 p3, int x, int y, bool bot = false)
    {   
        if (bot){
            ST.AddUv(new Vector2(x/8f + 1/8f, y + 1));
            ST.AddVertex(p1);

            ST.AddUv(new Vector2(x/8f + 0   , y + 1));
            ST.AddVertex(p2);

            ST.AddUv(new Vector2(x/8f + 1/8f, y + 0));
            ST.AddVertex(p3);
        }
        else {
            ST.AddUv(new Vector2(x/8f + 0   , y + 0));
            ST.AddVertex(p1);

            ST.AddUv(new Vector2(x/8f + 1/8f, y + 0));
            ST.AddVertex(p2);

            ST.AddUv(new Vector2(x/8f + 0   , y + 1));
            ST.AddVertex(p3);
        }
    }

    public static void GenerateFrontFace(SurfaceTool st, Vector3 index, ChunkData currentChunk, int[,] faces, int border){
        if(border == 0){
            Vector3 FTL = new Vector3(index.x    , index.y + 1, index.z + 1);   // Front-top-left
            Vector3 FTR = new Vector3(index.x + 1, index.y + 1, index.z + 1);   // Front-top-right
            Vector3 FBL = new Vector3(index.x    , index.y    , index.z + 1);   // Front-bottom-left
            Vector3 FBR = new Vector3(index.x + 1, index.y    , index.z + 1);   // Front-bottom-right
            Vector3 BBR = new Vector3(index.x + 1, index.y    , index.z    );   // Back-bottom-right
            Vector3 BTR = new Vector3(index.x + 1, index.y + 1, index.z    );   // Back-top-right
            Vector3 BTL = new Vector3(index.x    , index.y + 1, index.z    );   // Back-top-left
            Vector3 BBL = new Vector3(index.x    , index.y    , index.z    );   // Back-bottom-left

            addTri(st,FTL,FTR,FBL, faces[1,0], faces[1,1]); //front
            addTri(st,FBR,FBL,FTR, faces[1,0], faces[1,1], true);
        }
    }

    public static void GenerateBackFace(SurfaceTool st, Vector3 index, ChunkData currentChunk, int[,] faces, int border){
        if(border == 0){
            Vector3 FTL = new Vector3(index.x    , index.y + 1, index.z + 1);   // Front-top-left
            Vector3 FTR = new Vector3(index.x + 1, index.y + 1, index.z + 1);   // Front-top-right
            Vector3 FBL = new Vector3(index.x    , index.y    , index.z + 1);   // Front-bottom-left
            Vector3 FBR = new Vector3(index.x + 1, index.y    , index.z + 1);   // Front-bottom-right
            Vector3 BBR = new Vector3(index.x + 1, index.y    , index.z    );   // Back-bottom-right
            Vector3 BTR = new Vector3(index.x + 1, index.y + 1, index.z    );   // Back-top-right
            Vector3 BTL = new Vector3(index.x    , index.y + 1, index.z    );   // Back-top-left
            Vector3 BBL = new Vector3(index.x    , index.y    , index.z    );   // Back-bottom-left

            addTri(st,BTR,BTL,BBR, faces[1,0], faces[1,1]); //back
            addTri(st,BBL,BBR,BTL, faces[1,0], faces[1,1], true);
        }
    }

    public static void GenerateRightFace(SurfaceTool st, Vector3 index, ChunkData currentChunk, int[,] faces, int border){
        if(border == 0){
            Vector3 FTL = new Vector3(index.x    , index.y + 1, index.z + 1);   // Front-top-left
            Vector3 FTR = new Vector3(index.x + 1, index.y + 1, index.z + 1);   // Front-top-right
            Vector3 FBL = new Vector3(index.x    , index.y    , index.z + 1);   // Front-bottom-left
            Vector3 FBR = new Vector3(index.x + 1, index.y    , index.z + 1);   // Front-bottom-right
            Vector3 BBR = new Vector3(index.x + 1, index.y    , index.z    );   // Back-bottom-right
            Vector3 BTR = new Vector3(index.x + 1, index.y + 1, index.z    );   // Back-top-right
            Vector3 BTL = new Vector3(index.x    , index.y + 1, index.z    );   // Back-top-left
            Vector3 BBL = new Vector3(index.x    , index.y    , index.z    );   // Back-bottom-left

            addTri(st,FTR,BTR,FBR, faces[1,0], faces[1,1]); //right
            addTri(st,BBR,FBR,BTR, faces[1,0], faces[1,1], true);
        }
    }

    public static void GenerateLeftFace(SurfaceTool st, Vector3 index, ChunkData currentChunk, int[,] faces, int border){
        if(border == 0){
            Vector3 FTL = new Vector3(index.x    , index.y + 1, index.z + 1);   // Front-top-left
            Vector3 FTR = new Vector3(index.x + 1, index.y + 1, index.z + 1);   // Front-top-right
            Vector3 FBL = new Vector3(index.x    , index.y    , index.z + 1);   // Front-bottom-left
            Vector3 FBR = new Vector3(index.x + 1, index.y    , index.z + 1);   // Front-bottom-right
            Vector3 BBR = new Vector3(index.x + 1, index.y    , index.z    );   // Back-bottom-right
            Vector3 BTR = new Vector3(index.x + 1, index.y + 1, index.z    );   // Back-top-right
            Vector3 BTL = new Vector3(index.x    , index.y + 1, index.z    );   // Back-top-left
            Vector3 BBL = new Vector3(index.x    , index.y    , index.z    );   // Back-bottom-left

            addTri(st,BTL,FTL,BBL, faces[1,0], faces[1,1]); //left
            addTri(st,FBL,BBL,FTL, faces[1,0], faces[1,1], true);
        }
    }
}
