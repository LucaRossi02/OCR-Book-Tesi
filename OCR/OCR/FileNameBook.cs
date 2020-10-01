using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    class FileNameBook
    {
        //Lista dei numeri che può contenere una sottostringa cercata
        private string[] list = new string[]
            {
                "1","2","3","4","5","6","7","8","9"
            };

        //Succesiva stringa di file name dell'immagine sinsitra e destra del libro
        private string pageNextSx;
        private string pageNextDx;
        
        //Successivo numero identificativo nel file name dell immagine sinistra e destra 
        private int numNextSx;
        private int numNextDx;
        public FileNameBook()
        {

        }

        //Cerco il file name dell'ultima immagine del libro
        public string LastFile(string fileNameFirst)
        {
            string fileName = fileNameFirst;
            string barra = @"\";
            int num = fileName.LastIndexOf(barra);
            string folderWhitoutFile = fileName.Substring(0, num+1);
            var files = new DirectoryInfo(folderWhitoutFile).GetFiles();
            string latestFile = "";
            int indexOf_ = fileName.LastIndexOf("_");
            int n = int.Parse(fileName.Substring(indexOf_ + 1, 4));

            foreach(FileInfo file in files)
            {
                int indexOfNext_ = file.Name.LastIndexOf("_");
                int nNextOfLast = int.Parse(file.Name.Substring(indexOfNext_ + 1, 4));
                if (nNextOfLast> n)
                {
                    n = nNextOfLast;
                    latestFile = file.Name;
                }
            }   
            return latestFile;
        }

        //Ottengo il numero identificativo del file name dell'ultima immagine del libro
        public int NumberOfLastFile(string lastFile)
        {
            int indexOf_ = lastFile.LastIndexOf("_");
            int n = int.Parse(lastFile.Substring(indexOf_ + 1, 4));
            return n;
        }

        //Metodo che scorre l'intero libro pagina per pagina della parte sinistra
        public void GetNextPageSx(string fileNameBook, int nNext)
        {
            int indexOf_Next = fileNameBook.LastIndexOf("_");
            int nVerify = int.Parse(fileNameBook.Substring(indexOf_Next + 1, 4));
            nNext = nVerify;

            if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("000") && list.Any(fileNameBook.Substring(indexOf_Next + 4, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 1;
                if (int.Parse(nVerify.ToString().Substring(verify)) == 8 | int.Parse(nVerify.ToString().Substring(verify)) == 9)
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, "00" + numNextSx.ToString());
                }
                else
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, "000" + numNextSx.ToString());
                }
            }
            else if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("00") && list.Any(fileNameBook.Substring(indexOf_Next + 3, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 2;
                if (int.Parse(nVerify.ToString().Substring(verify, 2)) == 98 | int.Parse(nVerify.ToString().Substring(verify, 2)) == 99)
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, "0" + numNextSx.ToString());
                }
                else
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, "00" + numNextSx.ToString());
                }
            }
            else if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("0") && list.Any(fileNameBook.Substring(indexOf_Next + 2, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 3;
                if (int.Parse(nVerify.ToString().Substring(verify, 3)) == 998 | int.Parse(nVerify.ToString().Substring(verify, 3)) == 999)
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, numNextSx.ToString());
                }
                else
                {
                    numNextSx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextSx = remove.Insert(r + 1, "0" + numNextSx.ToString());
                }
            }
            else if (list.Any(fileNameBook.Substring(indexOf_Next + 1, 1).Equals))
            {
                numNextSx = nNext + 2;
                int r = fileNameBook.LastIndexOf("_");
                string remove = fileNameBook.Remove(r + 1, 4);
                pageNextSx = remove.Insert(r + 1, numNextSx.ToString());
            }
        }

        //Metodo che scorre l'intero libro pagina per pagina della parte destra
        public void GetNextPageDx(string fileNameBook, int nNext)
        {
            int indexOf_Next = fileNameBook.LastIndexOf("_");
            int nVerify = int.Parse(fileNameBook.Substring(indexOf_Next + 1, 4));
            nNext = nVerify;

            if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("000") && list.Any(fileNameBook.Substring(indexOf_Next + 4, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 1;
                if (int.Parse(nVerify.ToString().Substring(verify)) == 8 | int.Parse(nVerify.ToString().Substring(verify)) == 9)
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx = remove.Insert(r + 1, "00" + numNextDx.ToString());
                }
                else
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx= remove.Insert(r + 1, "000" + numNextDx.ToString());
                }
            }
            else if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("00") && list.Any(fileNameBook.Substring(indexOf_Next + 3, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 2;
                if (int.Parse(nVerify.ToString().Substring(verify, 2)) == 98 | int.Parse(nVerify.ToString().Substring(verify, 2)) == 99)
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx = remove.Insert(r + 1, "0" + numNextDx.ToString());
                }
                else
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx = remove.Insert(r + 1, "00" + numNextDx.ToString());
                }
            }
            else if (fileNameBook.Substring(indexOf_Next + 1, 4).Contains("0") && list.Any(fileNameBook.Substring(indexOf_Next + 2, 1).Equals))
            {
                int verify = nVerify.ToString().Length - 3;
                if (int.Parse(nVerify.ToString().Substring(verify, 3)) == 998 | int.Parse(nVerify.ToString().Substring(verify, 3)) == 999)
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx = remove.Insert(r + 1, numNextDx.ToString());
                }
                else
                {
                    numNextDx = nNext + 2;
                    int r = fileNameBook.LastIndexOf("_");
                    string remove = fileNameBook.Remove(r + 1, 4);
                    pageNextDx = remove.Insert(r + 1, "0" + numNextDx.ToString());
                }
            }
            else if (list.Any(fileNameBook.Substring(indexOf_Next + 1, 1).Equals))
            {
                numNextDx = nNext + 2;
                int r = fileNameBook.LastIndexOf("_");
                string remove = fileNameBook.Remove(r + 1, 4);
                pageNextDx = remove.Insert(r + 1, numNextDx.ToString());
            }
        }

        //Ottengo la pagina sx successiva
        public string GetPageNextSx()
        {
            return pageNextSx;
        }

        //Ottengo il numero del file name della pagina sx successiva
        public int GetNumNextSx()
        {
            return numNextSx;
        }

        //Ottengo la pagina dx successiva
        public string GetPageNextDx()
        {
            return pageNextDx;
        }

        //Ottengo il numero del file name della pagina dx successiva
        public int GetNumNextDx()
        {
            return numNextDx;
        }
    }
}
