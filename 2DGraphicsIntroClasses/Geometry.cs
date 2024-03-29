using System;

/// <summary>
/// Struct which contains classes and operator overloads for handling 2D Vectors with floating-point values
/// </summary>
struct Vec2f
{
    /// <summary>
    /// Distance from the point of origin on the horizontal axis
    /// </summary>
	public float x;

    /// <summary>
    /// Distance from the point of origin on the vertical axis
    /// </summary>
	public float y;

    /// <summary>
    /// Turns the coordinates (x, y) into an array with x being in the zero position and y in the one position
    /// </summary>
    /// <param name="i">The position in the array</param>
    /// <returns> x in the zero poisiton and y in the second</returns>
	public float this [int i] {
		get {
			if (i == 0) return x;
			if (i == 1) return y;
			throw new InvalidOperationException ();
		}
		set {
			if (i == 0) x = value;
			else if (i == 1) y = value;
			else throw new InvalidOperationException ();
		}
	}

    /// <summary>
    /// Divides by the absolute value to equal 1 or -1 to signify direction
    /// </summary>
    /// <returns> 1 or -1 to signify direction </returns>
	public Vec2f Normalize ()
	{
		return this / Norm ();
	}

    /// <summary>
    /// Finds the magnitude of the vector using the Distance Formula
    /// </summary>
    /// <returns>The magnitude of the given Vector2</returns>
	public float Norm ()
	{
		return (float)Math.Sqrt (x * x + y * y);
	}

    /// <summary>
    /// Individually divides the x and y coordinate by a number
    /// </summary>
    /// <param name="v">The Vector to be divided</param>
    /// <param name="num">The number to divide the vector by</param>
    /// <returns>The new vector</returns>
	public static Vec2f operator / (Vec2f v, float num)
	{
		v.x /= num;
		v.y /= num;

		return v;
	}

    /// <summary>
    /// Individually multiplies the x and y coordinates by a number
    /// </summary>
    /// <param name="v">The Vector to be multiplied</param>
    /// <param name="num">The number to multipy the vector by</param>
    /// <returns>The new vector</returns>
	public static Vec2f operator * (Vec2f v, float num)
	{
		v.x *= num;
		v.y *= num;

		return v;
	}

    /// <summary>
    /// Subtracts two vectors x, y coordinate pairs to create a new vector
    /// </summary>
    /// <param name="a">Vector to be subtracted from</param>
    /// <param name="b">Vector to be subtracted by</param>
    /// <returns>The new vector</returns>
	public static Vec2f operator - (Vec2f a, Vec2f b)
	{
		return new Vec2f { x = a.x - b.x, y = a.y - b.y };
	}

    /// <summary>
    /// Adds two vectors x, y coordinate pairs to create a new vector
    /// </summary>
    /// <param name="a">Vector to be added to</param>
    /// <param name="b">Vector to be added</param>
    /// <returns>The new vector</returns>
	public static Vec2f operator + (Vec2f a, Vec2f b)
	{
		return new Vec2f { x = a.x + b.x, y = a.y + b.y };
	}
}

/// <summary>
/// Struct which contains classes and operator overloads for handling 3D Vectors with floating-point values
/// </summary>
public struct Vec3f
{
    /// <summary>
    /// Distance from the point of origin on the horizontal axis
    /// </summary>
	public float x;

    /// <summary>
    /// Distance from the point of origin on the vertical axis
    /// </summary>
	public float y;

    /// <summary>
    /// Distance from the point of origin on the zed axis
    /// </summary>
	public float z;

    /// <summary>
    /// Turns the coordinates (x, y, z) into an array with x being in the zero position, y in the one position, and z in the two position
    /// </summary>
    /// <param name="i">The position in the array</param>
    /// <returns> x in the zero poisiton, y in the second, z in the third</returns>
	public float this [int i] {
		get {
			switch (i) {
			case 0: return x;
			case 1: return y;
			case 2: return z;
			default: throw new InvalidOperationException ();
			}
		}
		set {
			switch (i) {
			case 0: x = value; break;
			case 1: y = value; break;
			case 2: z = value; break;
			default: throw new InvalidOperationException ();
			}
		}
	}

