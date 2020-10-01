using AForge.Imaging;
using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Tesseract;

namespace OCR
{
    class BoxOCR
    {
        private String text;
        public BoxOCR()
        {

        }
        //Modifico l'immagine ricevuta in input(rettangolo dell'immagine selezionato) e lo adatto in modo che Tesseract 
        //possa rilevare il numero nel modo corretto possibile
        public Bitmap AdaptBox(Bitmap image)
        {
            Size size = new Size(image.Width * 3, image.Height * 3);
            Bitmap newBit = new Bitmap(image, size);
            // creo il filtro per i grigi (BT709)
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap newImage = filter.Apply(newBit);
            //Inserisco altri filtri per pulire l'immagine
            new BradleyLocalThresholding().ApplyInPlace(newImage);
            new Invert().ApplyInPlace(newImage);
            //Restriusce il bitmap senza eventuali parti che possano modificare il risulato voluto
            Bitmap redRect = MenageBox(newImage);
            Bitmap newRect = filter.Apply(redRect);
            new Invert().ApplyInPlace(newRect);
            return newRect;
        }

        //Entra in funzione Tesseract e restituisce la stringa presente nell'immagine
        public string GetStringTesseract(Bitmap box, TesseractEngine ocrengine)
        {
            using (var res = ocrengine.Process(box))
            {
                text = res.GetText();
            }
            return text;
        }

        //In ogni bitamp ricevuto, cerco eventuali aree che possano interferire nell'OCR e le elimino
        public Bitmap MenageBox(Bitmap boxAdapted)
        {
            Blob[] blobs1;
            //Lista dei rettangoli(blob) presenti nell'immagine
            List<Rectangle> checkRect = new List<Rectangle>();
            //Lista dei rettangoli(blob) presenti nella parte in alto dell'immagine
            List<Rectangle> topRect = new List<Rectangle>();
            //Lista dei rettangoli(blob) presenti nella parte a sinistra dell'immagine
            List<Rectangle> leftRect = new List<Rectangle>();
            //Lista dei rettangoli(blob) presenti nella parte a destra dell'immagine
            List<Rectangle> rightRect = new List<Rectangle>();

            // Crea un istanza di un BlobCounter 
            BlobCounterBase bc1 = new BlobCounter();
            Bitmap verify = new Bitmap(boxAdapted);
            // Processo l'immagine
            bc1.ProcessImage(verify);
            blobs1 = bc1.GetObjects(verify, false);  
            //Ogni blob presente nell immagine finisce in checkRect
            foreach (Blob blob in blobs1)
            {
                checkRect.Add(blob.Rectangle);
            }
            foreach(Rectangle rect in checkRect)
            {
                if (rect.Bottom < verify.Height / 6)
                {
                    topRect.Add(rect);
                }
                if (rect.Right < verify.Width / 8)
                {
                    leftRect.Add(rect);
                }
                if (rect.X > verify.Width-(verify.Width / 8))
                {
                    rightRect.Add(rect);
                }
            }
            //Ordino le liste in maniera decresente in base al parametro che cerco
            var rectTop = topRect.OrderByDescending(r => r.Bottom).ToList();
            var rectLeft = leftRect.OrderByDescending(r => r.Right).ToList();
            var rectRight = rightRect.OrderByDescending(r => r.X).ToList();
            //Lista di tutti dei rettangoli cercati
            List<Rectangle> rectMax = new List<Rectangle>();

            if (topRect.Count != 0)
            {
                rectMax.Add(rectTop[0]);
                Crop cutTop = new Crop(new Rectangle(0, rectTop[0].Bottom, verify.Width, verify.Height - (rectTop[0].Bottom)));
                verify = cutTop.Apply(verify);
            }
            if (leftRect.Count != 0)
            {
                rectMax.Add(rectLeft[0]);
                Crop cutLeft = new Crop(new Rectangle(rectLeft[0].Right, 0, verify.Width-(rectLeft[0].Right), verify.Height));
                verify = cutLeft.Apply(verify);
            }
            if (rightRect.Count != 0)
            {
                rectMax.Add(rectRight[0]);
                Crop cutRight = new Crop(new Rectangle(0, 0, verify.Width - (rectRight[0].Width), verify.Height));
                verify = cutRight.Apply(verify);

            }
            //La funzione using ha solo funzione visiva(Non è necessaria per il funzionamento del codice)
            //Mi mostra in rosso quali rettangoli sono stati trovati
            /*
            using (var gfx = Graphics.FromImage(verify))
            {
                foreach (Rectangle rect in rectMax)
                {
                    gfx.FillRectangle(Brushes.Red, rect);
                }
                gfx.Flush();
            }
            */
            return verify; 
        }
    }
}
