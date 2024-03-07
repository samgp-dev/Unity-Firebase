using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class Realtime : MonoBehaviour
{
    // Conexion con Firebase
    private FirebaseApp _app;
    // Singleton de la Base de Datos
    private FirebaseDatabase _db;

    // Referencia a la 'coleccion' Prefabs
    private DatabaseReference _refPrefabs;
    public GameObject P1;
    public GameObject P2;
    public GameObject P3;
    public GameObject P4;


    // Start is called before the first frame update
    void Start()
    {      
        // Realizamos la conexion a Firebase
        _app = Conexion();
        
        // Obtenemos el Singleton de la base de datos
        _db = FirebaseDatabase.DefaultInstance;

        // Definimos la referencia a la coleccion Prefabs
        _refPrefabs = _db.GetReference("Prefabs");

        // Recogemos los valores
        _refPrefabs.GetValueAsync().ContinueWithOnMainThread(task => {
            if(task.IsCompleted){

                DataSnapshot snapshot = task.Result;
                
                // Mostrar los datos
                //recorreSnapshot(snapshot);
                
                // Iguala los valores del GameObject a los valores de la base de datos
                setPosicionInicial(snapshot, "p1", P1);
                setPosicionInicial(snapshot, "p2", P2);
                setPosicionInicial(snapshot, "p3", P3);
                setPosicionInicial(snapshot, "p4", P4);
                
            } else {
                Debug.Log("Problema recogiendo valores.");
            }
        });
    
    }
    
    
    void setPosicionInicial(DataSnapshot snapshot, string px, GameObject prefab){
        // Lee la entrada de Firebase y guarda los datos en variables locales        
        float x = float.Parse(snapshot.Child(px).Child("x").Value.ToString());
        float y = float.Parse(snapshot.Child(px).Child("y").Value.ToString());
        float z = float.Parse(snapshot.Child(px).Child("z").Value.ToString());

        // Imprime el dato en la consola
        Debug.Log(px + ": X=" + x);
        Debug.Log(px + ": Y=" + y);
        Debug.Log(px + ". Z=" + z);

        // Crea un nuevo Vector3 con los datos obtenidos
        Vector3 posicionInicial = new Vector3(x, y, z);

        // Establece la posición inicial del GameObject en Unity
        prefab.transform.position = posicionInicial;

    }

    // Recorro un snapshot de un nivel
    void recorreSnapshot(DataSnapshot snapshot)
    {
        foreach(var resultado in snapshot.Children) // Prefabs
        {
            Debug.LogFormat("Key = {0}", resultado.Key);  // "Key = AAxx"
            foreach(var levels in resultado.Children)
            {
                Debug.LogFormat("(key){0}:(value){1}", levels.Key, levels.Value);
            }
        }
    }

    // Realizamos la conexion a Firebase
    // Devolvemos una instancia de esta aplicacion
    FirebaseApp Conexion()
    {
        FirebaseApp firebaseApp = null;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Conexión establecida.");
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                firebaseApp = null;
            }
        });
            
        return firebaseApp;
    }
    
}