# OCR-Book-Tesi

Il programma funziona in questo modo.

- **File -> Inserire pagina sinistra, inserire pagina destra**
    
    Qui è il punto iniziale del programma, cioè si scelgono le due pagina in cui si è deciso di far inizare il controllo.
    Se questa funzione è inserita in un softwear l'azione è tradotta appunto come -> arrivato ad un certo punto di digitalizzazione del libro decido che le pagine ottentute dopo       il ritaglio sono le pagine da cui voglio iniziare il controllo.
    
- **Controllo Pagine -> Rileva Area, Esegui**

    A questo punto si seleziona un area nelle due pagine dove si trova il numero che identifica la pagina, e poi si preme sul pulsante Esegui che restituisce i primi due numeri       rilevati delle pagine.
    
    Il progrmma restituisce tramite messaggio visivo solo i primi due numeri del rilevamento perchè **l'intera funzione del controllo si basa sul presupposto che i primi due           numeri siano esatti.**
    
    Poi il programma inizia a valutare tutte le pagine successive e continua fino a che il libro non finisce o c'è un errore durante il controllo(esempio pagine senza numero,         oppure si è saltata una pagina ecc)
    
    Sempre a livello di funzione inserita nel softwear l'azione è tradotta appunto con il click sul pulsante Rileva Area e poi con il click sul pulsante Esegui che restituisce i       due numeri, poi il programma valuta volta per volta ad ogni scatto successivo della macchinetta.
    
    **LA FUNZIONE RESTITUISCE UN ERRORE SE ENTRAMBE LE PAGINE CONTROLLATE SONO ERRATE, SE UNA E' VERA E L'ALTRA FALSA LE DUE PAGINE SONO VERE, CIOE' SONO IN ORDINE NELLA               SEQUENZA**
    
- **Continua/Modifica pagina -> Verifica le nuove pagine, Riprendi controllo**

   Dopo che la funzione ha identificato un errore, si può procedere in diverse maniere:
   
    -Se effettivamente si è verificato un errore e si è saltata una pagina si clicca su "Verifica le nuove pagine" e si scelgono le due nuove pagine con la restituzione per           messaggio del numero delle due pagine, in cui però l area rilevata è quella scelta precedentemente. Sempre a livello di funzione inserita nel softwear si traduce come           colui che ha scattato la foto ha notato l'errore e cancella le due pagine per inserire quelle giuste e quindi azzera il contatore e ottiene come messaggio il numero    delle       due nuove pagine per valutare se sono corrette.
   
   Premendo poi Riprendi controllo continua il controllo del restante libro. A livello softwear si traduce appunto come il pulsante Esegui cioe con il controllo ad ogni scatto      della macchinetta di nuove foto
   
   -Se invece non c'è nessuno errore ma ad esempio ci sono due pagine senza numero(Nuovo capitolo) oppure il sistema tesseract ha rilevato male i due numeri e quindi la pagina      è in oridne si fa esattamente la stessa cosa di prima.
   
   A livello softwear bastera premere il pulsante Verifica le Nuove pagine e al primo scatto dopo l'errore restituira i due numeri sempre per verificare se sono corretti e poi si continua con la digitalizazzione del restante libro
   
   -Se invece è errore perchè magari l area selezionata non contiene piu i numeri, si usa la procedura, di inizio ovvero Inserisci le due pagine poi Rileva area ed Esegui

    
    

