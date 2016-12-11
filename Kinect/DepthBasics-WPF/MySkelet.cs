using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.DepthBasics
{
    public class MyJoint{
        public double x=0,y=0,z=0;
        private double x2=0, y2=0, z2=0;
        public void add(double tx, double ty, double tz)
        {
            x2 += tx;
            y2 += ty;
            z2 += tz;
        }
        public void average(int count)
        {
            x = x2 / count;
            y = y2 / count;
            z = z2 / count;
            x2 = y2 = z2 = 0;
        }
        
    }

    class MySkelet
    {
        public MyJoint rHand, lHand, rElbow, lElbow, rShoulder, lShoulder;
        public void average(int count)
        {
            rHand.average(count);
            rElbow.average(count);
            rShoulder.average(count);
            lHand.average(count);
            lElbow.average(count);
            lShoulder.average(count);
        }
        public MySkelet()
        {
            rHand = new MyJoint();
            rElbow = new MyJoint();
            rShoulder = new MyJoint();
            lHand = new MyJoint();
            lElbow = new MyJoint();
            lShoulder = new MyJoint();
        }
        public string checkMoves()
        {
            string s;
            s="no";
            if (lHand.x < lElbow.x - 0.1 && lElbow.x < lShoulder.x && rShoulder.x < rElbow.x - 0.1 && rElbow.x < rHand.x - 0.1)
                if((lHand.y-lElbow.y)*(rHand.y-rElbow.y)<0)
                    s = "wave\n";
                else
                    s = "hug\n";
            else
                if (rHand.z < rElbow.z - 0.1 && rElbow.z < rShoulder.z - 0.1 && rHand.y < rShoulder.y)
                    s = "hs\n";
                else
                    if (rHand.y-0.2 > rShoulder.y && lHand.x>lElbow.x-0.1)
                        s = "hf\n";
            return s;
        }
    }
}
