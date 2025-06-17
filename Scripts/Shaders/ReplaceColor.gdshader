shader_type canvas_item;

uniform bool apply;

uniform bool invert_replacement_logic;
uniform bool keep_alpha = true;

uniform vec4 replaced_color;
uniform vec4 new_color;

void fragment() 
{	
	if (apply)
	{
		if ((COLOR == replaced_color) != invert_replacement_logic)
		{
			float a = COLOR.a;
			COLOR = new_color;
			if (keep_alpha)
				COLOR.a = a;
		}
	}
}
