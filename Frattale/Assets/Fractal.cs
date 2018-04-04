using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fractal : MonoBehaviour {
    Material[,] materiali;
    static public Vector3[] direzioni = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.down, Vector3.back };
    static public Quaternion[] orientamenti = { Quaternion.identity, Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, 90),
    Quaternion.Euler(90, 0, 0), Quaternion.Euler(0, -90, 0), Quaternion.Euler(-90, 0, 0) };

    public Mesh[] mesh;
    public float rotazioneMassima = 20;
    public Material material;

    public int maxDepth = 5;
    public int depth = 0;
    public float probabilita = 0.99f;
    public float scalaFiglio = 0.5f;

    void InstantializeMaterials()
    {
        materiali = new Material[maxDepth + 1, 2];
        for (int i = 0; i < maxDepth; i++)
        {
            float t = (float)i / (maxDepth - 1);
            materiali[i,0] = new Material(material);
            materiali[i,0].color = Color.Lerp(Color.red, material.color, t*t);
            materiali[i, 1] = new Material(material);
            materiali[i, 1].color = Color.Lerp(material.color, Color.green , t * t);
        }
        materiali[maxDepth,0] = new Material(material);
        materiali[maxDepth,0].color = Color.white;
        materiali[maxDepth, 1] = new Material(material);
        materiali[maxDepth, 1].color = Color.black;
    }
    
	// Use this for initialization
	void Start () {

        if (materiali == null)
        {
            InstantializeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = mesh[Random.Range(0,mesh.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materiali[depth, Random.Range(0,2)];
        transform.Rotate(Random.Range(-rotazioneMassima, rotazioneMassima), 0, 0);


        if (depth < maxDepth)
        {
            StartCoroutine(CreaFiglio());
        }
    }

    IEnumerator CreaFiglio()
    {
        for (int i = 0; i < direzioni.Length; i++)
        {
            if (Random.value < probabilita)
            {
                yield return new WaitForSeconds(0.2f);
                new GameObject("Frattale figlio").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }

    void Initialize (Fractal genitore, int i)
    {
        mesh = genitore.mesh;
        materiali = genitore.materiali; 
        maxDepth = genitore.maxDepth;
        depth = genitore.depth + 1;
        scalaFiglio = genitore.scalaFiglio;
        transform.parent = genitore.transform;
        transform.localScale = Vector3.one * scalaFiglio;
        transform.localPosition = direzioni[i] * (0.5f + 0.5f * scalaFiglio);
        transform.localRotation = orientamenti[i];
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * 15, 0);
	}
}
