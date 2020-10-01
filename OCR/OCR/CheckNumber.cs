using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCR
{
    class CheckNumber
    {
        //Numero sinistro e destro rilevato tramite OCR
        private int numSx;
        private int numDx;

        //Numero sinistro e destro salvati in memoria
        private int storageNumSx;
        private int storageNumDx;

        //Boolean per verificare se il numero sinisto e destro siano sequenzialmente gusti, true rispettano l'ordine false no.
        private Boolean sxIsRight = false;
        private Boolean dxIsRight = false;

        //Numero sinistro e destro in cui viene a presentarsi un errore
        private int numSxErr;
        private int numDxErr;

        //Numero corretto dopo essere stato controllato
        private int numBaseCorrect;

        private Boolean pageSxIsEmpty = false;
        private Boolean pageDxIsEmpty = false;


        //Inserisco i primi due numeri rilevati delle prime due pagine iniziali. I NUMERI INZIALI DEVONO ESSERE ESATTI
        public CheckNumber(int numSx, int numDx)
        {
            this.numSx = numSx;
            this.numDx = numDx;          
        }

        //Salvo i due numeri in memoria
        public void Storage()
        {
            storageNumSx = numSx;
            storageNumDx = numDx;
        }

        //Inseriscio i due numeri rilevati tramite Tesseract
        public void SetNextNum(int numSx, int numDx)
        {
            this.numSx = numSx;
            this.numDx = numDx;
        }

        //Controllo e aggiorno i numeri per verificare se seguono l'ordine sequenziale, prende come parametri anche le stringhe 
        //rilevate tramite Tesseract per poi fare un controllo più approfondito in caso di un numero non corretto
        public void Update(string stringSx, string stringDx)
        {
            //Se il numero sinisto è uguale al numero in meoria +2, viene salvato in memoria, e ottiene true
            if (numSx == storageNumSx + 2)
            {
                storageNumSx = numSx;
                sxIsRight = true;  
            }
            else
            {
                if (numSx == 0)
                {
                    pageSxIsEmpty = true;
                }
                //Ottiene false
                sxIsRight = false;
                //Faccio un ulteriore controllo per verificare se magari è comunque presente nella stringa il numero corretto ma è 
                //stato rilevato in modo errato
                int correctNumber = GetCorrectNumber(stringSx, storageNumSx);
                //Se il numero trovato è veramnte il numero esatto lo salvo e cambio la stringa Boolean in true
                if (correctNumber == storageNumSx + 2)
                {
                    storageNumSx = correctNumber;
                    sxIsRight = true;
                }
            }
            //Se il numero destro è uguale al numero in meoria +2, viene salvato in memoria, e ottiene true
            if (numDx == storageNumDx + 2)
            {
                storageNumDx = numDx;
                dxIsRight = true;
            }
            else
            {
                if(numDx == 0)
                {
                    pageDxIsEmpty = true;
                }
                //Ottiene false
                dxIsRight = false;
                //Faccio un ulteriore controllo per verificare se magari è comunque presente nella stringa il numero corretto ma è 
                //stato rilevato in modo errato
                int correctNumber = GetCorrectNumber(stringDx, storageNumDx);
                //Se il numero trovato è veramnte il numero esatto lo salvo e cambio la stringa Boolean in true
                if (correctNumber == storageNumDx + 2)
                {
                    storageNumDx = correctNumber;
                    dxIsRight = true;
                }  
            }
            //Se entrambe le pagine danno esito false, la pagina non è in sequenza e faccio un riferimento alle due pagine sbagliate
            if (sxIsRight== false && dxIsRight == false)
            {
                numSxErr = storageNumSx;
                numDxErr = storageNumDx;
            }
            //Se la pagina sx è sbalgiata mentre la dx è giusta, c'è stato un errore di rilevamento tramite ocr nella pagina sinistra
            //, quindi la pagina è in sequenza perchè essando due le pagine processate, se una è corretta anche l'altra lo è
            if(sxIsRight == false && dxIsRight == true)
            {
                numSx = storageNumSx + 2;
                storageNumSx = numSx;
            }
            //Se la pagina dx è sbalgiata mentre la sx è giusta, c'è stato un errore di rilevamento tramite ocr nella pagina destra
            //, quindi la pagina è in sequenza perchè essando due le pagine processate, se una è corretta anche l'altra lo è
            if (sxIsRight == true && dxIsRight == false)
            {
                numDx = storageNumDx + 2;
                storageNumDx = numDx;
            }
        }

        //Verifico lo stato della sequenza delle pagine
        //Per restituire false entrambe le pagine devono essere false
        public Boolean IsRight()
        {
            if (sxIsRight == true || dxIsRight == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean isEmpty()
        {
            if (pageSxIsEmpty == true && pageDxIsEmpty == true)
            {
                return true;
            }
            else
                return false;
        }

        //Ottengo la pagina sinistra errata
        public int GetErrorSx()
        {
            return numSxErr;
        }

        //Ottengo la pagina destra errata
        public int GetErrorDx()
        {
            return numDxErr;
        }

        //Verifico che nella stringa sia presente il numero che ad un primo rilevamento è risulato essere sbagliato.
        //Si utilizza questo metodo perchè è stato visto dopo molte prove che alcune pagine possono contenere un carattere che viene
        //erroneamente scambiato per un numero e il numero vero si trova dopo lo spazio.
        //Oppure il numero cercato ha piu di una cifra e può essere rilevato con uno spazio in mezzo scambiandoli per piu numeri.
        public int GetCorrectNumber(string checkString,int baseNumber)
        {
            Boolean emptySpace = false;
            List<string> singleChar = new List<string>();
            int i= 0;
            int s= 0 ;
            string rif= "";
            int d = 0;
            int h = 0;
            int checkCorrectNumber = 0;
            string correctNumber = "";
            string correctNumber2 = "";

            
            if (checkString.Length > 0)
            {
                //Verifico se nella stringa è presente almeno un numero senno restituisco 0(numero non trovato)
                Match match = Regex.Match(checkString, @"\d+");
                if (match.Success)
                {

                }
                else
                {
                    checkCorrectNumber = 0;
                    return checkCorrectNumber;
                }
                while (int.TryParse(checkString.Substring(i, 1), out s) == false)
                {
                    i++;
                    //if(checkString.Contains(@"\d+"))
                }
                if (checkString.Substring(i + 1, 1) == " ")
                {
                    emptySpace = true;
                }
                if (int.TryParse(checkString.Substring(i, 1), out s) == true && emptySpace && int.TryParse(checkString.Substring(i + 2, 1), out s) == true)
                {
                    int j = i + 2;

                    while (int.TryParse(checkString.Substring(j, 1), out d) == true)
                    {
                        rif = rif.Insert(h, checkString.Substring(j, 1));
                        j++;
                        h++;
                    }
                    correctNumber2 = correctNumber2.Insert(0, rif);
                }

                correctNumber = correctNumber.Insert(0, checkString.Substring(i, 1));
                correctNumber = correctNumber.Insert(1, rif);

                //Se correctNumber è una stringa in cui il numero esatto viene sepatato da uno sapzio viene inserito il numero esatto
                //in nameBaseCorrect
                if (correctNumber.Length > 0)
                {
                    if (int.Parse(correctNumber) == baseNumber + 2)
                    {
                        checkCorrectNumber = int.Parse(correctNumber);
                        numBaseCorrect = checkCorrectNumber;

                    }
                }
                //Se correctNumber2 è una stringa con il primo carattere scambiato come numero, e la restante parte è il numero esatto
                //viene inserito il numero esatto in nameBaseCorrect
                if (correctNumber2.Length > 0)
                {
                    if (int.Parse(correctNumber2) == baseNumber + 2)
                    {
                        checkCorrectNumber = int.Parse(correctNumber2);
                        numBaseCorrect = checkCorrectNumber;
                    }
                }
            }
            return checkCorrectNumber;
        }

        //Ottengo il numero della pagina sinistra in memoria
        public int GetNumberStorageSx()
        {
            return storageNumSx;
        }

        //Ottengo il numero della pagina destra in memoria
        public int GetNumberStorageDx()
        {
            return storageNumDx;
        }
        
        //Ottengo il numero corretto dopo la verifica tramite GetCorrectNumber
        public int GetNumberAfterGetCorrectNumber()
        {
            return numBaseCorrect;
        }
    }
}
