using Godot;
using System;

public class RayCast : MeshInstance
{
    SurfaceTool st = new SurfaceTool();
    public override void _Ready()
    {
        
        st.Begin(Mesh.PrimitiveType.Lines);

        Vector3 FTL = new Vector3(0,1,1);   // Front-top-left
        Vector3 FTR = new Vector3(1,1,1);   // Front-top-right
        Vector3 FBL = new Vector3(0,0,1);   // Front-bottom-left
        Vector3 FBR = new Vector3(1,0,1);   // Front-bottom-right
        Vector3 BBR = new Vector3(1,0,0);   // Back-bottom-right
        Vector3 BTR = new Vector3(1,1,0);   // Back-top-right
        Vector3 BTL = new Vector3(0,1,0);   // Back-top-left
        Vector3 BBL = new Vector3(0,0,0);   // Back-bottom-left

        AddLine(FTL,FTR);
        AddLine(FTL,FBL);
        AddLine(FTL,BTL);
        AddLine(FBR,FBL);
        AddLine(FBR,FTR);
        AddLine(FBR,BBR);
        AddLine(BTR,BTL);
        AddLine(BTR,BBR);
        AddLine(BTR,FTR);
        AddLine(BBL,BBR);
        AddLine(BBL,BTL);
        AddLine(BBL,FBL);

        Mesh = st.Commit();
    }

    void AddLine(Vector3 p1, Vector3 p2){
        st.AddColor(Color.Color8(20,20,20));
        st.AddVertex(p1);

        st.AddColor(Color.Color8(20,20,20));
        st.AddVertex(p2);
    }
}
