using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //This is a Singleton of the BoidSpawner. there is only one instance 
    // of BoidSpawner, so we can store it in a static variable named s.
    static public Spawner       S;
    static public List<Boid>    boids;

    // These fields allow you to adjust the spawning behavior of the boids
    [Header("Set in Inspector: Spawning")]
    public GameObject           boidPrefab;
    public Transform            boidAnchor;
    public int                  numBoids = 100;
    public float                spawnRadius = 100f;
    public float                spawnDelay = 0.1f;

    // These fields allow you to adjust the flocking behavior of the boids
    [Header("Set in Inspector: Boids")]
    public float                velocity = 30f;
    public float                neighborDist = 30f;
    public float                collDist = 4f;
    public float                velMatching = 0.25f;
    public float                flockCentering = 0.2f;
    public float                collAvoid = 2f;
    public float                attractPull = 2f;
    public float                attractPush = 2f;
    public float                attractPushDist = 5f;
    
    void Awake()
    {
        //Set the Singleton S to be this instance of BoidSpawner
        S = this;
        //Start instantiation of the Boids
        boids = new List<Boid>();
        InstantiateBoid();
        this.SphereFormation(10,3,7);
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        boids.Add(b);
        if (boids.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }

    private List<GameObject> HalfCircleFormation(int numberOfPoints, int radius)
    {
        // Reference to the numberOfPoints to spawn minus 1.
        int pieces = numberOfPoints - 1;

        // Reference to the list of spheres that will be created for the half circle.
        List<GameObject> spheres = new List<GameObject>();

        // Set a container variable that is used to hold child objects in the inspector.
        GameObject container = new GameObject("SphereContainer");
        // Set the position to 0.
        container.transform.position = Vector3.zero;

        // Loop through the numberOfPoints that are in the half circle.
        for (int i = 0; i < numberOfPoints; i++)
        {
            // Instantiate the prefab.
            GameObject instance = Instantiate(boidPrefab);

            // Get the angle of the current index being instantiated
            // from the center.
            // Mathf.PI represents half a circle.
            float theta = Mathf.PI * i / pieces;
            // Get the X Position of the theta angle times 1.5f. 1.5f is the radius.
            float X = Mathf.Cos(theta) * radius;
            // Get the Y Position of the theta angle times 1.5f. 1.5f is the radius.
            float Y = Mathf.Sin(theta) * radius;

            // Set the instantiated gameObjects position to a new Vector3 with the new variables.
            instance.transform.position = new Vector3(X, Y, 0);
            // Set the gameObjects color to green.
            instance.GetComponent<MeshRenderer>().material.color = Color.green;
            // Set the instantiated object as a child of the container gameObject.
            instance.transform.SetParent(container.transform);
            // Add the instance to the list.
            spheres.Add(instance);
        }
        // Return the half circle that was created.
        return spheres;
    }

    /// <summary>
    /// The SphereFormation function.
    /// Creates a Sphere Formation.
    /// </summary>
    /// <param name="numberOfPoints">The numberOfPoints in the sphere.</param>
    /// <param name="radius">The radius of the sphere.</param>
    /// <param name="numberOfMeridians">The number numberOfMeridians in the sphere.</param>
    private void SphereFormation(int numberOfPoints, int radius, int numberOfMeridians)
    {
        // Create a Half Circle Formation and return the list of gameObjects.
        List<GameObject> spheres = HalfCircleFormation(numberOfPoints, radius);
        // Create a list of colors.
        List<Color> colors = new List<Color>() { Color.black, Color.blue, Color.red, Color.yellow };
        // Reference to the index of color being set to spheres.
        int colorIndex = 0;

        // Find the container variable that is used to hold child objects in the inspector.
        GameObject sphereContainer = GameObject.Find("SphereContainer");

        // Loop through the numberOfMeridians.
        // Meridians are the amount of half circles you want to create.
        for (int i = 1; i < numberOfMeridians; i++)
        {// Get the angle of the current index being instantiated from the center.
            float phi = 2 * Mathf.PI * ((float)i / (float)numberOfMeridians);

            // Loop through the numberOfPoints you want to spawn in this meridian
            for (int j = 1; j < numberOfPoints; j++)
            {
                // Instantiate the prefab.
                GameObject instance = Instantiate(boidPrefab);

                // Get the X position of the current sphere being viewed.
                float X = spheres[j].transform.position.x;
                // Get the Y position of the current sphere being viewed.
                float Y = spheres[j].transform.position.y * Mathf.Cos(phi) - spheres[j].transform.position.z * Mathf.Sin(phi);
                // Get the Z position of the current sphere being viewed.
                float Z = spheres[j].transform.position.z * Mathf.Cos(phi) + spheres[j].transform.position.y * Mathf.Sin(phi);

                // Set the instantiated gameObjects position to a new Vector3 with the new variables.
                instance.transform.position = new Vector3(X, Y, Z);
                // Set the gameObjects color to the color being indexed from the colors list.
                instance.GetComponent<MeshRenderer>().material.color = colors[colorIndex];
                // Set the instantiated object as a child of the container gameObject.
                instance.transform.SetParent(sphereContainer.transform);
            }

            // If the colorIndex is greater than or equal to the colors count - 1.
            // Set the color index to 0.
            // This means reset the colors back from the beginning of the list.
            // This will prevent throwing an exception from indexing the list.
            // Else just increment in the colorIndex by 1.
            if (colorIndex >= colors.Count - 1)
                colorIndex = 0;
            else
                colorIndex++;
        }
    }

}