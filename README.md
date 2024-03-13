# Unity - Firebase

Implementaremos tres carácteristicas de a un proyecto de Unity sincronizado con Firebase.

## Inicio de juego

En el inicio del juego, recoger las posiciones de varios elementos (los prefab) y posicionarlos en función de los datos de la base de datos.

```csharp
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
```

Tenemos nuestra variable `_app`que recoge la conexion con la base de datos Firebase, `_db` que recoge una unica instancia de la base de datos, `_refPrefabs` que guarda la referencia a la tabla Prefabs en la base de datos.

Luego guardamos una variable `GameObject` por cada elemento del juego.

```csharp
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
```

En el método `Start()` hacemos conexión con nuestra base de datos llamando al método `Conexion()`, una vez hecho eso guardamos una instancia de la base de datos y obtenemos una referencia a la tabla "Prefabs".

A continuación conseguimos todos los valores de la tabla "Prefabs" haciendo uso de su referencia mediante una "snapshot".

Si esta tarea se completa y conseguimos nuestra 'snapshot' llamaremos al método `setPosicionInicial()` dandole la 'snapshot' de la base de datos, el nombre del campo del que queremos recoger las coordenadas y el 'GameObject' al que queremos cambiar la posición.

#### setPosicionInicial()

```csharp
void setPosicionInicial(DataSnapshot snapshot, string px, GameObject prefab){
        // Lee la entrada de Firebase y guarda los datos en variables locales        
        float x = float.Parse(snapshot.Child(px).Child("x").Value.ToString());
        float y = float.Parse(snapshot.Child(px).Child("y").Value.ToString());
        float z = float.Parse(snapshot.Child(px).Child("z").Value.ToString());
```



Dados los datos dichos anteriormente este método primeramente guardará en un 'float' por cada uno los parámetros de posicion sacados de la base de datos.

Para ello busca en el 'snapshot' __el hijo del hijo__ de "Prefabs" `px` que es el nombre de cada uno de los objetos (ej: "p1") 

![](/Documentacion/vista-firebase.png)

#### setPosicionInicial()

```csharp
        // Crea un nuevo Vector3 con los datos obtenidos
        Vector3 posicionInicial = new Vector3(x, y, z);

        // Establece la posición inicial del GameObject en Unity
        prefab.transform.position = posicionInicial;
    }
```

A continuación hace una variable local `posicionInicial` tipo `Vector3` (vector de tres ejes) con las variables sacadas de la base de datos anteriormente (`x`|`y`|`z`).

Después transforma la posición del `GameObject` dado a la de `posicionInicial`.

![](/Documentacion/inicio.gif)

<details open>
<summary>Script</summary>
<br>
Nuestro script estará ubicado en un 'GameObject' vacío.

![Script](/Documentacion/script.png)
</details>
