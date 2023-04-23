using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace ModificaImagen
{
    public partial class Form1 : Form
    {
        private string pathOriginalImage = string.Empty;
        public Form1()
        {
            InitializeComponent(); 
        }

        public void ImageProcessing(string originalImagePath) //, string outputPath
        {
            byte[] photoBytes = File.ReadAllBytes(originalImagePath); // change imagePath with a valid image path
            int quality = 100;
            var format = new PngFormat(); // we gonna convert a image to a png one
            var size = new Size(800, 600);

            using (var inStream = new MemoryStream(photoBytes))
            {
                // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                using (var imageFactory = new ImageFactory(preserveExifData: true))
                {
                    // Do your magic here
                    imageFactory.Load(inStream)                        
                        .Rotate(0)
                        .RoundedCorners(new RoundedCornerLayer(10, true, true, true, true))
                        .Watermark(new TextLayer()
                        {
                            DropShadow = true,
                            FontFamily = FontFamily.GenericSerif,
                            Text = "ALI",
                            FontSize = 115,
                            Style = FontStyle.Bold,
                            FontColor = Color.BlueViolet
                        })
                        .Resize(size)
                        .Format(format)
                        .Quality(quality);
                        //.Save(outputPath);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        imageFactory.Save(ms);
                        pictureBox2.Image = Image.FromStream(ms);
                    }
                }
            }
        }

        private void LblOriginalPicture_Paint(object sender, PaintEventArgs e)
        {
            Font myFont = LblImagen1.Font;
            Brush myBrush = new SolidBrush(LblImagen1.ForeColor);

            e.Graphics.TranslateTransform(30, 100);
            e.Graphics.RotateTransform(-90);
            e.Graphics.DrawString("Imagen Original", myFont, myBrush, 0, 0);
            // 
            System.Diagnostics.Debug.WriteLine("LblOriginalPicture_Paint");
        }

        private void LblImagen2_Paint(object sender, PaintEventArgs e)
        {
            Font myFont = LblImagen2.Font;
            Brush myBrush = new SolidBrush(LblImagen2.ForeColor);

            e.Graphics.TranslateTransform(30, 100);
            e.Graphics.RotateTransform(-90);
            e.Graphics.DrawString("Imagen Editada", myFont, myBrush, 0, 0);
        }

        private void BtnEditarImagen_Click(object sender, EventArgs e)
        {
            if (File.Exists(pathOriginalImage))
            {
                ImageProcessing(pathOriginalImage);
            }
        }

        private void BtnAbreImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imagenes (*.jpg)|*.jpg|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Selecciona una imagen";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathOriginalImage = openFileDialog.FileName;
                pictureBox1.ImageLocation = pathOriginalImage;
            }
        }



    }
}
