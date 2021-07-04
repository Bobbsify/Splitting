# Documentazione Software

In questo foglio verrà scritta la documentazione inerente ad ogni script.

In fondo a questo foglio si può trovare un template per descrivere gli script e i propri metodi


###Indice

[AudioController.cs](##AudioController.cs)

[SwitchCharacter.cs](##SwitchCharacter.cs)

[Move.cs](##Move.cs)

[LoadLevel.cs](##LodaLevel.cs)

[AwakeBehaviour.cs](##AwakeBehaviour.cs)

[Jump.cs](##Jump.cs)

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

        private Animator animator;

        [SerializeField] private float speed;
        private float horizontalInput;
        private float verticalInput;

        public bool canCrouch;
        [HideInInspector] public bool isCrouched;
        [HideInInspector] public bool canMove;
        [HideInInspector] public bool isObstructed;

- animator : _Animator dell'oggetto a cui è attaccato lo script_

- speed : _Velocità alla quale si muove il nostro personaggio_

- horizontalInput : _detecta l'input orizzontale del giocatore, ha un valore tra -1 e 1 e serve per la direzione dove si sta viaggiando_

- vericalInput : _ottiene l'input verticale del giocatore, ha un valore tra -1 e 1 e viene usato per determinare se l'entità si sta accucciando o alzando

- canCrouch : _utilizzato da componenti esterni tra cui [StateController](##StateController.cs) per determinare se l'entità può accovacciarsi o meno_

- isCrouched : _Determina se l'entità è accovacciata_

- canMove : _utilizzato da componenti esterni tra cui [StateController](##StateController.cs) per determinare se l'entità può muoversi o meno_

- isObstructed : _utilizzato da componenti esterni tra cui [StateController](##StateController.cs) per determinare se l'entità è ostruita sopra di se_


### Metodi

* Start(): _ottiene l'animatore dell'entità in questione_

* Update(): _ad ogni frame, se l'entità può muoversi farà dei calcoli per spostare, accovacciare o alzare l'entità, aggiornando l'animatore tramite **CallAnimator()**_

* CallAnimator(float speed): _aggiorna secondo quello che gli è fornito da Update() i parametri_ **velocityX** _e_ **isCrouched** _nell'animator del gameObject_

<hr>
## LoadLevel.cs

Load Level è un componente che attaccato ad un gameObject consente il cambiamento di livello, prima di poter utilizzare correttamente questo componento è necessario andare nei build settings (File --> Build Settings) e trascinare tutte le scene di gioco all'interno dei build settings.

Se viene acceso automaticamente avvia il livello segnalato, da far quindi partire spento e poi accenderlo tramite un [Awake Behaviour](##AwakeBehaviour.cs)

### Variabili

    [SerializeField] private InGameScenes sceneToLoad;
    [SerializeField] private bool transition;
    private List<string> gameScenes = new List<string>();
    
- sceneToLoad : _risultato di un enumerazione che segnala i livelli presenti nel gioco, quest'enumerazione è da aggiornare in base a ciò che c'è scritto nei build settings rispetto alle scene_

- transition : _segnala se il cambio di scena richiede una transizione ad un loadingscreen oppure no_

- gameScenes : _lista di scene presenti all'interno del progetto determinata dai BuildSettings_

### Metodi

* Awake(): _Prepara il componente al caricamento della scena selezionata_

* Start(): _Lancia il metodo LoadLevel() il primo frame che il componente è acceso_

* loadLevel(): _Lancia il metodo di SceneManager per caricare il livello selezionato_

<hr>

## AwakeBehaviour.cs

AwakeBehaviour accende o spegne le script presenti all'interno di un determinato gameObject in base ad una condizione specificata nelle impostazioni

### Variabili

    [SerializeField] private Turn actionType;
    [SerializeField] private ActivationTypes activationType;
    [SerializeField] private KeyCode buttonToPress; //Not compulsory
    [SerializeField] private MonoBehaviour[] scriptsToLoad;

    private bool isPlayerHere;
    private Collider2D objCollider;
    
- actionType : _Se le script sono da accendere, spegnere o toggleare_

- activationType : _Segnala il tipo di attivazione effettuato, onEnter attiverà l'evento non appena il player entrerà nell'area del collider, onClick si attiverà con un click del mouse in qualsiasi momento, enterAndClick invece obbliga il giocatore ad effettuare un click solo nell'area designata_

- buttonToPress : _Tasto da utilizzare per interagire_

- scriptsToLoad : _Array di script da lanciare quando la condizione è soddisfatta_

- isPlayerHere : _Booleana che serve a controllare se il giocatore si trova nell'area del collider_

- objCollider : _Collider recuperato automaticamente dell'oggetto a cui è attaccata la script_

### Metodi

* Start(): _Ottiene il collider dell'oggetto quando richiesto dall'activationType_

* Update(): _Effettua l'azione di awakeScripts() se la condizione dell'activationType è soddisfatta_

* OnTriggerStay2D(Collider2D col): _Aggiorna a true playerIsHere se c'è un GameObject con tag "Player" nel collider_

* OnTriggerExit2D(Collider2D col): _Aggiorna a false playerIsHere se un GameObject con tag "Player" nel collider ne esce_

* awakeScripts(): _Imposta tutte le script a true o folse basandosi sull'actionType_

<hr>

## Jump.cs

Lo script Jump permette di applicare una forza verticale al Rigidbody2D dell'entità in cui è assegnato tramite la lettura degli input provenienti dalla Barra Spaziatrice.
La forza applicata varia a seconda di quanto tempo è rimasta premuta la barra spaziatrice.
Al suo interno viene inoltre controllato se l'entità a cui è applicata la forza sta saltando o cadendo in modo da poter modificare l'animazione.

### Variabili

        public bool canJump;

        public bool isJumping;
        public bool isFalling;

        [SerializeField] private float jumpForce = 1000.0f;
        public bool jumpKeyDown;

        [SerializeField] private float timerJump = 2.0f;
        [SerializeField] private float elapsed;

        private float velocityY;

        new private Rigidbody2D rigidbody2D;
        
- canJump : _booleana che controlla se l'entità può saltare oppure no_

- isJumping : _booleana che indica se l'entità sta saltando in quel momento oppure no_

- isFalling : _booleana che indica se l'entità sta cadendo in quel momento oppure no_
 
- jumpForce : _forza verticale che verrà applicata all'entità_

- jumpKeyDown : _booleana che controlla quando la barra spaziatrice è premuta oppure no_

- timerJump : _valore che indica il tempo necessario a compiere il salto caricato_

- elapsed : _valore che si aggiorna in base al tempo passato con barra spaziatrice premuta_

- velocityY : _valore che indica la velocità dell'entità sull'asse Y_

- rigidbody2D : _variabile che stora al suo interno l'ID del rigidbody2D dell'entità_

### Metodi

* Start(): _la variabile rigidbody2D prende il componente Rigidbody2D dell'entità_

* Update(): _la variabile jumpKeyDown detecta se viene premuta o no la barra spaziatrice. La variabile elapsed aumenta fin tanto che jumpKeyDown restituisce true. Quando viene rilasciata la barra spaziatrice jumpKeyDown restituirà quindi false. Se questo avviene mentre canJump è true viene applicata una forza sul rigidbody2D. La forza applicata è normalmente uguale a jumpForce, ma se elapsed supera timerJump la forza applicata sarà jumpForce*2 e si otterrà quindi un salto caricato. Inoltre se velocityY è positiva isJumping sarà uguale a true e isFalling false, altrimenti con una velocityY negativa avremo che isJumping è false e isFalling è true.        

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
