using UnityEngine;
using System.Collections;

public class ColumnPool : MonoBehaviour
{
    public GameObject ColumnPrefab;                               
    public GameObject SingleColumnPrefab;
    public Transform ColumnsParent;
    public int ColumnPoolSize = 5;                                  
    public float ColumnMin = -1f;                                  
    public float ColumnMax = 3.5f;                                 

    private GameObject[] columns;                                   
    private int currentColumn = 0;                                  
    private Vector2 objectPoolPosition = new Vector2(-15, -25);     
    private float spawnXPosition = 10f;


    private Coroutine spawnColumnsCoroutine;


    void Start()
    {

        //Initialize the columns collection.
        columns = new GameObject[ColumnPoolSize];
        //Loop through the collection... 
        for (int i = 0; i < ColumnPoolSize; i++)
        {
            //Every three columns will instantiate a single floating column
            if (i == 3)
            {
                columns[i] = Instantiate(SingleColumnPrefab, objectPoolPosition, Quaternion.identity, ColumnsParent);
            }
            else
            {
            columns[i] = Instantiate(ColumnPrefab, objectPoolPosition, Quaternion.identity,ColumnsParent);
            }


        }
        
    }

    //Invoke coroutine
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