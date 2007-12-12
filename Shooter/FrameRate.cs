using System;
using System.Collections.Generic;
using System.Text;

namespace Shooter
{
    class FrameRate
    {
           public static int  CalculateFrameRate(){

        if( System.Environment.TickCount - lastTick >= 1000 ){
            lastFrameRate = frameRate;
            frameRate = 0;
            lastTick = System.Environment.TickCount;
           }

        frameRate += 1;

        return lastFrameRate;

           }

        private static int lastTick; 
    private static int lastFrameRate ;
    private static int frameRate ;
    }
}
