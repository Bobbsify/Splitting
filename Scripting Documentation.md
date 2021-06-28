# Documentazione Software

In questo foglio verrà scritta la documentazione inerente ad ogni script.

In fondo a questo foglio si può trovare un template per descrivere gli script e i propri metodi

<hr>

## AudioController.cs

L'audio controller permette di designare un Collider2D settato con is trigger messo a **true** ed attaccato ad un **Rigidbody 2D di tipo Kinematic** come luogo dove vengono
controllate le emissioni delle audiosource correttamente taggate come "**Audio**" : tutte quelle esterne alla box vengono mutate e quelle interne vengono regolate in base alla distanza

**Importante** con questo AudioController è importante che di Default l'audio degli emitter sia impostato a Zero

### Variabili

    private AudioSource audioIn;
    private Collider2D objCollider;
    private float maxY;
    private float maxX;

- audioIn: _utilizzata per lavorare sull'audio che si trova all'interno dell'area_

- objCollider: _il collider attaccato, serve per rendere dinamica la grandezza del collider_

- maxY && maxX: _il massimo della coordinataX e la coordinataY basate sull'objCollider divisa per due di modo da portare al centro l'area dove si sente_

### Metodi

* OnTriggerStay2D(Collider2D collider): _Viene controllato se l'oggetto che collide possiede il tag "**Audio**", se è vero viene calcolata la direzione da cui deve venire l'audio e il suo volume tramite *ProcessVolume* e *DetectStereoPan*_

* OnTriggerExit2D(Collider2D collider): _Qualunque collider che esca dall'audiocontroller vedrà il suo volume a 0_

* DetectStereoPan(float incomingX): _basandosi sulla posizione sull'asse x del centro dell'audio controller esso calcola la direzione da cui deve provenire il suono e la ritorna_

* ProcessVolume(float incomingX, float incomingY): _basandosi sulla distanza dall'epicentro delle audiosource all'interno del collider regola il volume_

<hr>

## SwitchCharacter.cs

Permette di cambiare da un personaggio ad un altro target, solo un personaggio deve iniziare con le script accese, gli altri devono **iniziare con le script spente** altrimenti saranno accesi in contemporanea all'inizio della scena

### Variabili

    [SerializeField] private KeyCode swapButton = KeyCode.Q; //Defaults to Q
    [SerializeField] private GameObject targetEntity;

- swapButton : _Segnala il bottone per cambiare personaggio. di default è Q_

- targetEntity : _GameObject con la quale si deve effettuare lo switch_

### Metodi

* Update(): _L'update detecta quando viene premuto il pulsante segnalato in **swapButton** ed esegue **turnThisOff()** e **turnOtherOn()**_

* TurnThisOff(): _Cerca tutte le script di questo gameObject e le spegne, disabilita il tag "Player" da quest'oggetto per far muovere la telecamera (vedi: [CameraController](##CameraController.cs))_

* TurnOtherOn(): _Cerca tutte le script del gameObject target e le accende e ne cambia il tag in "Player"_

<hr>

## Move.cs

Move è il componente che consente ad un entità di muoversi utilizzando gli input orizzontali.
Interagisce con l'animatore aggiornando il valore del parametro "velocityX" con la velocità verso la quale sta viaggiando l'entità (**Valore assoluto**)

### Variabili

    [SerializeField] private float speed;
    private float horizontalInput;
    public bool canMove;

- speed : _Velocità alla quale si muove il nostro personaggio_

- horizontalInput : _detecta l'input orizzontale del giocatore, ha un valore tra -1 e 1 e serve per la direzione dove si sta viaggiando_

- canMove : _utilizzato da componenti esterni tra cui [StateController](##StateController.cs) per determinare se il giocatore può muoversi o meno_

### Metodi

* Update(): _ad ogni frame, se l'entità può muoversi farà dei calcoli per spostare nella direzione scelta l'entità, aggiornando l'animatore tramite **CallAnimator()**_

* CallAnimator(float speed): _data la velocità alla quale si sta muovendo l'entità, aggiorna il parametro **velocityX** nell'animator del gameObject_

<hr>

## LoadLevel.cs

Load Level è un componente che attaccato ad un gameObject consente il cambiamento di livello, prima di poter utilizzare correttamente questo componento è necessario andare nei build settings (File --> Build Settings) e trascinare tutte le scene di gioco all'interno dei build settings.

Opera in modo simile a [DoorActivator.cs](##DoorActivator.cs) a livello di zone detection

### Variabili

    [SerializeField] private InGameScenes sceneToLoad;
    [SerializeField] private bool transition;
    private List<string> gameScenes = new List<string>();
    
    private bool playerIsHere;
    
- sceneToLoad : _risultato di un enumerazione che segnala i livelli presenti nel gioco, quest'enumerazione è da aggiornare in base a ciò che c'è scritto nei build settings rispetto alle scene_

- transition : _segnala se il cambio di scena richiede una transizione ad un loadingscreen oppure no_

- gameScenes : _lista di scene presenti all'interno del progetto determinata dai BuildSettings_

- playerIsHere : _serve a determinare se si può fare l'interazione per viaggiare nella scena successiva_

### Metodi

* Start(): _Aggiunge alla lista gameScenes tutte le scene nei BuildSettings_

* Update(): _Serve a determinare quando ricaricare la scena_

* loadLevel(): _Lancia il metodo di SceneManager per caricare il livello selezionato_

* OnTriggerStay2D(Collider2D collision): _Setta a true playerIsHere se collide con un gameObject col tag "Player"_

* OnTriggerExit2D(Collider2D collision): _Setta a false playerIsHere se esce dalla hitbox un gameObject col tag "Player"_

<hr>

## [Nome Scripts].cs

Introduzione all'utilizzo + note per l'implementazione nell'engine

### Variabili

Lista Variabili

- Variabile 1 : _[utilizzo]_

- Variabile 2 : _[utilizzo]_

- Variabile N : _[utilizzo]_

### Metodi

* Metodo 1(Variabili Richieste): _[cosa fa]_

* Metodo N(Variabili Richieste): _[cosa fa]_

<hr>
