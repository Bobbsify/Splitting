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

- Audiosource: _utilizzata per lavorare sull'audio che si trova all'interno dell'area_

- objCollider: _il collider attaccato, serve per rendere dinamica la grandezza del collider_

- maxY && maxX: _il massimo della coordinataX e la coordinataY basate sull'objCollider divisa per due di modo da portare al centro l'area dove si sente_

### Metodi

* OnTriggerStay2D(Collider2D collider): _Viene controllato se l'oggetto che collide possiede il tag "**Audio**", se è vero viene calcolata la direzione da cui deve venire l'audio e il suo volume tramite *ProcessVolume* e *DetectStereoPan*_

* OnTriggerExit2D(Collider2D collider): _Qualunque collider che esca dall'audiocontroller vedrà il suo volume a 0_

* DetectStereoPan(float incomingX): _basandosi sulla posizione sull'asse x del centro dell'audio controller esso calcola la direzione da cui deve provenire il suono e la ritorna_

* ProcessVolume(float incomingX,float incomingY): _basandosi sulla distanza dall'epicentro delle audiosource all'interno del collider regola il volume_

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
