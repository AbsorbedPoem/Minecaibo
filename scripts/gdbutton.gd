extends TextureButton

func _ready():
	connect("button_down", self, "play")
	
func play():
	get_parent().get_node("Sound").play(0);
	if name == "Play":
		get_tree().change_scene("res://Scenes/nivel.tscn")
