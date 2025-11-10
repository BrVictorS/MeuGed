using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Services.API;
using SkiaSharp;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace SGD.Services.Barcode
{
    public class BarcodeService:IBarcodeServiceInterface
    {
        // Altere o campo BarReader para BarcodeReaderClass para compatibilidade com DestroyBarcodeReader
        private DTKBarReaderLib.BarcodeReaderClass? BarReader = null;
        private readonly IApiInterface _api;
        private readonly IConnectionFactory _connectionFactory;

        public BarcodeService(IApiInterface api)
        {
            _api = api;
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" }; // Mude para o host do seu RabbitMQ
            ConfigurarCodigoBarras();
        }

        #region Funções
        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("DTKBarReader.dll", CharSet = CharSet.Unicode), PreserveSig]
        private static extern int CreateBarcodeReader(out DTKBarReaderLib.BarcodeReaderClass barReader);
        [DllImport("DTKBarReader.dll", CharSet = CharSet.Unicode), PreserveSig]
        private static extern int DestroyBarcodeReader(DTKBarReaderLib.BarcodeReaderClass barReader);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        // Altere o método ConfigurarCodigoBarras para usar BarcodeReaderClass
        #endregion
        public void ConfigurarCodigoBarras()
        {
            if (BarReader != null)
            {
                DestroyBarcodeReader(BarReader);
                BarReader = null;
            }

            if (CreateBarcodeReader(out BarReader) != 0)
            {
                Console.WriteLine("Erro ao chamar a Dll de Códigos de Barras");
            }
            else
            {
                string s = "";
                //TPLIYM2417C4LJPICKSA
                //BarReader.LicenseManager.AddLicenseKey("YOUR DEVELOPER LICENSE KEY");

                // BarReader.LicenseManager.AddLicenseKey("762DF92100FB3EF73B9F");
                // BarReader.LicenseManager.GetActivationLink("762DF92100FB3EF73B9F", ref s);
                // BarReader.LicenseManager.SetActivationCode("0A167D97AFFC9D5858BAE35BA65F75B322CB10BACE1152803AB92D76ED4D41BBA069F5068B2F295A");

                //BarReader.LicenseManager.GetActivationLink("TPLIYM2417C4LJPICKSA", ref s);
                //BarReader.LicenseManager.SetActivationCode("096102E5DC8AEF5E51BBE12AD22877C255CB10BAAD772C8D25CD2D73EC4A44CDDF66E378FB26435D");
                //BarReader.LicenseManager.AddLicenseKey("TPLIYM2417C4LJPICKSA");

                //Alterado em:14-08-2010 lê codigo de barra em todo documento
                //BarReader.BarcodeTypes = DTKBarReaderLib.BarcodeTypeEnum.BT_Code39;
                //BarReader.BarcodeOrientation = DTKBarReaderLib.BarcodeOrientationEnum.BO_LeftToRight | DTKBarReaderLib.BarcodeOrientationEnum.BO_TopToBottom;
                //BarReader.BarcodeTypes = DTKBarReaderLib.BarcodeTypeEnum.BT_Code39;
                //BarReader.BarcodeOrientation = DTKBarReaderLib.BarcodeOrientationEnum.BO_All;
            }
        }
        // No método CapturarCodigoBarras, troque o tipo de BarReader para BarcodeReaderClass
        public string CapturarCodigoBarras(string v_sImagem)
        {
            string aTemp = string.Empty;
            BarReader.BarcodesToRead = 1;
            BarReader.ScanInterval = 1;
            BarReader.QuietZoneSize = DTKBarReaderLib.QuietZoneSizeEnum.QZ_Normal;
            BarReader.PDFReadingType = DTKBarReaderLib.PDFReadingTypeEnum.PDF_Images;
            BarReader.ThresholdMode = DTKBarReaderLib.ThresholdModeEnum.TM_Automatic;
            BarReader.Threshold = 128;
            BarReader.ThresholdStep = 16;
            BarReader.ThresholdCount = 8;
            BarReader.ScanPage = 0;
            BarReader.I2of5Checksum = true;//false;
            BarReader.Code11Checksum = false;
            BarReader.Code39Checksum = false;
            BarReader.Code93Checksum = false;
            BarReader.ImageInvert = false;
            BarReader.ImageDespeckle = 0;
            BarReader.ImageErode = 0;
            BarReader.ImageDilate = 0;
            BarReader.ImageSharp = 0;
            //BarReader.BarcodeTypes = DTKBarReaderLib.BarcodeTypeEnum.BT_Inter2of5;
            BarReader.BarcodeOrientation = DTKBarReaderLib.BarcodeOrientationEnum.BO_All;

            //BarReader.BarcodeTypes = DTKBarReaderLib.BarcodeTypeEnum.BT_Inter2of5 | DTKBarReaderLib.BarcodeTypeEnum.BT_Code39;
            BarReader.BarcodeTypes = DTKBarReaderLib.BarcodeTypeEnum.BT_Inter2of5;

            BarReader.BarcodeOrientation = DTKBarReaderLib.BarcodeOrientationEnum.BO_All;
            BarReader.BarcodesToRead = 1;
            BarReader.ScanInterval = 1;
            BarReader.QuietZoneSize = DTKBarReaderLib.QuietZoneSizeEnum.QZ_Normal; ;
            BarReader.PDFReadingType = DTKBarReaderLib.PDFReadingTypeEnum.PDF_Images;
            BarReader.ThresholdMode = DTKBarReaderLib.ThresholdModeEnum.TM_Automatic; ;
            BarReader.Threshold = 128;
            BarReader.ThresholdStep = 16;
            BarReader.ThresholdCount = 8;
            BarReader.ScanPage = 0;
            BarReader.I2of5Checksum = false;
            BarReader.Code11Checksum = false;
            BarReader.Code39Checksum = false;
            BarReader.Code93Checksum = false;
            BarReader.ImageDespeckle = 0;
            BarReader.ImageErode = 0;
            BarReader.ImageDilate = 0;
            BarReader.ImageSharp = 0;
            /*BarReader.ImageInvert = 0;*/
            /* retirado */
            try
            {
                BarReader.ReadFromFile(v_sImagem);

                /*Bitmap oImg = (Bitmap)Bitmap.FromFile(v_sImagem);
                IntPtr hBitmap = oImg.GetHbitmap();
                BarReader.ReadFromBitmap((int)hBitmap, 0);*/
                BarReader.ReadFromFile(v_sImagem);
                /*       DeleteObject(hBitmap);
                oImg.Dispose();
                oImg = null;*/

                if (BarReader.Barcodes.Count == 0)
                {
                   /* oImg = (Bitmap)Bitmap.FromFile(v_sImagem);
                    hBitmap = oImg.GetHbitmap();
                    BarReader.ReadFromBitmap((int)hBitmap, 0);
                    DeleteObject(hBitmap);
                    oImg.Dispose();
                    oImg = null;*/
                }

                if (BarReader.Barcodes.Count == 0)
                    aTemp = string.Empty;
                else
                {
                    int iX = 0;

                    for (int iIndex = 0; iIndex < BarReader.Barcodes.Count; iIndex++)
                    {
                        if ((BarReader.Barcodes.get_Item(iIndex).BarcodeDataLen == 10) ||
                            (BarReader.Barcodes.get_Item(iIndex).BarcodeDataLen == 9))
                        {
                            DTKBarReaderLib.Barcode oBarcode = BarReader.Barcodes.get_Item(iIndex);

                            if (iX == 0)
                                iX = oBarcode.BorderStartX1;

                            if (iX >= oBarcode.BorderStartX1)
                            {
                                if (aTemp == null)
                                    aTemp = string.Empty;
                                aTemp = oBarcode.BarcodeString;

                                //regra para validaçao código de barras
                                string codbarra;
                                codbarra = aTemp.ToString().PadLeft(10, '0');

                                codbarra = codbarra.Substring(0, 9);
                                codbarra += CalculoDV10(codbarra);

                                if (!codbarra.Equals(aTemp.PadLeft(10, '0')))
                                {
                                    aTemp = string.Empty;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception oError)
            {
                throw oError;
            }

            return aTemp;
        }
        public static string CalculoDV10(string numero)
        {
            if (string.IsNullOrEmpty(numero))
            {
                return string.Empty;
            }

            foreach (char c in numero)
            {
                if (!char.IsDigit(c))
                {
                    return string.Empty;
                }
            }           
            int soma = 0;
            int multiplicador = 2;

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                int produto = (numero[i] - '0') * multiplicador;               
                if (produto > 9)
                {
                    soma += (produto / 10) + (produto % 10);
                }
                else
                {
                    soma += produto;
                }
                multiplicador = (multiplicador == 2) ? 1 : 2;
            }
            int resto = soma % 10;
            int digitoVerificador = (resto == 0) ? 0 : 10 - resto;

            return digitoVerificador.ToString();
        }

        public void QuebraLote(LoteApiDto lote)
        {
            ConfigurarCodigoBarras();
            foreach (ImagemLoteDto imagem in lote.imagens)
            {
                string arquivo = imagem.caminho;
                string etiqueta = CapturarCodigoBarras(arquivo);
                if (!string.IsNullOrEmpty(etiqueta))
                {
                    _api.InsereDocumento(new Dtos.Verify.InsereDocumentoDto()
                    {
                        documento = etiqueta,
                        id = imagem.id

                    });
                }
            }
        }

        

    }
}