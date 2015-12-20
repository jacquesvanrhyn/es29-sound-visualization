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
            state = 18;
            //state = 12;
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
            
            if (state == 0)
            {
                angleXY = time;
                angleZ = 0.0;
            }
            else if (state == 1)
            {
                angleXY = time;
            }
            else if (state == 2)
            {
                angleXY = time + SoundIN.TheSoundIN.AverageVolume * 300.0;
                angleZ = Math.PI / 5.0;
            }
            else if (state == 3)
            {
                angleXY = time / 0.5;
                angleZ = Math.PI * SoundIN.TheSoundIN.AverageVolume * 60.0;
            }
            else if (state == 4)
            {
                angleXY = time;
            }
            else if (state == 5)
            {
                angleXY = time;
                angleZ = 0.0;
            }
            else if (state == 6)
            {
                angleXY = time;
            }
            else if (state == 7)
            {
                angleXY = time;
                angleZ = Math.PI / 5.0;
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
                angleXY = time / 4.0;
                //angleZ = 0.2 + (Math.PI / 6.0) * Math.Abs (Math.Cos(time / 5.0));
                //angleZ = 0.35 + (Math.PI / 30.0) * Math.Cos(time * 2.0 );
                angleZ = Math.PI / 5.0;
            }
            else if (state == 11)
            {
                angleXY = time;
                angleZ = 0.0;
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
                angleZ = Math.PI / 12.0;
            }
            else if (state == 15)
            {
                angleZ = Math.PI / 5.0;
                angleXY = time / 15.0;
            }
            else if (state == 16)
            {
                angleZ = Math.PI / 5.0;
                angleXY = time / 4.0;
            }
            else if (state == 17)
            {
                angleXY = time;
            }
            else if (state == 18)
            {
                angleXY = time;
                angleZ = 0;
            }

            // represent cartesian coordinates of the spherical coordinates passed in
            double eyeX = viewTarget.X + viewDistance * Math.Cos(angleXY)*Math.Cos(angleZ);
            double eyeY = viewTarget.Y + viewDistance * Math.Sin(angleXY)*Math.Cos(angleZ);  
            double eyeZ = viewTarget.Z + viewDistance * Math.Sin(angleZ);
        
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
           
            // ..............................................  the awesome circle can oscilate with Math - wowow
            
            double radius = 2.0;
            if (MediaIO.SoundIn.Listening)
            {
                MediaIO.SoundIn.GetLatestSample();

                SoundIN Mic = MediaIO.SoundIn;

                int size = Mic.WaveLeft.Count / 2; 
                //we'll just use half of the recorded sample to improve performance
                double analysisDuration = (double)size / (double)Mic.SamplesPerSecond;
                dx = 5.0 / (double)size;



                if (state == 0)
                {
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
                    GL.Color4(0.0, 0.0, 0.0, 1.0);
                    GL.Begin(PrimitiveType.Lines);
                    for (int i = 0; i < 72; i = i + 3)
                    {
                        GL.Vertex3(vertexarray[i], vertexarray[i + 1], vertexarray[i + 2]);
                    }
                    GL.End();
                }
                else if (state == 2)
                {
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
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.5);
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
                else if (state == 4)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0;
                    double spiralTime = time - 5.0;
                    GL.LineWidth((float)0.5);
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
                else if (state == 5)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 100.0;
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

                }
                else if (state == 6)
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
                else if (state == 7)
                {

                    double b = 0.2;
                    //double b = Mic.AverageVolume * 15.0;
                    double offset = b + Math.Sqrt(Mic.AverageVolume) * 3.0;
                    
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
                else if (state == 8)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    //double a = Mic.PeakFrequencyHz * 20.0;
                    double a = Mic.AverageVolume * 1000.0;
                    double spiralTime = time - 5.0;
                    double b = Mic.AverageVolume * 100.0;
                    GL.PointSize((float)0.1);
                    GL.Begin(PrimitiveType.LineStrip);
                    int points = 1000;
                    //int points = (int) Mic.AverageVolume * 1000;
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double helixX = b * Math.Cos(a * t) * Math.Cos(a * t) * Math.Cos(a * t);
                        double helixY = b * Math.Sin(a * t) * Math.Sin(a * t) * Math.Sin(a * t);
                        double helixZ = b * Math.Sin(t);
                        GL.Vertex3(helixZ, helixY, helixX);
                    }
                    GL.End();

                }
                else if (state == 9)
                {
                    GL.Color4(1.0, 1.0, 1.0, 1.0);
                    double a = Mic.AverageVolume * 20.0 + 1.0;
                    double spiralTime = time - 5.0;
                    radius = Mic.AverageVolume * 100.0 + 0.7;
                    GL.PointSize((float)Mic.AverageVolume * (float)150.0);
                    GL.Begin(PrimitiveType.Points);

                    int points = Mic.WaveLeft.Count / ((int)a * 10);
                    for (int i = 0; i < points; i++)
                    {
                        double t = 30.0 * (Math.PI / (double)points) * i;
                        double spiralX = radius * Math.Cos(t) * Math.Cos(a * t);
                        double spiralY = radius * Math.Sin(t) * Math.Cos(a * t);
                        double spiralZ = radius * Math.Sin(a * t);
                        GL.Vertex3(spiralX, spiralY, spiralZ + 3.0 * Mic.WaveLeft[i % Mic.WaveLeft.Count]);

                    }
                    GL.End();
                    radius = 2.0;
                }
                else if (state == 10)
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
                else if (state == 11)
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
                else if (state == 12)
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
                else if (state == 13)
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
                else if (state == 14)
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
                else if (state == 15)
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
                else if (state == 16)
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
                else if (state == 17)
                {
                    GL.Begin(PrimitiveType.Points);
                    double unit = .15;
                    int boxsize = 10;
                    for (int h = -boxsize; h < boxsize; h++)
                    {
                        for (int i = -boxsize; i < boxsize; i++)
                        {
                            for (int j = -boxsize; j < boxsize; j++)
                            {
                                GL.Vertex3( i * unit,  j * unit, h * unit * Mic.AverageVolume * 200.0);                               
                            }
                        }
                    }
                    GL.End();
                }
                else if (state == 18)
                {
                    //Here I want a "hairy" visualization
                    int num = 10;
                    int len = 5;
                    double unit = 0.1;
                    double zlength = 0.2;
                    double xoffset = 0.0;
                    double yoffset = 0.0;
                    for (int i = -num; i < num; i++)
                    {
                        for (int j = -num; j < num; j++)
                        {
                            int r2 = i * i + j * j;
                            if (r2 < num * num)
                            {
                                GL.Begin(PrimitiveType.LineStrip);
                                for (int k = 0; k < len; k++)
                                {
                                    xoffset = (k * i * k / 1000.0) * Mic.AverageVolume * 900.0;
                                    yoffset = (k * j * k / 1000.0) * Mic.AverageVolume * 900.0;

                                    GL.Vertex3(i * unit + xoffset, j * unit + yoffset, k * zlength / (Mic.AverageVolume * 500.0) - 1.8);

                                }
                                GL.End();
                            }



                            //GL.Vertex3(i * unit + k/20.0, j * unit + k/20.0, k * zlength);

                        }
                    }
                }   
             
            } 
            
        }
    }
}
