using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xynapse.DataTypes {
    public class RollingPCMBuffer {
        int channels = 2;
        int segmentLength = 64;
        Segment first = null;
        Segment last = null;
        List<Segment> pool = new List<Segment>();

        int numSegments = 0;
        int samplesWritten = 0; // number of samples written TO THE LAST SEGMENT
        int samplesConsumed = 0;

        public int SampleLength { get { return -samplesConsumed + ((numSegments - 1) * segmentLength) + samplesWritten; } }

        public RollingPCMBuffer(int channels = 2, int segmentLength = 64, int premake = 0) {
            this.channels = channels;
            this.segmentLength = segmentLength;
            for (int i = 0; i < premake; i++) pool.Add(new Segment(channels, segmentLength));
        }

        public void Add(PCMBuffer buffer) {
            if (buffer.Channels != channels) throw new ArgumentException("Wrong number of channels");

            int length = buffer.SampleLength;
            int pos = 0;

            if (last == null) AddSegment();

            while (pos < length) {
                for (int ch = 0; ch < channels; ch++) last.data[ch, samplesWritten] = buffer[ch, pos];
                pos++;
                samplesWritten++;
                if (samplesWritten == segmentLength) {
                    AddSegment();
                    samplesWritten = 0;
                }
            }
        }

        public PCMBuffer Read(int samples, bool consume) {
            List<double[]> dataOut = new List<double[]>();
            for (int i = 0; i < channels; i++) dataOut.Add(new double[samples]);

            int smp = samplesConsumed;
            int seg = 0;
            int pos = 0;

            Segment cur = first;
            while (pos < samples) { // read data sample by sample
                for (int i = 0; i < channels; i++) dataOut[i][pos] = cur.data[i, smp];
                pos++;
                smp++;
                if (smp == segmentLength) {
                    smp = 0;
                    seg++;
                    cur = cur.next;
                }
            }

            if (consume) { // snap out segments passed if they should be consumed
                for (int i = 0; i < seg; i++) ConsumeSegment();
                samplesConsumed = smp;
            }

            // and return result
            return new PCMBuffer(-1, dataOut.ToArray());
        }

        void AddSegment() {
            Segment s = null;
            if (pool.Count > 0) {
                s = pool[0];
                s.next = null;
                pool.RemoveAt(0);
            }
            else s = new Segment(channels, segmentLength);

            if (last == null) {
                first = last = s;
            }
            else {
                last.next = s;
                last = s;
            }
            numSegments++;
        }

        void ConsumeSegment() {
            if (first == null) throw new Exception("No segment to consume");
            Segment s = first;
            first = s.next;
            s.next = null;
            pool.Add(s);
            numSegments--;
        }

        private class Segment {
            //public int length = 64;
            public double[,] data;
            public Segment next = null;

            public Segment(int channels, int length) {
                data = new double[channels, length];
            }
        }
    }
}
