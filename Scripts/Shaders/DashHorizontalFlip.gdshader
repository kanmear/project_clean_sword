shader_type canvas_item;

uniform bool isFlipped;

void vertex() {
	if (isFlipped)
		VERTEX.x = 1.0 - VERTEX.x;
}
