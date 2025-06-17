shader_type canvas_item;

uniform bool keep_alpha;
uniform vec4 color;

void fragment() 
{
	float a = COLOR.a;
	
	COLOR = color;
	
	if (keep_alpha)
		COLOR.a = a;
}
