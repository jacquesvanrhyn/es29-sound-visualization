using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C_sawapan_media;
using OpenTK.Graphics.OpenGL;


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

        public void Terminate()
        {
            MediaIO.SoundIn.Stop();
        }



        //animation function. This contains code executed 20 times per second.
        public void OnFrameUpdate()
        {
            //get normalized mouse coordinates
            double mouseXnorm = MouseX / Width;
            double mouseYnorm = MouseY / Height;
            double dx = 0.0;

            if (mouseXnorm < 0.0) mouseXnorm = 0.0;
            if (mouseYnorm < 0.0) mouseYnorm = 0.0;

            GL.ClearColor(0.6f, 0.6f, 0.6f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            //GL.Viewport(0, 0, Width, Height);
            GL.Ortho(1.0, Width, Height, 0.0 , 1.0, -1.0);
            //GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

            GL.Color4(1.0, 1.0, 1.0, 1.0);

            /*
            //...........................................A simple tone
            if (!Sound1.IsPlaying)
            {
                double fr = 440.0; //frequency of tone to generate in Hz
                double duration = Sound1.DurationSec; //overall duration of sound sample sec
                double dt = 1.0 / Sound1.SamplesPerSecond; //duration of a single sample point in seconds
                double PI2 = Math.PI * 2.0;

                for (int k = 0; k < Wave1.GetLength(0); ++k)
                {
                    double t = k * dt;
                    Wave1[k, 0] = 0.3 * Math.Cos(t * fr * PI2);
                    Wave1[k, 1] = 0.3 * Math.Cos(t * fr * PI2);
                }

                Sound1.SetWaveFormD(Wave1);
                Sound1.Play(true);
            }
             */
           
            
            /*
            //...........................................A simple tone interactive [pan, freq]
           if (!Sound1.IsPlaying)
            {
                double fr = 100+mouseYnorm*500; //frequency of tone to generate in Hz
                double duration = Sound1.DurationSec; //overall duration of sound sample sec
                double dt = 1.0 / Sound1.SamplesPerSecond; //duration of a single sample point in seconds
                double PI2 = Math.PI * 2.0;

                for (int k = 0; k < Wave1.GetLength(0); ++k)
                {
                    double t = k * dt;
                    Wave1[k, 0] = mouseXnorm * Math.Cos(t * fr * PI2);
                    Wave1[k, 1] = (1.0-mouseXnorm) * Math.Cos(t * fr * PI2);
                }

                Sound1.SetWaveFormD(Wave1);
                Sound1.Play(false);
            }
             */
            
           
            //...........................................Envelope
            if (!Sound1.IsPlaying)
             {
                 double fr = 100 + mouseYnorm * 100; //frequency of tone to generate in Hz
                 double duration = Sound1.DurationSec; //overall duration of sound sample sec
                 double dt = 1.0 / Sound1.SamplesPerSecond; //duration of a single sample point in seconds
                 double PI2 = Math.PI * 2.0;


                 for (int k = 0; k < Wave1.GetLength(0); ++k)
                 {
                     double t = k * dt;
                     //double env = t * 5.0;
                     //double env = (t-0.5)*(t-0.5) * 5.0;
                     double ddt=t-duration*0.5;
                     double env = Math.Exp(-400.0*mouseXnorm*ddt*ddt);

                     Wave1[k, 0] = 0.5 * Math.Cos(t * fr * PI2) * env;
                     Wave1[k, 1] = 0.5 * Math.Cos(t * fr * PI2) * env;
                 }

                 Sound1.SetWaveFormD(Wave1);
                 Sound1.Play(false);
             }
            

            //...........................................wave addition
            /*if (!Sound1.IsPlaying)
            {
                double fr1 = 200; //frequency of tone to generate in Hz
                double fr2 = 100 + mouseXnorm * 800; //frequency of tone to generate in Hz

                double duration = Sound1.DurationSec; //overall duration of sound sample sec
                double dt = 1.0 / Sound1.SamplesPerSecond; //duration of a single sample point in seconds
                double PI2 = Math.PI * 2.0;


                for (int k = 0; k < Wave1.GetLength(0); ++k)
                {
                    double t = k * dt;

                    Wave1[k, 0] = 0.3 * Math.Cos(t * fr1 * PI2) + 0.5*mouseYnorm * Math.Cos(t * fr2 * PI2);
                    Wave1[k, 1] = Wave1[k, 0];
                }

                Sound1.SetWaveFormD(Wave1);
                Sound1.Play(false);
            }*/

            //.............................Real time mic input Analysis
            
            /*
          if (MediaIO.SoundIn.Listening)
            {
                MediaIO.SoundIn.GetLatestSample();

                SoundIN Mic = MediaIO.SoundIn;

                int size = Mic.WaveLeft.Count/2; //we'll just use half of the recorded sample to improve performance
                double analysisDuration = (double)size / (double)Mic.SamplesPerSecond;
                dx = Width / (double)size;


                //draw input waveform
                GL.Color4(1.0, 1.0, 1.0, 1.0);
                GL.Begin(PrimitiveType.LineStrip);
                for (int k = 0; k < size; ++k)
                {
                    GL.Vertex2(k * dx, 100.0 + Mic.WaveLeft[k] * 300.0);
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
            
            */
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

            //......................................................Draw Wave1
            dx = Width / (double)Wave1.GetLength(0);
            GL.Color4(1.0, 0.0, 0.5, 0.3);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < Wave1.GetLength(0); ++k)
            {
                GL.Vertex2(k * dx, Height * 0.6 - Wave1[k, 0] * 200.0);
            }
            GL.End();

            GL.Color4(0.2, 0.5, 1.0, 0.3);
            GL.Begin(PrimitiveType.LineStrip);
            for (int k = 0; k < Wave1.GetLength(0); ++k)
            {
                GL.Vertex2(k * dx, Height * 0.3 - Wave1[k, 1] * 200.0);
            }
            GL.End();

            /*
            //...............................................Wave file
            if (!SoundFromFile.IsPlaying)
            {
                SoundFromFile.Play(false);
            }
            */

            
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
            
            

        }
    }
}
