using System;
using System.Collections.Generic;
using System.Text;

public class Format {
	private const float sixtieth = 1f / 60f;
	public static string Time(float time) {
		return string.Format("{0:0}", (time * sixtieth)) + ":" + string.Format("{0:00.0}", time % 60);
	}
	public static string TimeNamed(float time) {
		if (time % 60 == 0) {
			return string.Format("{0:0} Min", (time * sixtieth));
		} else {
			return string.Format("{0:0} Min", (time * sixtieth)) + ", " + string.Format("{0:0.0} Secs", time % 60); 
		}
	}
}

