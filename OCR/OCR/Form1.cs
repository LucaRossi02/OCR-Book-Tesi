using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Le immagini originali.
        private Bitmap OriginalImageSx;
        private Bitmap OriginalImageDx;

        //Vero quando stiamo selezionando il rettangolo
        private bool IsSelectingSx = false;
        private bool IsSelectingDx = false;

        // L'area che stiamo selezionando
        private int X0Sx, Y0Sx, X1Sx, Y1Sx;
        private int X0Dx, Y0Dx, X1Dx, Y1Dx;

        //Coordinate delle aree selezionate
        private Rectangle coordAreaSx;
        private Rectangle coordAreaDx;

        //Immagini delle prime due aree selezionate
        private Bitmap areaBaseSx;
        private Bitmap areaBaseDx;

        //Le impostazioni di Tesseract
        TesseractEngine ocrengine;

        //I file name delle due pagine
        private String fileNamePageSx;
        private String fileNamePageDx;

        //Turue se funziona il puntatore del mouse sulle immagini, false se non funziona
        Boolean canSelectedArea = false;

        //Il numero della pagina sinistro e destra presente nel file name di ogni immagine sinistra e destra
        private int nNextSx = 0;
        private int nNextDx = 0;

        //Numero che sta ad indicare l'ultima pagina del libro scelto
        private int lastNumber;

        //Oggetti della classe FileNameBook sia per l'immagine sinsitra che destra
        private FileNameBook pageNextSxFN = new FileNameBook();
        private FileNameBook pageNextDxFN = new FileNameBook();

        //File name di appoggio della pagina sx e dx
        private string insertFileNameSx;
        private string insertFileNameDx;

        //Numeri rilevati da Tesseract della pagina sx e dx
        private int numSxPageSx;
        private int numDxPageDx;

        //Oggetti della classe CheckNumber per il controllo e la verifica dell'ordine delle pagine
        private CheckNumber first;
        private CheckNumber firstAfterCont;

        //Stringa sx e dx rilevata da Tesseract in riferimento all'area scelta in input
        private string stringSxDetected;
        private string stringDxDetected;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //Il caricamento dei file per tesseract viene fatto una volta solo al momento dell'apertura del programma
            ocrengine = new TesseractEngine(@".\tessdata", "ita", EngineMode.Default);
            ocrengine.DefaultPageSegMode = PageSegMode.SingleBlock;
            ocrengine.SetVariable("tessedit_char_whitelist", "0123456789-_abcdfghmnpqrtuvzxyk");
        }

        private void inserirePaginaSinistraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Scelgo la pagina sinistra e la adatto alle dimensioni della pictureBox
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Verifico che venga inserita una pagina sinistra
                    string fileNamePageSxCorrect = openFileDialog1.FileName;
                    int indexOf_Sx = fileNamePageSxCorrect.LastIndexOf("_");
                    int n = int.Parse(fileNamePageSxCorrect.Substring(indexOf_Sx + 1, 4));
                    if (n % 2 == 0)
                        throw new System.ArgumentException("Inserire immagine di una pagina sinistra");
                    else
                    {
                        Size size = new Size(picOriginalSx.Width, picOriginalSx.Height);
                        Bitmap newPhoto = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                        Bitmap newPhotoSize = new Bitmap(newPhoto, size);
                        picOriginalSx.Image = newPhotoSize;

                        OriginalImageSx = (Bitmap)picOriginalSx.Image;
                        canSelectedArea = false;

                        fileNamePageSx = openFileDialog1.FileName;
                    }
                }
            }
            catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }
        
        private void inserirePaginaDestraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Scelgo la pagina destra e la adatto alle dimensioni della pictureBox
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Verifico che venga inserita una pagina destra
                    string fileNamePageDxCorrect = openFileDialog1.FileName;
                    int indexOf_Dx = fileNamePageDxCorrect.LastIndexOf("_");
                    int n = int.Parse(fileNamePageDxCorrect.Substring(indexOf_Dx + 1, 4));
                    if (n % 2 != 0)
                        throw new System.ArgumentException("Inserire immagine di una pagina destra");
                    else
                    {
                        Size size = new Size(picOriginalDx.Width, picOriginalDx.Height);
                        Bitmap newPhoto = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                        Bitmap newPhotoSize = new Bitmap(newPhoto, size);
                        picOriginalDx.Image = newPhotoSize;

                        OriginalImageDx = (Bitmap)picOriginalDx.Image;
                        canSelectedArea = false;

                        fileNamePageDx = openFileDialog1.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }

        // Inizia la selezione del rettangolo.Si verifica quando il puntatore del mouse si trova sull'immagine
        //mentre viene premuto un pulsante del mouse.
        private void picOriginalSx_MouseDown(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                IsSelectingSx = true;
                // Salvo i punti iniziali
                X0Sx = e.X;
                Y0Sx = e.Y;
            }
        }

        // Continua la selezione. Si verifica quando il puntatore del mouse viene spostato sull'immagine.
        private void picOriginalSx_MouseMove(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                //Non fare niente se non è selezionata un area
                if (!IsSelectingSx) return;

                //Salvo i nuovi punti.
                X1Sx = e.X;
                Y1Sx = e.Y;

                Bitmap bm = new Bitmap(OriginalImageSx);

                //Disegno il rettangolo.
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.DrawRectangle(Pens.Red,
                        Math.Min(X0Sx, X1Sx), Math.Min(Y0Sx, Y1Sx),
                        Math.Abs(X0Sx - X1Sx), Math.Abs(Y0Sx - Y1Sx));
                }

                picOriginalSx.Image = bm;
            }
        }

        //Fine selezione area. Si verifica quando il puntatore del mouse si trova sull'immagine
        //mentre viene rilasciato un pulsante del mouse.
        private void picOriginalSx_MouseUp(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                //Non fare niente se non è selezionata un area
                if (!IsSelectingSx) return;
                IsSelectingSx = false;

                picOriginalSx.Image = OriginalImageSx;

                int wid = Math.Abs(X0Sx - X1Sx);
                int hgt = Math.Abs(Y0Sx - Y1Sx);
                if ((wid < 1) || (hgt < 1)) return;

                Bitmap area = new Bitmap(wid, hgt);
                using (Graphics gr = Graphics.FromImage(area))
                {
                    Rectangle source_rectangle =
                        new Rectangle(Math.Min(X0Sx, X1Sx), Math.Min(Y0Sx, Y1Sx),
                            wid, hgt);
                    Rectangle dest_rectangle =
                        new Rectangle(0, 0, wid, hgt);
                    gr.DrawImage(OriginalImageSx, dest_rectangle,
                        source_rectangle, GraphicsUnit.Pixel);
                }
                //Coordinate dell' area selezionata per la pagina sinistra
                coordAreaSx = new Rectangle(Math.Min(X0Sx, X1Sx), Math.Min(Y0Sx, Y1Sx), wid, hgt);

                //picOriginalSx.Image = area;
                areaBaseSx = area;
            }
        }

        // Inizia la selezione del rettangolo.Si verifica quando il puntatore del mouse si trova sull'immagine
        //mentre viene premuto un pulsante del mouse.
        private void picOriginalDx_MouseDown(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                IsSelectingDx = true;
                X0Dx = e.X;
                Y0Dx = e.Y;
            }
        }

        // Continua la selezione. Si verifica quando il puntatore del mouse viene spostato sull'immagine.
        private void picOriginalDx_MouseMove(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                if (!IsSelectingDx) return;
                X1Dx = e.X;
                Y1Dx = e.Y;

                Bitmap bm = new Bitmap(OriginalImageDx);

                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.DrawRectangle(Pens.Red,
                        Math.Min(X0Dx, X1Dx), Math.Min(Y0Dx, Y1Dx),
                        Math.Abs(X0Dx - X1Dx), Math.Abs(Y0Dx - Y1Dx));
                }
                picOriginalDx.Image = bm;
            }
        }

        //Fine selezione area. Si verifica quando il puntatore del mouse si trova sull'immagine
        //mentre viene rilasciato un pulsante del mouse.
        private void picOriginalDx_MouseUp(object sender, MouseEventArgs e)
        {
            if (canSelectedArea == true)
            {
                if (!IsSelectingDx) return;
                IsSelectingDx = false;

                picOriginalDx.Image = OriginalImageDx;

                int wid = Math.Abs(X0Dx - X1Dx);
                int hgt = Math.Abs(Y0Dx - Y1Dx);
                if ((wid < 1) || (hgt < 1)) return;

                Bitmap area = new Bitmap(wid, hgt);
                using (Graphics gr = Graphics.FromImage(area))
                {
                    Rectangle source_rectangle =
                        new Rectangle(Math.Min(X0Dx, X1Dx), Math.Min(Y0Dx, Y1Dx),
                            wid, hgt);
                    Rectangle dest_rectangle =
                        new Rectangle(0, 0, wid, hgt);
                    gr.DrawImage(OriginalImageDx, dest_rectangle,
                        source_rectangle, GraphicsUnit.Pixel);
                }
                //Coordinate dell' area selezionata per la pagina destra
                coordAreaDx = new Rectangle(Math.Min(X0Dx, X1Dx), Math.Min(Y0Dx, Y1Dx), wid, hgt);
                //picOriginalDx.Image = area;
                areaBaseDx = area;
            }
        }

        //Rende attivo la selezione dei rettangoli tramite il puntatore del mouse
        private void rilevaAreaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canSelectedArea = true;
        }

        //Inizia il controllo del libro dopo aver inserito le due pagine iniziali
        private void eseguiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                canSelectedArea = false;

                Thread pageSx = new Thread(() =>
                {
                    BoxOCR firstPageSx = new BoxOCR();
                //Ottengo l'immagine elaborata del riquadro
                Bitmap boxSx = firstPageSx.AdaptBox(areaBaseSx);
                //picOriginalSx.Image = boxSx;
                picOriginalSx.Image = OriginalImageSx;
                //Blocco il thread finche il thread che ha un riferimentoa ocrengine non termina l'esecuzione del codice
                lock (ocrengine)
                    {
                    //Ottengo la stringa del riquadro selezionato
                    stringSxDetected = firstPageSx.GetStringTesseract(boxSx, ocrengine);
                    }
                    Match match = Regex.Match(stringSxDetected, @"\d+");
                    if (match.Success)
                    {
                    //Ottengo il primo numero della stringa
                    numSxPageSx = int.Parse(match.Value);
                    }
                    MessageBox.Show(numSxPageSx.ToString());
                //MessageBox.Show(stringSxDetected + " prova");
            });

                Thread pageDx = new Thread(() =>
                {
                    BoxOCR firstPageDx = new BoxOCR();
                //Ottengo l'immagine elaborata del riquadro
                Bitmap boxDx = firstPageDx.AdaptBox(areaBaseDx);
                //picOriginalDx.Image = boxDx;
                picOriginalDx.Image = OriginalImageDx;
                //Blocco il thread finche il thread che ha un riferimentoa ocrengine non termina l'esecuzione del codice
                lock (ocrengine)
                    {
                    //Ottengo la stringa del riquadro selezionato
                    stringDxDetected = firstPageDx.GetStringTesseract(boxDx, ocrengine);
                    }
                    Match match = Regex.Match(stringDxDetected, @"\d+");
                    if (match.Success)
                    {
                    //Ottengo il primo numero della stringa
                    numDxPageDx = int.Parse(match.Value);
                    }
                    MessageBox.Show(numDxPageDx.ToString());
                //MessageBox.Show(stringDxDetected + " prova");
            });

                pageSx.Start();
                pageDx.Start();

                while (pageSx.IsAlive == true && pageDx.IsAlive == true)
                {
                }
                //Una volta finito l'esecuzione dei thread li elimino
                pageSx.Abort();
                pageDx.Abort();

                int firstSx = numSxPageSx;
                int firstDx = numDxPageDx;

                //MessageBox.Show(firstSx.ToString() + "  " + firstDx.ToString());
                //Inserisco i due nuemri nell'oggetto CheckNumber per inziare il controllo del libro, I PRIMI DUE NUMERI DEVONO ESSERE 
                //ESATTI
                first = new CheckNumber(firstSx, firstDx);
                //Salvo i due numeri in memoria
                first.Storage();

                //Inserisco i due nomi delle immagini nelle rispettive stringhe di appoggio
                insertFileNameSx = fileNamePageSx;
                insertFileNameDx = fileNamePageDx;

                FileNameBook fileName = new FileNameBook();
                //Ottengo il nome dell'ultima file del libro
                string lastFileName = fileName.LastFile(fileNamePageSx);
                //Ottengo il numero dell'ultimo file del libro
                lastNumber = fileName.NumberOfLastFile(lastFileName);

                //Inzio il controllo delle pagine
                StartControl();
            }
            catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }

        //Questo pulsante va attivato dopo che si è verificato un messaggio di errore nella sequenza dopo la prima esecuzione, 
        //sta a simulare la modifica delle pagine se effettivamente c'è stato un errore duarnte lo scatto della foto oppure se 
        //semplicemente ci sono state due pagine vuote(inzio di un nuovo capitolo), oppure un mal rilevametno e quindi si procede con 
        //il controllo del libro.
        //Si verifica se la numerazione delle due nuove pagine sia correttamente rilevata da Tesseract
        private void verificaLeNuovePagineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap newPhotoSx = (Bitmap)picOriginalSx.Image;
                Bitmap newPhotoDx = (Bitmap)picOriginalDx.Image;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Verifico che venga inserita una pagina sinistra
                    string fileNamePageSxCorrect = openFileDialog1.FileName;
                    int indexOf_Sx = fileNamePageSxCorrect.LastIndexOf("_");
                    int n = int.Parse(fileNamePageSxCorrect.Substring(indexOf_Sx + 1, 4));
                    if (n % 2 == 0)
                        throw new System.ArgumentException("Inserire immagine di una pagina sinistra");
                    else
                    {
                        Size size = new Size(picOriginalSx.Width, picOriginalSx.Height);
                        Bitmap newPhoto = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                        newPhotoSx = new Bitmap(newPhoto, size);
                        picOriginalSx.Image = newPhotoSx;

                        OriginalImageSx = (Bitmap)picOriginalSx.Image;
                        canSelectedArea = false;

                        fileNamePageSx = openFileDialog1.FileName;
                    }
                }
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Verifico che venga inserita una pagina destra
                    string fileNamePageDxCorrect = openFileDialog1.FileName;
                    int indexOf_Dx = fileNamePageDxCorrect.LastIndexOf("_");
                    int n = int.Parse(fileNamePageDxCorrect.Substring(indexOf_Dx + 1, 4));
                    if (n % 2 != 0)
                        throw new System.ArgumentException("Inserire immagine di una pagina destra");
                    else
                    {
                        Size size = new Size(picOriginalDx.Width, picOriginalDx.Height);
                        Bitmap newPhoto = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                        newPhotoDx = new Bitmap(newPhoto, size);
                        picOriginalDx.Image = newPhotoDx;

                        OriginalImageDx = (Bitmap)picOriginalDx.Image;
                        canSelectedArea = false;

                        fileNamePageDx = openFileDialog1.FileName;
                    }
                }
                //Ottengo l'immagine sinistra dell'area selezionata all'inzio 
                Crop cutSx = new Crop(coordAreaSx);
                Bitmap pagNSx = cutSx.Apply(OriginalImageSx/*newPhotoSx*/);
                //Ottengo l'immagine destra dell'area selezionata all'inzio 
                Crop cutDx = new Crop(coordAreaDx);
                Bitmap pagNDx = cutDx.Apply(OriginalImageDx/*newPhotoDx*/);

                canSelectedArea = false;

                string firstNumSxStringCont = "";
                string firstNumDxStringCont = "";

                //Eseguo thread page sx
                Thread pageSxCont = new Thread(() =>
                {
                    BoxOCR firstC = new BoxOCR();
                    Bitmap boxC = firstC.AdaptBox(pagNSx);
                //picOriginalSx.Image = boxC;
                lock (ocrengine)
                    {
                        firstNumSxStringCont = firstC.GetStringTesseract(boxC, ocrengine);
                    }
                    Match match = Regex.Match(firstNumSxStringCont, @"\d+");
                    if (match.Success)
                    {
                    //Numero rilevato pagina sinistra
                    numSxPageSx = int.Parse(match.Value);
                    }
                    MessageBox.Show(numSxPageSx.ToString());
                //MessageBox.Show(firstNumSxStringCont + " prova");
            });
                //Eseguo thread page dx
                Thread pageDxCont = new Thread(() =>
                {
                    BoxOCR DxFirstPage = new BoxOCR();
                    Bitmap boxDx;
                    boxDx = DxFirstPage.AdaptBox(pagNDx);
                //picOriginalDx.Image = boxDx;
                lock (ocrengine)
                    {
                        firstNumDxStringCont = DxFirstPage.GetStringTesseract(boxDx, ocrengine);
                    }
                    Match match = Regex.Match(firstNumDxStringCont, @"\d+");
                    if (match.Success)
                    {
                    //Numero rilevato pagina sinistra
                    numDxPageDx = int.Parse(match.Value);
                    }
                    MessageBox.Show(numDxPageDx.ToString());
                //MessageBox.Show(firstNumDxStringCont + " prova");
            });
                pageSxCont.Start();
                pageDxCont.Start();

                while (pageSxCont.IsAlive == true && pageDxCont.IsAlive == true)
                {
                }
                pageSxCont.Abort();
                pageDxCont.Abort();
            }
            catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }

        //Ricomincio il controllo del libro dal nuovo punto di partenza
        private void riprendiControlloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Riferimento aii nuovi numeri rilevati della pagina sx e dx
                int firstSx = numSxPageSx;
                int firstDx = numDxPageDx;

                // qui MessageBox.Show(firstSx.ToString() + "  " + firstDx.ToString());
                //Li salvo in memoria
                firstAfterCont = new CheckNumber(firstSx, firstDx);
                firstAfterCont.Storage();

                //Riferimento ai due File name delle pagine
                insertFileNameSx = fileNamePageSx;
                insertFileNameDx = fileNamePageDx;

                //Inizo il controllo dopo che il primo controllo è stato fatto ed è stato trovato un errore
                StartControlAfterFirstCon();
            }
            catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }
       
        private async Task StartControl()
        {
            try
            {
                Bitmap nextImageSize = (Bitmap)picOriginalSx.Image;
                //Continua finche la pagina sinsitra e destra non arriva alla fine del libro
                while (nNextSx + 2 <= lastNumber && nNextDx + 2 <= lastNumber)
                {
                    //Creo i due task uno per la pagina sinistra, uno per la destra e li eseguo
                    var sxTask = Task.Run(() => FirstThreadPageSxAsync());
                    var dxTask = Task.Run(() => FirstThreadPageDxAsync());

                    //Attendo il completamento dei due task paralleli
                    var numSxTesseract = await sxTask;
                    var numDxTesseract = await dxTask;

                    //Inserisco i due numeri torvati della pagina sx e dx dopo il rilevamento Tesseract e li controllo in sequenza
                    first.SetNextNum(numSxTesseract, numDxTesseract);
                    //Verifico e aggiorno la sequenza
                    first.Update(stringSxDetected, stringDxDetected);

                    //MessageBox.Show(first.GetNumberAfterGetCorrectNumber().ToString());

                    //Quando isRight da false perchè si è verificato un errore durante il controllo
                    //(es. si è saltata una pagina durante la digitalizzazione oppure ci sono due pagine senza numero)
                    if (first.IsRight() == false)
                    {
                        //Azzero i contatoti delle pagine
                        nNextSx = 0;
                        nNextDx = 0;
                        Boolean rightIsFalse = first.IsRight();
                        Boolean empty = first.isEmpty();
                        //Se c'è un errore mostro i file name delle due pagine sbagliate
                        if (rightIsFalse == false)
                        {
                            if (empty == true)
                            {
                                MessageBox.Show("NON E' STATO RILEVATO ALCUN NUMERO NELLE PAGINE " + insertFileNameSx + " E " + insertFileNameDx + "");
                            }
                            else
                            {
                                MessageBox.Show("C'è un errore nel controllo della numerazione nelle pagine " + insertFileNameSx + "  e  " + insertFileNameDx.ToString() + "");
                            }
                        }
                        return;
                    }
                }
                Boolean rightIsTrue = first.IsRight();
                if (rightIsTrue == true)
                {
                    MessageBox.Show("Controllo eseguito con successo, LIBRO IN ORDINE");
                }
            }catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }
        
        //Il primo task per la pagina sinistra 
        private async Task<int> FirstThreadPageSxAsync()
        {
            Bitmap newFoto;
            //Inizio a scorrere il libro dalla parte sx
            pageNextSxFN.GetNextPageSx(insertFileNameSx, nNextSx);
            //Ottengo la pagina sx
            insertFileNameSx = pageNextSxFN.GetPageNextSx();
            //Ottengo il numero del file name della pagina sx
            nNextSx = pageNextSxFN.GetNumNextSx();
            //Adatto la pagina sx alle dimensioni corrette
            Size size = new Size(picOriginalSx.Width, picOriginalSx.Height);
            Bitmap nextImage = (Bitmap)Image.FromFile(insertFileNameSx);
            newFoto = new Bitmap(nextImage, size);
            //Ritaglio la nuova immagine con l'area del riquardo di riferimento
            Crop cut = new Crop(coordAreaSx);
            Bitmap pageArea = cut.Apply(newFoto);

            //Adatto il riquadro e eseguo Tesseract
            BoxOCR boxPage = new BoxOCR();
            Bitmap box = boxPage.AdaptBox(pageArea);
            //picOriginalSx.Image = box;
            picOriginalSx.Image = newFoto;

            lock (ocrengine)
            {
                stringSxDetected = boxPage.GetStringTesseract(box, ocrengine);
            }

            int numberSx = 0;
            Match match = Regex.Match(stringSxDetected, @"\d+");
            if (match.Success)
            {
                numberSx = int.Parse(match.Value);
            }
            //Il numero ottenuto dopo l'esecizone dell'OCR
            numSxPageSx = numberSx;

            //MessageBox.Show(stringSxDetected+" provaSX ");
            //MessageBox.Show(numSxPageSx.ToString());
            return numSxPageSx;


        }
        
        //Il primo task per la pagina destra
        private async Task<int> FirstThreadPageDxAsync()
        {
        
            Bitmap newFoto;
            //Inizio a scorrere il libro dalla parte dx
            pageNextDxFN.GetNextPageDx(insertFileNameDx, nNextDx);
            //Ottengo la pagina dx
            insertFileNameDx = pageNextDxFN.GetPageNextDx();
            //Ottengo il numero del file name della pagina dx
            nNextDx = pageNextDxFN.GetNumNextDx();
            //Adatto la pagina dx alle dimensioni corrette
            Size size = new Size(picOriginalDx.Width, picOriginalDx.Height);
            Bitmap nextImage = (Bitmap)Image.FromFile(insertFileNameDx);
            newFoto = new Bitmap(nextImage, size);
            //Ritaglio la nuova immagine con l'area del riquardo di riferimento
            Crop cut = new Crop(coordAreaDx);
            Bitmap pagArea = cut.Apply(newFoto);

            //Adatto il riquadro e eseguo Tesseract
            BoxOCR boxPage = new BoxOCR();
            Bitmap box = boxPage.AdaptBox(pagArea);
            //picOriginalDx.Image = box;
            picOriginalDx.Image = newFoto;

            lock (ocrengine)
            {
                stringDxDetected = boxPage.GetStringTesseract(box, ocrengine);
            }

            int numberDx = 0;
            Match match = Regex.Match(stringDxDetected, @"\d+");
            if (match.Success)
            {
                numberDx = int.Parse(match.Value);
            }
            //Il numero ottenuto dopo l'esecizone dell'OCR
            numDxPageDx = numberDx;

            //MessageBox.Show(stringDxDetected+" provaDx ");
            //MessageBox.Show(numDxPageDx.ToString());
            return numDxPageDx;            
        }

        private async Task StartControlAfterFirstCon()
        {
            try
            {
                Bitmap nextImageSize = (Bitmap)picOriginalSx.Image;
                //Continua finche la pagina sinsitra e destra non arriva alla fine del libro
                while (nNextSx + 2 <= lastNumber && nNextDx + 2 <= lastNumber)
                {

                    //Creo i due task uno per la pagina sinistra, uno per la destra e li eseguo
                    var sxTask = Task.Run(() => FirstThreadPageSxAsync());
                    var dxTask = Task.Run(() => FirstThreadPageDxAsync());

                    //Attendo il completamento dei due task paralleli
                    var numSxTesseract = await sxTask;
                    var numDxTesseract = await dxTask;

                    //Inserisco i due numeri torvati della pagina sx e dx dopo il rilevamento Tesseract e li controllo in sequenza
                    firstAfterCont.SetNextNum(numSxTesseract, numDxTesseract);
                    //Verifico e aggiorno la sequenza
                    firstAfterCont.Update(stringSxDetected, stringDxDetected);
                    //MessageBox.Show(firstAfterCont.GetNumberAfterGetCorrectNumber().ToString());

                    //Quando isRight da false perchè si è verificato un errore durante il controllo
                    //(es. si è saltata una pagina durante la digitalizzazione oppure ci sono due pagine senza numero)
                    if (firstAfterCont.IsRight() == false)
                    {
                        //Azzero i contatoti delle pagine
                        nNextSx = 0;
                        nNextDx = 0;
                        Boolean rightIsFalse = firstAfterCont.IsRight();
                        Boolean empty = firstAfterCont.isEmpty();
                        //Se c'è un errore mostro i file name delle due pagine sbagliate
                        if (rightIsFalse == false)
                        {
                            if (empty == true)
                            {
                                MessageBox.Show("NON E' STATO RILEVATO ALCUN NUMERO NELLE PAGINE " + insertFileNameSx + " E " + insertFileNameDx + "");
                            }
                            else
                            {
                                //MessageBox.Show("C'è un errore dopo la pagina"+numSxErr.ToString()+"e"+numDxErr.ToString()+"");
                                MessageBox.Show("C'è un errore nel controllo della numerazione nelle pagine " + insertFileNameSx + "  e  " + insertFileNameDx.ToString() + "");
                            }
                        }
                        return;
                    }
                }
                Boolean rightIsTrue = firstAfterCont.IsRight();
                if (rightIsTrue == true)
                {
                    MessageBox.Show("Controllo eseguito con successo, LIBRO IN ORDINE");
                }
            }
            catch(Exception ex)
            {
                DialogResult result;
                result = MessageBox.Show(ex.Message);
            }
        }    
    }
}
