using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using C_sawapan_media;

namespace workshop17
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();

            glControl1.Resize += glControl1_Resize;
            glControl1.Load += glControl1_Load;
            glControl1.Paint += glControl1_Paint;
            glControl1.MouseMove += GlControl1_MouseMove;
            this.FormClosing += Form1_FormClosing;

            nextButton.Click += nextButton_Click;
            prevButton.Click += prevButton_Click;
        }

        void nextButton_Click(object sender, EventArgs e)
        {
            mediawin.state++;
            if (mediawin.state > 11) mediawin.state = 0;
        }

        void prevButton_Click(object sender, EventArgs e)
        {
            mediawin.state--;
            if (mediawin.state <= 0) mediawin.state = 10;
        }

        private void GlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            mediawin.MouseX = e.X;
            mediawin.MouseY = mediawin.Height - e.Y;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            if (mediawin != null) mediawin.Terminate();
            

            MediaIO.UnInitialize();
        }

        MediaWindow mediawin = new MediaWindow();
        bool loaded = false;
        Timer timer = new Timer();


        void UpdateFrame()
        {
            if (!loaded) return;
            mediawin.OnFrameUpdate();
            glControl1.SwapBuffers();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            MediaIO.Initialize(this.Handle.ToInt32());
            mediawin.Initialize();

            loaded = true;
            timer.Interval = 35;
            timer.Enabled = true;
            timer.Start();
            timer.Tick += new EventHandler(timer_Tick);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);

            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Normalize);


            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!loaded) return;
            UpdateFrame();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            mediawin.Width = glControl1.Width;
            mediawin.Height = glControl1.Height;
            

            if (!loaded) return;

            GL.Viewport(0, 0, mediawin.Width, mediawin.Height); // Use all of the glControl painting area

            UpdateFrame();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            UpdateFrame();
        }


    }
}