    /// <summary>
    /// Divides by the absolute value to equal 1 or -1 to signify direction
    /// </summary>
    /// <returns> 1 or -1 to signify direction </returns>
	public Vec3f Normalize ()
	{
		return this / Norm ();
	}

    /// <summary>
    /// Finds the magnitude of the vector using the Distance Formula
    /// </summary>
    /// <returns>The magnitude of the given Vector3</returns>
	public float Norm ()
	{
		return (float)Math.Sqrt (x * x + y * y + z * z);
	}

    /// <summary>
    /// Subtracts two vectors x, y, and z coordinate pairs to create a new vector
    /// </summary>
    /// <param name="a">Vector to be subtracted from</param>
    /// <param name="b">Vector to be subtracted by</param>
    /// <returns>The new vector</returns>
	public static Vec3f operator - (Vec3f a, Vec3f b)
	{
		return new Vec3f { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z };
	}

    /// <summary>
    /// Individually divides the x, y, and z coordinates by a number
    /// </summary>
    /// <param name="v">The Vector to be divided</param>
    /// <param name="num">The number to divide the vector by</param>
    /// <returns>The new vector</returns>
	public static Vec3f operator / (Vec3f v, float num)
	{
		v.x /= num;
		v.y /= num;
		v.z /= num;

		return v;
	}

    /// <summary>
    /// Individually multiplies the x, y, and z coordinates by a number
    /// </summary>
    /// <param name="v">The Vector to be multiplied</param>
    /// <param name="num">The number to multipy the vector by</param>
    /// <returns>The new vector</returns>
	public static Vec3f operator * (Vec3f v, float num)
	{
		v.x *= num;
		v.y *= num;
		v.z *= num;

		return v;
	}
}

/// <summary>
/// Struct which contains classes and operator overloads for handling 4D Vectors with floating-point values
/// </summary>
struct Vec4f
{
    /// <summary>
    /// Distance from the point of origin on the horizontal axis
    /// </summary>
	public float x;

    /// <summary>
    /// Distance from the point of origin on the vertical axis
    /// </summary>
	public float y;

    /// <summary>
    /// Distance from the point of origin on the zed axis
    /// </summary>
	public float z;

    /// <summary>
    /// the homogeneous vertex coordinate normally called w
    /// </summary>
	public float h;

    /// <summary>
    /// Turns the coordinates (x, y, z, w) into an array with x being in the zero position, y in the one position, z in the two position, and h in the 3 position
    /// </summary>
    /// <param name="i">The position in the array</param>
    /// <returns> x in the zero poisiton, y in the second, z in the third</returns>
	public float this [int i] {
		get {
			switch (i) {
				case 0: return x;
				case 1: return y;
				case 2: return z;
				case 3: return h;
				default: throw new InvalidOperationException ();
			}
		}
		set {
			switch (i) {
				case 0: x = value; break;
				case 1: y = value; break;
				case 2: z = value; break;
				case 3: h = value; break;
				default: throw new InvalidOperationException ();
			}
		}
	}

    /// <summary>
    /// Divides by the absolute value to equal 1 or -1 to signify direction
    /// </summary>
    /// <returns> 1 or -1 to signify direction </returns>
	public Vec4f Normalize ()
	{
		var len = Norm ();
		return this / len;
	}

    /// <summary>
    /// Finds the magnitude of the vector using the Distance Formula (absolute value)
    /// </summary>
    /// <returns>The magnitude of the given Vector3</returns>
	public float Norm ()
	{
		return (float)Math.Sqrt (x * x + y * y + z * z + h * h);
	}

    /// <summary>
    /// subtracts each of vector b's cordinates individually from vector a's
    /// </summary>
    /// <param name="a">the vector being subtracted from</param>
    /// <param name="b">the vector being subtracted</param>
    /// <returns>a new vector created with the results</returns>
	public static Vec4f operator - (Vec4f a, Vec4f b)
	{
		return new Vec4f { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z, h = a.h - b.h };
	}

    /// <summary>
    /// divides each of vector v's cordinates by num
    /// </summary>
    /// <param name="v">the vector being divided</param>
    /// <param name="num">the number that is being used to divide</param>
    /// <returns>the result of each of vector v's cordinates divided by num</returns>
	public static Vec4f operator / (Vec4f v, float num)
	{
		v.x /= num;
		v.y /= num;
		v.z /= num;
		v.h /= num;

		return v;
	}
}

