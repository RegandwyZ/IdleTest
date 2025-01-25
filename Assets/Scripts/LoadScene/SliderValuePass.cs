using UnityEngine;
using UnityEngine.UI;

namespace LoadScene
{
	public class SliderValuePass : MonoBehaviour 
	{
		[SerializeField] private Text _progress;
	
		public void UpdateProgress (float content) 
		{
			_progress.text = Mathf.Round( content*100) +"%";
		}
	}
}
