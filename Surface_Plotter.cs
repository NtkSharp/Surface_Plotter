using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Plotter : MonoBehaviour
{

    //--------------------------Surface_Plotter--------------------------//
    //
    //
    //
    //


    [Range(2, 200)]
    public int Resolution = 2;
    //<0.028f better Approximation
    //[Range(0.01f, 0.5f)]
    float DBP = 0.028f;

    //User Parameters for the sinewave
    [Range(-10, 10)]
    public float MagnifyX = 1;
    [Range(-10, 10)]
    public float MagnifyY = 1;
    [Range(0.1f, 1000)]
    public float Frequency = 1;
    [Range(0.1f, 100)]
    public float Par1 = 1;
    [Range(-10, 10)]
    public float Amplitude = 1;

    //Parameters for the mesh
    Mesh Surface;
    
    //Set the material for the mesh
    public Material Mymaterial;
    public float XMove = 0;
    public float ZMove = 0;
    Vector3[] VerticesArray;
    Vector2[] UVArray;
    Vector3 TriangleArray;

    public Vector3 IND = new Vector3(0, 0, 0);
    


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Surface = GetComponent<MeshFilter>().mesh;

        Surface.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        Surface.vertices = SetVerticesToMesh(Resolution);
        Surface.uv = SetUVToMesh(Surface.vertices, Resolution);
        Surface.triangles = SetTrianglesToMesh(Resolution);
        GetComponent<Renderer>().material = Mymaterial;

    }

    // Update is called once per frame
    void Update()
    {
        SetPositionOfSurface();
        Surface.Clear();

        Surface.vertices = SetVerticesToMesh(Resolution);
        Surface.uv = SetUVToMesh(Surface.vertices, Resolution);
        Surface.triangles = SetTrianglesToMesh(Resolution);
        Surface.RecalculateNormals();

    }

    // Here you set the X,Y,Z values of the surface. The parametric equations should be given in i and j in the double loop. 
    Vector3[] SetVerticesToMesh(int Dimensions)
    {
        Vector3[] OutputArray = new Vector3[Dimensions * Dimensions];
        for (int i = 0; i < Dimensions; i++)
        {
            for (int j = 0; j < Dimensions; j++)
            {
                //Set X values
                float ParX = i * 100f * MagnifyX * DBP;
                //Set Y values
                float ParY = Amplitude * Mathf.Sin(2 * Mathf.PI * i * Frequency  * DBP / 300f);
                //Set Z values
                float ParZ = j * 100f * MagnifyY * DBP;
                OutputArray[j + i * (Dimensions)] = new Vector3(ParX, ParY, ParZ);

            }

        }
        return OutputArray;

    }

    Vector2[] SetUVToMesh(Vector3[] InputArray, int Dimensions)
    {

        Vector2[] OutputArray = new Vector2[Dimensions * Dimensions];
        for (int i = 0; i < Dimensions; i++)
        {
            for (int j = 0; j < Dimensions; j++)
            {

                OutputArray[i + j * Dimensions] = new Vector2(InputArray[i + j * Dimensions].x, InputArray[i + j * Dimensions].z);
            }

        }
        return OutputArray;


    }


    //Assigns triangles to a mesh
    int[] SetTrianglesToMesh(int Dimensions)
    {
        int LocalTempDim = Dimensions - 1;
        int[] TempArrayA = new int[LocalTempDim * LocalTempDim];
        int[] TempArrayB = new int[LocalTempDim * LocalTempDim];
        int[] TempArrayC = new int[LocalTempDim * LocalTempDim];

        //For the othe meshes

        int[] TempArrayD = new int[LocalTempDim * LocalTempDim];
        int[] TempArrayE = new int[LocalTempDim * LocalTempDim];
        int[] TempArrayF = new int[LocalTempDim * LocalTempDim];


        int[] OutputArray = new int[LocalTempDim * LocalTempDim];
        for (int i = 0; i < LocalTempDim; i++)
        {
            for (int j = 0; j < LocalTempDim; j++)
            {

                TempArrayA[j + i * LocalTempDim] = i + j * Dimensions;
                TempArrayB[j + i * LocalTempDim] = (i + 1) + j * Dimensions;
                TempArrayC[j + i * LocalTempDim] = i + (j + 1) * Dimensions;
                //Debug.Log("------------At place" + "(" + i + "," + j + ") We have"+(i + j * LocalTempDim));

                TempArrayD[j + i * LocalTempDim] = (i + 1) + (j + 1) * Dimensions;
                TempArrayE[j + i * LocalTempDim] = (i + 1) + j * Dimensions;
                TempArrayF[j + i * LocalTempDim] = i + (j + 1) * Dimensions;


            }


        }
        return ConcArray(Interleaving(TempArrayB, TempArrayA, TempArrayC), Interleaving(TempArrayD, TempArrayE, TempArrayF));

    }

    //Interleaves three Arrays. 
    int[] Interleaving(int[] InputArray, int[] InputArray2, int[] InputArray3)
    {
        int[] OutputArray = new int[InputArray.Length + InputArray2.Length + InputArray3.Length];
        for (int i = 0; i < InputArray.Length; i++)
        {

            OutputArray[3 * i] = InputArray2[i];
            OutputArray[3 * i + 1] = InputArray[i];
            OutputArray[3 * i + 2] = InputArray3[i];


        }
        return OutputArray;
    }

    //Makes a new array with lenght equal to the sum of lenghts of the inputs. First puts
    //InputArrayA then InputArrayB
    int[] ConcArray(int[] InputArrayA, int[] InputArrayB)
    {
        int[] OutputArray = new int[InputArrayA.Length + InputArrayB.Length];
        for (int i = 0; i < InputArrayA.Length; i++)
        {

            OutputArray[i] = InputArrayA[i];
        }

        for (int j = 0; j < InputArrayB.Length; j++)
        {

            OutputArray[j + InputArrayA.Length] = InputArrayB[j];
        }
        return OutputArray;
    }


    void SetPositionOfSurface()
    {
        transform.position = new Vector3(XMove, 0, ZMove);

    }


}
