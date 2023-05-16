using Differ.Shapes;
using UnityEngine;
using Collision = Differ.Collision;

public class Test {

	public static void TestStuff() {
		var circle = new Circle(0, 0, 5);
		var anotherCircle = new Circle(3, 0, 3);

		var res = Collision.shapeWithShape(circle, anotherCircle);
		Debug.Log(":)");
	}
}
