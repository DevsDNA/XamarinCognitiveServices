namespace DNIDetector
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Newtonsoft.Json;

	public class FaceModel
	{
		[JsonProperty("faceId")]
		public string FaceId { get; set; }
	}
}
