extends KinematicBody

var speed = 7
var acceleration = 5
var gravity = 0.45
var jump = 8

var mouse_sensitivity = 0.40

var direction = Vector3()
var velocity = Vector3()
var fall = Vector3() 

onready var head = $Head
onready var raycast = $Head/RayCast
onready var bounding_box = $"selection box"
onready var chunk_gen = get_parent().get_node("Chunks")

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	
func _input(event):
	if event is InputEventMouseMotion:
		var rot = head.rotation_degrees
		
		head.rotation_degrees = Vector3(rot.x -event.relative.y * mouse_sensitivity, rot.y -event.relative.x * mouse_sensitivity, 0)
		head.rotation.x = clamp(head.rotation.x, deg2rad(-90), deg2rad(90))

func _process(_delta):
	var format_string = "[font=Fonts/minecraft_font.tres](%d, %d, %d)[/font]"
	get_parent().get_node("Control").get_node("RichTextLabel").bbcode_text = format_string % [int(floor(translation.x)), int(floor(translation.y)), int(floor(translation.z))]

func _physics_process(delta):
	direction = Vector3()
	
	move_and_slide(fall, Vector3.UP)
	
	if not is_on_floor():
		fall.y -= gravity
	else:
		fall.y = -gravity
		
	if Input.is_action_pressed("ui_accept") and is_on_floor():
		fall.y = jump

	# degub
	if Input.is_action_pressed("ui_extra"):
		gravity = 0
		fall.y = 0
		if Input.is_action_just_pressed("ui_accept"):
			fall.y = jump * 10
	else:
		gravity = 0.45

	if Input.is_action_pressed("ui_up"):
		direction += Vector3(-sin(head.rotation.y), 0, -cos(head.rotation.y))
	elif Input.is_action_pressed("ui_down"):
		direction += Vector3(sin(head.rotation.y), 0, cos(head.rotation.y))
	
	if Input.is_action_pressed("ui_left"):
		direction += Vector3(-cos(head.rotation.y), 0, sin(head.rotation.y))
	elif Input.is_action_pressed("ui_right"):
		direction += Vector3(cos(head.rotation.y), 0, -sin(head.rotation.y))
		
	direction = direction.normalized()
	velocity = velocity.linear_interpolate(direction * speed, acceleration * delta) 
	velocity = move_and_slide(velocity, Vector3.UP)

	var col = raycast.get_collision_point() - raycast.get_collision_normal() * 0.1;

	var raycast_block_index = Vector3(floor(col.x), floor(col.y), floor(col.z))

	var colput = raycast.get_collision_point() + raycast.get_collision_normal() * 0.1;

	var raycast_put_block_index = Vector3(floor(colput.x), floor(colput.y), floor(colput.z))

	bounding_box.visible = raycast.is_colliding();
	bounding_box.global_translation = raycast_block_index;
	
	if (Input.is_action_just_pressed("attack") and raycast.get_collider() != null):
		chunk_gen.Destroy(raycast_block_index, raycast.get_collider().get_parent())

	if (Input.is_action_just_pressed("action") and raycast.get_collider() != null):
		chunk_gen.Place(raycast_put_block_index, raycast.get_collider().get_parent())
		
