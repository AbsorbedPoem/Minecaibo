using Godot;
using System;

public class HellowMessage : AnimationPlayer
{
    public override void _Ready()
    {
        string[] frases = {"MOLLEJA",
        "b1.0 - llega el meú :3",
        "VERGACIOOON!!!!",
        "Foo",
        "I'm not Notch :/",
        "a",
        "co-tu-fas",
        "random splash text"};

        int n = new System.Random().Next(frases.GetLength(0));

        GetParent<RichTextLabel>().BbcodeText = "[center][color=black][font=Fonts/minecraft_font.tres]" + frases[n] + "[/font]";
        GetParent().GetNode<RichTextLabel>("yellow").BbcodeText = "[center][color=yellow][font=Fonts/minecraft_font.tres]" + frases[n] + "[/font]";
        Play("flicker");
    }
}
