using UnityEngine;

namespace Models.NPCScripts
{
    /// <summary>
    ///     Класс-генератор маршрута из пула точек
    /// </summary>
    public class RouteCompile
    {
        private int lastNum; //последняя добавленная точка(устаревшее)
        private float X;
        private float Y;
        private float Z;

        public Vector3[] Compile(Vector3 startPosition, float range)
        {
            var length = Random.Range(4, 10); //генерация размера маршрута
            //Debug.Log("Length: " + length);
            var route = new Vector3[length];
            for (var i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    X = Random.Range(startPosition.x - range,
                        startPosition.x + range); //генерация случайной координаты Х в заданной области
                    Z = startPosition.z +
                        Mathf.Sqrt(Mathf.Pow(range, 2) -
                                   Mathf.Pow(X - startPosition.x,
                                       2)); //рассчет координаты Z исходя из значения координаты Х
                    Y = Terrain.activeTerrain.SampleHeight(new Vector3(X, 0, Z));
                }
                else if (i % 2 == 0)
                {
                    X = Random.Range(startPosition.x, startPosition.x + range);
                    Z = startPosition.z +
                        Mathf.Sqrt(Mathf.Pow(range, 2) -
                                   Mathf.Pow(X - startPosition.x,
                                       2)); //рассчет координаты Z исходя из значения координаты Х
                    Y = Terrain.activeTerrain.SampleHeight(new Vector3(X, 0, Z));
                }
                else if (i % 3 == 0)
                {
                    X = Random.Range(startPosition.x - range, startPosition.x);
                    Z = startPosition.z +
                        Mathf.Sqrt(Mathf.Pow(range, 2) -
                                   Mathf.Pow(X - startPosition.x,
                                       2)); //рассчет координаты Z исходя из значения координаты Х
                    Y = Terrain.activeTerrain.SampleHeight(new Vector3(X, 0, Z));
                }
                else
                {
                    var Xdelta = Mathf.Abs(startPosition.x) - Mathf.Abs(X);
                    if (X < startPosition.x)
                        X = startPosition.x + Xdelta;
                    else
                        X = startPosition.x - Xdelta;
                    var Zdelta = Mathf.Abs(startPosition.z) - Mathf.Abs(Z);
                    if (Z < startPosition.z)
                        Z = startPosition.z + Zdelta;
                    else
                        Z = startPosition.z - Zdelta;
                    Y = Terrain.activeTerrain.SampleHeight(new Vector3(X, 0, Z));
                }

                route[i] = new Vector3(X, Y, Z);
            }

            //Debug.Log("Route created");
            return route;
        }
    }
}