/// <summary>
/// Struct which contains classes and operator overloads for handling 2D Vectors with integer values
/// </summary>
struct Vec2i
{
    /// <summary>
    /// Distance from the point of origin on the horizontal axis
    /// </summary>
	public int x;

    /// <summary>
    /// Distance from the point of origin on the vertical axis
    /// </summary>
	public int y;

    /// <summary>
    /// Subtracts two vectors x, y coordinate pairs to create a new vector
    /// </summary>
    /// <param name="a">Vector to be subtracted from</param>
    /// <param name="b">Vector to be subtracted by</param>
    /// <returns>The new vector</returns>
	public static Vec2i operator - (Vec2i a, Vec2i b)
	{
		return new Vec2i { x = a.x - b.x, y = a.y - b.y };
	}
}

/// <summary>
/// Struct which contains classes and operator overloads for handling 3D Vectors with integer values
/// </summary>
struct Vec3i
{
    /// <summary>
    /// Distance from the point of origin on the horizontal axis
    /// </summary>
	public int x;

    /// <summary>
    /// Distance from the point of origin on the vertical axis
    /// </summary>
	public int y;

    /// <summary>
    /// Distance from the point of origin on the zed axis
    /// </summary>
	public int z;

    /// <summary>
    /// Subtracts two vectors x, y, and z coordinate pairs to create a new vector
    /// </summary>
    /// <param name="a">Vector to be subtracted from</param>
    /// <param name="b">Vector to be subtracted by</param>
    /// <returns>The new vector</returns>
	public static Vec3i operator - (Vec3i a, Vec3i b)
	{
		return new Vec3i { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z };
	}
}

/// <summary>
/// A collection of advanced mathematical equations and conversion methods
/// </summary>
static class Geometry
{
    /// <summary>
    /// takes the cross product of the 2 vectors
    /// </summary>
    /// <param name="l">the first vector</param>
    /// <param name="r">the second vector</param>
    /// <returns></returns>
	public static Vec3f Cross (Vec3f l, Vec3f r)
	{
		return new Vec3f {
			x = l.y * r.z - l.z * r.y,
			y = l.z * r.x - l.x * r.z,
			z = l.x * r.y - l.y * r.x
		};
	}

    /// <summary>
    /// the dot product
    /// multiplies each of the individual respective coordinates then adds those results together
    /// </summary>
    /// <param name="l">the first vector</param>
    /// <param name="r">the second vector</param>
    /// <returns>the result of multiplying all the coordinates and adding them together</returns>
	public static float Dot (Vec3f l, Vec3f r)
	{
		return l.x * r.x + l.y * r.y + l.z * r.z;
	}

    /// <summary>
    /// creates a vector 4 from a vector3 with fill as the h coordinate
    /// </summary>
    /// <param name="v">the vector 3 being converted</param>
    /// <param name="fill">the h value for the new vector4</param>
    /// <returns> the new vector 4</returns>
	public static Vec4f Embed4D (Vec3f v, float fill = 1)
	{
		return new Vec4f { x = v.x, y = v.y, z = v.z, h = fill };
	}

    /// <summary>
    /// converts a vector 3 into a vector 2
    /// creates the vector 2 with the x and y values of the vector 3
    /// </summary>
    /// <param name="v">the vector 3 to convert</param>
    /// <returns>the new vector 2</returns>
	public static Vec2f Project2D (Vec3f v)
	{
		return new Vec2f { x = v.x, y = v.y };
	}

    /// <summary>
    /// converts a vector 4 into a vector 2
    /// creates the vector 2 with the x and y values of the vector 4
    /// </summary>
    /// <param name="v">the vector 4 to convert</param>
    /// <returns>the new vector 2</returns>
	public static Vec2f Project2D (Vec4f v)
	{
		return new Vec2f { x = v.x, y = v.y };
	}

    /// <summary>
    /// converts a vector 4 into a vector 3
    /// creates the vector 3 with the x and y values of the vector 4
    /// </summary>
    /// <param name="v">the vector 4 to convert</param>
    /// <returns>the new vector 3</returns>
	public static Vec3f Project3D (Vec4f v)
	{
		return new Vec3f { x = v.x, y = v.y, z = v.z };
	}
}