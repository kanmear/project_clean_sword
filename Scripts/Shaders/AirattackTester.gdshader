shader_type canvas_item;

uniform bool isWhite;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	if (isWhite) {
		COLOR = texture(TEXTURE, UV);
		COLOR.b = 1.0;
		COLOR.g = 1.0;
		COLOR.r = 1.0;
	}
}

