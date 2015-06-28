using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.DataTypes {
    public struct PCMBuffer {
        List<double[]> data;
        int sampleRate;

        /*public PCMBuffer(int sampleRate = -1, int numChannels = 1) {
            data = new List<List<double>>();
            this.sampleRate = sampleRate;
        }*/
        public PCMBuffer(bool nothing = false) {
            sampleRate = -1;
            _sampleLength = -1;
            data = new List<double[]>();
        }
        public PCMBuffer(int sampleRate, params double[][] data) {
            this.sampleRate = sampleRate;
            _sampleLength = -1;
            this.data = new List<double[]>();
            foreach (double[] ch in data) this.data.Add(new List<double>(ch).ToArray());

        }

        int _sampleLength;
        public int SampleLength {
            get {
                if (_sampleLength != -1) return _sampleLength;

                _sampleLength = 0;
                foreach (double[] ch in data) if (ch.Length > _sampleLength) _sampleLength = ch.Length;
                return _sampleLength;
            }
        }

        public int Channels { get { return data.Count; } }
        public double[] RawData {
            get {
                pad();
                List<double> raw = new List<double>();
                int ch = data.Count;
                for (int i = 0; i < SampleLength; i++) {
                    for (int c = 0; c < ch; c++) {
                        raw.Add(data[c][i]);
                    }
                }

                return raw.ToArray();
            }
        }

        public static PCMBuffer operator +(PCMBuffer left, PCMBuffer right) {
            int sampleRate = Math.Max(left.sampleRate, right.sampleRate);
            if (sampleRate != -1 && left.sampleRate != right.sampleRate) throw new NotSupportedException("Sample rates unmatched");
            
            int chdiff = Math.Abs(left.data.Count - right.data.Count);
            if (chdiff == 0) {
                List<List<double>> fdata = new List<List<double>>();
                int length = 0;
                int ch = left.data.Count;
                for (int i = 0; i < ch; i++) {
                    fdata.Add(new List<double>());
                    if (left.data[i].Length > length) length = left.data[i].Length;
                    if (right.data[i].Length > length) length = right.data[i].Length;
                }
                for (int c = 0; c < ch; c++) {
                    double res = 0;
                    for (int i = 0; i < length; i++) {
                        if (left.data[c].Length > i) res += left.data[c][i];
                        if (right.data[c].Length > i) res += right.data[c][i];
                        fdata[c].Add(res);
                    }
                }
                PCMBuffer b = new PCMBuffer(sampleRate);
                b.data = new List<double[]>();
                foreach (List<double> cht in fdata) b.data.Add(cht.ToArray());
                return b;
            }
            else if (left.data.Count < 3 && right.data.Count < 3) {
                return (left.asStereo() + right.asStereo());
            }
            else {
                throw new NotSupportedException("Addition of PCMBuffers of differing channel counts only supported for stereo+mono");
            }
        }

        public static PCMBuffer operator *(PCMBuffer pcm, double mult) {
            if (mult == 1.0) return pcm;
            if (mult == 0.0) {
                PCMBuffer zb = new PCMBuffer(pcm.sampleRate);
                //double[] silence = new double[pcm.SampleLength];
                foreach (double[] c in pcm.data) zb.data.Add(new double[pcm.SampleLength]);
                return zb;
            }

            PCMBuffer b = new PCMBuffer(pcm.sampleRate);
            foreach (double[] ch in pcm.data) {
                List<double> nc = new List<double>();
                for (int i = 0; i < ch.Length; i++) nc.Add(ch[i] * mult);
                b.data.Add(nc.ToArray());
            }

            return pcm;
        }

        PCMBuffer asStereo() {
            if (data.Count == 2) return this;
            return new PCMBuffer(sampleRate, data[1], data[1]);
        }
        void pad() { // if channel lengths are mismatched, pad to longest
            int sl = SampleLength;
            for (int i = 0; i < data.Count; i++) {
                if (data[i].Length != sl) {
                    double[] ch = data[i];
                    Array.Resize(ref ch, sl);//ch.AddRange(new double[sl - ch.Count]);
                    data[i] = ch;
                }
            }
        }

        public PCMBuffer Delay(int samples) {
            double[] silence = new double[samples];

            PCMBuffer b = new PCMBuffer(sampleRate);
            b.data = new List<double[]>();
            
            for (int c = 0; c < data.Count; c++) b.data.Add(silence.Concat(data[c]).ToArray());
            b.pad();

            return b;
        }
    }
}
