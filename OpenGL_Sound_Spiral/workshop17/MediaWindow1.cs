using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using C_sawapan_media;
using OpenTK.Graphics.OpenGL;
using OpenTK;


namespace workshop17
{
    public class MediaWindow
    {
        public int Width = 0;       //width of the viewport in pixels
        public int Height = 0;      //height of the viewport in pixels
        public double MouseX = 0.0; //location of the mouse along X
        public double MouseY = 0.0; //location of the mouse along Y

        SoundSampleFreq Sound1;
        double[,] Wave1;
        SoundSample SoundFromFile;
        double[,] WaveFromFile;

        List<double> VolumeHistory = new List<double>();
        List<double> PianoKeyHistory = new List<double>();


        CFFT fft = new CFFT();


        //initialization function. Everything you write here is executed once in the begining of the program
        public void Initialize()
        {
            Sound1 = SoundOUT.TheSoundOUT.AddEmptyFreqSample(0.5, 0.5);
            Wave1 = Sound1.GetWaveFormD();

            SoundFromFile = SoundOUT.TheSoundOUT.OpenWaveFile(@"piano.wav");
            WaveFromFile = SoundFromFile.GetWaveFormD();
            

            MediaIO.SoundIn.Start();
        }

        double time = 0.0;

        public void Terminate()
        {
            MediaIO.SoundIn.Stop();
        }

        double angleXY = 0.0;
        double angleZ = 0.0;

        double viewDistance = 10.0;

        Vector3d viewTarget = new Vector3d(0.0, 0.0, 0.0);

        public int state = 0;

        double[] vertexarray = new double[72] {
                                                 1.0, 1.0, 1.0, 
                                                 1.0, -1.0, 1.0,
                                                 1.0, -1.0, 1.0,
                                                 -1.0, -1.0, 1.0,
                                                 -1.0, -1.0, 1.0, 
                                                 -1.0, 1.0, 1.0,
                                                 -1.0, 1.0, 1.0,
                                                 1.0, 1.0, 1.0,
                                                 1.0, 1.0, -1.0, 
                                                 1.0, -1.0, -1.0,
                                                 1.0, -1.0, -1.0,
                                                 -1.0, -1.0, -1.0,
                                                 -1.0, -1.0, -1.0, 
                                                 -1.0, 1.0, -1.0,
                                                 -1.0, 1.0, -1.0,
                                                 1.0, 1.0, -1.0,
                                                 1.0, 1.0, 1.0,
                                                 1.0, 1.0, -1.0,
                                                 1.0, -1.0, 1.0,
                                                 1.0, -1.0, -1.0,
                                                 -1.0, -1.0, 1.0,
                                                 -1.0, -1.0, -1.0,
                                                 -1.0, 1.0, 1.0,
                                                 -1.0, 1.0, -1.0
                                                  };
       
