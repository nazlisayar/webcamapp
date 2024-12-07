using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace webcamapp
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices; 
        private VideoCaptureDevice videoSource;
        private Bitmap currentFrame;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            capturephoto();
        }
        private void capturephoto()
        {

            if (currentFrame != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg";
                saveFileDialog.Title = "fotoğrafı kaydet";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) 
                { 
                    currentFrame.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);//foto kaydet
                }

            }
            else
            {
                MessageBox.Show("webcam görüntüsü alınamadı");
            }














        }
        //form yüklendiğinde webcami başlatması için;
        private void Form1_Load(object sender, EventArgs e)
        {
          
            videoDevices= new FilterInfoCollection(FilterCategory.VideoInputDevice);   //video cihazlarını al
            if (videoDevices.Count == 0) 
            {
                MessageBox.Show("webcam bulunamadı");
                return;
            }
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_Newframe);
            videoSource.Start();    
        }
        private void videoSource_Newframe(object sender,NewFrameEventArgs eventArgs)
        {
             currentFrame=(Bitmap)eventArgs.Frame.Clone();   //anlık görüntü alma
          
            pictureBox1.Image =  new Bitmap(currentFrame, pictureBox1.Width,pictureBox1.Height);
            

        }
        private void Form1_Formclosing(object sender, FormClosingEventArgs e)
        {
            if(videoSource.IsRunning)
            { videoSource.SignalToStop(); 
              videoSource.WaitForStop();
            }
        }
    }
}
