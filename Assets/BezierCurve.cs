		using UnityEngine;
		using System.IO;
		using System;

public class BezierCurve : MonoBehaviour
{

    public Vector3[] points;
	public float[] velocity;
    public int numberOfPoints = 100;

    // HERE: Unicos parametros que parecem fazer alguma coisa nos calculos
    public float e;
    public float omega;
    public float longitudeOfAscendingNode;
    public float inclination;
	public float a;
	public float rotationAngle;
	public float rotationSpeed;
	public double u;
	public float orbitScale;
    // HERE: Acho que daria para simplificar estas variaveis todas de forma a optimizar, mas nao ha que preocupar com isso
    float counter = 0;
    double cos = 0;
    double sin = 0;
    float cos1 = 0;
    float sin1 = 0;
	double speed;
    double b;
    float b1;
	float normalizer;
    double cosomega;
     double sinomega;
     float cosomega1;
     float sinomega1;
     double cosInclination;
     float cosInclination1;
     double sinInclination;
     float sinInclination1;
     double coslongitudeOfAscendingNode;
     float coslongitudeOfAscendingNode1;
     double sinlongitudeOfAscendingNode;
     float sinlongitudeOfAscendingNode1;
     double doublenormalizedx;
    double doublenormalizedy;
    double doublenormalizedz;
    float normalizedx;
    float normalizedy;
    float normalizedz;
    double doublenormalizer;
     float vectorx1;
     float vectorx2;
     float vectorx3;
     float vectory1;
     float vectory2;
     float vectory3;
     float vectorz1;
     float vectorz2;
     float vectorz3;
     double doublevectorx1;
     double doublevectory2;
     double doublevectorz3;
     float x;
     float y;
     float z;
     float finalx;
     float finaly;
     float finalz;
	static float deg;

    // HERE: Movi do start para generatecurve. assim o inspector pode regenerate a funcao
    public void GenerateCurve()
    {
        // HERE: tavas a meter inclination, omega, longitudeOfAscendingNode a zero e isso fazia com que nao deixasse mudar no inspector
        points = new Vector3[numberOfPoints];
		velocity = new float[numberOfPoints];
		for(int i=0; i<numberOfPoints ; i++){
			counter = i * 2f * 3.1415f * 0.01f ;
			cos = Math.Cos (counter);
			sin = Math.Sin (counter);
			cosomega = Math.Cos (omega);
			sinomega = Math.Sin (omega);
			cosInclination = Math.Cos (inclination);
			sinInclination = Math.Sin (inclination);
			coslongitudeOfAscendingNode = Math.Cos (longitudeOfAscendingNode);
			sinlongitudeOfAscendingNode = Math.Sin (longitudeOfAscendingNode);
			cos1 = (float)cos;
			sin1 = (float)sin;
			cosomega1 = (float)cosomega;
			sinomega1 = (float)sinomega;
			cosInclination1 = (float)cosInclination;
			sinInclination1 = (float)sinInclination;
			coslongitudeOfAscendingNode1 = (float)coslongitudeOfAscendingNode;
			sinlongitudeOfAscendingNode1 = (float)sinlongitudeOfAscendingNode;
			b = Math.Pow (1 - e * e, 0.5);
			b1 = (float)b;
			doublenormalizedx = -sinInclination*coslongitudeOfAscendingNode;
			doublenormalizedy = cosInclination;
			doublenormalizedz = sinInclination*sinlongitudeOfAscendingNode;
			doublenormalizer = Math.Pow (doublenormalizedx, 2)+Math.Pow (doublenormalizedy, 2)+Math.Pow (doublenormalizedz, 2);
			doublenormalizer = Math.Pow (doublenormalizer, 0.5);
			doublenormalizedx = doublenormalizedx/doublenormalizer;
			doublenormalizedy = doublenormalizedy/doublenormalizer;
			doublenormalizedz = doublenormalizedz/doublenormalizer;
			normalizedx = (float)doublenormalizedx;
			normalizedy = (float)doublenormalizedy;
			normalizedz = (float)doublenormalizedz;
			doublevectorx1 = cosomega + Math.Pow (normalizedx, 2) * (1 - cosomega);
			vectorx1 = (float)doublevectorx1;
			vectorx2 = (1 - cosomega1)*normalizedx*normalizedy-normalizedz*sinomega1;
			vectorx3 = (1 - cosomega1)*normalizedx*normalizedz+normalizedy*sinomega1;
			doublevectory2 = cosomega + Math.Pow (normalizedy, 2) * (1 - cosomega);
			vectory2 = (float)doublevectory2;
			vectory1 = (1 - cosomega1)*normalizedx*normalizedy+normalizedz*sinomega1;
			vectory3 = (1 - cosomega1)*normalizedy*normalizedz-normalizedx*sinomega1;
			doublevectorz3 = cosomega + Math.Pow (normalizedz, 2) * (1 - cosomega);
			vectorz3 = (float)doublevectorz3;
			vectorz1 = (1 - cosomega1)*normalizedx*normalizedz-normalizedy*sinomega1;
			vectorz2 = (1 - cosomega1)*normalizedy*normalizedz+normalizedx*sinomega1;
			x = sinlongitudeOfAscendingNode1 * (b1 * sin1) + coslongitudeOfAscendingNode1 * ((cos1 + e) * cosInclination1);
			y = sinInclination1 * (cos1 + e);
			z = coslongitudeOfAscendingNode1 * (b1 * sin1) - sinlongitudeOfAscendingNode1 * (cos1 + e) * cosInclination1;
			finalx = x * vectorx1 + y * vectorx2 + z * vectorx3;
			finaly = x * vectory1 + y * vectory2 + z * vectory3;
			finalz = x * vectorz1 + y * vectorz2 + z * vectorz3;
			points[i]=new Vector3(10*orbitScale*a*finalx,10*orbitScale*a*finaly,10*orbitScale*a*finalz);
			speed = Math.Pow (u * (2 / (Math.Pow ((Math.Pow (a*finalx, 2) + Math.Pow (a*finaly, 2) + Math.Pow (a*finalz, 2)), 0.5)*149597871) - 1 / (a*149597871)), 0.5);
			speed = orbitScale*speed *0.21095*10/(365.25);
			velocity [i] = (float)speed;
        }
    }

}