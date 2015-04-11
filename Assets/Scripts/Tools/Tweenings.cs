using UnityEngine;
using System.Collections;

public static class Tweenings {

	// Tweening Functions - Thanks to Robert Penner and GFX47
	
	public static float easeOutQuadOpt( float start, float diff, float ratioPassed ){
		return -diff * ratioPassed * (ratioPassed - 2) + start;
	}
	
	public static float easeInQuadOpt( float start, float diff, float ratioPassed ){
		return diff * ratioPassed * ratioPassed + start;
	}
	
	public static float easeInOutQuadOpt( float start, float diff, float ratioPassed ){
		ratioPassed /= .5f;
		if (ratioPassed < 1) return diff / 2 * ratioPassed * ratioPassed + start;
		ratioPassed--;
		return -diff / 2 * (ratioPassed * (ratioPassed - 2) - 1) + start;
	}
	
	public static float linear(float start, float end, float val){
		return Mathf.Lerp(start, end, val);
	}
	
	public static float clerp(float start, float end, float val){
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);
		float retval = 0.0f;
		float diff = 0.0f;
		if ((end - start) < -half){
			diff = ((max - start) + end) * val;
			retval = start + diff;
		}else if ((end - start) > half){
			diff = -((max - end) + start) * val;
			retval = start + diff;
		}else retval = start + (end - start) * val;
		return retval;
	}
	
	public static float spring(float start, float end, float val){
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * Mathf.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + (1.2f * (1f - val)));
		return start + (end - start) * val;
	}
	
	public static float easeInQuad(float start, float end, float val){
		end -= start;
		return end * val * val + start;
	}
	
	public static float easeOutQuad(float start, float end, float val){
		end -= start;
		return -end * val * (val - 2) + start;
	}
	
	public static float easeInOutQuad(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val + start;
		val--;
		return -end / 2 * (val * (val - 2) - 1) + start;
	}
	
	public static float easeInCubic(float start, float end, float val){
		end -= start;
		return end * val * val * val + start;
	}
	
	public static float easeOutCubic(float start, float end, float val){
		val--;
		end -= start;
		return end * (val * val * val + 1) + start;
	}
	
	public static float easeInOutCubic(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val + start;
		val -= 2;
		return end / 2 * (val * val * val + 2) + start;
	}
	
	public static float easeInQuart(float start, float end, float val){
		end -= start;
		return end * val * val * val * val + start;
	}
	
	public static float easeOutQuart(float start, float end, float val){
		val--;
		end -= start;
		return -end * (val * val * val * val - 1) + start;
	}
	
	public static float easeInOutQuart(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val * val + start;
		val -= 2;
		return -end / 2 * (val * val * val * val - 2) + start;
	}
	
	public static float easeInQuint(float start, float end, float val){
		end -= start;
		return end * val * val * val * val * val + start;
	}
	
	public static float easeOutQuint(float start, float end, float val){
		val--;
		end -= start;
		return end * (val * val * val * val * val + 1) + start;
	}
	
	public static float easeInOutQuint(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val * val * val + start;
		val -= 2;
		return end / 2 * (val * val * val * val * val + 2) + start;
	}
	
	public static float easeInSine(float start, float end, float val){
		end -= start;
		return -end * Mathf.Cos(val / 1 * (Mathf.PI / 2)) + end + start;
	}
	
	public static float easeOutSine(float start, float end, float val){
		end -= start;
		return end * Mathf.Sin(val / 1 * (Mathf.PI / 2)) + start;
	}
	
	public static float easeInOutSine(float start, float end, float val){
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * val / 1) - 1) + start;
	}
	
	public static float easeInExpo(float start, float end, float val){
		end -= start;
		return end * Mathf.Pow(2, 10 * (val / 1 - 1)) + start;
	}
	
	public static float easeOutExpo(float start, float end, float val){
		end -= start;
		return end * (-Mathf.Pow(2, -10 * val / 1) + 1) + start;
	}
	
	public static float easeInOutExpo(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * Mathf.Pow(2, 10 * (val - 1)) + start;
		val--;
		return end / 2 * (-Mathf.Pow(2, -10 * val) + 2) + start;
	}
	
	public static float easeInCirc(float start, float end, float val){
		end -= start;
		return -end * (Mathf.Sqrt(1 - val * val) - 1) + start;
	}
	
	public static float easeOutCirc(float start, float end, float val){
		val--;
		end -= start;
		return end * Mathf.Sqrt(1 - val * val) + start;
	}
	
	public static float easeInOutCirc(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return -end / 2 * (Mathf.Sqrt(1 - val * val) - 1) + start;
		val -= 2;
		return end / 2 * (Mathf.Sqrt(1 - val * val) + 1) + start;
	}
	
	public static float easeInBounce(float start, float end, float val){
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d-val) + start;
	}
	
	public static float easeOutBounce(float start, float end, float val){
		val /= 1f;
		end -= start;
		if (val < (1 / 2.75f)){
			return end * (7.5625f * val * val) + start;
		}else if (val < (2 / 2.75f)){
			val -= (1.5f / 2.75f);
			return end * (7.5625f * (val) * val + .75f) + start;
		}else if (val < (2.5 / 2.75)){
			val -= (2.25f / 2.75f);
			return end * (7.5625f * (val) * val + .9375f) + start;
		}else{
			val -= (2.625f / 2.75f);
			return end * (7.5625f * (val) * val + .984375f) + start;
		}
	}
	
	public static float easeInOutBounce(float start, float end, float val){
		end -= start;
		float d= 1f;
		if (val < d/2) return easeInBounce(0, end, val*2) * 0.5f + start;
		else return easeOutBounce(0, end, val*2-d) * 0.5f + end*0.5f + start;
	}
	
	public static float easeInBack(float start, float end, float val){
		end -= start;
		val /= 1;
		float s= 1.70158f;
		return end * (val) * val * ((s + 1) * val - s) + start;
	}
	
	public static float easeOutBack(float start, float end, float val){
		float s= 1.70158f;
		end -= start;
		val = (val / 1) - 1;
		return end * ((val) * val * ((s + 1) * val + s) + 1) + start;
	}
	
	public static float easeInOutBack(float start, float end, float val){
		float s= 1.70158f;
		end -= start;
		val /= .5f;
		if ((val) < 1){
			s *= (1.525f);
			return end / 2 * (val * val * (((s) + 1) * val - s)) + start;
		}
		val -= 2;
		s *= (1.525f);
		return end / 2 * ((val) * val * (((s) + 1) * val + s) + 2) + start;
	}
	
	public static float easeInElastic(float start, float end, float val){
		end -= start;
		
		float d = 1f;
		float p = d * .3f;
		float s= 0;
		float a = 0;
		
		if (val == 0) return start;
		val = val/d;
		if (val == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		val = val-1;
		return -(a * Mathf.Pow(2, 10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p)) + start;
	}		
	
	public static float easeOutElastic(float start, float end, float val){
		end -= start;
		
		float d = 1f;
		float p= d * .3f;
		float s= 0;
		float a= 0;
		
		if (val == 0) return start;
		
		val = val / d;
		if (val == 1) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		return (a * Mathf.Pow(2, -10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p) + end + start);
	}		
	
	public static float easeInOutElastic(float start, float end, float val)
	{
		end -= start;
		
		float d = 1f;
		float p= d * .3f;
		float s= 0;
		float a = 0;
		
		if (val == 0) return start;
		
		val = val / (d/2);
		if (val == 2) return start + end;
		
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4;
		}else{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}
		
		if (val < 1){
			val = val-1;
			return -0.5f * (a * Mathf.Pow(2, 10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p)) + start;
		}
		val = val-1;
		return a * Mathf.Pow(2, -10 * val) * Mathf.Sin((val * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
	}
}
