using UnityEngine;
using System.Collections;

public class ColumnPool : MonoBehaviour
{
    public GameObject ColumnPrefab;                                 //The column game object.
    public Transform ColumnsParent;
    public int ColumnPoolSize = 5;                                  //How many columns to keep on standby.
    public float SpawnRate = 3f;                                    //How quickly columns spawn.
    public float ColumnMin = -1f;                                   //Minimum y value of the column position.
    public float ColumnMax = 3.5f;                                  //Maximum y value of the column position.

    private GameObject[] columns;                                   //Collection of pooled columns.
    private int currentColumn = 0;                                  //Index of the current column in the collection.

    private Vector2 objectPoolPosition = new Vector2(-15, -25);     //A holding position for our unused columns offscreen.
    private float spawnXPosition = 10f;


    private Coroutine spawnColumnsCoroutine;


    void Start()
    {

        //Initialize the columns collection.
        columns = new GameObject[ColumnPoolSize];
        //Loop through the collection... 
        for (int i = 0; i < ColumnPoolSize; i++)
        {
            //...and create the individual columns.
            columns[i] = Instantiate(ColumnPrefab, objectPoolPosition, Quaternion.identity,ColumnsParent);
        }


        InvokeSpawnColumnsCoroutine(3);
    }

    public void InvokeSpawnColumnsCoroutine(int delay)
    {
        if (spawnColumnsCoroutine != null)
        {
            StopCoroutine(spawnColumnsCoroutine);
            spawnColumnsCoroutine = null;
        }

        spawnColumnsCoroutine = StartCoroutine(SpawnColumns(delay));
    }

    public IEnumerator SpawnColumns(int delay)
    {

        while (GameManager.Instance.GameOver == false)
        {
            //Set a random y position for the column
            float spawnYPosition = Random.Range(ColumnMin, ColumnMax);
            //...then set the current column to that position.
            columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            //Increase the value of currentColumn. If the new size is too big, set it back to zero
            currentColumn++;

            if (currentColumn >= ColumnPoolSize)
            {
                currentColumn = 0;
            }

            yield return new WaitForSeconds(delay);
        }
    }

   
}