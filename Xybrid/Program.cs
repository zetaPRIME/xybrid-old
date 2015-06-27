using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

using Xynapse;

using Xybrid.UI;

namespace Xybrid {
    public class Program {
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UIForm());

            UIForm f1 = new UIForm(1);
            UIForm f2 = new UIForm(2);

            Button b = new Button();
            b.SetBounds(48, 48, 48, 48);
            b.Click += (Object s, EventArgs e) => {
                //Application.Exit();
                new UIForm(1).Show();
            };
            f2.Controls.Add(b);

            f2.Show();

            UIManager.SpawnUIUpdateThread();
            Application.Run(f1);

            bool yes = true;
            if (yes) return;

            //
            XAudio2 audio = new XAudio2();
            WaveFormat format = new WaveFormat(44100, 32, 2);
            MasteringVoice mvoice = new MasteringVoice(audio);
            SourceVoice voice = new SourceVoice(audio, format);

            int bufferSize = format.ConvertLatencyToByteSize(500);

            DataStream stream = new DataStream(bufferSize, true, true);

            int samples = bufferSize / format.BlockAlign;
            for (int i = 0; i < samples; i++) {
                float val = (float)(Math.Sin(2*Math.PI*500*i/format.SampleRate));// * (0.5 + Math.Sin(2*Math.PI*2.2*i/format.SampleRate)*0.5));
                //if (val == 0) val = 1;
                //val = val / Math.Abs(val);
                for (int j = 0; j < 3; j++) val = val * val;
                stream.Write(val);
                stream.Write(val);
                //stream.Write(val * (float)(0.5 + Math.Sin(2 * Math.PI * 2.2 * i / format.SampleRate) * 0.5));
                //stream.Write(val * (float)(0.5 + Math.Cos(2 * Math.PI * 2.2 * i / format.SampleRate) * 0.5));
            }

            stream.Position = 0;

            AudioBuffer buffer = new AudioBuffer { Stream = stream, Flags = BufferFlags.EndOfStream, AudioBytes = bufferSize };
            //buffer.LoopCount = AudioBuffer.LoopInfinite;
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            voice.SubmitSourceBuffer(buffer, null);
            
            voice.Start();
            Thread.Sleep(3000);
            long pos = stream.Position;
            stream.Position = 0;
            for (int i = 0; i < samples; i++) {
                float val = (float)(Math.Sin(2 * Math.PI * 500 * i / format.SampleRate));// * (0.5 + Math.Sin(2*Math.PI*2.2*i/format.SampleRate)*0.5));
                //if (val == 0) val = 1;
                //val = val / Math.Abs(val);
                //for (int j = 0; j < 8; j++) val = val * val;
                stream.Write(val);
                stream.Write(val);
                //stream.Write(val * (float)(0.5 + Math.Sin(2 * Math.PI * 2.2 * i / format.SampleRate) * 0.5));
                //stream.Write(val * (float)(0.5 + Math.Cos(2 * Math.PI * 2.2 * i / format.SampleRate) * 0.5));
            }
            //stream.Position = pos;
            //voice.State.p
            while (voice.State.BuffersQueued > 0) Thread.Sleep(100);
            //Thread.Sleep(6000);
        }
    }
}