        //animation function. This contains code executed 20 times per second.
        public void OnFrameUpdate()
        {
            //get normalized mouse coordinates
            double mouseXnorm = MouseX / Width;
            double mouseYnorm = MouseY / Height;
            double dx = 0.0;

            if (mouseXnorm < 0.0) mouseXnorm = 0.0;
            if (mouseYnorm < 0.0) mouseYnorm = 0.0;

            // comment the part below out when not testing
            //state = 7;
            // state = 5;

            if (state==0)
            GL.ClearColor(0.6f, 0.6f, 0.6f, 1.0f);
            else if (state % 2 == 1)
            {
                GL.ClearColor(0.8f, 0.6f, 0.6f, 1.0f);
            }
            else
            {
                GL.ClearColor(0.6f, 0.6f, 0.7f, 1.0f);
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            time += 0.1;

            angleXY = time;
            viewDistance = 5.0;
            angleZ =Math.PI*SoundIN.TheSoundIN.AverageVolume*50.0;
            if (state == 1)
            {
                angleXY = time / 0.5;
                angleZ = Math.PI * SoundIN.TheSoundIN.AverageVolume * 60.0;
            }
            else if (state == 2)
            {
                angleXY = time;
            }
            else if (state == 3)
            {
                angleXY = time;
            }
            else if (state == 4)
            {
                angleXY = time;
            }
            else if (state == 5)
            {
                angleXY = time;
            }
            else if (state == 6)
            {
                angleXY = time;
            }
            else if (state == 7)
            {
                angleXY = time;
            }
            else if (state == 8)
            {
                angleXY = time;
            }
            else if (state == 9)
            {
                angleXY = time;
            }
            else if (state == 10)
            {
                angleXY = time;
            }
            else if (state == 11)
            {
                angleXY = time;
            }
            else if (state == 12)
            {
                angleXY = time;
            }
            else if (state == 13)
            {
                angleXY = time;
            }
            else if (state == 14)
            {
                angleXY = time;
            }
            else if (state == 15)
            {
                angleXY = time;
            }
            else
            {
                angleXY = time;
            }

            // represent cartesian coordinates of the spherical coordinates passed in
            double eyeX = viewTarget.X + viewDistance * Math.Cos(angleXY)*Math.Cos(angleZ);
            double eyeY = viewTarget.Y + viewDistance * Math.Sin(angleXY)*Math.Cos(angleZ);  
            double eyeZ = viewTarget.Z +  viewDistance* Math.Sin(angleZ);
        
            GL.MatrixMode(MatrixMode.Projection);

            // @ Linda, this is the matrix that determines how much is getting animated
            Matrix4d proj = Matrix4d.Perspective(Math.PI * 0.3, Width / (double)Height, 0.1, 100.0);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            
            //view matrix
             Matrix4d look = Matrix4d.LookAt(eyeX, eyeY, eyeZ, viewTarget.X, viewTarget.Y,  viewTarget.Z, 0.0, 0.0, 1.0);
            GL.LoadMatrix(ref look);
            
            GL.Color4(0.0, 0.0, 0.0, 1.0);
            GL.LineWidth((float)0.5);
            
            //GL.Begin(PrimitiveType.Lines);
            
            //creates a cube 
            
            

            /*
            for (int i = 0; i < 72; i = i + 3)
            {
                GL.Vertex3(vertexarray[i], vertexarray[i + 1], vertexarray[i + 2]);
            }
            GL.End();
             */

            // ..............................................  the awesome circle can oscilate with Math - wowow
            // @ Linda - it's about to get 2kewl4skewl
            double radius = 2.0;
            if (MediaIO.SoundIn.Listening)
            {
                MediaIO.SoundIn.GetLatestSample();

                SoundIN Mic = MediaIO.SoundIn;

                int size = Mic.WaveLeft.Count / 2; 
                //we'll just use half of the recorded sample to improve performance
                double analysisDuration = (double)size / (double)Mic.SamplesPerSecond;
                dx = 5.0 / (double)size;

                // @ Linda, try changing this up
                // @jacques -- trying to incorporate the spiral with the input wave 

                if (state == 0)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.Begin(PrimitiveType.LineStrip);
                    int points = Mic.WaveLeft.Count * 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();

                    GL.Color4(0.0, 0.0, 0.0, 1.0);
                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 0; i < 72; i = i + 3)
                    {
                        GL.Vertex3(vertexarray[i], vertexarray[i + 1], vertexarray[i + 2]);
                    }
                    GL.End();
                }
                else if (state == 1)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.Begin(PrimitiveType.LineStrip);
                    int points = Mic.WaveLeft.Count * 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Sin(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Tan(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 2)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.Begin(PrimitiveType.LineStrip);
                    int points = Mic.WaveLeft.Count * 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Sin(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();

                    GL.Color4(0.0, 0.0, 0.0, 1.0);
                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 0; i < 72; i = i + 3)
                    {
                        GL.Vertex3(vertexarray[i], vertexarray[i + 1], vertexarray[i + 2]);
                    }
                    GL.End();
                }
                else if (state == 3)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    //double t = Math.Floor(time / 2);
                    double spiralTime = time - 5.0;
                    GL.PointSize((float)radius);
                    GL.Begin(PrimitiveType.Points);
                    
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i * Math.Sin(spiralTime);
                        double spiralX = radius * Math.Cos(t) * Math.Sin(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Tan(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                        
                    }
                    GL.End();

                    GL.Color4(0.0, 0.0, 0.0, 1.0);
                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 0; i < 72; i = i + 3)
                    {
                        GL.Vertex3(vertexarray[i], vertexarray[i + 1], vertexarray[i + 2]);
                    }
                    GL.End();
                }
                else if (state == 4)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.PointSize((float)0.5);
                    GL.Begin(PrimitiveType.Points);
                    int points = Mic.WaveLeft.Count / 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Sin(a * t * t);
                        double spiralY = radius * Math.Sin(t) * Math.Tan(a * t * t);
                        double spiralZ = radius * Math.Sin(a * t * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 5)
                {

                    double b = 0.2;
                    //double b = Mic.AverageVolume * 15.0;
                    double offset = b + Mic.AverageVolume * 10.0;
                    
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(b, b, offset); GL.Vertex3(-b, b, offset); GL.Vertex3(-b, -b, offset); GL.Vertex3(b, -b, offset);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(b, b, -offset); GL.Vertex3(-b, b, -offset); GL.Vertex3(-b, -b, -offset); GL.Vertex3(b, -b, -offset);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(b, -offset, b); GL.Vertex3(b, -offset, -b); GL.Vertex3(-b, -offset, -b); GL.Vertex3(-b, -offset, b);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(b, offset, b); GL.Vertex3(b, offset, -b); GL.Vertex3(-b, offset, -b); GL.Vertex3(-b, offset, b);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(offset, b, b); GL.Vertex3(offset, b, -b); GL.Vertex3(offset, -b, -b); GL.Vertex3(offset,- b, b);
                    GL.End();
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(-offset, b, b); GL.Vertex3(-offset, b, -b); GL.Vertex3(-offset, -b, -b); GL.Vertex3(-offset, -b, b);
                    GL.End();
                }
                else if (state == 6)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.PeakFrequencyHz * 20.0;
                    double spiralTime = time - 5.0;
                    GL.PointSize((float)0.1);
                    GL.Begin(PrimitiveType.Points);
                    int points = Mic.WaveLeft.Count / 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 7)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    //double a = Mic.PeakFrequencyHz * 20.0;
                    double a = time / 10.0;
                    double spiralTime = time - 5.0;
                    GL.PointSize((float)0.1);
                    GL.Begin(PrimitiveType.LineLoop);
                    int points = Mic.WaveLeft.Count / 2;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double helixX = Math.Cos(a * t) * Math.Cos(a * t) * Math.Cos(a * t);
                        double helixY = Math.Sin(a * t) * Math.Sin(a * t) * Math.Sin(a * t);
                        double helixZ = Math.Sin(t);
                        GL.Vertex3(helixX, helixY, helixZ);
                    }
                    GL.End();
                }
                else if (state == 8)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.PeakFrequencyHz * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.Lines);
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 9)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.PeakNote * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.Lines);
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 10)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.PeakVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.Lines);
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 11)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.MaxFrequencyHz * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.Lines);
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();

                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    a = Mic.PeakFrequencyHz * 20.0;
                    GL.Begin(PrimitiveType.Points);
                    for (int i = 0; i < points * 2; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralY = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralX = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 13)
                {
                    GL.Color4(1.0, 1.0, 1.0, 0.05);
                    double c = Mic.PeakVolume * 20.0;
                    double b = Mic.PeakFrequencyHz * 20.0;
                    double a = Mic.PeakNote * 20.0;
                    double spiralTime = time - 5.0;

                    GL.Begin(PrimitiveType.Triangles);
                    int points = Mic.WaveLeft.Count / 300;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos((a + b) * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos((a + b) * t);
                        double spiralZ = radius * Math.Sin(c * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();

                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos((a + b) * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos((a + b) * t);
                        double spiralZ = radius * Math.Sin(c * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }
                else if (state == 15)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double c = Mic.PeakVolume * 20.0;
                    double b = Mic.PeakFrequencyHz * 20.0;
                    double a = Mic.PeakNote * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.1);
                    GL.Begin(PrimitiveType.Points);
                    int points = Mic.WaveLeft.Count / 10;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();

                    // dominant tone? 
                    double maxPianoKey = 49.0 + 12.0 * Math.Log(Mic.MaxFrequencyHz / 440.0) / Math.Log(2.0);
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 0; i < points / 10; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(maxPianoKey * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(maxPianoKey * t);
                        double spiralZ = radius * Math.Sin(maxPianoKey * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);
                    }
                    GL.End();
                }

                /*
                // @ jacques -- trying to incorporate the input data... 
                //..........................................Frequency spectrum

                List<double> Real = new List<double>(size);
                List<double> Imaginary = new List<double>(size);

                List<double> abs2 = new List<double>(size);

                for (int k = 0; k < size; ++k)
                {
                    Real.Add(MediaIO.SoundIn.WaveLeft[k]);
                    Imaginary.Add(0.0);
                }
                fft.ForwardTransform(ref Real, ref Imaginary);


                //compute magnitude for each frequency
                for (int k = 0; k < size; ++k)
                {
                    abs2.Add(Real[k] * Real[k] + Imaginary[k] * Imaginary[k]);
                }


                int maxFreqIndex = 1; //skip first frequency as it corresponds to the average volume
                for (int k = 1; k < size / 2; ++k)
                {
                    if (abs2[k] > abs2[maxFreqIndex])
                    {
                        maxFreqIndex = k;
                    }
                }

                double maxFreqHz = maxFreqIndex / analysisDuration;
                double maxPianoKey = 49.0 + 12.0 * Math.Log(maxFreqHz / 440.0) / Math.Log(2.0);

                PianoKeyHistory.Add(maxPianoKey);
                if (PianoKeyHistory.Count > 200) PianoKeyHistory.RemoveAt(0);

                // Console.WriteLine(maxFreqHz + "Hz , Piano Key:" + maxPianoKey);


                dx = Width / (double)VolumeHistory.Count;
                GL.Color4(0.0, 0.0, 0.0, 1.0);

                for (int k = 0; k < PianoKeyHistory.Count; ++k)
                {
                    GL.PointSize((float)(VolumeHistory[k] * 100.0));
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(k * dx, 300.0 + PianoKeyHistory[k] * 3.0);
                    GL.End();
                }
                */



                /* // normal white wave input circle
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < 360; i++)
                {
                    double inradians = Math.PI / 180 * (i/10);
                    GL.Vertex3(Math.Cos(inradians) * radius, Math.Sin(inradians) * radius, Mic.WaveLeft[i] * 5.0);
                }
                GL.End(); */
                /*
                GL.Color4(0.0, 0.5, 0.0, 0.5);
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < 360; i++)
                {
                    double inradians = Math.PI / 180 * i;
                    GL.Vertex3(Math.Cos(inradians) * radius, Math.Sin(inradians) * radius, Mic.WaveLeft[i] * 5.0 + 2.0);
                }
                GL.End();




                GL.Color4(1.0, 0.0, 0.5, 0.3);
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < 180; i++)
                {
                    double inradians = Math.PI / 180 * i * 2;
                    GL.Vertex3(Math.Cos(inradians) * radius, Math.Sin(inradians) * radius, Mic.WaveLeft[i] * 5.0 - 2.0);
                }
                GL.End();

                
                GL.Color4(0.5, 0.5, 0.5, 0.5);
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < 90; i++)
                {
                    double inradians = Math.PI / 180 * (i * 4);
                    GL.Vertex3(Math.Cos(inradians) * radius, Math.Sin(inradians) * radius, Mic.WaveLeft[i] * 5.0);
                }
                GL.End();   
               */
             
            } 
            
            
            // ........................... make a spherical point cloud

            /* 
            // @Linda- can you try to get this array implemented outside of this function so that these points are permanently visible?
            // first, we need to generate a metric fuckton of indiviual points
            int spherenum = 1000;
            // theta and phi can be no more than 2 radians
            double max = Math.PI * 2;

            double maxrad = 3.0;

            double phi = 0.0;
            double theta = 0.0;
            double rad = 0.0;
            double x = 0.0; 
            double y = 0.0;
            double z = 0.0;

            GL.Color4(1.0, 1.0, 1.0, 1.0);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < spherenum; i++)
            {
                phi = Random() / 100.0 * max;
                theta = Random() / 100.0 * max;
                rad = Random() / 100.0 * maxrad;
                x = rad * Math.Cos(theta) * Math.Sin(phi);
                y = rad * Math.Sin(theta) * Math.Cos(phi);
                z = rad * Math.Cos(phi);
                GL.Vertex3(x, y, z);
            }
                
            GL.End(); */
            

            
            /*
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < spherenum; i++)
            {
                phi = Random() * max;
                theta = Random() * max;
                rad = Random() * maxrad;

                x = rad * Math.Cos(theta) * Math.Sin(phi);
                y = rad * Math.Sin(theta) * Math.Cos(phi);
                z = rad * Math.Cos(phi);

                GL.Vertex3(x, y, z);
            }

            GL.End();
            */




            //.............................Real time mic input Analysis

            /*
           if (MediaIO.SoundIn.Listening)
             {
                 MediaIO.SoundIn.GetLatestSample();

                 SoundIN Mic = MediaIO.SoundIn;

                 int size = Mic.WaveLeft.Count/2; //we'll just use half of the recorded sample to improve performance
                 double analysisDuration = (double)size / (double)Mic.SamplesPerSecond;
                 dx = 5.0 / (double)size;

                
                 // Pan's wave
                 //draw input waveform
                 GL.Color4(1.0, 1.0, 1.0, 1.0);
                 GL.Begin(PrimitiveType.LineStrip);
                 for (int k = 0; k < size; ++k)
                 {
                     GL.Vertex3(k * dx,0.0,  Mic.WaveLeft[k] * 10.0);
                 }
                 GL.End();

                 //find maximum and average volume in current sample
                 double MaxVolume = 0.0;
                 double AvgVolume = 0.0;
                 for (int k = size-1000; k < size; ++k)
                 {
                     double v = Math.Abs(Mic.WaveLeft[k]);
                     if (v > MaxVolume)
                         MaxVolume = v;

                     AvgVolume += v;
                 }

                 AvgVolume /= 999.0;

                 //append current maximum volume to history list
                 VolumeHistory.Add( MaxVolume);
                 if (VolumeHistory.Count > 200) VolumeHistory.RemoveAt(0);

                 //draw volume variation for the past 200 samples
                 dx = Width / (double)VolumeHistory.Count;
                 GL.Color4(0.0, 1.0, 1.0, 1.0);
                 GL.Begin(PrimitiveType.LineStrip);
                 for (int k = 0; k < VolumeHistory.Count; ++k)
                 {
                     GL.Vertex2( k * dx, 300.0+VolumeHistory[k] * 200.0);
                 }
                 GL.End();

                 //..........................................Frequency spectrum

                 List<double> Real=new List<double>(size);
                 List<double> Imaginary=new List<double>(size);

                 List<double> abs2 = new List<double>(size);
               
                 for (int k = 0; k < size; ++k)
                 {
                     Real.Add(MediaIO.SoundIn.WaveLeft[k]);
                     Imaginary.Add(0.0);
                 }
                 fft.ForwardTransform(ref Real, ref Imaginary);

                
                 //compute magnitude for each frequency
                 for (int k = 0; k < size; ++k)
                 {
                     abs2.Add(Real[k] * Real[k] + Imaginary[k] * Imaginary[k]);
                 }


                 //draw power spectrum
                 dx = Width / (double)size;
                 GL.Color4(0.0, 0.0, 0.0, 0.5);
                 GL.Begin(PrimitiveType.LineStrip);
                 for (int k = 0; k < size/2; ++k)
                 {
                     GL.Vertex2(2.0*k * dx, 20.0+abs2[k] * 0.1);
                 }
                 GL.End();


                 int maxFreqIndex = 1; //skip first frequency as it corresponds to the average volume
                 for (int k = 1; k < size / 2; ++k)
                 {
                    if (abs2[k]>abs2[maxFreqIndex])
                     {
                         maxFreqIndex = k;
                     }
                 }

                 double maxFreqHz = maxFreqIndex / analysisDuration;
                 double maxPianoKey = 49.0 + 12.0 * Math.Log(maxFreqHz / 440.0) / Math.Log(2.0);

                 PianoKeyHistory.Add(maxPianoKey);
                 if (PianoKeyHistory.Count > 200) PianoKeyHistory.RemoveAt(0);

                 Console.WriteLine(maxFreqHz + "Hz , Piano Key:"+maxPianoKey);


                 dx = Width / (double)VolumeHistory.Count;
                 GL.Color4(0.0, 0.0, 0.0, 1.0);
                
                 for (int k = 0; k < PianoKeyHistory.Count; ++k)
                 {
                     GL.PointSize((float)(VolumeHistory[k]*100.0));
                     GL.Begin(PrimitiveType.Points);
                     GL.Vertex2(k * dx, 300.0 + PianoKeyHistory[k] * 3.0);
                     GL.End();
                 }
               
                 //alternative visualization in log scale corresponding to piano notes
                 dx =  Width / 200.0;
                 GL.Color4(0.0, 0.0, 0.0, 1.0);
                 GL.Begin(PrimitiveType.LineStrip);
                 int nstep = 1;
                 for (int k = 1; k < 200; k+=nstep)
                 {
                     double nv = MediaIO.SoundIn.NoteBandVolume((k*0.5 - nstep*0.5), (k*0.5+nstep*0.5));
                     GL.Vertex2(k * dx , 200.0 + nv* 20000.0);    

                 }
                 GL.End();
             }
            
             * */

            /*
            //........................................................Frequency synthesis
             if (!Sound1.IsPlaying)
             {
                
                 if (MouseX > 1 && MouseX < Width)
                 {
                     Sound1.SilenceAllFrequencies();
                     Sound1.AddFreq(440.0, 0.1);
                     Sound1.AddFreq(mouseYnorm*1000.0+200.0, 0.1);

                     Sound1.BuildSoundSample();

                     Sound1.GetWaveFormD(Wave1);
                 }                


                 Sound1.Play(false);
             }*/

            
            //........................................................Noise
            /*
            if (!Sound1.IsPlaying)
            {
                Random rn = new Random();
                Sound1.SilenceAllFrequencies();
                for (int k = 1; k < 10000; ++k)
                {
                    Sound1.SetFreq(k, rn.NextDouble() * 0.001, rn.NextDouble() * Math.PI * 2.0);
                }

                Sound1.BuildSoundSample();
                Sound1.GetWaveFormD(Wave1);

                Sound1.Play(false);
            }*/

            //...............................................Colored noise with envelope 
            /* if (!Sound1.IsPlaying)
             {
                 Random rn = new Random();
                 Sound1.SilenceAllFrequencies();
                 for (int k = 1; k < 10000; ++k)
                 {
                     double t = k / (double)10000.0;
                     double env = Math.Exp(-300.0 * mouseYnorm * (t - mouseXnorm) * (t - mouseXnorm));
                     Sound1.SetFreq(k, rn.NextDouble() * 0.001*env, rn.NextDouble() * Math.PI * 2.0);
                 }

                 Sound1.BuildSoundSample();
                 Sound1.GetWaveFormD(Wave1);

                 Sound1.Play(false);
             }*/

            /*
            //......................................................Draw Wave1
            dx = Width / (double)Wave1.GetLength(0);
            GL.Color4(1.0, 0.0, 0.5, 0.3);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < Wave1.GetLength(0); ++k)
            {
                GL.Vertex3(k * dx, Height * 0.6 - Wave1[k, 0] * 200.0, dx);
            }
            GL.End();

            GL.Color4(0.2, 0.5, 1.0, 0.3);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < Wave1.GetLength(0); ++k)
            {
                GL.Vertex3(k * dx, Height * 0.3 - Wave1[k, 1] * 200.0, dx);
            }
            GL.End(); */
           
            /*
            //...............................................Wave file
            if (!SoundFromFile.IsPlaying)
            {
                SoundFromFile.Play(false);
            }
            */

            /*
            //......................................................Draw Wave2
            dx = Width / (double)WaveFromFile.GetLength(0);
            GL.Color4(0.0, 0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < WaveFromFile.GetLength(0); ++k)
            {
                GL.Vertex2(k * dx, Height * 0.6 - WaveFromFile[k, 0] * 200.0);
            }
            GL.End();

            GL.Color4(1.0, 0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < WaveFromFile.GetLength(0); ++k)
            {
                GL.Vertex2(k * dx, Height * 0.3 - WaveFromFile[k, 1] * 200.0);
            }
            GL.End();
            */

            // GL.End();
        }

        /*
        private double randomnumber(double p, double max)
        {
            throw new NotImplementedException();
        }
        */
        
        // making a randomization function 
        private int Random()
        {
            Random random = new Random();
            Int32 minimum = 0;
            Int32 maximum = 100; 
            return random.Next(minimum,maximum);
        }
        


    }
}
